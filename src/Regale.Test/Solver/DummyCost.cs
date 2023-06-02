using Regale.Estimation;

namespace Regale.Test.Solver;

public sealed class DummyCost : ICost
{
    public (int cost, Position depot) GetCost(Map map, ReadOnlySpan<Position> depots, Position present)
    => (Functions.ManhattanMetric(depots[0], present), depots[0]);
}
