using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpifier
{
    public class CSFileNode : CSNode
    {
        public override CSNodeType NodeType()
        {
            return CSNodeType.File;
        }
    }
}
