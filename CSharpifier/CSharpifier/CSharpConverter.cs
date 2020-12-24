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

        public override void EnterUsingDirective([NotNull] CPPCXParser.UsingDirectiveContext context)
        {
            LineAppendTerm("using");

            LineAppendTerm(InterpretDoubleColon(context.attributeSpecifierSeq()?.GetText()));
            LineAppendTerm(InterpretDoubleColon(context.nestedNameSpecifier()?.GetText()), context.attributeSpecifierSeq() != null);
            LineAppendTerm(InterpretDoubleColon(context.namespaceName()?.GetText()), context.nestedNameSpecifier() != null);
            LineAppendSemi();

            ResultsAppendLine();
        }

        private string InterpretDoubleColon(string origin)
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

        private StringBuilder _line;
        private StringBuilder _results;
    }
}
