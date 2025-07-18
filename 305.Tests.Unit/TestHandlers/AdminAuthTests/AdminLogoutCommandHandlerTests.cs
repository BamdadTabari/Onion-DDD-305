﻿using _305.Application.Features.AdminAuthFeatures.Command;
using _305.Application.Features.AdminAuthFeatures.Handler;
using _305.Application.IUOW;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq.Expressions;
using System.Security.Claims;

namespace _305.Tests.Unit.TestHandlers.AdminAuthTests;
public class AdminLogoutCommandHandlerTests
{

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenAuthorizationHeaderIsMissing()
    {
        // ساخت user با claim
        const string userId = "1";
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId)
    };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        // ساخت کامل HttpContext با header و response
        var context = new DefaultHttpContext
        {
            User = principal
        };
        context.Request.Headers["Authorization"] = ""; // حذف هدر یا خالی گذاشتن
        context.Response.Body = new MemoryStream();     // اگر نیاز به stream داری

        // Mock IHttpContextAccessor
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(h => h.HttpContext).Returns(context);

        // Mock IUnitOfWork و برگشتن کاربر تستی
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.UserRepository.FindSingle(It.IsAny<Expression<Func<User, bool>>>(), null))
            .ReturnsAsync(AdminUserDataProvider.Row(id: int.Parse(userId)));

        // Handler
        var handler = new AdminLogoutCommandHandler(unitOfWorkMock.Object, httpContextAccessorMock.Object);

        // Act
        var result = await handler.Handle(new AdminLogoutCommand(), CancellationToken.None);

        // Assert
        Assert.False(result.is_success);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserIdIsMissing()
    {
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity()) // No NameIdentifier
        };

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContext);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var handler = new AdminLogoutCommandHandler(unitOfWorkMock.Object, httpContextAccessorMock.Object);

        var result = await handler.Handle(new AdminLogoutCommand(), CancellationToken.None);

        Assert.False(result.is_success);
    }

    [Fact]
    public async Task Handle_ShouldCatchException_AndReturnFailure()
    {
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.NameIdentifier, "invalid_id")
        }))
        };

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContext);

        var unitOfWorkMock = new Mock<IUnitOfWork>(); // بدون setup برای ایجاد خطا

        var handler = new AdminLogoutCommandHandler(unitOfWorkMock.Object, httpContextAccessorMock.Object);

        var result = await handler.Handle(new AdminLogoutCommand(), CancellationToken.None);

        Assert.False(result.is_success);
    }

}
