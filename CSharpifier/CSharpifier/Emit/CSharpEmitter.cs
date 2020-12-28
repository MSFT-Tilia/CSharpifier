
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
            _typer = new CSharpTyper(OutputStream);
            OnOutputStreamChanged = (newOutput) => { _typer.OutputStream = newOutput; };
        }

        public override void OnEnterNamespace(CSNamespaceNode node)
        {
            _typer.LineAppendsTerm("namespace");
            _typer.LineAppendsTerm(node.Name);
            _typer.OutputAppendsLine();

            _typer.LineAppendsLeftBrace();
            _typer.OutputAppendsLine();

            _typer.Indent();

            base.OnEnterNamespace(node);
        }

        public override void OnExitNamespace(CSNamespaceNode node)
        {
            _typer.Outdent();

            _typer.LineAppendsRightBrace();
            _typer.OutputAppendsLine();
            _typer.OutputAppendsLine(); // empty line
        }

        public override void OnEnterClass(CSClassNode node)
        {
            _typer.OutputAppendsLine(); // empty line

            _typer.LineAppendsTerm("public class");
            _typer.LineAppendsTerm(node.Name);
            _typer.OutputAppendsLine();

            _typer.LineAppendsLeftBrace();
            _typer.OutputAppendsLine();

            _typer.Indent();

            base.OnEnterClass(node);
        }

        public override void OnExitClass(CSClassNode node)
        {
            _typer.Outdent();

            _typer.LineAppendsRightBrace();
            _typer.OutputAppendsLine();
            _typer.OutputAppendsLine(); // empty line
        }

        public override void OnEnterMethod(CSMethodNode node)
        {
            _typer.LineAppendsTerm(Utils.InterpretAccessSpecifier(node.Access));

            HandleFunctionRetType(node.RetValType);

            HandleFunctionName(node.Name);

            if(node.Parameters.Count > 0)
            {
                HandleFunctionParameters(node.Parameters);
            }
            else
            {
                _typer.LineAppendsTerm("()");
            }

            _typer.OutputAppendsLine();

            if(node.Body.Count > 0)   
            { // the method has definition
                // TODO: use regex to interpret method's body
                //OutputAppendsLine();
                HandleFunctionBody(node.Body);
            }
            else
            { // declaration only, put a default implementation
                _typer.LineAppendsLeftBrace();
                _typer.LineAppendsTerm("throw new System.NotImplementedException(); /* CSharpifier Warning */");
                _typer.LineAppendsRightBrace();
            }

            _typer.OutputAppendsLine();

            base.OnEnterMethod(node);
        }

        public override void OnExitMethod(CSMethodNode node)
        {
            _typer.OutputAppendsLine();
        }

        public override void OnEnterField(CSFieldNode node)
        {
            _typer.LineAppendsTerm(Utils.InterpretAccessSpecifier(node.Access));
            HandleFieldRetType(node.RetValType);
            HandleFieldName(node.Name);
            _typer.LineAppendsSemi();

            base.OnEnterField(node);
        }

        public override void OnExitField(CSFieldNode node)
        {
            _typer.OutputAppendsLine();
        }

        #region string format helpers

        private void HandleFunctionRetType(string rettype)
        {
            if(!string.IsNullOrEmpty(rettype))
            {
                rettype = Utils.TrimDefault(rettype);

                foreach(var pair in _typeMappingCPPCX2CS)
                {
                    rettype = rettype.Replace(pair.Key, pair.Value);
                }

                foreach(var pair in _tokenMappingCPPCX2CS)
                {
                    rettype = rettype.Replace(pair.Key, pair.Value);
                }

                _typer.LineAppendsTerm(rettype);
            }
        }

        private void HandleFunctionName(string name)
        {
            HandleFieldName(name);
        }

        private void HandleFieldRetType(string rettype)
        {
            HandleFunctionRetType(rettype);
        }

        private void HandleFieldName(string name)
        {
            name = Utils.TrimDefault(name);
            if(!string.IsNullOrEmpty(name))
            {
                foreach(var pair in _tokenMappingCPPCX2CS)
                {
                    name = name.Replace(pair.Key, pair.Value);
                }

                _typer.LineAppendsTerm(name);
            }
        }

        private void HandleFunctionParameters(List<ParsedToken> tokens)
        {
            _typer.LineAppendsLeftParen();
            foreach(var token in tokens)
            {
                var destToken = InterpretToken(token.Name);
                if(!string.IsNullOrEmpty(destToken))
                {
                    _typer.LineAppendsTerm(InterpretToken(token.Name));
                }
            }
            _typer.LineAppendsRightParen();
        }

        private void HandleFunctionBody(List<ParsedToken> tokens)
        {
            foreach(var token in tokens)
            {
                if(_typer.IsOutdentToken(token.Name))
                {
                    _typer.Outdent();
                }

                _typer.LineAppendsTerm(token.Name);

                if(_typer.IsLineFeedToken(token.Name))
                {
                    _typer.OutputAppendsLine();
                }

                if(_typer.IsIndentToken(token.Name))
                {
                    _typer.Indent();
                }
            }
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
        private static readonly Dictionary<string, string> _tokenMappingCPPCX2CS = new Dictionary<string, string>() {
            {"::", "."}, {"^", ""}
        };

        private static readonly Dictionary<string, string> _typeMappingCPPCX2CS = new Dictionary<string, string>() {
            {"concurrency::task<void>", "Task" }
        };

        private CSharpTyper _typer;
        #endregion // private members
    }

}


