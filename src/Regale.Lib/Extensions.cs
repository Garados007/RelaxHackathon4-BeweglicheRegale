namespace Regale;

public static class Extensions
{
    public static Position GetDelta(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => new(0, -1),
            Direction.Right => new(1, 0),
            Direction.Down => new(0, 1),
            Direction.Left => new(-1, 0),
            _ => new(0, 0),
        };
    }
}
