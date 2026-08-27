using _305.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace _305.Infrastructure.DomainEvents;

/// <summary>
/// انتشار دهنده رویدادهای دامنه به صورت عمومی با استفاده از MediatR
/// </summary>
public static class DomainEventDispatcher
{
    public static async Task DispatchEventsAsync(DbContext context, IMediator mediator, CancellationToken cancellationToken = default)
    {
        var domainEntities = context.ChangeTracker
            .Entries<IBaseEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = domainEntities.SelectMany(x => x.DomainEvents).ToList();

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

        foreach (var entity in domainEntities)
        {
            entity.ClearDomainEvents();
        }
    }
}
