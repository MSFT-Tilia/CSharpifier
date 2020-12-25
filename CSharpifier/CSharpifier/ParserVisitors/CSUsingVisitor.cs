using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharpifier
{
    public class CSUsingVisitor : CSVisitorBase<CSUsingNode>
    {
        public CSUsingVisitor(ICharStream inputStream, ITokenStream tokenStream)
            : base(inputStream, tokenStream)
        {}
    }
}
