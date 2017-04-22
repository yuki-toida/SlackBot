using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlackBot.UI.Extensions
{
    public static class EnumerableExtensions
    {
        public static T Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid()).First();
        }
    }
}
