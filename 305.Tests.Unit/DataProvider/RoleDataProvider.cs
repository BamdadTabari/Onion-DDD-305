﻿using _305.Application.Features.RoleFeatures.Command;
using _305.Application.Features.RoleFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Domain.Entity;

namespace _305.Tests.Unit.DataProvider;
public static class RoleDataProvider
{
    public static CreateRoleCommand Create(string name = "role-name", string slug = "role-slug")
    => new()
    {
        name = name,
        slug = slug
    };

    public static EditRoleCommand Edit(string name = "name", long id = 1, string slug = "slug")
        => new()
        {
            id = id,
            name = name,
            slug = slug,
            updated_at = DateTime.UtcNow,
        };


    public static Role Row(string name = "name", long id = 1, string slug = "slug")
        => new()
        {
            id = id,
            name = name,
            slug = slug,
            updated_at = DateTime.UtcNow,
        };

    public static DeleteRoleCommand Delete(long id = 1)
        => new()
        {
            id = id,
        };

    public static GetRoleBySlugQuery GetBySlug(string slug = "slug")
        => new()
        {
            slug = slug,
        };

    public static GetPaginatedRoleQuery GetByQueryFilter(string searchTerm = "")
        => new()
        {
            Page = 1,
            PageSize = 10,
            SearchTerm = searchTerm,
        };

    public static PaginatedList<Role> GetPaginatedList()
        => PaginatedListFactory.Create(new List<Role>
        {
            new () { id = 1, name = "Tech" },
            new () { id = 2, name = "Health" }
        });
}
