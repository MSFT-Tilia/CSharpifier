namespace CSharpifier
{
    public class CSMethodNode : CSNode
    {
        public override CSNodeType NodeType()
        {
            return CSNodeType.Method;
        }

        public string RetValType;
        public bool IsVirtual;
        public AccessSpecifier Access;
        public string BodyCPPCX;
    }
}





