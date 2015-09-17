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
                if (kvp.Value is ExpandoObject)
                {
                    edict[kvp.Key] = Transform((ExpandoObject)kvp.Value);
                }
                else if (kvp.Value is List<object>)
                {
                    var result = ConvertList((List<object>)kvp.Value);

                    if (result.GetType().GetElementType() == typeof(ExpandoObject))
                    {
                        var expandoArray = (ExpandoObject[])result;

                        for (var i = 0; i < expandoArray.Length; i++)
                        {
                            expandoArray[i] = Transform(expandoArray[i]);
                        }
                    }

                    edict[kvp.Key] = result;
                }
                else
                {
                    edict[kvp.Key] = kvp.Value;
                }
            }

            return newExpando;
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
                tList.Add(v);
            }

            return tList.ToArray(hasSingleType && listType != null ? listType : typeof(object));
        }
    }
}
