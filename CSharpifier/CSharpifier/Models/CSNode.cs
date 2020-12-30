
using System;
using System.Collections.Generic;

namespace CSharpifier
{
    public enum CSNodeType
    {
        File,
        Namespace,
        Using,
        Class,
        Method,
        Field,
    }

    public abstract class CSNode
    {
        public string Name = string.Empty;
        public CSNode Parent = null;
        public List<CSNode> Children = new List<CSNode>();

        public virtual CSNodeType NodeType()
        {
            throw new NotImplementedException();
        }
    }

    public static class CSNodeUtils
    {
        public static CSMethodNode SeekForMethodDefinition(CSNode root, string name, string parent, bool exactMatch = false)
        {
            if(root.GetType() == typeof(CSMethodNode))
            {
                var mnode = root as CSMethodNode;
                if(mnode.Body.Count > 0 &&
                    IsNodeNameEqual(name, mnode.Name, exactMatch) &&
                    IsNodeNameEqual(parent, mnode.ParentType, exactMatch))
                {
                    return mnode;
                }
            }

            foreach(var child in root.Children)
            {
                var seek = SeekForMethodDefinition(child, name, parent, exactMatch);
                if(seek != null)
                {
                    return seek;
                }
            }

            return null;
        }

        public static string GetFullName(CSNode node)
        {
            if(node.Parent != null && node.Parent.GetType() != typeof(CSFileNode))
            {
                string parent = GetFullName(node.Parent);
                return parent + "::" + node.Name;
            }
            else
            {
                return node.Name;
            }
        }

        public static bool IsNodeNameEqual(string name1, string name2, bool exactMatch)
        {
            if (name1 == null || name2 == null)
            {
                return false;
            }

            if(exactMatch)
            {
                return name1 == name2;
            }
            else
            {
                var trimmedName1 = Utils.TrimDefault(name1.Trim(new char[] { ':', ' ' }));
                var trimmedName2 = Utils.TrimDefault(name2.Trim(new char[] { ':', ' ' }));

                return trimmedName1.EndsWith(trimmedName2) || trimmedName2.EndsWith(trimmedName1);
            }
        }
    }
}


