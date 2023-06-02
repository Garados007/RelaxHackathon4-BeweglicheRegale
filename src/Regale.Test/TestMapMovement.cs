namespace Regale.Test;

#pragma warning disable NUnit2007, NUnit2005, NUnit2003

public sealed class TestMapMovement
{
    [Test]
    public void TestCircle()
    {
        var map = new Map(2, 2);
        map.Init(new[]
        {
            new[]{ Field.Package, Field.Present },
            new[]{ Field.Present, Field.Package },
        });
        var move = new MoveMap(2, 2);
        move.Init(new[]
        {
            new[]{ Direction.Right, Direction.Down },
            new[]{ Direction.Up,    Direction.Left },
        });
        var target = new Map(2, 2);
        move.ApplyMovement(map, target);

        Assert.AreEqual(Field.Present, target[0, 0]);
        Assert.AreEqual(Field.Package, target[1, 0]);
        Assert.AreEqual(Field.Package, target[0, 1]);
        Assert.AreEqual(Field.Present, target[1, 1]);
    }

    [Test]
    public void MoveSomething()
    {
        var map = new Map(2, 2);
        map.Init(new[]
        {
            new[]{ Field.Package, Field.None    },
            new[]{ Field.Present, Field.None    },
        });
        var move = new MoveMap(2, 2);
        move.Init(new[]
        {
            new[]{ Direction.Right, Direction.None },
            new[]{ Direction.None,    Direction.None },
        });
        var target = new Map(2, 2);
        move.ApplyMovement(map, target);

        Assert.AreEqual(Field.None,    target[0, 0]);
        Assert.AreEqual(Field.Package, target[1, 0]);
        Assert.AreEqual(Field.Present, target[0, 1]);
        Assert.AreEqual(Field.None,    target[1, 1]);
    }
}
