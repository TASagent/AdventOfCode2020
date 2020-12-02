using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoCTools
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> GetUnorderedSubsets<T>(this IEnumerable<T> elements, int count)
        {
            if (count < 0)
            {
                throw new Exception();
            }

            if (count == 0)
            {
                return new[] { Enumerable.Empty<T>() };
            }

            return elements.SelectMany((x, i) => elements.Skip(i + 1).GetUnorderedSubsets(count - 1).Select(y => y.Prepend(x)));
        }
    }
}
