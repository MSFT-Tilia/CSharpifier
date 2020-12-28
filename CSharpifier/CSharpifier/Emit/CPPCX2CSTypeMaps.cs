using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpifier
{
    public class CPPCX2CSTypeMaps
    {
        public static string Map(string original)
        {
            string found;
            if(_types.TryGetValue(original.Trim(), out found))
            {
                return found;
            }
            else
            {
                return original.Trim();
            }
        }

        public static string Convert(string original)
        {
            string res = original;
            foreach(var t in _types)
            {
                res = res.Replace(t.Key, t.Value);
            }
            return res;
        }

        private static readonly Dictionary<string, string> _types = new Dictionary<string, string>() {
            { "auto", "var" },
            {"concurrency::task<void>", "Task" }
        };
    }
}
