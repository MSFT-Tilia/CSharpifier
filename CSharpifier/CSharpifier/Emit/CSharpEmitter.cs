
using System;
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

            if(!string.IsNullOrEmpty(node.BodyCPPCX))
            { // the method has definition
                // TODO: use regex to interpret method's body
                OutputAppendsLine(ostream);
                LineAppendsTerm(node.BodyCPPCX);
            }
            else
            { // declaration only
                LineAppendsSemi();
            }

            OutputAppendsLine(ostream);
        }


        #region string format helpers


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

        private void OutputAppendsLine(StreamWriter outputStream)
        {
            outputStream.WriteLine(_line.ToString());
            LineClear();
        }

        #endregion // string format helpers

        #region private members
        // private readonly string _indentSymbol = "\t";  // indent using Tab
        private readonly string _indentSymbol = "    ";   // indent using spaces
        private bool _lineStarted;
        private int _indentation;
        private StringBuilder _line;
        #endregion // private members
    }

}


