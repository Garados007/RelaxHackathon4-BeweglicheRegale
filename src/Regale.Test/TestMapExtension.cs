namespace Regale.Test;

#pragma warning disable NUnit2007, NUnit2005, NUnit2003, NUnit2019, NUnit2017

public class TestMapExtension
{
    [Test]
    public void TestNoMovement()
    {
        var map = new Map(3, 3);
        var list = map.ExtendDirection(new(1, 1), Direction.None);
        Assert.IsNotNull(list);
        Assert.AreEqual(1, list!.Count);
        Assert.AreEqual(new Position(1, 1), list[0]);
    }

    [Test]
    public void TestExtension()
    {
        var map = new Map(3, 3);
        map[0, 1] = Field.Package;
        map[1, 1] = Field.Package;
        var list = map.ExtendDirection(new(0, 1), Direction.Right);
        Assert.IsNotNull(list);
        Assert.AreEqual(2, list!.Count);
        Assert.AreEqual(new Position(0, 1), list[0]);
        Assert.AreEqual(new Position(1, 1), list[1]);
    }

    [Test]
    public void TestCollision()
    {
        var map = new Map(3, 3);
        map[0, 1] = Field.Package;
        map[1, 1] = Field.Package;
        map[2, 1] = Field.Package;
        var list = map.ExtendDirection(new(0, 1), Direction.Right);
        Assert.IsNull(list);
    }
}
