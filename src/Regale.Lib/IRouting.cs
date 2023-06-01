namespace Regale;

/// <summary>
/// Creates a route from a single present from its starting position to a depot
/// </summary>
public interface IRouting
{
    IEnumerable<(Position, Direction)> GetRoute(Map map, ReadOnlySpan<Position> depots, Position present);
}
