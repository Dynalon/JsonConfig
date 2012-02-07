// THIS FILE IS LICENSED UNDER CC-BY-SA 2.5
// oroginating from: http://stackoverflow.com/questions/4580397/json-formatter-in-c

using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

namespace JsonConfig.Tests
{
	class JsonHelper
	{
	    private const string INDENT_STRING = "    ";
	    public static string Format(string str)
	    {
	        var indent = 0;
	        var quoted = false;
	        var sb = new StringBuilder();
	        for (var i = 0; i < str.Length; i++)
	        {
	            var ch = str[i];
	            switch (ch)
	            {
	                case '{':
	                case '[':
	                    sb.Append(ch);
	                    if (!quoted)
	                    {
	                        sb.AppendLine();
	                        Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
	                    }
	                    break;
	                case '}':
	                case ']':
	                    if (!quoted)
	                    {
	                        sb.AppendLine();
	                        Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
	                    }
	                    sb.Append(ch);
	                    break;
	                case '"':
	                    sb.Append(ch);
	                    bool escaped = false;
	                    var index = i;
	                    while (index > 0 && str[--index] == '\\')
	                        escaped = !escaped;
	                    if (!escaped)
	                        quoted = !quoted;
	                    break;
	                case ',':
	                    sb.Append(ch);
	                    if (!quoted)
	                    {
	                        sb.AppendLine();
	                        Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
	                    }
	                    break;
	                case ':':
	                    sb.Append(ch);
	                    if (!quoted)
	                        sb.Append(" ");
	                    break;
	                default:
	                    if (quoted)
	                        sb.Append(ch);
	                    break;
	            }
	        }
	        return sb.ToString(); ;
	    }
	}
	
	static class Extensions
	{
	    public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
	    {
	        foreach (var i in ie)
	        {
	            action(i);
	        }
	    }
	}
}
