using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public static class EnumHelpers
    {
        public static List<T> ToEnumList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(e => e).ToList();
        }
    }
}
