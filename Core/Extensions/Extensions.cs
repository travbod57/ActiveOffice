using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class Extensions
    {
        public static void AddRange<T>(this ICollection<T> source, IList<T> items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }
    }
}
