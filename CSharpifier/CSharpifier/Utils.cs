using Antlr4.Runtime.Tree;
using System;

namespace CSharpifier
{
    public static class Utils
    {
        public static string InterpretDoubleColon(string origin)
        {
            if(!string.IsNullOrEmpty(origin))
            {
                return origin.Replace("::", ".");
            }
            return null;
        }

        public static string DeclSpecifierSeqToString(CPPCXParser.DeclSpecifierSeqContext context)
        {
            string res = string.Empty;

            foreach(var child in context.children)
            {
                if(res.Length > 0)
                {
                    res += " ";
                }
                res += child.GetText();
            }

            return res;
        }

        public static T GetContextFirstChild<T>(IParseTree children)
        {
            for(int i = 0; i < children.ChildCount; ++i)
            {
                var child = children.GetChild(i);
                if(typeof(T) == child.GetType())
                {
                    return (T)child;
                }
            }
            return default(T);
        }


        public static string DumpCSNode(CSNode root, int level = 0)
        {
            string res = string.Empty;
            string indents = LeveledIndent(level);

            res = string.Format("\n{0}Name = '{1}'\n", indents, root.Name);
            res += string.Format("{0}Children Num= '{1}'\n", indents, root.Children.Count);

            foreach (var child in root.Children)
            {
                res += DumpCSNode(child, level + 1);
            }

            return res;
        }

        public static string LeveledIndent(int level)
        {
            string res = string.Empty;
            for(int i = 0; i < level; ++i)
            {
                res += "    ";
            }
            return res;
        }

    }
}






