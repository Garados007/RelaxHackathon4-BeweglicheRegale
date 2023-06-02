namespace Regale.Test;

#pragma warning disable NUnit2007, NUnit2005, NUnit2003

class TestLinqExtensions {

    [Test]
    public void ArgMinBy_EmptyEnumerator_ReturnsNone () {
        var emptyEnumerator = new List<uint>();

        var result = emptyEnumerator.ArgMinBy((_) => 0);

        Assert.IsTrue(result.IsT1);
    }

    [Test]
    public void ArgMinBy_DefaultCase_ReturnsArgumentWithMinimumValue () {
        var minTuple = (3u, -7);
        var exampleEnumerator = new List<(uint, int)>() {
            (0, 2),
            (1, -1),
            (2, 0),
            minTuple,
            (4, 5),
        };

        var result = exampleEnumerator.ArgMinBy((t) => t.Item2);

        Assert.IsTrue(result.IsT0);
        Assert.AreEqual(result.AsT0, minTuple);
    }
}
