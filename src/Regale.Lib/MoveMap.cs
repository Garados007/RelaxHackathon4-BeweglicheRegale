using System.Text;
using System.Text.Json;

namespace Regale;

public class MoveMap : Map<Direction>
{
    public MoveMap(int width, int height) : base(width, height)
    {
    }

    public void Save(Utf8JsonWriter writer)
    {
        var sb = new StringBuilder(Width * 4 + 2);
        writer.WriteStartArray();
        var indent = Span<byte>.Empty;
        var indentOffset = 0;
        if (writer.Options.Indented)
        {
            Span<byte> nl = System.Text.Encoding.UTF8.GetBytes(Environment.NewLine);
            indentOffset = writer.CurrentDepth * 4 + nl.Length;
            indent = new byte[indentOffset + sb.Capacity];
            indent.Fill((byte)' ');
            nl.CopyTo(indent[..nl.Length]);
        }
        for (uint y = 0; y < Height; ++y)
        {
            var row = GetRow(y);
            sb.Clear();
            sb.Append('"');
            for (int x = 0; x < row.Length; ++x)
            {
                sb.Append(row[x] switch
                {
                    Direction.None => "🟨",
                    Direction.Up => "⏫",
                    Direction.Right => "⏩",
                    Direction.Down => "⏬",
                    Direction.Left => "⏪",
                    _ => throw new NotSupportedException($"{row[x]} is not supported"),
                });
            }
            sb.Append('"');
            var text = sb.ToString();
            Span<byte> bytes = System.Text.Encoding.UTF8.GetBytes(text);
            if (writer.Options.Indented)
            {
                bytes.CopyTo(indent[indentOffset..]);
                writer.WriteRawValue(indent[..(indentOffset + bytes.Length)], false);
            }
            else writer.WriteRawValue(bytes, false);

        }
        writer.WriteEndArray();
    }
}
