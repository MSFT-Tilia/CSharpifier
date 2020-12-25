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

        public CSFileNode RootNode
        {
            get
            {
                return _resultNode;
            }
        }

        public void Parse(string filename)
        {
            ICharStream cstr = CharStreams.fromPath(filename);

            CPPCXLexer lexer = new CPPCXLexer(cstr);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            CPPCXParser parser = new CPPCXParser(tokens);

            parser.BuildParseTree = true;
            var tu = parser.translationUnit();

            CSFileVisitor fileVisitor = new CSFileVisitor(cstr, tokens);
            fileVisitor.Visit(tu);
            _resultNode = fileVisitor.File;

            string dump = Utils.DumpCSNode(_resultNode);
        }


        private CSFileNode _resultNode;
    }
}
