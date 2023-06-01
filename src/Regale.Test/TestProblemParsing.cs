using OneOf;
using OneOf.Types;

namespace Regale.Test;

#pragma warning disable NUnit2007, NUnit2005, NUnit2003

public class TestProblemParsing
{
    private static OneOf<Problem, Error<string>> ParseWrapper(string code)
    {
        using var m = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(code));
        return Problem.Parse(m);
    }

    [Test]
    public void TestEmptyFile()
    {
        Assert.That(() => ParseWrapper(""), Throws.Exception);
    }

    [Test]
    public void TestInvalidLengths()
    {
        var result = ParseWrapper("""
        [
            "📦📦",
            "📦"
        ]
        """);
        Assert.IsTrue(result.IsT1);
    }

    [Test]
    public void TestSimpleParsingOfAllPossibleEntities()
    {
        var result = ParseWrapper("""
        [
            "📦🎁🟨🙋👷"
        ]
        """);
        Assert.IsTrue(result.IsT0);
        var problem = result.AsT0!;
        Assert.AreEqual(5, problem.Map.Width);
        Assert.AreEqual(1, problem.Map.Height);
        Assert.AreEqual(Field.Package, problem.Map[0, 0]);
        Assert.AreEqual(Field.Present, problem.Map[1, 0]);
        Assert.AreEqual(Field.None, problem.Map[2, 0]);
        Assert.AreEqual(Field.None, problem.Map[3, 0]);
        Assert.AreEqual(Field.Package, problem.Map[4, 0]);
        Assert.AreEqual(2, problem.Depots.Length);
        Assert.AreEqual(new Position(3, 0), problem.Depots.Span[0]);
        Assert.AreEqual(new Position(4, 0), problem.Depots.Span[1]);
    }

}
