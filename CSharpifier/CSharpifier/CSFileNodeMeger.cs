using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpifier
{
    public class CSFileNodeMeger
    {
        public CSFileNodeMeger()
        { }

        public CSFileNode Merge(CSFileNode headerFile, CSFileNode sourceFile)
        {
            var hvisitor = new HeaderFileLinker(sourceFile);
            hvisitor.Visit(headerFile);

            return headerFile;
        }

        class HeaderFileLinker : CSNodeVisitorBase
        {
            public HeaderFileLinker(CSFileNode souurceFileNode)
            {
                sourceFile = souurceFileNode;
            }

            public override void OnEnterMethod(CSMethodNode node)
            {
                if(node.Body.Count == 0)
                { // declaration only, seek for its body in the given source file
                    var found = CSNodeUtils.SeekForMethodDefinition(sourceFile, node.Name, node.ParentType);
                    if(found != null)
                    {
                        node.Body = found.Body;
                    }
                }

                base.OnEnterMethod(node);
            }

            public override void OnEnterField(CSFieldNode node)
            {
                base.OnEnterField(node);
            }


            private CSFileNode sourceFile;
        }
    }
}
