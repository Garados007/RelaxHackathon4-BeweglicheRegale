using OneOf;
using OneOf.Types;

namespace Regale.Estimation;

/// <summary>
/// Determines the cost and the position of the next depot
/// </summary>
public interface ICost
{
    /// <summary>
    /// computes the cost of a single package and the position of the used depot for the estimation
    /// </summary>
    /// <param name="map">complete map of the problem</param>
    /// <param name="depots">
    ///     positions of all depots,
    ///     must at least contain one position, will throw an exception otherwise
    /// </param>
    /// <param name="present">present whose cost should be estimated</param>
    /// <returns>
    /// ( cost of this package, position of the best depot )
    /// </returns>
    /// <exception cref="Exception">throws an exception, iff <paramref name="depots"/> is empty</exception>
    (int cost, Position depot) GetCost(Map map, ReadOnlySpan<Position> depots, Position present);
}
