
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

            CSFuncBodyInterpreter.Translate(_typer, node);

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
                rettype = CPPCX2CSTypeMaps.Convert(rettype);
                rettype = CPPCX2CSTokenMaps.Convert(rettype);
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
                name = CPPCX2CSTokenMaps.Convert(name);
                _typer.LineAppendsTerm(name);
            }
        }

        private void HandleFunctionParameters(List<ParsedToken> tokens)
        {
            _typer.LineAppendsLeftParen();
            foreach(var token in tokens)
            {
                var destToken = CPPCX2CSTokenMaps.Map(token.Name);
                if(!string.IsNullOrEmpty(destToken))
                {
                    _typer.LineAppendsTerm(CPPCX2CSTokenMaps.Map(token.Name));
                }
            }
            _typer.LineAppendsRightParen();
        }

        #endregion // string format helpers

        #region private members


        private CSharpTyper _typer;
        #endregion // private members
    }

}


