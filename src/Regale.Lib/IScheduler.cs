namespace Regale;

/// <summary>
/// Creates a usefull order of present
/// </summary>
public interface IScheduler
{
    IEnumerable<Position> GetSchedule(Map map, ReadOnlySpan<Position> depots);
}
