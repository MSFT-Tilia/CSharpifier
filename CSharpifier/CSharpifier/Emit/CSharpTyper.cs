using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSharpifier
{
    public class CSharpTyper
    {
        public CSharpTyper(StreamWriter outputStream)
        {
            _output = outputStream;
            _line = new StringBuilder();
            _lineStarted = false;
            _prevTokenWasDot = false;
        }

        public StreamWriter OutputStream
        {
            get
            {
                return _output;
            }

            set
            {
                _output = value;
            }
        }


        public  void Indent()
        {
            ++_indentation;
            LineClear();
        }

        public  void Outdent()
        {
            --_indentation;

            if(_indentation < 0)
            {
                // _indentation must never be negative.
                throw new NotSupportedException();
            }

            LineClear();
        }

        public  void MakeIndentations()
        {
            for(int i= 0; i < _indentation; ++i)
            {
                _line.Append(_indentSymbol);
            }
        }

        public  void LineClear()
        {
            _line.Clear();
            MakeIndentations();
            _lineStarted = false;
        }

        public  void LineAppendsTerm(string term, bool forceNoSpace = false)
        {
            if(!string.IsNullOrEmpty(term))
            {
                if(_lineStarted && !forceNoSpace)
                {
                    if(!_prevTokenWasDot && term != ".")
                    {
                        _line.Append(" ");
                    }
                    else if(_prevTokenWasDot && term != ".")
                    {
                        _prevTokenWasDot = false;
                    }
                    else if(term == ".")
                    {
                        _prevTokenWasDot = true;
                    }
                }
                else
                {
                    _lineStarted = true;
                }

                _line.Append(term);
            }
        }

        public  void LineAppendsLeftParen()
        {
            LineAppendsTerm("(");
        }

        public  void LineAppendsRightParen()
        {
            LineAppendsTerm(")");
        }

        public  void LineAppendsLeftBrace()
        {
            LineAppendsTerm("{");
        }

        public  void LineAppendsRightBrace()
        {
            LineAppendsTerm("}");
        }

        public  void LineAppendsSemi()
        {
            LineAppendsTermNoSpace(";");
        }

        public  void LineAppendsTermNoSpace(string term)
        {
            if(!string.IsNullOrEmpty(term))
            {
                _line.Append(term);
            }
        }

        public  void OutputAppendsTerms(StringBuilder terms)
        {
            _output.Write(terms.ToString());
            LineClear();
        }

        public  void OutputAppendsLine()
        {
            _output.WriteLine(_line.ToString());
            LineClear();
        }

        public bool IsLineFeedToken(string token)
        {
            return _lineFeedTokens.Contains(token);
        }

        public bool IsIndentToken(string token)
        {
            return _indentTokens.Contains(token);
        }

        public bool IsOutdentToken(string token)
        {
            return _outdentTokens.Contains(token);
        }

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

        private bool _lineStarted;
        private bool _prevTokenWasDot;
        private int _indentation;
        private StringBuilder _line;
        private StreamWriter _output;
    }
}
