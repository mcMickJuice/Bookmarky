using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;

namespace Bookmarky.Utility.Extensions
{
    public static class ValueInjectorExtensions
    {
        public static T MapTo<T>(this object source) where T : new()
        {
            var obj = new T();

            return (T)obj.InjectFrom(source);
        }

        public static T MapTo<T>(this object source, ConventionInjection customInjection) where T : new()
        {
            var obj = new T();

            return (T)obj.InjectFrom(customInjection, source);
        }
    }

    public class DefaultValueInjection : ConventionInjection
    {
        protected override bool Match(ConventionInfo c)
		{
            if (c.SourceProp.Name != c.TargetProp.Name)
                return false;

            if (c.SourceProp.Type.IsPrimitive || c.SourceProp.Type.Name == "String")
                return true;

            if (c.SourceProp.Type.IsEnum)
                return true;

            return false;
		}
    }
}
