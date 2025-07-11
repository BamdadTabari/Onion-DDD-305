﻿using _305.Application.Base.Response;
using _305.Application.Features.AdminAuthFeatures.Command;
using _305.Application.Features.AdminAuthFeatures.Response;
using _305.Application.Helpers;
using _305.Application.IService;
using _305.Application.IUOW;
using _305.BuildingBlocks.Configurations;
using _305.BuildingBlocks.Helper;
using _305.BuildingBlocks.Security;
using _305.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace _305.Application.Features.AdminAuthFeatures.Handler;

public class AdminLoginCommandHandler(
    IUnitOfWork unitOfWork,
    IJwtService jwtService,
    IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env
) : IRequestHandler<AdminLoginCommand, ResponseDto<LoginResponse>>
{
    public static readonly JwtConfig Config = new();
    public static readonly LockoutConfig lockOutConfig = new();
    public async Task<ResponseDto<LoginResponse>> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await unitOfWork.UserRepository.FindSingle(x => x.email == request.email);
            if (user is not { is_active: true })
                return Responses.NotFound<LoginResponse>(null, name: "کاربر");

            // بررسی قفل شدن کاربر
            if (IsAccountLocked(user))
                return Responses.Fail<LoginResponse>(null, message: "اکانت شما موقتا قفل شده است", code: 401);
            // بررسی رمز عبور
            if (!PasswordHasher.Check(user.password_hash, request.password))
                return await HandleInvalidPassword(user, cancellationToken);

            // موفقیت در ورود
            user.failed_login_count = 0;
            user.last_login_date_time = DateTime.Now;
            var role = await unitOfWork.UserRoleRepository.FindListAsync(x => x.userid == user.id, cancellationToken: cancellationToken);
            var token = await JwtTokenHelper.GenerateUniqueAccessToken(
                jwtService,
                unitOfWork,
                user,
                role.Select(x => x.role.name).ToList());
            var refreshToken = await JwtTokenHelper.GenerateUniqueRefreshToken(jwtService, unitOfWork);


            user.refresh_token = refreshToken;
            user.refresh_token_expiry_time = DateTime.Now.Add(Config.AdminRefreshTokenLifetime);

            unitOfWork.UserRepository.Update(user);
            await unitOfWork.CommitAsync(cancellationToken);

            var isDevelopment = env.IsDevelopment();
			// ذخیره توکن در کوکی به صورت یکسان
			var context = httpContextAccessor.HttpContext;

            CookieHelper.SetJwtCookie(
                context.Response,
                refreshToken,
                Config.AdminRefreshTokenLifetime,
                secure: !isDevelopment,
                domain:isDevelopment? null : "https://your-front-domain.com",
                sameSiteMode: isDevelopment? SameSiteMode.Lax: SameSiteMode.None);


            return Responses.Success(data:
                new LoginResponse()
                {
                    access_token = token,
                    expire_in = Config.AccessTokenLifetime.TotalMinutes
                },
                message: "ورود با موفقیت انجام شد",
                code: 200
                );
        }
        catch (Exception ex)
        {
            // لاگ‌گیری با Serilog برای ثبت خطاهای غیرمنتظره
            Log.Error(ex, "خطا در زمان ایجاد موجودیت: {Message}", ex.Message);
            return Responses.ExceptionFail<LoginResponse>(null, null);
        }
    }

    /// <summary>
    /// بررسی وضعیت قفل بودن حساب
    /// </summary>
    private static bool IsAccountLocked(User user) =>
        user.is_locked_out || user.lock_out_end_time > DateTime.Now;

    /// <summary>
    /// مدیریت خطا در صورت وارد کردن رمز نادرست
    /// </summary>
    private async Task<ResponseDto<LoginResponse>> HandleInvalidPassword(User user, CancellationToken cancellationToken)
    {
        user.failed_login_count++;
        if (user.failed_login_count >= lockOutConfig.FailedLoginLimit)
        {
            user.is_locked_out = true;
            user.lock_out_end_time = DateTime.Now.Add(lockOutConfig.LockoutDuration); // قفل موقت
        }

        unitOfWork.UserRepository.Update(user);
        await unitOfWork.CommitAsync(cancellationToken);
        return Responses.Fail<LoginResponse>(null, message: "رمز عبور یا نام کاربری اشتباه است.", code: 401);
    }
}
