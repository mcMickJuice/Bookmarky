using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarky.Utility.Extensions
{
    public static class EnumExtensions
    {
        ////how to write extension for generics to convert them to objects easily
        //public static IEnumerable<object> ToAnonymousObjects<TEnum>(this Enum source)
        //{
        //    if (typeof(TEnum) != typeof(Enum))
        //    {
        //        throw new InvalidOperationException("Type provided is not an Enum");
        //    }

        //    Enum.GetValues(typeof(TEnum))
        //        .OfType<TEnum>()
        //        .Select(t => new
        //        {
        //            Id = (int)t;

        //        })
        //}
    }
}
