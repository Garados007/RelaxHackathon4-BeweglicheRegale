using Regale;

namespace Regale;

public static class Functions {

    /// <summary>
    /// computes the distance of <paramref name="pos1"/> and <paramref name="pos2"/> in manhattan metric
    /// </summary>
    /// <remarks>
    /// does not check whether the positions are valid
    /// </remarks>
    public static int ManhattanMetric(Position pos1, Position pos2) {
        var diff = pos1 - pos2;
        return Math.Abs(diff.X) + Math.Abs(diff.Y);
    }
}

