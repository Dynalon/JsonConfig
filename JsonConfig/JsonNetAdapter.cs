using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace JsonConfig
{
    public static class JsonNetAdapter
    {
        public static ExpandoObject Transform(ExpandoObject data)
        {
            var newExpando = new ExpandoObject();

            var edict = (IDictionary<string, object>)newExpando;

            foreach (var kvp in data)
            {
                edict[kvp.Key] = TransformByType(kvp.Value);
            }

            return newExpando;
        }

        private static object TransformByType(object value)
        {
            if (value is ExpandoObject)
            {
                return Transform((ExpandoObject)value);
            }

            if (value is List<object>)
            {
                return ConvertList((List<object>)value);
            }

            return value;
        }

        private static object ConvertList(List<object> list)
        {
            var hasSingleType = true;

            ArrayList tList = new ArrayList(list.Count);

            Type listType = null;

            if (list.Count > 0)
            {
                listType = list.First().GetType();
            }

            foreach (var v in list)
            {
                hasSingleType = hasSingleType && listType == v.GetType();
                tList.Add(TransformByType(v));
            }

            return tList.ToArray(hasSingleType && listType != null ? listType : typeof(object));
        }
    }
}
