﻿using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.AdminUserFeatures.Command;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.AdminUserFeatures.Handler;

public class DeleteAdminUserCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteAdminUserCommand, ResponseDto<string>>
{
    private readonly DeleteHandler _handler = new(unitOfWork);

    public async Task<ResponseDto<string>> Handle(DeleteAdminUserCommand request, CancellationToken cancellationToken)
    {
        return await _handler.HandleAsync<User, string>(
            findEntityAsync: () => unitOfWork.UserRepository.FindSingle(x => x.id == request.id),
            onDeleteAsync: entity => unitOfWork.UserRepository.Remove(entity),
            entityName: "کاربر ادمین",
            notFoundMessage: "کاربر ادمین پیدا نشد",
            successMessage: "کاربر ادمین با موفقیت حذف شد",
            cancellationToken: cancellationToken
        );
    }
}
