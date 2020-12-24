using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpifier
{
    public class CSUsingNode : CSNode
    {
        public override CSNodeType NodeType()
        {
            return CSNodeType.Using;
        }
    }
}
