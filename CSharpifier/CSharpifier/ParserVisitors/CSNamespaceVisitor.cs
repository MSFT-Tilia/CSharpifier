using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharpifier
{
    public class CSNamespaceVisitor : CSVisitorBase<CSNamespaceNode>
    {
        public CSNamespaceNode Namespace
        {
            get
            {
                return _ns;
            }
        }

        public CSNamespaceVisitor(ICharStream inputStream, ITokenStream tokenStream)
            : base(inputStream, tokenStream)
        {
            _ns = new CSNamespaceNode();
        }

        public override CSNamespaceNode VisitNamespaceDefinition([NotNull] CPPCXParser.NamespaceDefinitionContext context)
        {
            CSNamespaceVisitor visitor = new CSNamespaceVisitor(InputStream, TokenStream);
            var node = visitor.Namespace;
            node.Name = context.qualifiednamespacespecifier().GetText();

            visitor.Visit(context.namespaceBody);
            _ns.Children.Add(node);

            return _ns;
        }

        public override CSNamespaceNode VisitClassSpecifier([NotNull] CPPCXParser.ClassSpecifierContext context)
        {
            CSClassVisitor visitor = new CSClassVisitor(InputStream, TokenStream);
            var node = visitor.Class;

            // class name
            node.Name = context.classHead().classHeadName().GetText();

            // access
            if(context.classHead().classKey().GetText().Contains("class"))
            {
                visitor.CurrentAccess = AccessSpecifier.Private;
            }
            else
            {
                visitor.CurrentAccess = AccessSpecifier.Public;
            }

            // visit 
            visitor.Visit(context.memberSpecification());

            // add children
            _ns.Children.Add(node);

            return _ns;
        }

        private CSNamespaceNode _ns;

    }
}
