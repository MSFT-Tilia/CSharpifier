using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharpifier
{
    public class CSClassVisitor : CPPCXParserBaseVisitor<CSClassNode>
    {
        public CSClassNode Class
        {
            get
            {
                return _class;
            }
        }

        public AccessSpecifier CurrentAccess
        {
            get;set;
        }

        public CSClassVisitor()
        {
            _class = new CSClassNode();
            _currentAccess = AccessSpecifier.Private;
        }

        public override CSClassNode VisitClassSpecifier([NotNull] CPPCXParser.ClassSpecifierContext context)
        {
            CSClassVisitor visitor = new CSClassVisitor();
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
            _class.Children.Add(node);

            return _class;
        }

        public override CSClassNode VisitAccessSpecifier([NotNull] CPPCXParser.AccessSpecifierContext context)
        {
            switch(context.GetText())
            {
                case "public":
                    _currentAccess = AccessSpecifier.Public;
                    break;

                case "internal":
                    _currentAccess = AccessSpecifier.Internal;
                    break;

                case "protected":
                    _currentAccess = AccessSpecifier.Protected;
                    break;

                case "private":
                    _currentAccess = AccessSpecifier.Private;
                    break;
            }

            return base.VisitAccessSpecifier(context);
        }

        private AccessSpecifier _currentAccess;
        private CSClassNode _class;
    }
}
