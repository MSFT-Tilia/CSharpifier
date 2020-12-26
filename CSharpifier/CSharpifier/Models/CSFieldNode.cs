namespace CSharpifier
{
    public class CSFieldNode : CSNode
    {
        public override CSNodeType NodeType()
        {
            return CSNodeType.Field;
        }

        public string RetValType;
        public AccessSpecifier Access;
    }
}



