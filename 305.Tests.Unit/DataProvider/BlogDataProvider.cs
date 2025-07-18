﻿using _305.Application.Features.BlogFeatures.Command;
using _305.Application.Features.BlogFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;

namespace _305.Tests.Unit.DataProvider;
public static class BlogDataProvider
{
    public static CreateBlogCommand Create(string name = "test", string? slug = "slug", long categoryId = 1)
        => new()
        {
            name = name,
            description = "Test",
            image_alt = "Test",
            image_file = FakeFileFactory.FromText(),
            blog_category_id = categoryId,
            blog_text = "Test",
            created_at = DateTime.UtcNow,
            updated_at = DateTime.UtcNow,
            estimated_read_time = 5,
            keywords = "a,b,c",
            show_blog = true,
            meta_description = "Test",
            slug = slug,
        };

    public static EditBlogCommand Edit(string name = "test", long id = 1, long categoryId = 1,
        string slug = "slug")
    => new()
    {
        id = id,
        name = name,
        description = "Test",
        image_alt = "Test",
        image_file = FakeFileFactory.FromText(),
        blog_category_id = categoryId,
        blog_text = "Test",
        updated_at = DateTime.UtcNow,
        estimated_read_time = 5,
        keywords = "a,b,c",
        show_blog = true,
        meta_description = "Test",
        slug = slug,
        image = "image.png",
    };


    public static Blog Row(string name = "name", long id = 1, string slug = "slug", string image = "image.jpg")
    => new()
    {
        id = id,
        name = name,
        description = "Test",
        image_alt = "Test",
        blog_category_id = 1,
        blog_text = "Test",
        updated_at = DateTime.UtcNow,
        estimated_read_time = 5,
        keywords = "a,b,c",
        show_blog = true,
        meta_description = "Test",
        slug = slug,
        image = image,
        created_at = DateTime.UtcNow,
        blog_category = BlogCategoryDataProvider.Row()
    };

    public static DeleteBlogCommand Delete(long id = 1)
        => new()
        {
            id = id,
        };

    public static GetBlogBySlugQuery GetBySlug(string slug = "slug")
    => new()
    {
        slug = slug,
    };

    public static GetPaginatedBlogQuery GetByQueryFilter(string searchTerm = "")
    => new()
    {
        Page = 1,
        PageSize = 10,
        SearchTerm = searchTerm,
    };

    public static PaginatedList<Blog> GetPaginatedList()
        => PaginatedListFactory.Create(new List<Blog>
        {
            new()
            {
                id = 1,
                name = "slug 1",
                description = "Test",
                image_alt = "Test",
                blog_category_id = 1,
                blog_text = "Test",
                updated_at = DateTime.UtcNow,
                estimated_read_time = 5,
                keywords = "a,b,c",
                show_blog = true,
                meta_description = "Test",
                slug = "slug-1",
                image = "test.jpg",
                created_at = DateTime.UtcNow,
                blog_category = BlogCategoryDataProvider.Row()
            },
            new()
            {
                id = 1,
                name = "slug 2",
                description = "Test",
                image_alt = "Test",
                blog_category_id = 1,
                blog_text = "Test",
                updated_at = DateTime.UtcNow,
                estimated_read_time = 5,
                keywords = "a,b,c",
                show_blog = true,
                meta_description = "Test",
                slug = "slug-2",
                image = "test.jpg",
                created_at = DateTime.UtcNow,
                blog_category = BlogCategoryDataProvider.Row()
            }
        });

}



