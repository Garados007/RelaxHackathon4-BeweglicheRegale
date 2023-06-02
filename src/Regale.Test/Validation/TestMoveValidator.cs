using Regale.Validation;

namespace Regale.Test.Validation;

#pragma warning disable NUnit2007, NUnit2005, NUnit2003, NUnit2002

public class TestMoveValidator
{
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
        for (uint y = 0; y < 2; ++y)
            for (uint x = 0; x < 5; ++x)
                Assert.AreEqual(Direction.None, validator.MoveMap[x, y]);
        Assert.AreEqual(Direction.None,  validator.MoveMap[0, 2]);
        Assert.AreEqual(Direction.Right, validator.MoveMap[1, 2]);
        Assert.AreEqual(Direction.Right, validator.MoveMap[2, 2]);
        Assert.AreEqual(Direction.Right, validator.MoveMap[3, 2]);
        Assert.AreEqual(Direction.None,  validator.MoveMap[4, 2]);
        for (uint y = 3; y < 4; ++y)
            for (uint x = 0; x < 5; ++x)
                Assert.AreEqual(Direction.None, validator.MoveMap[x, y]);
    }
}
