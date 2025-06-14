using _305.Application.IUOW;
using _305.BuildingBlocks.IService;

namespace _305.WebApi.Assistants.Tasks;

/// <summary>
/// وظیفه پاک‌سازی توکن‌های منقضی‌شده از لیست سیاه (Blacklist).
/// </summary>
public class TokenCleanupTask
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public TokenCleanupTask(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    /// <summary>
    /// اجرای پاک‌سازی توکن‌هایی که تاریخ انقضای آن‌ها گذشته است.
    /// </summary>
    /// <param name="unitOfWork">واحد کاری برای دسترسی به ریپازیتوری‌ها و ثبت تغییرات.</param>
    public async Task ExecuteAsync(IUnitOfWork unitOfWork)
    {
        // یافتن توکن‌هایی که منقضی شده‌اند
        var tokens = unitOfWork.TokenBlacklistRepository
            .FindList(t => t.expiry_date <= _dateTimeProvider.UtcNow);

        // حذف دسته‌ای توکن‌های منقضی‌شده
        unitOfWork.TokenBlacklistRepository.RemoveRange(tokens);

        // ثبت تغییرات در پایگاه داده
        await unitOfWork.CommitAsync(CancellationToken.None);
    }
}
