using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonConfig;
using System.Collections;
using Microsoft.CSharp.RuntimeBinder;

namespace JsonConfig.Extensions
{
    public static class JsonConfigExtensions
    {
        public static T GetMappedValue<T>(this IEnumerable<dynamic> list, string key, T defaultValue = default(T))
        {
            T result = defaultValue;
            try
            {
                var match = list.SingleOrDefault(item => item.Key == key);
                if (match != null)
                {
                    try
                    {
                        result = match.Value;
                    }
                    catch (RuntimeBinderException ex)
                    {
                        //Occurs if the value is not directly convertible to the default type. Attempt the IConvertible method of casting instead.
                        result = Convert.ChangeType(match.Value, typeof(T));    
                    }
                    
                }
            }
            catch (Exception)
            {
                //TODO: Fix this.
                throw;
            }

            return result;
        }
    }
}
