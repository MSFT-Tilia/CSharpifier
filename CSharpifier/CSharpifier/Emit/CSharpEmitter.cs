
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSharpifier
{
    public class CSharpEmitter : CSharpEmitterBase
    {
        public CSharpEmitter()
        {
            _line = new StringBuilder();
            _lineStarted = false;
            _indentation = 0;
        }

        public override void OnEnterNamespace(StreamWriter ostream, CSNamespaceNode node)
        {
            LineAppendsTerm("namespace");
            LineAppendsTerm(node.Name);
            OutputAppendsLine(ostream);

            LineAppendsLeftBrace();
            OutputAppendsLine(ostream);

            Indent();
        }

        public override void OnExitNamespace(StreamWriter ostream, CSNamespaceNode node)
        {
            Outdent();

            LineAppendsRightBrace();
            OutputAppendsLine(ostream);
            OutputAppendsLine(ostream); // empty line
        }

        public override void OnEnterClass(StreamWriter ostream, CSClassNode node)
        {
            OutputAppendsLine(ostream); // empty line

            LineAppendsTerm("public class");
            LineAppendsTerm(node.Name);
            OutputAppendsLine(ostream);

            LineAppendsLeftBrace();
            OutputAppendsLine(ostream);

            Indent();
        }

        public override void OnExitClass(StreamWriter ostream, CSClassNode node)
        {
            Outdent();

            LineAppendsRightBrace();
            OutputAppendsLine(ostream);
            OutputAppendsLine(ostream); // empty line
        }

        public override void OnEnterMethod(StreamWriter ostream, CSMethodNode node)
        {
            LineAppendsTerm(Utils.InterpretAccessSpecifier(node.Access));
            LineAppendsTerm(node.RetValType);
            LineAppendsTerm(node.Name);

            if(node.Body.Count > 0)   
            { // the method has definition
                // TODO: use regex to interpret method's body
                //OutputAppendsLine(ostream);
                HandleFunctionBody(ostream, node.Body);
            }
            else
            { // declaration only, put a default implementation
                LineAppendsLeftBrace();
                LineAppendsTerm("throw new NotImplementedException(); /* CSharpifier Warning */");
                LineAppendsRightBrace();
            }

            OutputAppendsLine(ostream);
        }


        #region string format helpers

        private void HandleFunctionBody(StreamWriter ostream, List<ParsedToken> tokens)
        {
            foreach(var token in tokens)
            {
                if(IsOutdentToken(token.Name))
                {
                    Outdent();
                }

                LineAppendsTerm(token.Name);

                if(IsLineFeedToken(token.Name))
                {
                    OutputAppendsLine(ostream);
                }

                if(IsIndentToken(token.Name))
                {
                    Indent();
                }
            }
        }

        private void Indent()
        {
            ++_indentation;
            LineClear();
        }

        private void Outdent()
        {
            --_indentation;

            if(_indentation < 0)
            {
                // _indentation must never be negative.
                throw new NotSupportedException();
            }

            LineClear();
        }

        private void MakeIndentations()
        {
            for(int i= 0; i < _indentation; ++i)
            {
                _line.Append(_indentSymbol);
            }
        }

        private void LineClear()
        {
            _line.Clear();
            MakeIndentations();
            _lineStarted = false;
        }

        private void LineAppendsTerm(string term, bool forceNoSpace = false)
        {
            if(!string.IsNullOrEmpty(term))
            {
                if(_lineStarted && !forceNoSpace)
                {
                    _line.Append(" ");
                }
                else
                {
                    _lineStarted = true;
                }

                _line.Append(term);
            }
        }

        private void LineAppendsLeftBrace()
        {
            LineAppendsTerm("{");
        }

        private void LineAppendsRightBrace()
        {
            LineAppendsTerm("}");
        }

        private void LineAppendsSemi()
        {
            LineAppendsTermNoSpace(";");
        }

        private void LineAppendsTermNoSpace(string term)
        {
            if(!string.IsNullOrEmpty(term))
            {
                _line.Append(term);
            }
        }

        private void FlushLine(StreamWriter ostream)
        {
            if(_line.Length > 0)
            {
                OutputAppendsTerms(ostream, _line);
            }
        }

        private void OutputAppendsTerms(StreamWriter outputStream, StringBuilder terms)
        {
            outputStream.Write(terms.ToString());
            LineClear();
        }

        private void OutputAppendsLine(StreamWriter outputStream)
        {
            outputStream.WriteLine(_line.ToString());
            LineClear();
        }

        private bool IsLineFeedToken(string token)
        {
            return _lineFeedTokens.Contains(token);
        }

        private bool IsIndentToken(string token)
        {
            return _indentTokens.Contains(token);
        }

        private bool IsOutdentToken(string token)
        {
            return _outdentTokens.Contains(token);
        }

        #endregion // string format helpers

        #region private members
        // private readonly string _indentSymbol = "\t";  // indent using Tab
        private readonly string _indentSymbol = "    ";   // indent using spaces
        private readonly HashSet<string> _lineFeedTokens = new HashSet<string>() {
            ",", "{", "}", ":"
        };

        private readonly HashSet<string> _indentTokens = new HashSet<string>() {
            "{",
        };

        private readonly HashSet<string> _outdentTokens = new HashSet<string>() {
            "}",
        };

        private bool _lineStarted;
        private int _indentation;
        private StringBuilder _line;
        #endregion // private members
    }

}


