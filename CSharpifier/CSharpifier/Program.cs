using Antlr4.Runtime;
using System;
using System.IO;

namespace CSharpifier
{
    class Program
    {
        static void Main(string[] args)
        {
            TestSimpleCPP();
            //TestAppXamlCPP();
        }

        static void TestSimpleCPP()
        {
            using (FileStream fstr = new FileStream(@"..\..\..\Samples\simple.cpp", FileMode.Open))
            {
                AntlrInputStream astr = new AntlrInputStream(fstr);
                CPPCXLexer lexer = new CPPCXLexer(astr);
                CommonTokenStream tokens = new CommonTokenStream(lexer);

                CPPCXParser parser = new CPPCXParser(tokens);

                var tu = parser.translationUnit();
            }
        }

        static void TestAppXamlCPP()
        {
            using (FileStream fstr = new FileStream(@"..\..\..\Samples\App.xaml.cpp", FileMode.Open))
            {
                AntlrInputStream astr = new AntlrInputStream(fstr);
                CPPCXLexer lexer = new CPPCXLexer(astr);
                CommonTokenStream tokens = new CommonTokenStream(lexer);

                CPPCXParser parser = new CPPCXParser(tokens);

                var tu = parser.translationUnit();
            }
        }
    }
}
