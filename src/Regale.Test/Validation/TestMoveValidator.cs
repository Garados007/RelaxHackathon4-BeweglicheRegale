using Regale.Validation;

namespace Regale.Test.Validation;

#pragma warning disable NUnit2007, NUnit2005, NUnit2003, NUnit2002

public class TestMoveValidator
{
    private static void CheckEqual<T>(Map<T> expected, Map<T> actual)
        where T : struct
    {
        foreach (var (field, pos) in expected.GetFields())
        {
            Assert.AreEqual(field, actual[pos], $"At position ({pos})");
        }
    }

    [Test]
    public void TestSimpleFlows()
    {
        var map = new Map(5, 5);
        map.Init(new[]
        {
            new[] { Field.Package, Field.None,    Field.None,    Field.Package, Field.None    },
            new[] { Field.Package, Field.Package, Field.Package, Field.Package, Field.Package },
            new[] { Field.Package, Field.Package, Field.Package, Field.Package, Field.None    },
            new[] { Field.Package, Field.Package, Field.Package, Field.Package, Field.Package },
            new[] { Field.Package, Field.Package, Field.Package, Field.Package, Field.Package },
        });
        // apply move
        var validator = new MoveValidator(map);
        Assert.IsTrue(validator.Apply(new(1, 2), Direction.Right)); // normal flow
        Assert.IsFalse(validator.Apply(new(1, 4), Direction.Up)); // collides with start
        Assert.IsFalse(validator.Apply(new(2, 4), Direction.Up)); // collides in between
        Assert.IsFalse(validator.Apply(new(4, 4), Direction.Up)); // collides with target
        // verify directions
        var expected = new MoveMap(5, 5);
        expected.Init(new[]
        {
            new[] { Direction.None, Direction.None,  Direction.None,  Direction.None,  Direction.None, },
            new[] { Direction.None, Direction.None,  Direction.None,  Direction.None,  Direction.None, },
            new[] { Direction.None, Direction.Right, Direction.Right, Direction.Right, Direction.None, },
            new[] { Direction.None, Direction.None,  Direction.None,  Direction.None,  Direction.None, },
            new[] { Direction.None, Direction.None,  Direction.None,  Direction.None,  Direction.None, },
        });
        CheckEqual(expected, validator.MoveMap);
    }

    [Test]
    public void TestJunktionTermination()
    {
        var map = new Map(5, 5);
        map.Init(new[]
        {
            new[] { Field.Package, Field.Package, Field.Package, Field.Package, Field.Package },
            new[] { Field.Package, Field.Package, Field.Package, Field.Package, Field.Package },
            new[] { Field.Package, Field.None,    Field.Package, Field.Package, Field.None    },
            new[] { Field.Package, Field.Package, Field.Package, Field.Package, Field.Package },
            new[] { Field.Package, Field.Package, Field.Package, Field.Package, Field.Package },
        });
        // apply move
        var validator = new MoveValidator(map);
        Assert.IsTrue(validator.Apply(new(1, 2), Direction.Right)); // normal flow
        Assert.IsFalse(validator.Apply(new(1, 0), Direction.Down)); // collides with start
        // verify directions
        var expected = new MoveMap(5, 5);
        expected.Init(new[]
        {
            new[] { Direction.None, Direction.None,  Direction.None,  Direction.None,  Direction.None, },
            new[] { Direction.None, Direction.None,  Direction.None,  Direction.None,  Direction.None, },
            new[] { Direction.None, Direction.Right, Direction.Right, Direction.Right, Direction.None, },
            new[] { Direction.None, Direction.None,  Direction.None,  Direction.None,  Direction.None, },
            new[] { Direction.None, Direction.None,  Direction.None,  Direction.None,  Direction.None, },
        });
        CheckEqual(expected, validator.MoveMap);
    }

    [Test]
    public void TestJunktionTermination2()
    {
        var map = new Map(5, 5);
        map.Init(new[]
        {
            new[] { Field.Package, Field.Package, Field.Package, Field.Package, Field.Package },
            new[] { Field.Package, Field.Package, Field.Package, Field.Package, Field.Package },
            new[] { Field.Package, Field.None,    Field.Package, Field.Package, Field.None    },
            new[] { Field.Package, Field.Package, Field.Package, Field.Package, Field.Package },
            new[] { Field.Package, Field.Package, Field.Package, Field.Package, Field.Package },
        });
        // apply move
        var validator = new MoveValidator(map);
        Assert.IsTrue(validator.Apply(new(1, 0), Direction.Down)); // normal flow
        Assert.IsFalse(validator.Apply(new(1, 2), Direction.Right)); // collides with start
        // verify directions
        var expected = new MoveMap(5, 5);
        expected.Init(new[]
        {
            new[] { Direction.None, Direction.Down,  Direction.None,  Direction.None,  Direction.None, },
            new[] { Direction.None, Direction.Down,  Direction.None,  Direction.None,  Direction.None, },
            new[] { Direction.None, Direction.None,  Direction.None,  Direction.None,  Direction.None, },
            new[] { Direction.None, Direction.None,  Direction.None,  Direction.None,  Direction.None, },
            new[] { Direction.None, Direction.None,  Direction.None,  Direction.None,  Direction.None, },
        });
        CheckEqual(expected, validator.MoveMap);
    }
}
