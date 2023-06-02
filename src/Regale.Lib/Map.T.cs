using OneOf.Types;

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

    public void Init(T[][] data)
    {
        if (data.Length != Height)
            throw new ArgumentException("height doesn't match", nameof(data));
        for (int y = 0; y < Height; ++y)
        {
            if (data[y].Length != Width)
                throw new ArgumentException($"width of row {y + 1} doesn't match", nameof(data));
            for (int x = 0; x < Width; ++x)
                this[(uint)x, (uint)y] = data[y][x];
        }
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

    public Span<T> GetRow(uint y)
    {
        if (y >= Height)
            throw new ArgumentOutOfRangeException(nameof(y));
        var offset = ((int)y) * Width;
        return data.Span.Slice(offset, Width);
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

    /// <summary>
    /// Adds the delta to the start position. If the resulting position is
    /// contained within this map it is returned. Otherwise, it returns null.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    /// <returns></returns>
    public Position? GetTargetPosition(Position start, int dx, int dy)
    {
        var tx = start.X + dx;
        var ty = start.Y + dy;
        if (tx >= 0 && tx < Width && ty >= 0 && ty < Height)
            return new((uint)tx, (uint)ty);
        else return null;
    }
}
