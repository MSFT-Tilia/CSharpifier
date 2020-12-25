using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;

namespace CSharpifier
{
    public class CSClassVisitor : CSVisitorBase<CSClassNode>
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

        public CSClassVisitor(ICharStream inputStream, ITokenStream tokenStream)
            : base(inputStream, tokenStream)
        {
            _class = new CSClassNode();
            _currentAccess = AccessSpecifier.Private;
        }

        public override CSClassNode VisitClassSpecifier([NotNull] CPPCXParser.ClassSpecifierContext context)
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
            _class.Children.Add(node);

            return _class;
        }

        public override CSClassNode VisitMemberdeclaration([NotNull] CPPCXParser.MemberdeclarationContext context)
        {
            if(context.ChildCount > 0)
            {
                var child = context.children[0];

                if(child.GetType() == typeof(CPPCXParser.FunctionDefinitionContext))
                { // function definition
                    var functx = child as CPPCXParser.FunctionDefinitionContext;
                    var node = new CSMethodNode();

                    node.Name = functx.declarator().GetText();
                    node.Access = _currentAccess;

                    Utils.GetParserRuleText(ref node.Body, functx.functionBody(), TokenStream);

                    if(functx.declSpecifierSeq() != null)
                    {
                        node.RetValType = Utils.DeclSpecifierSeqToString(functx.declSpecifierSeq());
                    }

                    _class.Children.Add(node);
                }
                else if(child.GetType() == typeof(CPPCXParser.PropertyDefinitionContext))
                { // property definition
                    
                }
                else if(child.GetType() == typeof(CPPCXParser.AttributeSpecifierSeqContext) ||
                    child.GetType() == typeof(CPPCXParser.DeclSpecifierSeqContext) ||
                    child.GetType() == typeof(CPPCXParser.MemberDeclaratorListContext))
                { // declaration
                    var attributeSpecSeq = Utils.GetContextFirstChild<CPPCXParser.AttributeSpecifierSeqContext>(context);
                    // TODO: handle attributeSpecSeq 

                    string declSpecSeq = string.Empty;
                    var declSpecSeqCtx = Utils.GetContextFirstChild<CPPCXParser.DeclSpecifierSeqContext>(context);
                    if(declSpecSeqCtx != null)
                    {
                        declSpecSeq = Utils.DeclSpecifierSeqToString(declSpecSeqCtx);
                    }

                    var declaratorList = Utils.GetContextFirstChild<CPPCXParser.MemberDeclaratorListContext>(context);
                    if(declaratorList != null)
                    {
                        foreach(var declchild in declaratorList.children)
                        {
                            var decl = declchild as CPPCXParser.MemberDeclaratorContext;
                            string name = decl.GetText();

                            if (name.Contains("(") && name.Contains(")"))
                            {
                                var node = new CSMethodNode();
                                node.Name = name;
                                node.RetValType = declSpecSeq;
                                node.Access = _currentAccess;
                                _class.Children.Add(node);
                            }
                        }
                    }


                }
            }

            return base.VisitMemberdeclaration(context);
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
