namespace _305.BuildingBlocks.IService;

/// <summary>
/// Abstraction for retrieving current date and time values.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>Gets current local time.</summary>
    DateTime Now { get; }

    /// <summary>Gets current UTC time.</summary>
    DateTime UtcNow { get; }
}
