using _305.Domain.Events;
using MediatR;
using Serilog;

namespace _305.Application.DomainEventHandlers;

/// <summary>
/// هندلر نمونه برای رویداد ایجاد کاربر
/// </summary>
public class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    public Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Log.Information("Domain event handled for user {UserId}", notification.User.id);
        return Task.CompletedTask;
    }
}
