﻿using _305.BuildingBlocks.Enums;

namespace _305.Application.Filters.Pagination;

// فیلتر صفحه‌بندی پیش‌فرض با امکانات جستجو و مرتب‌سازی
public class DefaultPaginationFilter : PaginationFilter
{
    // سازنده با مقداردهی پیش‌فرض صفحه و تعداد آیتم‌ها
    public DefaultPaginationFilter(int pageNumber = 1, int pageSize = 10) : base(pageNumber, pageSize) { }

    // سازنده بدون آرگومان
    public DefaultPaginationFilter() { }

    // عبارت جستجو برای فیلتر کردن نتایج
    public string? SearchTerm { get; set; }

    // نوع مرتب‌سازی بر اساس Enum تعریف شده (پیش‌فرض: تاریخ ایجاد)
    public SortByEnum SortBy { get; set; } = SortByEnum.created_at;
}
