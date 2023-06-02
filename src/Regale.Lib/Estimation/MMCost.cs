using OneOf;
using OneOf.Types;

namespace Regale.Estimation;
public class MMCost : ICost {

    /// <inheritdoc cref="ICost"/>
    public (int cost, Position depot) GetCost(Map map, ReadOnlySpan<Position> depots, Position present) =>
        depots
            .ToArray()
            .Select((depot) => (hmDist: Functions.ManhattanMetric(present, depot), depot))
            .ArgMinBy(tuple => tuple.hmDist)
            .AsT0;
}
