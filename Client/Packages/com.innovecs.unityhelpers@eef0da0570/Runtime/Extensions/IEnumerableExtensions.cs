using System.Collections.Generic;
using System.Linq;

namespace Innovecs.UnityHelpers
{
    public static class IEnumerableExtensions
    {
        public static TValue Random<TValue>(this IEnumerable<TValue> enumerable)
        {
            return enumerable.ElementAtOrDefault(UnityEngine.Random.Range(0, enumerable.Count()));
        }
    }
}