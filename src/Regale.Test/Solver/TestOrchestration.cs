using Regale.Solver;

namespace Regale.Test.Solver;

#pragma warning disable NUnit2007, NUnit2005, NUnit2003, NUnit2019, NUnit2017

public sealed class TestOrchestration
{
    [Test]
    public void TestFinishedOrchestration()
    {
        var map = new Map(3, 3);
        map.Init(new[]
        {
            new[]{ Field.None, Field.None, Field.None },
            new[]{ Field.None, Field.None, Field.None },
            new[]{ Field.None, Field.None, Field.None },
        });
        var problem = new Problem(map, new Position[]
        {
            new(2, 1),
        });
        var orchester = new Orchestration<DummyCost, AlwaysRightRouting>(problem);
        var moves = orchester.IterateStep();
        Assert.IsNull(moves);
    }

    [Test]
    public void TestBasicOrchestration()
    {
        var map = new Map(3, 3);
        map.Init(new[]
        {
            new[]{ Field.None,    Field.None, Field.None },
            new[]{ Field.Present, Field.None, Field.None },
            new[]{ Field.None,    Field.None, Field.None },
        });
        var problem = new Problem(map, new Position[]
        {
            new(2, 1),
        });
        var orchester = new Orchestration<DummyCost, AlwaysRightRouting>(problem);
        var moves = orchester.IterateStep();
        Assert.IsNotNull(moves);
        Assert.AreEqual(Direction.None,  moves![0, 0]);
        Assert.AreEqual(Direction.None,  moves![1, 0]);
        Assert.AreEqual(Direction.None,  moves![2, 0]);
        Assert.AreEqual(Direction.Right, moves![0, 1]);
        Assert.AreEqual(Direction.None,  moves![1, 1]);
        Assert.AreEqual(Direction.None,  moves![2, 1]);
        Assert.AreEqual(Direction.None,  moves![0, 2]);
        Assert.AreEqual(Direction.None,  moves![1, 2]);
        Assert.AreEqual(Direction.None,  moves![2, 2]);
    }

    [Test]
    public void TestBasicOrchestrationCleanup()
    {
        var map = new Map(3, 3);
        map.Init(new[]
        {
            new[]{ Field.None,    Field.None, Field.None },
            new[]{ Field.Present, Field.None, Field.None },
            new[]{ Field.None,    Field.None, Field.None },
        });
        var problem = new Problem(map, new Position[]
        {
            new(2, 1),
        });
        var orchester = new Orchestration<DummyCost, AlwaysRightRouting>(problem);
        var moves = orchester.IterateStep();
        Assert.IsNotNull(moves);
        Assert.AreEqual(Direction.None,  moves![0, 0]);
        Assert.AreEqual(Direction.None,  moves![1, 0]);
        Assert.AreEqual(Direction.None,  moves![2, 0]);
        Assert.AreEqual(Direction.Right, moves![0, 1]);
        Assert.AreEqual(Direction.None,  moves![1, 1]);
        Assert.AreEqual(Direction.None,  moves![2, 1]);
        Assert.AreEqual(Direction.None,  moves![0, 2]);
        Assert.AreEqual(Direction.None,  moves![1, 2]);
        Assert.AreEqual(Direction.None,  moves![2, 2]);

        moves = orchester.IterateStep();
        Assert.IsNotNull(moves);
        Assert.AreEqual(Direction.None,  moves![0, 0]);
        Assert.AreEqual(Direction.None,  moves![1, 0]);
        Assert.AreEqual(Direction.None,  moves![2, 0]);
        Assert.AreEqual(Direction.None,  moves![0, 1]);
        Assert.AreEqual(Direction.Right,  moves![1, 1]);
        Assert.AreEqual(Direction.None,  moves![2, 1]);
        Assert.AreEqual(Direction.None,  moves![0, 2]);
        Assert.AreEqual(Direction.None,  moves![1, 2]);
        Assert.AreEqual(Direction.None,  moves![2, 2]);

        moves = orchester.IterateStep();
        Assert.IsNull(moves);
    }
}
