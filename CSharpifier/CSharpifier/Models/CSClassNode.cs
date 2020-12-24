namespace CSharpifier
{
    public enum AccessSpecifier
    {
        Public,
        Internal,
        Protected,
        Private,
    }

    public class CSClassNode : CSNode
    {
        public CSClassNode()
        {
        }

        public override CSNodeType NodeType()
        {
            return CSNodeType.Class;
        }

    }
}



