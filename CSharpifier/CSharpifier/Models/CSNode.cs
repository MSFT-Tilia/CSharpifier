
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

}


