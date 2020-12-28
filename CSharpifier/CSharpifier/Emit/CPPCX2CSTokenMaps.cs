using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpifier
{
    public class CPPCX2CSTokenMaps
    {
        public static string Map(string original)
        {
            string found;
            if(_tokens.TryGetValue(original.Trim(), out found))
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
            foreach(var token in _tokens)
            {
                res = res.Replace(token.Key, token.Value);
            }
            return res;
        }

        private static readonly Dictionary<string, string> _tokens = new Dictionary<string, string>() {
            {"::", "."}, {"^", ""}, {"->", "."}
        };
    }
}
