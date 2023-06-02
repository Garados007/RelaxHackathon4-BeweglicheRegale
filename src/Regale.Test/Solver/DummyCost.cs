namespace Regale.Test.Solver;

public sealed class DummyCost : ICost
{
    public (int cost, Position depot) GetCost(Map map, ReadOnlySpan<Position> depots, Position present)
    {
        var cost = (depots[0] - present).Abs();
        return (cost.X + cost.Y, depots[0]);
    }
}
