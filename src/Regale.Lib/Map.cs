namespace Regale;

public class Map : Map<Field>
{
    public Map(int width, int height) : base(width, height)
    {
    }

    /// <summary>
    /// Extends the flow from a starting point into a direction until it reaches
    /// a space. The returned list contains all positions including the starting
    /// one but won't contain the position of the space. If the flow collides
    /// with a wall this function returns null.
    /// </summary>
    /// <param name="start">the start of the flow</param>
    /// <param name="direction">the direction to follow</param>
    /// <returns></returns>
    public List<Position>? ExtendDirection(Position start, Direction direction)
    {
        var list = new List<Position>
        {
            start
        };
        var delta = direction.GetDelta();
        if (delta == Position.Zero)
            return list;

        Position? target = start;
        while ((target = GetTargetPosition(target.Value, delta)) != null)
        {
            if (this[target.Value] == Field.None)
                return list;
            list.Add(target.Value);
        }
        // collision with a wall
        return null;
    }
}
