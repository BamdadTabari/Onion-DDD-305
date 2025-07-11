﻿using _305.Domain.Common;

namespace _305.Domain.Entity;
public class BlacklistedToken : BaseEntity
{
    public string token { get; set; } = null!; // The JWT token string
    public DateTime expiry_date { get; set; } // The expiration date of the token
    public DateTime black_listed_on { get; set; } = DateTime.Now; // When the token was blacklisted

    /// <summary>
    /// سازنده برای ایجاد توکن بلاک‌شده
    /// </summary>
    public BlacklistedToken(string token, DateTime expiry_date) : base()
    {
        this.token = token;
        this.expiry_date = expiry_date;
    }

    public BlacklistedToken() { }
}
