using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Runtime.InteropServices.WindowsRuntime;

namespace CSharpifier
{
    public class CSMethodVisitor : CPPCXParserBaseVisitor<CSMethodNode>
    {
        public CSMethodNode Method
        {
            get
            {
                return _method;
            }
        }

        public CSMethodVisitor()
        {
            _method = new CSMethodNode();
        }

        public override CSMethodNode VisitMemberSpecification([NotNull] CPPCXParser.MemberSpecificationContext context)
        {
            VisitChildren(context);
            return _method;
        }

        public override CSMethodNode VisitMemberdeclaration([NotNull] CPPCXParser.MemberdeclarationContext context)
        {
            var c = context.GetText();
            return _method;
        }

        private CSMethodNode _method;
    }
}
