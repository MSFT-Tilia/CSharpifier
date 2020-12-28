using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime;
using System.Text;

namespace CSharpifier
{
    public static class CSFuncBodyInterpreter
    {
        public static void Translate(CSharpTyper typer, CSMethodNode method)
        {
            if(method.Body.Count > 0)   
            { // the method has definition
              // TODO: use regex to interpret method's body

                foreach (var token in method.Body)
                {
                    var name = CPPCX2CSTypeMaps.Convert(token.Name);
                    name = CPPCX2CSTokenMaps.Convert(name);

                    if (typer.IsOutdentToken(name))
                    {
                        typer.OutputAppendsLine();
                        typer.Outdent();
                    }

                    typer.LineAppendsTerm(name, name == ",");

                    if (typer.IsLineFeedToken(name) && name != ",")
                    {
                        typer.OutputAppendsLine();
                    }

                    if (typer.IsIndentToken(name))
                    {
                        typer.Indent();
                    }
                }
            }
            else
            { // declaration only, put a default implementation
                typer.LineAppendsLeftBrace();
                typer.LineAppendsTerm("throw new System.NotImplementedException(); /* CSharpifier Warning */");
                typer.LineAppendsRightBrace();
            }
        }

    }
}
