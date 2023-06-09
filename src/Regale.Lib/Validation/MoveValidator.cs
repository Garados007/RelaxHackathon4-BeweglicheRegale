namespace Regale.Validation;

public sealed class MoveValidator
{
    public Map Map { get; }

    public MoveMap MoveMap { get; }

    /// <summary>
    /// This map stores the direction from which it receives the a package. To
    /// simplify the algorithm it just stores the direction from the movement.
    /// To know the real direction from which it receives a package you have to
    /// inverse the stored direction. Also the starting position is locked to
    /// prevent any colliding incoming flow.
    /// </summary>
    private readonly MoveMap receivingMap;

    public MoveValidator(Map map)
    {
        Map = map;
        MoveMap = new(map.Width, map.Height);
        receivingMap = new(map.Width, map.Height);
    }

    /// <summary>
    /// Applies a move operation <paramref name="direction"/>  to the position <paramref name="start"/>.
    /// If e.g. additional moves need to be made to "create space" this also effects other positions.
    /// </summary>
    /// <returns>
    /// true: successful applied the operation, the map has changed<br/>
    /// false: could not apply the operation, nothing changed
    /// </returns>
    public bool Apply(Position start, Direction direction)
    {
        // check for valid directions
        if (direction == Direction.None)
            return false;
        // get flow
        var positions = Map.ExtendDirection(start, direction);
        if (positions is null)
            return false;
        // get delta
        var delta = direction.GetDelta();
        // check if all positions are allowed
        Position? target = null;
        if (positions.Count > 0)
        {
            if (Map[positions[0]] == Field.None)
                return false;
            var receive = receivingMap[positions[0]];
            if (receive != Direction.None && receive != direction)
                return false;
        }
        foreach (var pos in positions)
        {
            target = receivingMap.GetTargetPosition(pos, delta);
            if (target is null)
                return false;
            var receive = receivingMap[target.Value];
            if (receive != Direction.None && receive != direction)
                return false;
        }
        // apply direction
        foreach (var pos in positions)
        {
            // set in move map
            MoveMap[pos] = direction;
            // lock direction in receiving map
            receivingMap[pos] = direction;
        }
        if (target is not null)
            receivingMap[target.Value] = direction;
        // success
        return true;
    }
}
