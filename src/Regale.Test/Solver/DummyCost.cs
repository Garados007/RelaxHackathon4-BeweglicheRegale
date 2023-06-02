namespace Regale.Test.Solver;

public sealed class DummyCost : ICost
{
    public (int cost, Position depot) GetCost(Map map, ReadOnlySpan<Position> depots, Position present)
    {
        var (dx, dy) = ((int)depots[0].X, (int)depots[0].Y);
        var (px, py) = ((int)present.X, (int)present.Y);
        return (Math.Abs(dx - px) + Math.Abs(dy - py), depots[0]);
    }
}
