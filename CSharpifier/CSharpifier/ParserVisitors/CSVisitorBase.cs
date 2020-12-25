using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharpifier
{
    public abstract class CSVisitorBase<ResultT> : CPPCXParserBaseVisitor<ResultT>
    {
        public CSVisitorBase(ICharStream inputStream, ITokenStream tokenStream)
        {
            InputStream = inputStream;
            TokenStream = tokenStream;
        }

        protected ICharStream InputStream;
        protected ITokenStream TokenStream;
    }
}
