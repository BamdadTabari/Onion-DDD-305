﻿using System.Collections.Generic;
namespace _305.Domain.Common;

/// <summary>
/// اینترفیس پایه برای تمام موجودیت‌ها (Entities) در سیستم
/// شامل ویژگی‌های مشترک مثل شناسه، نام، اسلاگ و زمان‌های ایجاد و به‌روزرسانی است
/// </summary>
public interface IBaseEntity
{
    /// <summary>
    /// شناسه یکتا (Primary Key)
    /// </summary>
    long id { get; }

    /// <summary>
    /// نام اختیاری موجودیت
    /// </summary>
    string? name { get; }

    /// <summary>
    /// اسلاگ یکتا (معمولاً برای URLها استفاده می‌شود)
    /// </summary>
    string slug { get; }

    /// <summary>
    /// زمان ایجاد موجودیت (UTC)
    /// </summary>
    DateTime created_at { get; }

    /// <summary>
    /// زمان آخرین بروزرسانی موجودیت (UTC)
    /// </summary>
    DateTime updated_at { get; }

    /// <summary>
    /// لیست رویدادهای دامنه مرتبط با این موجودیت
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
}
