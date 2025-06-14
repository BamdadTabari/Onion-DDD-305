﻿using _305.Application.Base.Response;
using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using _305.Domain.Common;
using _305.Tests.Unit.Assistant;
using Moq;
using System.Linq.Expressions;

namespace _305.Tests.Unit.GenericHandlers;
public static class GetBySlugHandlerTestHelper
{
    /// <summary>
    /// تست موفقیت‌آمیز دریافت موجودیت با استفاده از slug
    /// این متد با ساخت موک UnitOfWork و Repository، 
    /// متد FindSingle را برای دریافت موجودیت مشخص تنظیم و اجرای هندلر را بررسی می‌کند.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت که IBaseEntity را پیاده‌سازی کرده</typeparam>
    /// <typeparam name="TDto">نوع DTO خروجی</typeparam>
    /// <typeparam name="TRepository">نوع ریپازیتوری که IRepository<TEntity> است</typeparam>
    /// <typeparam name="THandler">نوع هندلر</typeparam>
    /// <param name="handlerFactory">تابعی برای ساخت هندلر با ورودی UnitOfWork</param>
    /// <param name="execute">تابعی که هندلر و توکن لغو می‌گیرد و نتیجه اجرای هندلر را برمی‌گرداند</param>
    /// <param name="repoSelector">اکسپریشن برای انتخاب ریپازیتوری از UnitOfWork</param>
    /// <param name="entity">موجودیت نمونه‌ای که باید توسط موک ریپازیتوری برگردانده شود</param>
    public static async Task TestGetBySlug_Success<TEntity, TDto, TRepository, THandler>(
    Func<IUnitOfWork, THandler> handlerFactory,
    Func<THandler, CancellationToken, Task<ResponseDto<TDto>>> execute,
    Expression<Func<IUnitOfWork, TRepository>> repoSelector,
    TEntity entity
)
    where TEntity : class, IBaseEntity
    where TDto : class, new()
    where TRepository : class, IRepository<TEntity>
    where THandler : class
    {
        // ساخت موک UnitOfWork و Repository و اتصال آن‌ها
        var (unitOfWorkMock, repoMock) = RepositoryMockFactory.CreateFor(repoSelector);

        // ignore includeFunc in Moq verification
        repoMock.Setup(r =>
            r.FindSingle(
                It.IsAny<Expression<Func<TEntity, bool>>>(),
                It.IsAny<Func<IQueryable<TEntity>, IQueryable<TEntity>>>()
            )
        ).ReturnsAsync(entity);

        var handler = handlerFactory(unitOfWorkMock.Object);
        var result = await execute(handler, CancellationToken.None);

        Assert.True(result.is_success);
        Assert.NotNull(result.data);
    }



    /// <summary>
    /// تست حالت یافت نشدن موجودیت هنگام جستجو بر اساس slug
    /// این متد مطمئن می‌شود که در صورت عدم یافتن موجودیت،
    /// نتیجه‌ای با is_success = false و داده null بازگردانده شود.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت</typeparam>
    /// <typeparam name="TDto">نوع DTO خروجی</typeparam>
    /// <typeparam name="TRepository">نوع ریپازیتوری</typeparam>
    /// <typeparam name="THandler">نوع هندلر</typeparam>
    /// <param name="handlerFactory">تابع ساخت هندلر</param>
    /// <param name="execute">تابع اجرای هندلر</param>
    /// <param name="repoSelector">اکسپریشن انتخاب ریپازیتوری</param>
    public static async Task TestGetBySlug_NotFound<TEntity, TDto, TRepository, THandler>(
        Func<IUnitOfWork, THandler> handlerFactory,
        Func<THandler, CancellationToken, Task<ResponseDto<TDto>>> execute,
        Expression<Func<IUnitOfWork, TRepository>> repoSelector
    )
        where TEntity : class, IBaseEntity
        where TDto : class, new()
        where TRepository : class, IRepository<TEntity>
        where THandler : class
    {
        // ایجاد موک UnitOfWork و Repository
        var (unitOfWorkMock, repoMock) = RepositoryMockFactory.CreateFor(repoSelector);

        // تنظیم FindSingle برای بازگرداندن null (موجودیت یافت نشد)
        repoMock.Setup(r => r.FindSingle(It.IsAny<Expression<Func<TEntity, bool>>>(), null)).ReturnsAsync((TEntity?)null);

        // ساخت هندلر با UnitOfWork موک شده
        var handler = handlerFactory(unitOfWorkMock.Object);

        // اجرای هندلر
        var result = await execute(handler, CancellationToken.None);

        // بررسی شکست عملیات و null بودن داده برگشتی
        Assert.False(result.is_success);
        Assert.Null(result.data);
    }
}

