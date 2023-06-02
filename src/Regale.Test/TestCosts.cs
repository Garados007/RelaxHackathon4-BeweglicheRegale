using Regale.Estimation;

namespace Regale.Test;

/// <summary>
/// test all costs
/// </summary>
class TestCosts {

    private static MMCost mmCost = new MMCost();

    [Test]
    public void MMCost_NoDepot_ThrowsException () {
        var emptyMap = new Map(2,2);
        var noDepots = Array.Empty<Position>();
        var position = new Position(1,0);

        Assert.That(() => mmCost.GetCost(emptyMap, noDepots, position), Throws.Exception);
    }

    [Test]
    public void MMCost_SomeDepot_ReturnsBestWithCost () {
        var map = new Map(3,3);
        map.Init(new[]
        {
            new[]{ Field.None,    Field.None, Field.None },
            new[]{ Field.Present, Field.None, Field.None },
            new[]{ Field.None,    Field.None, Field.None },
        });
        var presentPos = new Position(0, 1);
        var closerPosition = new Position(1, 2);
        var furtherPosition = new Position(2, 0);
        ReadOnlySpan<Position> depots = new Position[] {
            closerPosition,
            furtherPosition,
        };
        var expectedCost = 2;

        var result = mmCost.GetCost(map, depots, presentPos);

        Assert.That(result.cost, Is.EqualTo(expectedCost));
        Assert.That(result.depot, Is.EqualTo(closerPosition));
    }
}
