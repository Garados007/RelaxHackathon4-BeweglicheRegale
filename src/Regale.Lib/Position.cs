namespace Regale;

public record struct Position(
    int X,
    int Y
) : IComparable<Position>
{
    public readonly Position Abs()
    => new(Math.Abs(X), Math.Abs(Y));

    public int CompareTo(Position other)
    {
        var res = Y.CompareTo(other.Y);
        if (res != 0)
            return res;
        else return X.CompareTo(other.X);
    }

    public static Position Zero => new(0, 0);

    public static Position operator +(Position pos, (int dx, int dy) offset)
    => new(pos.X + offset.dx, pos.Y + offset.dy);

    public static Position operator -(Position pos, (int dx, int dy) offset)
    => new(pos.X - offset.dx, pos.Y - offset.dy);

    public static Position operator +(Position a, Position b)
    => new(a.X + b.X, a.Y + b.Y);

    public static Position operator -(Position a, Position b)
    => new(a.X - b.X, a.Y - b.Y);

    /// <summary>
    /// Checks if the current position is between the position a and b. Positions a and b
    /// should be on the same x or y-axis. If the current position is at the same position
    /// as either a or b this also returns true.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool IsInLine(Position a, Position b)
    {
        if (a.X != b.X && a.Y != b.Y)
            return false; // there is even no line between a and b. Are you stupid?
        if (a.X == b.X)
        {
            if (X != a.X)
                return false;
            var (y1, y2) = (a.Y, b.Y);
            if (y1 > y2)
                (y1, y2) = (y2, y1);
            return Y >= y1 && Y <= y2;
        }
        else
        {
            if (Y != a.Y)
                return false;
            var (x1, x2) = (a.X, b.X);
            if (x1 > x2)
                (x1, x2) = (x2, x1);
            return X >= x1 && X <= x2;
        }
    }
}
