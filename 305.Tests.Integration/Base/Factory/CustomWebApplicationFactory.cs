﻿using _305.Application.IBaseRepository;
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
    private const string TestDatabaseName = "TestDb_305";

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

            // اتصال به SQL Server محلی (یا Docker)
            const string connectionString = $"Server=.;Database={TestDatabaseName};Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True";

            // ثبت مجدد DbContext با SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
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
                // حذف دیتابیس تستی قبلی برای شروع تمیز
                db.Database.EnsureDeleted();

                // اعمال تمام مایگریشن‌ها (دقیق‌تر از EnsureCreated)
                db.Database.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "خطا در آماده‌سازی دیتابیس تستی: {Message}", ex.Message);
                throw;
            }
        });
    }
}
