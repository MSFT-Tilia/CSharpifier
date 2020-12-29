using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.IO;

namespace CSharpifier
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestSimpleCPP();
            //TestAppXamlCPP();
            TestConverting();
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

                var tree = tu.ToStringTree();
            }
        }

        static void TestCaclXaml()
        {
            CSharpFile hfile = new CSharpFile();
            hfile.Parse(@"..\..\..\Samples\.xaml.h");

            CSharpFile cppfile = new CSharpFile();
            cppfile.Parse(@"..\..\..\Samples\App.xaml.cpp");

            CSFileNode onefile = (new CSFileNodeMeger()).Merge(hfile.RootNode, cppfile.RootNode);

            CSharpGen gen = new CSharpGen();
            gen.Generate(@"..\..\..\Samples\output\App.xaml.cs", onefile, new CSharpEmitter());

        }

        static void TestAppXamlCPP()
        {
            CSharpFile file = new CSharpFile();
            file.Parse(@"..\..\..\Samples\App.xaml.h");
            //file.Parse(@"..\..\..\Samples\App.xaml.cpp");

            CSharpGen gen = new CSharpGen();
            gen.Generate(@"..\..\..\Samples\output\App.xaml.h.cs", file.RootNode, new CSharpEmitter());
            //gen.Generate(@"..\..\..\Samples\output\App.xaml.cpp.cs", file.RootNode, new CSharpEmitter());
        }

        static void TestConverting()
        {
            CSharpFile hfile = new CSharpFile();
            hfile.Parse(@"..\..\..\Samples\App.xaml.h");

            CSharpFile cppfile = new CSharpFile();
            cppfile.Parse(@"..\..\..\Samples\App.xaml.cpp");

            CSFileNode onefile = (new CSFileNodeMeger()).Merge(hfile.RootNode, cppfile.RootNode);

            CSharpGen gen = new CSharpGen();
            gen.Generate(@"..\..\..\Samples\output\App.xaml.cs", onefile, new CSharpEmitter());
        }

    }
}
