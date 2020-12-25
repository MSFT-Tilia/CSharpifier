using System.Collections.Generic;

namespace CSharpifier
{
    public class CSMethodNode : CSNode
    {
        public bool HasDefinition
        {
            get
            {
                return Body.Count > 0;
            }
        }

        public CSMethodNode()
        {
            Body = new List<ParsedToken>();
            Parameters = new List<ParsedToken>();
            BaseSpecifiers = new List<ParsedToken>();
        }

        public override CSNodeType NodeType()
        {
            return CSNodeType.Method;
        }

        public string RetValType;
        public bool IsVirtual;
        public AccessSpecifier Access;
        public List<ParsedToken> Parameters;
        public List<ParsedToken> BaseSpecifiers;
        public List<ParsedToken> Body;
    }
}





