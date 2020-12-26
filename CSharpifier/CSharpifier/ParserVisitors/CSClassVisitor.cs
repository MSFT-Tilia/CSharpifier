using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

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

                    List<ParsedToken> funcdeclTokens = new List<ParsedToken>();
                    Utils.GetParserRuleText(ref funcdeclTokens, functx.declarator(), TokenStream);

                    List<ParsedToken> funcbodyTokens = new List<ParsedToken>();
                    Utils.GetParserRuleText(ref funcbodyTokens, functx.functionBody(), TokenStream);
                    Debug.Assert(funcbodyTokens .Count > 0);

                    node.Access = _currentAccess;

                    var idExprCtx = Utils.GetContextFirstChildOffspring<CPPCXParser.IdExpressionContext>(functx);
                    Debug.Assert(idExprCtx != null);
                    node.Name = Utils.GetParserRuleText(idExprCtx, TokenStream);

                    FetchFunctionParameters(ref node.Parameters, funcdeclTokens);
                    FetchFunctionBody(ref node.Body, funcbodyTokens);

                    // TODO: fetch base-specifiers

                    if (functx.declSpecifierSeq() != null)
                    {
                        var declSpecifierList = new List<CPPCXParser.DeclSpecifierContext>();
                        Utils.GetContextChildrenOffspring(ref declSpecifierList, functx.declSpecifierSeq());

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

                    _class.Children.Add(node);
                }
                else if(child.GetType() == typeof(CPPCXParser.PropertyDefinitionContext))
                { // property definition
                    
                }
                else if (child.GetType() == typeof(CPPCXParser.AttributeSpecifierSeqContext) ||
                    child.GetType() == typeof(CPPCXParser.DeclSpecifierSeqContext) ||
                    child.GetType() == typeof(CPPCXParser.MemberDeclaratorListContext))
                { // declaration
                    var attributeSpecSeq = Utils.GetContextFirstChild<CPPCXParser.AttributeSpecifierSeqContext>(context);
                    // TODO: handle attributeSpecSeq 

                    var declSpecSeqCtx = Utils.GetContextFirstChild<CPPCXParser.DeclSpecifierSeqContext>(context);
                    if (declSpecSeqCtx != null)
                    {
                        var declaratorList = Utils.GetContextFirstChild<CPPCXParser.MemberDeclaratorListContext>(context);
                        if (declaratorList != null)
                        {
                            foreach (var declchild in declaratorList.children)
                            {
                                var decl = declchild as CPPCXParser.MemberDeclaratorContext;

                                var text = decl.GetText();
                                if (DoesTheTextLookLikeAFunction(text))
                                { // method
                                    List<ParsedToken> funcdeclTokens = new List<ParsedToken>();
                                    Utils.GetParserRuleText(ref funcdeclTokens, decl, TokenStream);

                                    Debug.Assert(funcdeclTokens.Count > 0);

                                    var node = new CSMethodNode();
                                    node.RetValType = Utils.GetParserRuleText(declSpecSeqCtx, TokenStream);
                                    node.Access = _currentAccess;
                                    FetchMemberName(ref node.Name, funcdeclTokens);
                                    FetchFunctionParameters(ref node.Parameters, funcdeclTokens);

                                    _class.Children.Add(node);
                                }
                                else
                                { // field.
                                    List<ParsedToken> fielddeclTokens = new List<ParsedToken>();
                                    Utils.GetParserRuleText(ref fielddeclTokens, decl, TokenStream);

                                    Debug.Assert(fielddeclTokens.Count > 0);

                                    var node = new CSFieldNode();
                                    FetchMemberName(ref node.Name, fielddeclTokens);
                                    node.RetValType = Utils.GetParserRuleText(declSpecSeqCtx, TokenStream);
                                    node.Access = _currentAccess;
                                    _class.Children.Add(node);
                                }
                            }
                        }
                        else
                        { // try to peek simple field
                            var declSpecSeq = Utils.GetParserRuleText(declSpecSeqCtx, TokenStream);
                            Debug.Assert(declSpecSeq.Length > 0);

                            if(DoesTheTextLookLikeAnSimpleFieldDecl(declSpecSeq))
                            {
                                var node = new CSFieldNode();
                                node.Name = declSpecSeq;
                                node.Access = _currentAccess;
                                _class.Children.Add(node);
                            }
                        }
                    }
                    else
                    { // constructor and destructtor will go into this path
                        // TODO: add logs here to check if we missed other types of member.
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

        public static void FetchMemberName(ref string name, List<ParsedToken> src)
        {
            Debug.Assert(src.Count > 0 || src.Count > 1);

            if(src[0].Name != "~" && src[0].Name != "^" && src[0].Name != "*")
            { // normal function name
                name = src[0].Name;
            }
            else
            { // destructor name
                Debug.Assert(src.Count > 1);
                name = src[0].Name + " " + src[1].Name;
            }
        }

        public static void FetchFunctionParameters(ref List<ParsedToken> paramList, List<ParsedToken> src)
        {
            int parenCount = 0;
            foreach(var token in src)
            {
                if (token.Name == "(")
                {
                    ++parenCount;
                }
                else if(token.Name == ")")
                {
                    --parenCount;
                    Debug.Assert(parenCount >= 0);
                }
                else if(parenCount > 0)
                {
                    paramList.Add(token);
                }
            }
        }

        public static void FetchFunctionBody(ref List<ParsedToken> body, List<ParsedToken> src)
        {
            int braceCount = 0;
            foreach(var token in src)
            {
                if (token.Name == "{")
                {
                    ++braceCount;
                }
                else if(token.Name == "}")
                {
                    --braceCount;
                    Debug.Assert(braceCount >= 0);
                }
                else if(braceCount > 0)
                {
                    body.Add(token);
                }
            }
        }

        private static bool DoesTheTextLookLikeAFunction(string text)
        {
            return _regexFunctionSignature.IsMatch(text);
        }

        private static bool DoesTheTextLookLikeAnSimpleFieldDecl(string text)
        {
            return _regexSimpleFieldDecl.IsMatch(text);
        }

        private AccessSpecifier _currentAccess;
        private CSClassNode _class;
        private static readonly Regex _regexFunctionSignature = new Regex(@"^(~|\^|\*)?\w+\(.*\)\w*$");
        private static readonly Regex _regexSimpleFieldDecl = new Regex(@"^\w+\s+\w+$");

    }
}
