namespace Regale;

public static class Extensions
{
    public static (int dx, int dy) GetDelta(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => (0, -1),
            Direction.Right => (1, 0),
            Direction.Down => (0, 1),
            Direction.Left => (-1, 0),
            _ => (0, 0)
        };
    }
}
