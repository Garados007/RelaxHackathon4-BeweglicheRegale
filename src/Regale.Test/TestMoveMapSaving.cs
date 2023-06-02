using System.Text.Json;

namespace Regale.Test;

#pragma warning disable NUnit2007, NUnit2005, NUnit2003

public class TestMoveMapSaving
{
    private static string Store(Direction[][] dirs)
    {
        var map = new MoveMap(dirs[0].Length, dirs.Length);
        for (int y = 0; y < dirs.Length; ++y)
            for (int x = 0; x < dirs[y].Length; ++x)
                map[x, y] = dirs[y][x];
        using var m = new MemoryStream();
        using var w = new Utf8JsonWriter(m, new JsonWriterOptions
        {
            Indented = true,
        });
        map.Save(w);
        w.Flush();
        return System.Text.Encoding.UTF8.GetString(m.ToArray());
    }

    [Test]
    public void TestSimpleSaving()
    {
        Assert.AreEqual("""
            [
                "🟨⏫⏩",
                "⏪⏬🟨"
            ]
            """,
            Store(new[]
            {
                new[] { Direction.None, Direction.Up, Direction.Right },
                new[] { Direction.Left, Direction.Down, Direction.None },
            })
        );
    }
}
