namespace Regale;

public class Map<T>
    where T : struct
{
    private readonly Memory<T> data;

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
        data = new T[Width * Height];
    }

    public T this[uint x, uint y]
    {
        get => this[new(x, y)];
        set => this[new(x, y)] = value;
    }

    public T this[Position position]
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

    public IEnumerable<(T field, Position position)> GetFields()
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
