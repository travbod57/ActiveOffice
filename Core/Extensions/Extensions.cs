using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class Extensions
    {
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }

        public static void RemoveRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                source.Remove(item);
            }
        }

        public static List<T> Shuffle<T>(this List<T> source)
        {
            Random random = new Random();
            return source.OrderBy<T, int>((item) => random.Next()).ToList();
        }
    }
}
