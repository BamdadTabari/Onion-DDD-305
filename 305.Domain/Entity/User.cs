﻿using _305.Domain.Common;

namespace _305.Domain.Entity;
public class User : BaseEntity
{
    #region identity

    public required string mobile { get; set; }
    public bool is_mobile_confirmed { get; set; }

    public required string email { get; set; }

    #endregion

    #region Login

    public string password_hash { get; set; } = string.Empty;

    public int failed_login_count { get; set; }
    public DateTime? lock_out_end_time { get; set; }

    public DateTime? last_login_date_time { get; set; }

    #endregion

    #region Management

    public required string security_stamp { get; set; }
    public required string concurrency_stamp { get; set; }
    public bool is_locked_out { get; set; }
    public bool is_active { get; set; }
    public string? refresh_token { get; set; }
    public DateTime refresh_token_expiry_time { get; set; }

    public bool is_delete_able { get; set; } = true;
    #endregion

    #region Navigations
    public ICollection<UserRole>? user_roles { get; set; }
    #endregion

    /// <summary>
    /// سازنده برای ایجاد کاربر با مقادیر ضروری
    /// </summary>
    public User(string name, string slug, string mobile, string email, string security_stamp, string concurrency_stamp) : base(name, slug)
    {
        this.mobile = mobile;
        this.email = email;
        this.security_stamp = security_stamp;
        this.concurrency_stamp = concurrency_stamp;
    }

    public User() { }
}
