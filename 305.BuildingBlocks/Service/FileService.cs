using _305.BuildingBlocks.Helper;
using _305.BuildingBlocks.IService;
using Microsoft.AspNetCore.Http;

namespace _305.BuildingBlocks.Service;

/// <summary>
/// سرویس فایل برای بارگذاری و حذف تصاویر.
/// وابسته به HttpContext برای دسترسی به اطلاعات درخواست (مانند آدرس Base).
/// </summary>
public class FileService : IFileService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IFileManager _fileManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileService"/> class.
    /// </summary>
    /// <param name="contextAccessor">Provides access to the current HTTP context.</param>
    /// <param name="fileManager">Handles file operations such as upload and delete.</param>
    public FileService(IHttpContextAccessor contextAccessor, IFileManager fileManager)
    {
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        _fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
    }

    /// <summary>
    /// حذف فایل از مسیر مشخص‌شده در URL
    /// </summary>
    /// <param name="imageUrl">آدرس نسبی فایل که باید حذف شود</param>
    public void DeleteFile(string imageUrl)
    {
        _fileManager.DeleteFile(imageUrl);
    }

    /// <summary>
    /// آپلود فایل به سرور و بازگرداندن مسیر URL آن
    /// </summary>
    /// <param name="file">فایل دریافت‌شده از فرم</param>
    /// <returns>مسیر URL فایل آپلودشده</returns>
    public async Task<string> UploadFile(IFormFile file)
    {
        if (file == null)
            throw new ArgumentNullException(nameof(file));

        // دریافت شیء HttpRequest برای استخراج مسیر دامنه جهت تولید URL کامل
        var request = _contextAccessor.HttpContext?.Request
                      ?? throw new InvalidOperationException("HttpContext در دسترس نیست.");

        // استفاده از FileManager برای ذخیره فایل و تولید URL نهایی
        return await _fileManager.UploadFileAsync(file, request);
    }
}

