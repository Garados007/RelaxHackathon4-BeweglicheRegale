namespace Regale;

/// <summary>
/// The Arguments for <see cref="IRouting.GetMoves(RoutingArgs)" />.
/// </summary>
/// <param name="map">the current map with the current state. This map shouldn't be changed.</param>
/// <param name="CurrentMoves">the current registered moves. This map shouldn't be changed.</param>
/// <param name="SpaceUsed">
/// A map that marks spaces that are already used from other presents. This can and should be changed.
/// </param>
/// <param name="Present">The position of the present that should be moved.</param>
/// <param name="Depot">The target of the present.</param>
public sealed record class RoutingArgs(
    Map Map,
    MoveMap CurrentMoves,
    SpaceUseMap SpaceUsed,
    Position Present,
    Position Depot
);
