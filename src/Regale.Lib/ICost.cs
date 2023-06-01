namespace Regale;

/// <summary>
/// Determines the cost and the position of the next depot
/// </summary>
public interface ICost
{
    (int cost, Position depot) GetCost(Map map, ReadOnlySpan<Position> depots, Position present);
}
