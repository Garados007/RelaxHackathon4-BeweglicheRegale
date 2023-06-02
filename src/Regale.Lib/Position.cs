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
}
