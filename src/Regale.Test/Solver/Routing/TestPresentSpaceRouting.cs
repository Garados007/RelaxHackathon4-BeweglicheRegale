using Regale.Solver.Routing;

namespace Regale.Test.Solver.Routing;

#pragma warning disable NUnit2007, NUnit2005, NUnit2003, NUnit2019, NUnit2017

public sealed class TestPresentSpaceRouting
{
    private static List<Position> GetSpaces(Map map)
    {
        var list = new List<Position>();
        foreach (var (field, pos) in map.GetFields())
            if (field == Field.None)
                list.Add(pos);
        return list;
    }

    private static void VerifyRoute(List<(Position position, Direction direction)> solution, params (Position position, Direction direction)[] expected)
    {
        Assert.AreEqual(expected.Length, solution.Count, "verify solution length");
        for (int i = 0; i < expected.Length; ++i)
        {
            Assert.AreEqual(expected[i], solution[i], $"invalid data at {i + 1}");
        }
    }

    [Test]
    public void MovePresentOneFieldToDepot()
    {
        var map = new Map(5, 5);
        map.Fill(Field.Package);
        map[1, 3] = Field.Present;
        map[2, 3] = Field.None;
        var depot = new Position(5, 3);

        var args = new RoutingArgs(map, new MoveMap(5, 5), new Map<bool>(5, 5), new(1, 3), depot, GetSpaces(map));

        VerifyRoute(new PresentSpaceRouting().GetMoves(args),
            (new(1, 3), Direction.Right)
        );
    }

    [Test]
    public void MovePresentDiagonalOneFieldToDepot()
    {
        var map = new Map(5, 5);
        map.Fill(Field.Package);
        map[1, 3] = Field.Present;
        map[2, 3] = Field.None;
        var depot = new Position(5, 5);

        var args = new RoutingArgs(map, new MoveMap(5, 5), new Map<bool>(5, 5), new(1, 3), depot, GetSpaces(map));

        VerifyRoute(new PresentSpaceRouting().GetMoves(args),
            (new(1, 3), Direction.Right)
        );
    }

    [Test]
    public void MovePresentDiagonalOneFieldToDepotWithTwoSpaces()
    {
        var map = new Map(5, 5);
        map.Fill(Field.Package);
        map[1, 3] = Field.Present;
        map[2, 3] = Field.None;
        map[1, 4] = Field.None;
        var depot = new Position(5, 5);

        var args = new RoutingArgs(map, new MoveMap(5, 5), new Map<bool>(5, 5), new(1, 3), depot, GetSpaces(map));

        VerifyRoute(new PresentSpaceRouting().GetMoves(args),
            (new(1, 3), Direction.Down),
            (new(2, 4), Direction.Up)
        );
    }

    [Test]
    public void MovePresentOneFieldAndBringSpaceCloserX()
    {
        var map = new Map(5, 5);
        map.Fill(Field.Package);
        map[1, 3] = Field.Present;
        map[2, 3] = Field.None;
        map[0, 4] = Field.None;
        var depot = new Position(5, 5);

        var args = new RoutingArgs(map, new MoveMap(5, 5), new Map<bool>(5, 5), new(1, 3), depot, GetSpaces(map));

        VerifyRoute(new PresentSpaceRouting().GetMoves(args),
            (new(1, 3), Direction.Right),
            (new(2, 4), Direction.Left)
        );
    }

    [Test]
    public void MovePresentOneFieldAndBringSpaceCloserY()
    {
        var map = new Map(5, 5);
        map.Fill(Field.Package);
        map[1, 3] = Field.Present;
        map[2, 3] = Field.None;
        map[3, 0] = Field.None;
        var depot = new Position(5, 5);

        var args = new RoutingArgs(map, new MoveMap(5, 5), new Map<bool>(5, 5), new(1, 3), depot, GetSpaces(map));

        VerifyRoute(new PresentSpaceRouting().GetMoves(args),
            (new(1, 3), Direction.Right),
            (new(3, 3), Direction.Up)
        );
    }

    [Test]
    public void MovePresentOneFieldAndBring2SpaceCloserY()
    {
        var map = new Map(5, 5);
        map.Fill(Field.Package);
        map[1, 3] = Field.Present;
        map[2, 3] = Field.None;
        map[3, 0] = Field.None;
        map[0, 4] = Field.None;
        var depot = new Position(5, 5);

        var args = new RoutingArgs(map, new MoveMap(5, 5), new Map<bool>(5, 5), new(1, 3), depot, GetSpaces(map));

        VerifyRoute(new PresentSpaceRouting().GetMoves(args),
            (new(1, 3), Direction.Right),
            (new(2, 4), Direction.Left),
            (new(3, 3), Direction.Up)
        );
    }

    [Test]
    public void MoveOneSpaceCloser()
    {
        var map = new Map(5, 5);
        map.Fill(Field.Package);
        map[2, 2] = Field.Present;
        map[3, 0] = Field.None;
        var depot = new Position(5, 5);

        var args = new RoutingArgs(map, new MoveMap(5, 5), new Map<bool>(5, 5), new(2, 2), depot, GetSpaces(map));

        VerifyRoute(new PresentSpaceRouting().GetMoves(args),
            (new(3, 2), Direction.Up)
        );
    }

    [Test]
    public void MoveTwoSpacesCloser()
    {
        var map = new Map(5, 5);
        map.Fill(Field.Package);
        map[2, 2] = Field.Present;
        map[3, 0] = Field.None;
        map[0, 4] = Field.None;
        var depot = new Position(5, 5);

        var args = new RoutingArgs(map, new MoveMap(5, 5), new Map<bool>(5, 5), new(2, 2), depot, GetSpaces(map));

        VerifyRoute(new PresentSpaceRouting().GetMoves(args),
            (new(2, 4), Direction.Left),
            (new(3, 2), Direction.Up)
        );
    }

    [Test]
    public void DontMoveBlockedSpace()
    {
        var map = new Map(5, 5);
        map.Fill(Field.Package);
        map[1, 1] = Field.Present;
        map[4, 2] = Field.None;
        map[3, 2] = Field.Present;
        var depot = new Position(5, 5);
        var used = new Map<bool>(5, 5);
        used[4, 1] = true;
        used[3, 2] = true;

        var args = new RoutingArgs(map, new MoveMap(5, 5), used, new(1, 1), depot, GetSpaces(map));

        VerifyRoute(new PresentSpaceRouting().GetMoves(args)
        );
    }

    [Test]
    public void SpecialCase1()
    {
        var map = new Map(3, 3);
        map.Fill(Field.Package);
        map[0, 2] = Field.Present;
        map[2, 1] = Field.None;
        var depot = new Position(2, 0);
        var used = new Map<bool>(3, 3);


        var args = new RoutingArgs(map, new MoveMap(3, 3), used, new(0, 2), depot, GetSpaces(map));

        VerifyRoute(new PresentSpaceRouting().GetMoves(args),
            (new(2, 2), Direction.Up)
        );
    }

    [Test]
    public void SpecialCase2()
    {
        var map = new Map(3, 3);
        map.Fill(Field.Package);
        map[1, 1] = Field.Present;
        map[0, 2] = Field.Present;
        map[1, 2] = Field.None;
        var depot = new Position(2, 0);
        var used = new Map<bool>(3, 3);

        var args = new RoutingArgs(map, new MoveMap(3, 3), used, new(1, 1), depot, GetSpaces(map));

        VerifyRoute(new PresentSpaceRouting().GetMoves(args),
            (new(2, 2), Direction.Left)
        );
    }

    [Test]
    public void SpecialCase3()
    {
        var map = new Map(3, 3);
        map.Fill(Field.Package);
        map[0, 2] = Field.Present;
        map[2, 0] = Field.None;
        var depot = new Position(2, 0);
        var used = new Map<bool>(3, 3);

        var args = new RoutingArgs(map, new MoveMap(3, 3), used, new(0, 2), depot, GetSpaces(map));

        VerifyRoute(new PresentSpaceRouting().GetMoves(args),
            (new(0, 0), Direction.Right)
        );
    }

    [Test]
    public void SpecialCase4()
    {
        var map = new Map(3, 3);
        map.Fill(Field.Package);
        map[0, 2] = Field.Present;
        map[2, 2] = Field.None;
        var depot = new Position(2, 0);
        var used = new Map<bool>(3, 3);

        var args = new RoutingArgs(map, new MoveMap(3, 3), used, new(0, 2), depot, GetSpaces(map));

        VerifyRoute(new PresentSpaceRouting().GetMoves(args),
            (new(0, 2), Direction.Right)
        );
    }
}
