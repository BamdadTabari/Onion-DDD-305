﻿using _305.Application.Features.RoleFeatures.Handler;
using _305.Application.Features.RoleFeatures.Response;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.RoleTests;
public class GetRoleBySlugQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnData_WhenRoleExists()
    {
        var role = RoleDataProvider.Row(name: "Name", id: 1, slug: "slug");


        await GetBySlugHandlerTestHelper.TestGetBySlug_Success<
            Role,
            RoleResponse,
            IRepository<Role>,
            GetRoleBySlugQueryHandler>(
            uow => new GetRoleBySlugQueryHandler(uow),
            (handler, token) => handler.Handle(RoleDataProvider.GetBySlug(slug: "slug"), token),
            uow => uow.RoleRepository,
            role
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenRoleDoesNotExist()
    {
        await GetBySlugHandlerTestHelper.TestGetBySlug_NotFound<
            Role,
            RoleResponse,
            IRepository<Role>,
            GetRoleBySlugQueryHandler>(
            uow => new GetRoleBySlugQueryHandler(uow),
            (handler, token) => handler.Handle(RoleDataProvider.GetBySlug(slug: "not-found"), token),
            uow => uow.RoleRepository
        );
    }
}
