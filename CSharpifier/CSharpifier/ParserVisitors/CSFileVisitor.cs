using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using System.Diagnostics;

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

            var node = new CSMethodNode();
            // C++ doesn't have access context in this case. use Public as default.
            // the right access will get known while declaration and definition has been merged.
            node.Access = AccessSpecifier.Public;

            if (context.declSpecifierSeq() != null)
            { // try get type
                var declSpecifierList = new List<CPPCXParser.DeclSpecifierContext>();
                Utils.GetContextChildrenOffspring(ref declSpecifierList, context.declSpecifierSeq());

                switch (declSpecifierList.Count)
                {
                    case 0: // no return-value-type defined
                            // maybe a constructor or a destructor
                        break;

                    case 1:
                        node.RetValType = Utils.GetParserRuleText(declSpecifierList[0], TokenStream);
                        break;

                    case 2:
                        node.RetValType = Utils.GetParserRuleText(declSpecifierList[0], TokenStream);
                        node.ParentType = Utils.GetParserRuleText(declSpecifierList[1], TokenStream); // prefix
                        break;

                    default:
                        // what is this?
                        Debug.Assert(false);
                        break;
                }
            }

            var idExprCtx = Utils.GetContextFirstChildOffspring<CPPCXParser.IdExpressionContext>(context);
            Debug.Assert(idExprCtx != null);

            node.Name += Utils.GetParserRuleText(idExprCtx, TokenStream);

            //Utils.FetchMemberName(ref node.Name, funcdeclTokens);
            //Utils.FetchFunctionParameters(ref node.Parameters, funcdeclTokens);
            //Utils.FetchFunctionBody(ref node.Body, funcbodyTokens);

            // TODO: fetch base-specifiers

            _file.Children.Add(node);

            return _file;
        }

        public override CSFileNode VisitSimpleDeclaration([NotNull] CPPCXParser.SimpleDeclarationContext context)
        { // declaration that is outside of any namespace
            return _file;
        }

        private CSFileNode _file;
    }



}


