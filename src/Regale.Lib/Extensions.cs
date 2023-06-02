using OneOf;
using OneOf.Types;

namespace Regale;

public static class Extensions
{
    public static Position GetDelta(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => new(0, -1),
            Direction.Right => new(1, 0),
            Direction.Down => new(0, 1),
            Direction.Left => new(-1, 0),
            _ => new(0, 0),
        };
    }

    public static OneOf<TElement, None> ArgMinBy<TElement, TResult>(this IEnumerable<TElement> enumerable, Func<TElement, TResult> func)
        where TResult : IComparable<TResult>
    {
        var enumerator = enumerable.GetEnumerator();
        if (!enumerator.MoveNext()) {
            return new None();
        }

        TElement first = enumerator.Current;
        var (min, minVal) = (first, func(first));
        while (enumerator.MoveNext()) {

            var tuple = (enumerator.Current, itemVal: func(enumerator.Current));
            if (tuple.itemVal.CompareTo(minVal) < 0) {
                (min, minVal) = tuple;
            }
        }
        return min;
    }
}
