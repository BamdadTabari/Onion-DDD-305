using _305.BuildingBlocks.IService;

namespace _305.BuildingBlocks.Service;

/// <summary>
/// Default implementation of <see cref="IDateTimeProvider"/> using <see cref="DateTime"/>.
/// </summary>
public class SystemDateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTime Now => DateTime.Now;

    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;
}
