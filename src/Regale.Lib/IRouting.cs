namespace Regale;

public interface IRouting
{
    /// <summary>
    /// Creates a list of routes that be used for the next move map. The caller has to decide
    /// which of the returned moves are valid and can be used. If a move in a direction has
    /// another item there it will retry the current direction at this location.
    /// </summary>
    /// <param name="map">the current map with the current state. This map shouldn't be changed.</param>
    /// <param name="currentMoves">the current registered moves. This map shouldn't be changed.</param>
    /// <param name="spaceUsed">
    /// A map that marks spaces that are already used from other presents. This can and should be changed.
    /// </param>
    /// <param name="present">The position of the present that should be moved.</param>
    /// <param name="depot">The target of the present.</param>
    /// <returns>
    /// Some moves for the current present. If no moves are possible just return <see cref="Span.Empty" />.
    /// </returns>
    ReadOnlySpan<(Position position, Direction direction)> GetMoves(
        Map map,
        MoveMap currentMoves,
        SpaceUseMap spaceUsed,
        Position present,
        Position depot
    );
}
