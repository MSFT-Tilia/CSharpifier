using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CSharpifier
{
    public struct ParsedToken
    {
        public string Name;
        public Type ParentContextType;
    }

    public static class Utils
    {
        public static void GetParserRuleText(ref List<ParsedToken> res, IParseTree node, ITokenStream tokenStream)
        {
            if(node.ChildCount == 0)
            {
                res.Add(new ParsedToken()
                {
                    Name = tokenStream.GetText(node.SourceInterval),
                    ParentContextType = node.Parent?.GetType()
                });
            }

            for(int i = 0; i < node.ChildCount; ++i)
            {
                var child = node.GetChild(i);
                GetParserRuleText(ref res, child, tokenStream);
            }
        }

        public static string GetParserRuleText(IParseTree root, ITokenStream tokenStream)
        {
            string res = string.Empty;
            GetParserRuleText(ref res, root, tokenStream);
            return res;
        }

        public static void GetParserRuleText(ref string res, IParseTree node, ITokenStream tokenStream)
        {
            if(node.ChildCount == 0)
            {
                if(res.Length > 0)
                {
                    res += " ";
                }
                res += tokenStream.GetText(node.SourceInterval);
            }
            
            for(int i = 0; i < node.ChildCount; ++i)
            {
                var child = node.GetChild(i);
                GetParserRuleText(ref res, child, tokenStream);
            }
        }


        public static string TrimDefaultCombo(string src)
        {
            return TrimAroundDot(TrimAroundDoubleColon(TrimAroundGreaterAndLess(TrimLeftComma(src))));
        }

        public static string TrimAroundDot(string src)
        {
            return src.Replace(" .", ".").Replace(". ", ".");
        }

        public static string TrimAroundDoubleColon(string src)
        {
            return src.Replace(" ::", "::").Replace(":: ", "::");
        }

        public static string TrimAroundGreaterAndLess(string src)
        {
            return src.Replace(" >", ">").Replace("> ", ">")
                .Replace(" <", "<").Replace("< ", "<");
        }

        public static string TrimLeftComma(string src)
        {
            return src.Replace(" ,", ",");
        }

        public static string InterpretAccessSpecifier(AccessSpecifier acc)
        {
            switch(acc)
            {
                case AccessSpecifier.Public:
                    return "public";
                case AccessSpecifier.Internal:
                    return "internal";
                case AccessSpecifier.Protected:
                    return "protected";
                case AccessSpecifier.Private:
                    return "private";

                default:
                    throw new NotSupportedException();
            }
        }

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

        public static T GetContextFirstChild<T>(IParseTree node)
        {
            for(int i = 0; i < node.ChildCount; ++i)
            {
                var child = node.GetChild(i);
                if(typeof(T) == child.GetType())
                {
                    return (T)child;
                }
            }
            return default(T);
        }

        public static T GetContextFirstChildOffspring<T>(IParseTree node)
        {
            if(node.GetType() == typeof(T))
            {
                return (T)node;
            }

            for(int i = 0; i < node.ChildCount; ++i)
            {
                var child = node.GetChild(i);
                var found = GetContextFirstChildOffspring<T>(child);
                if(found != null)
                {
                    return found;
                }
            }
            return default(T);
        }

        public static void GetContextChildrenOffspring<T>(ref List<T> results, IParseTree node)
        {
            if(node.GetType() == typeof(T))
            {
                results.Add((T)node);
            }

            for(int i = 0; i < node.ChildCount; ++i)
            {
                var child = node.GetChild(i);
                GetContextChildrenOffspring<T>(ref results, child);
            }
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






