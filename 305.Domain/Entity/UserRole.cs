using _305.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace _305.Domain.Entity;
public class UserRole : BaseEntity
{
[Column("roleid")]
public long role_id { get; set; }
[Column("userid")]
public long user_id { get; set; }

    public User? user { get; set; }
    public Role? role { get; set; }

    /// <summary>
    /// سازنده برای ایجاد ارتباط کاربر و نقش
    /// </summary>
    public UserRole(long user_id, long role_id) : base()
    {
        this.user_id = user_id;
        this.role_id = role_id;
    }

    public UserRole() { }
}

