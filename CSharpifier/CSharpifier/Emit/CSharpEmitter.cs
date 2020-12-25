
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

            HandleFunctionRetType(ostream, node.RetValType);

            LineAppendsTerm(node.Name);

            if(node.Parameters.Count > 0)
            {
                HandleFunctionParameters(ostream, node.Parameters);
            }
            else
            {
                LineAppendsTerm("()");
            }

            OutputAppendsLine(ostream);

            if(node.Body.Count > 0)   
            { // the method has definition
                // TODO: use regex to interpret method's body
                //OutputAppendsLine(ostream);
                HandleFunctionBody(ostream, node.Body);
            }
            else
            { // declaration only, put a default implementation
                LineAppendsLeftBrace();
                LineAppendsTerm("throw new System.NotImplementedException(); /* CSharpifier Warning */");
                LineAppendsRightBrace();
            }

            OutputAppendsLine(ostream);
        }

        public override void OnExitMethod(StreamWriter ostream, CSMethodNode node)
        {
            OutputAppendsLine(ostream);
        }


        #region string format helpers

        private void HandleFunctionRetType(StreamWriter ostream, string rettype)
        {
            if(rettype != null)
            {
                foreach(var pair in _typeMappingCPPCX2CS)
                {
                    rettype = rettype.Replace(pair.Key, pair.Value);
                }

                foreach(var pair in _tokenMappingCPPCX2CS)
                {
                    rettype = rettype.Replace(pair.Key, pair.Value);
                }

                LineAppendsTerm(rettype);
            }
        }

        private void HandleFunctionParameters(StreamWriter ostream, List<ParsedToken> tokens)
        {
            LineAppendsLeftParen();
            foreach(var token in tokens)
            {
                var destToken = InterpretToken(token.Name);
                if(!string.IsNullOrEmpty(destToken))
                {
                    LineAppendsTerm(InterpretToken(token.Name));
                }
            }
            LineAppendsRightParen();
        }

        private void HandleFunctionBody(StreamWriter ostream, List<ParsedToken> tokens)
        {
            LineAppendsLeftBrace();

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

            LineAppendsRightBrace();
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

        private void LineAppendsLeftParen()
        {
            LineAppendsTerm("(");
        }

        private void LineAppendsRightParen()
        {
            LineAppendsTerm(")");
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

        private static bool IsLineFeedToken(string token)
        {
            return _lineFeedTokens.Contains(token);
        }

        private static bool IsIndentToken(string token)
        {
            return _indentTokens.Contains(token);
        }

        private static bool IsOutdentToken(string token)
        {
            return _outdentTokens.Contains(token);
        }

        private static string InterpretToken(string original)
        {
            string dest;
            if(_tokenMappingCPPCX2CS.TryGetValue(original, out dest))
            {
                return dest;
            }
            else
            {
                return original;
            }
        }

        #endregion // string format helpers

        #region private members
        // private readonly string _indentSymbol = "\t";  // indent using Tab
        private static readonly string _indentSymbol = "    ";   // indent using spaces
        private static readonly HashSet<string> _lineFeedTokens = new HashSet<string>() {
            ",", "{", "}", ":"
        };

        private static readonly HashSet<string> _indentTokens = new HashSet<string>() {
            "{",
        };

        private static readonly HashSet<string> _outdentTokens = new HashSet<string>() {
            "}",
        };

        private static readonly Dictionary<string, string> _tokenMappingCPPCX2CS = new Dictionary<string, string>() {
            {"::", "."}, {"^", ""}
        };

        private static readonly Dictionary<string, string> _typeMappingCPPCX2CS = new Dictionary<string, string>() {
            {"concurrency::task<void>", "Task" }
        };

        private bool _lineStarted;
        private int _indentation;
        private StringBuilder _line;
        #endregion // private members
    }

}


