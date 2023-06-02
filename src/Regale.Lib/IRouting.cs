namespace Regale;

public interface IRouting
{
    /// <summary>
    /// Creates a list of routes that be used for the next move map. The caller has to decide
    /// which of the returned moves are valid and can be used. If a move in a direction has
    /// another item there it will retry the current direction at this location.
    /// </summary>
    /// <param name="args">The arguments for this function.</param>
    /// <returns>
    /// Some moves for the current present. If no moves are possible just return <see cref="Span.Empty" />.
    /// </returns>
    List<(Position position, Direction direction)> GetMoves(
        RoutingArgs args
    );
}
