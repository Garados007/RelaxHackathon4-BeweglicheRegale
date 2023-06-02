namespace Regale;

public record struct Position(
    int X,
    int Y
)
{
    public readonly Position Abs()
    => new(Math.Abs(X), Math.Abs(Y));

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
