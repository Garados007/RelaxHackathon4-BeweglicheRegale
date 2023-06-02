namespace Regale.Test;

#pragma warning disable NUnit2007, NUnit2005, NUnit2003, NUnit2019, NUnit2017, NUnit2002

public sealed class TestPosition
{
    private static void TestInLine(Position pos, Position a, Position b)
    {
        Assert.IsTrue(pos.IsInLine(a, b), $"{pos} in line between {a} and {b}");
    }

    private static void TestNotInLine(Position pos, Position a, Position b)
    {
        Assert.IsFalse(pos.IsInLine(a, b), $"{pos} not in line between {a} and {b}");
    }


    [Test]
    public void TestIsInLine()
    {
        TestInLine(new(1, 1), new(0, 1), new(2, 1));
        TestInLine(new(0, 1), new(0, 1), new(2, 1));
        TestInLine(new(2, 1), new(0, 1), new(2, 1));
        TestNotInLine(new(-1, 1), new(0, 1), new(2, 1));
        TestNotInLine(new(3, 1), new(0, 1), new(2, 1));

        TestInLine(new(1, 1), new(1, 0), new(1, 2));
        TestInLine(new(1, 0), new(1, 0), new(1, 2));
        TestInLine(new(1, 2), new(1, 0), new(1, 2));
        TestNotInLine(new(1, -1), new(1, 0), new(1, 2));
        TestNotInLine(new(1, 3), new(1, 0), new(1, 2));

        TestNotInLine(new(1, 2), new(0, 1), new(2, 1));
        TestNotInLine(new(2, 1), new(1, 0), new(1, 2));
        TestNotInLine(new(1, 1), new(0, 0), new(2, 2));
    }
}
