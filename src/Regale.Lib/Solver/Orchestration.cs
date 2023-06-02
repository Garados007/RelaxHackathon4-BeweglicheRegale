using Regale.Estimation;

namespace Regale.Solver;

public sealed class Orchestration<TCost, TRouting>
    where TCost : ICost, new()
    where TRouting : IRouting, new()
{
    private readonly TCost costFunc = new();
    private readonly TRouting routingFunc = new();

    // we use two maps to make application of movements easier
    private Map primary;
    private Map secondary;

    private readonly ReadOnlyMemory<Position> depots;

    public Orchestration(Problem problem)
    {
        depots = problem.Depots;
        primary = problem.Map;
        secondary = new Map(primary.Width, primary.Height);
    }

    public IEnumerable<MoveMap> IterateSteps()
    {
        MoveMap? map;
        while ((map = IterateStep()) is not null)
            yield return map;
    }

#if DEBUG
    private long stepCounter = 0;
    public bool printDebugInfo = false;
#endif

    public MoveMap? IterateStep()
    {
        // get presents
        var presents = GetPresents();
#if DEBUG
        stepCounter++;
        if (printDebugInfo)
        {
            if (presents.Count == 0)
                Console.WriteLine($"Orchestration: Step={stepCounter}; Sum=0; Max=0");
            else
                Console.WriteLine($"Orchestration: Step={stepCounter}; Sum={presents.Sum(x => x.cost)}; Max={presents.Max(x => x.cost)}");
        }
#endif
        if (presents.Count == 0)
            return null;

        // get movements
        var spaceUsed = new SpaceUseMap(primary.Width, primary.Height);
        var validator = new Validation.MoveValidator(primary);
        var anyValidStep = false;
        foreach (var (present, _, depot) in presents)
        {
            var moves = routingFunc.GetMoves(primary, validator.MoveMap, spaceUsed, present, depot);
            foreach (var (pos, dir) in moves)
            {
                anyValidStep |= validator.Apply(pos, dir);
            }
        }

        if (!anyValidStep)
        {
            throw new InvalidOperationException(
                "BUG: Cannot create any valid movement for the current map state"
            );
        }

        // apply movements
        validator.MoveMap.ApplyMovement(primary, secondary);
        (primary, secondary) = (secondary, primary);

        // remove presents from depots
        foreach (var depot in depots.Span)
        {
            if (primary[depot] == Field.Present)
                primary[depot] = Field.None;
        }

        // return move map. We are finished ✨
        return validator.MoveMap;
    }

    private List<(Position pos, int cost, Position depot)> GetPresents()
    {
        var list = new List<(Position pos, int cost, Position depot)>();
        for (int y = 0; y < primary.Height; ++y)
        {
            var row = primary.GetRow(y);
            for (int x = 0; x < primary.Width; ++x)
            {
                if (row[x] != Field.Present)
                    continue;
                var (cost, depot) = costFunc.GetCost(primary, depots.Span, new(x, y));
                list.Add((new(x, y), cost, depot));
            }
        }
        list.Sort(CompareCost);
        return list;
    }

    private static int CompareCost((Position pos, int cost, Position depot) a, (Position pos, int cost, Position depot) b)
    {
        var res = a.cost.CompareTo(b.cost);
        if (res != 0)
            return res;
        else return a.pos.CompareTo(b.pos);
    }
}
