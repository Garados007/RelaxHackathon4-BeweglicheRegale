using OneOf;
using OneOf.Types;

namespace Regale.Solver.Routing;

public class PresentSpaceRouting : IRouting
{
    public List<(Position position, Direction direction)> GetMoves(
        RoutingArgs args
    )
    {
        var neighbourLv1 = GetPreferredNeighbours(args, args.Present);
        var list = new List<(Position position, Direction direction)>();
        // check if there is a close
        var closeResult = AddCloseSpaces(args, neighbourLv1, list);
        // get a new position for lv2 neighbour detection. If present wasn't moved use the
        // current position
        var presentPosition = closeResult.Match(
            x => x,
            _ => args.Present
        );
        // get new set of neighbours
        var neighbourLv2 = GetPreferredNeighbours(args, presentPosition);
        // move some spaces to the new neighbours
        var prevLocked = args.SpaceUsed[args.Present];
        args.SpaceUsed[args.Present] = true;
        MoveSpacesToNeighbour(args, neighbourLv2, presentPosition, list);
        args.SpaceUsed[args.Present] = prevLocked || list.Count > 0;
        // return the modification list
        return list;
    }

    private static void MoveSpacesToNeighbour(
        RoutingArgs args,
        HashSet<Position> positions,
        Position presentLocation,
        List<(Position position, Direction direction)> list
    )
    {
        while (positions.Count > 0)
        {
            ((Position space, Position intermediate, int steps) data, Position neighbour, (int, int) cost)? best = null;
            foreach (var position in positions)
            {
                var field = args.Map[position];
                // only move packages to this position
                if (field != Field.Package)
                    continue;
                // get optimal space that can be moved to this neighbour
                if (!GetOptimalSpaceToMove(args, position, presentLocation).TryPickT0(out var optimum, out _))
                    continue;
                var cost = (optimum.steps, Functions.ManhattanMetric(optimum.space, optimum.intermediate));
                if (best is null || best.Value.cost.CompareTo(cost) > 0)
                    best = (optimum, position, cost);
            }
            if (best is null)
                return; // cannot find a good movement for any neighbour
            // add space to movement list
            list.Add((best.Value.data.intermediate, (best.Value.data.space - best.Value.data.intermediate).GetDirection()));
            ReserveSpaceMovement(args.SpaceUsed, best.Value.data.space, best.Value.data.intermediate);
            // remove neighbour as finished
            positions.Remove(best.Value.neighbour);
        }
    }

    private static void ReserveSpaceMovement(SpaceUseMap useMap, Position start, Position end)
    {
        if (start.X == end.X)
        {
            var (y1, y2) = (start.Y, end.Y);
            if (y1 > y2)
                (y1, y2) = (y2, y1);
            for (int y = y1; y <= y2; y++)
                useMap[start.X, y] = true;
        }
        else if (start.Y == end.Y)
        {
            var (x1, x2) = (start.X, end.X);
            if (x1 > x2)
                (x1, x2) = (x2, x1);
            for (int x = x1; x <= x2; x++)
                useMap[x, start.Y] = true;
        }
        else throw new InvalidOperationException("Cannot a one dimensional line of a two dimensional region was selected");
    }

    private static OneOf<(Position space, Position intermediate, int steps), NotFound> GetOptimalSpaceToMove(
        RoutingArgs args,
        Position target,
        Position newPresentLocation
    )
    {
        ((Position space, Position intermediate, int steps) value, (int, int) cost)? optimum = null;
        foreach (var space in args.Spaces)
        {
            if (!CheckIfSpaceCanBeMoved(args, space, target, newPresentLocation).TryPickT0(out var result, out _))
                continue;
            if (args.Map[result.intermediate] == Field.None)
                continue; // do not move spaces!
            var cost = (result.steps, Functions.ManhattanMetric(space, target));
            if (optimum is null || optimum.Value.cost.CompareTo(cost) > 0)
                optimum = ((space, result.intermediate, result.steps), cost);
        }
        if (optimum is null)
            return new NotFound();
        else return optimum.Value.value;
    }

    /// <summary>
    /// Checks if the space can be moved closer to the target position.
    /// </summary>
    /// <param name="args">some routing arguments with data</param>
    /// <param name="space">the position of the space</param>
    /// <param name="target">the position where it should be moved to</param>
    /// <returns>
    /// returns the intermediate position or target position of the space and the number of steps
    /// that is needed to move the space to the target. If steps is 1 the resulting position is
    /// equal to the target position. If this space cannot be moved to the target (because it's
    /// blocked) it returns just NotFound.
    /// </returns>
    private static OneOf<(Position intermediate, int steps), NotFound> CheckIfSpaceCanBeMoved(
        RoutingArgs args,
        Position space,
        Position target,
        Position newPresentLocation
    )
    {
        // one dimensional move
        if (space.X == target.X)
        {
            // check for blockage
            if (!CheckBlockageY(args, space.X, space.Y, target.Y))
                return (target, target.IsInLine(newPresentLocation, space) ? 0 : 1);
            else return new NotFound();
        }
        if (space.Y == target.Y)
        {
            // check for blockage
            if (!CheckBlockageX(args, space.Y, space.X, target.X))
                return (target, target.IsInLine(newPresentLocation, space) ? 0 : 1);
            else return new NotFound();
        }
        // two moves: move along x-axis
        (Position pos, (int rank, int) cost)? best = null;
        Position intermediate = new(target.X, space.Y);
        if (!CheckBlockageX(args, space.Y, space.X, target.X) && !newPresentLocation.IsInLine(intermediate, target))
        {
            var cost = (target.IsInLine(newPresentLocation, intermediate) ? 1 : 2, Functions.ManhattanMetric(space, intermediate));
            best = (intermediate, cost);
        }
        // two moves: move along y-axis
        intermediate = new(space.X, target.Y);
        if (!CheckBlockageY(args, space.X, space.Y, target.Y) && !newPresentLocation.IsInLine(intermediate, target))
        {
            var cost = (target.IsInLine(newPresentLocation, intermediate) ? 1 : 2, Functions.ManhattanMetric(space, intermediate));
            if (best == null || cost.CompareTo(best.Value.cost) < 0)
            {
                best = (intermediate, cost);
            }
        }
        if (best is not null)
            return (best.Value.pos, best.Value.cost.rank);
        // the move is more complicated. Lets wait
        return new NotFound();
    }

    private static bool CheckBlockageX(RoutingArgs args, int y, int x1, int x2)
    {
        if (x1 > x2)
            (x1, x2) = (x2, x1);
        for (int x = x1; x <= x2; x++)
        {
            // if (args.Map[x, y] == Field.Present || args.SpaceUsed[x, y])
            if (args.SpaceUsed[x, y])
                return true;
        }
        return false;
    }

    private static bool CheckBlockageY(RoutingArgs args, int x, int y1, int y2)
    {
        if (y1 > y2)
            (y1, y2) = (y2, y1);
        for (int y = y1; y <= y2; y++)
        {
            // if (args.Map[x, y] == Field.Present || args.SpaceUsed[x, y])
            if (args.SpaceUsed[x, y])
                return true;
        }
        return false;
    }

    private static OneOf<Position, NotFound> AddCloseSpaces(
        RoutingArgs args,
        HashSet<Position> positions,
        List<(Position position, Direction direction)> list
    )
    {
        foreach (var position in positions)
        {
            var dir = position - args.Present;
            Position latestPos = args.Present;
            Position? targetPos;
            while ((targetPos = args.Map.GetTargetPosition(latestPos, dir)) != null)
            {
                if (args.SpaceUsed[targetPos.Value])
                    break;
                latestPos = targetPos.Value;
                if (args.Map[latestPos] == Field.None)
                    break;
            }
            if (args.Map[latestPos] != Field.None)
                continue;
            // move the present to this position
            list.Add((args.Present, (position - args.Present).GetDirection()));
            ReserveSpaceMovement(args.SpaceUsed, args.Present, latestPos);
            return position;
        }
        return new NotFound();
    }

    private static HashSet<Position> GetPreferredNeighbours(
        RoutingArgs args,
        Position start
    )
    {
        return args.Map
            .GetPreferredNeighbours(start, args.Depot)
            // position shouldn't be a used space
            .Where(x => !args.SpaceUsed[x])
            // position shouldn't have a conflicting move
            .Where(x =>
            {
                var move = args.CurrentMoves[x];
                if (move == Direction.None)
                    return true;
                var intendedMove = (x - start).GetDirection();
                return intendedMove == move;
            })
            // return hashset to remove duplicates
            .ToHashSet();
    }
}
