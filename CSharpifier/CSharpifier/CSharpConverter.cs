using System;
using System.Collections.Generic;
using System.Text;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharpifier
{
    public class CSharpConverter : ParserListenerNoActionImpl
    {
        public CSharpConverter()
        {
            _results = new StringBuilder();
            _line = new StringBuilder();
        }

        public String Results
        {
            get
            {
                return _results.ToString();
            }
        }

        #region visitors

        // Using Directive
        public override void EnterUsingDirective([NotNull] CPPCXParser.UsingDirectiveContext context)
        {
            LineAppendTerm("using");
            LineAppendTerm(InterpretDoubleColon(context.attributeSpecifierSeq()?.GetText()));
            LineAppendTerm(InterpretDoubleColon(context.nestedNameSpecifier()?.GetText()), context.attributeSpecifierSeq() != null);
            LineAppendTerm(InterpretDoubleColon(context.namespaceName()?.GetText()), context.nestedNameSpecifier() != null);
        }

        public override void ExitUsingDirective([NotNull] CPPCXParser.UsingDirectiveContext context)
        {
            LineAppendSemi();
            ResultsAppendLine();
        }

        // Namespace Definition
        public override void EnterNamespaceDefinition([NotNull] CPPCXParser.NamespaceDefinitionContext context)
        {
            LineAppendTerm("namespace");
            LineAppendTerm(InterpretDoubleColon(context.qualifiednamespacespecifier()?.GetText()));
            ResultsAppendLine();

            LineAppendLeftBrace();
            ResultsAppendLine();
        }

        public override void ExitNamespaceDefinition([NotNull] CPPCXParser.NamespaceDefinitionContext context)
        {
            LineAppendRightBrace();
            ResultsAppendLine();
        }


        public override void EnterSimpleDeclaration([NotNull] CPPCXParser.SimpleDeclarationContext context)
        {
            LineAppendTerm(InterpretDoubleColon(context.attributeSpecifierSeq()?.GetText()));
            LineAppendTerm(InterpretDoubleColon(context.declSpecifierSeq()?.GetText()));
            LineAppendTerm(context.initDeclaratorList()?.GetText());
            ResultsAppendLine();
        }



        #endregion // visitors

        #region string format helpers

        private static string InterpretDoubleColon(string origin)
        {
            if(!string.IsNullOrEmpty(origin))
            {
                return origin.Replace("::", ".");
            }
            return null;
        }

        private void LineClear()
        {
            _line.Clear();
        }

        private void LineAppendTerm(string term, bool forceNoSpace = false)
        {
            if(!string.IsNullOrEmpty(term))
            {
                if(_line.Length > 0 && !forceNoSpace)
                {
                    _line.Append(" ");
                }

                _line.Append(term);
            }
        }

        private void LineAppendLeftBrace()
        {
            LineAppendTerm("{");
        }

        private void LineAppendRightBrace()
        {
            LineAppendTerm("}");
        }

        private void LineAppendSemi()
        {
            LineAppendTermNoSpace(";");
        }

        private void LineAppendTermNoSpace(string term)
        {
            if(!string.IsNullOrEmpty(term))
            {
                _line.Append(term);
            }
        }

        private void ResultsAppendLine()
        {
            _results.AppendLine(_line.ToString());
            LineClear();
        }

        #endregion // string format helpers

        #region private members
        private StringBuilder _line;
        private StringBuilder _results;
        #endregion // private members
    }
}
