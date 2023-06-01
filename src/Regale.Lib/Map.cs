namespace Regale;

public class Map
{
    private readonly Memory<Field> data;

    public int Width { get; }

    public int Height { get; }

    public Map(int width, int height)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width));
        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height));

        Width = width;
        Height = height;
        data = new Field[Width * Height];
    }

    public Field this[uint x, uint y]
    {
        get => this[new(x, y)];
        set => this[new(x, y)] = value;
    }

    public Field this[Position position]
    {
        get => data.Span[GetOffset(position)];
        set => data.Span[GetOffset(position)] = value;
    }

    private int GetOffset(Position position)
    {
        if (position.X >= Width)
            throw new ArgumentOutOfRangeException(nameof(position), "X value is larger than width");
        if (position.Y >= Height)
            throw new ArgumentOutOfRangeException(nameof(position), "Y value is larger than height");

        return (int)(position.Y * Width + position.X);
    }

    public IEnumerable<(Field field, Position position)> GetFields()
    {
        uint x = 0;
        uint y = 0;
        for (int i = 0; i < data.Length; ++i)
        {
            yield return (data.Span[i], new(x, y));
            x++;
            if (x >= Width)
            {
                x = 0;
                y++;
            }
        }
    }
}
