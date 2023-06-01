using System.Text;
using System.Text.Json;
using OneOf;
using OneOf.Types;

namespace Regale;

public class Problem
{
    public Map Map { get; }

    public ReadOnlyMemory<Position> Depots { get; }

    public Problem(Map map, ReadOnlyMemory<Position> depots)
    {
        Map = map;
        Depots = depots;
    }

    public static OneOf<Problem, Error<string>> Parse(Stream stream)
    {
        var doc = JsonDocument.Parse(stream, new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
        });
        if (doc.RootElement.ValueKind != JsonValueKind.Array)
            return new Error<string>("expected an object");
        var lines = new List<ReadOnlyMemory<Rune>>();
        foreach (var json in doc.RootElement.EnumerateArray())
        {
            if (json.ValueKind != JsonValueKind.String)
                return new Error<string>("expect a string for a line");
            lines.Add(json.GetString()!.EnumerateRunes().ToArray()); // is always a string at this point
        }

        if (lines.Count == 0)
            return new Error<string>("expect at least one line");
        var width = lines[0].Length;
        for (int i = 1; i < lines.Count; ++i)
            if (lines[i].Length != width)
                return new Error<string>($"line {i + 1} has a different length than the first line");

        var map = new Map(width, lines.Count);
        var depots = new List<Position>();
        for (int y = 0; y < lines.Count; ++y)
            for (int x = 0; x < lines[y].Length; ++x)
            {
                switch (lines[y].Span[x].Value)
                {
                    case 128230: // 📦
                        map[(uint)x, (uint)y] = Field.Package;
                        break;
                    case 127873: // 🎁
                        map[(uint)x, (uint)y] = Field.Present;
                        break;
                    case 129000: // 🟨
                        break;
                    case 128587: // 🙋
                        depots.Add(new((uint)x, (uint)y));
                        break;
                    case 128119: // 👷
                        depots.Add(new((uint)x, (uint)y));
                        map[(uint)x, (uint)y] = Field.Package;
                        break;
                    default:
                        return new Error<string>($"invalid character '{lines[y].Span[x]}' ({lines[y].Span[x].Value}) at {x+1}:{y+1}");
                }
            }

        return new Problem(map, depots.ToArray());
    }
}
