using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharpifier
{
    class CSharpFile 
    {
        public CSharpFile()
        {
            _resultNode = new CSFileNode();
        }

        public CSFileNode Result
        {
            get
            {
                return _resultNode;
            }
        }

        public void Parse(string filename)
        {
            using (FileStream fstr = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                AntlrInputStream astr = new AntlrInputStream(fstr);
                CPPCXLexer lexer = new CPPCXLexer(astr);
                CommonTokenStream tokens = new CommonTokenStream(lexer);
                CPPCXParser parser = new CPPCXParser(tokens);

                parser.BuildParseTree = true;
                
                var tu = parser.translationUnit();

                CSFileVisitor fileVisitor = new CSFileVisitor();
                fileVisitor.Visit(tu);
                var csfile = fileVisitor.File;

                string dump = Utils.DumpCSNode(csfile);
            }
        }


        private CSFileNode _resultNode;
    }
}
