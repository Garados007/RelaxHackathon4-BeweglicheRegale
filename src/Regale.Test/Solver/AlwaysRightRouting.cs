namespace Regale.Test.Solver;

public sealed class AlwaysRightRouting : IRouting
{
    public ReadOnlySpan<(Position position, Direction direction)> GetMoves(Map map, MoveMap currentMoves, Map<bool> spaceUsed, Position present, Position depot)
    {
        return new[]
        {
            (present, Direction.Right),
        };
    }
}
