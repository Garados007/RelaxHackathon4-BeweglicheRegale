using SpaceUseMap = Regale.Map<bool>;

namespace Regale;

public class DummyRouter : IRouting {
    public ReadOnlySpan<(Position position, Direction direction)> GetMoves(
        Map map,
        MoveMap currentMoves,
        SpaceUseMap spaceUsed,
        Position present,
        Position depot
    ){
        return new (Position, Direction)[]{
            (new Position(0,0), Direction.None),
        };
    }
}
