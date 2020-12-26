using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharpifier
{

    public class CSFileVisitor : CSVisitorBase<CSFileNode>
    {
        public CSFileNode File
        {
            get
            {
                return _file;
            }
        }

        public CSFileVisitor(ICharStream inputStream, ITokenStream tokenStream)
            : base(inputStream, tokenStream)
        {
            this._file = new CSFileNode();
        }

        public override CSFileNode VisitNamespaceDefinition([NotNull] CPPCXParser.NamespaceDefinitionContext context)
        {
            CSNamespaceVisitor visitor = new CSNamespaceVisitor(InputStream, TokenStream);
            var ns = visitor.Namespace;
            ns.Parent = _file;
            ns.Name = context.qualifiednamespacespecifier().GetText();

            visitor.Visit(context.namespaceBody);
            _file.Children.Add(ns);

            return _file;
        }

        public override CSFileNode VisitFunctionDefinition([NotNull] CPPCXParser.FunctionDefinitionContext context)
        { // function definition that is outside of any namespace

            return _file;
        }

        public override CSFileNode VisitSimpleDeclaration([NotNull] CPPCXParser.SimpleDeclarationContext context)
        {
            return _file;
        }

        private CSFileNode _file;
    }



}


