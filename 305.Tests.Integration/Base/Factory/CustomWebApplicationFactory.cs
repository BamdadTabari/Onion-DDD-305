using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;
using _305.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace _305.Tests.Integration.Base.Factory;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    // نام دیتابیس تستی در حالت in-memory
    private const string InMemoryConnectionString = "DataSource=TestDb_305;Mode=Memory;Cache=Shared";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            // حذف DbContext قبلی
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // اتصال به دیتابیس SQLite در حافظه
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(InMemoryConnectionString);
                options.EnableSensitiveDataLogging(); // اختیاری برای دیباگ
            });

            // ثبت سرویس‌های مورد نیاز
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ایجاد سرویس‌پروایدر موقت
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();

            try
            {
                // باز کردن اتصال و ایجاد دیتابیس در حافظه
                db.Database.OpenConnection();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "خطا در آماده‌سازی دیتابیس تستی: {Message}", ex.Message);
                throw;
            }
        });
    }
}
