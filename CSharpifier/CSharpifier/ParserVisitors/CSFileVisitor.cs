using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharpifier
{

    public class CSFileVisitor : CPPCXParserBaseVisitor<CSFileNode>
    {
        public CSFileNode File
        {
            get
            {
                return _file;
            }
        }

        public CSFileVisitor()
        {
            this._file = new CSFileNode();
        }

        public override CSFileNode VisitNamespaceDefinition([NotNull] CPPCXParser.NamespaceDefinitionContext context)
        {
            CSNamespaceVisitor visitor = new CSNamespaceVisitor();
            var ns = visitor.Namespace;
            ns.Name = context.qualifiednamespacespecifier().GetText();

            visitor.Visit(context.namespaceBody);
            _file.Children.Add(ns);

            return _file;
        }

        private CSFileNode _file;
    }



}


