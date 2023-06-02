namespace Regale.Test.Solver;

public sealed class AlwaysRightRouting : IRouting
{
    public List<(Position position, Direction direction)> GetMoves(RoutingArgs args)
    {
        return new()
        {
            (args.Present, Direction.Right),
        };
    }
}
