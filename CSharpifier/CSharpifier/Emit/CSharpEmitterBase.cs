using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CSharpifier
{
    public abstract class CSharpEmitterBase
    {
        public virtual void OnEnterFile(StreamWriter ostream, CSFileNode node)
        { }

        public virtual void OnExitFile(StreamWriter ostream, CSFileNode node)
        { }


        public virtual void OnEnterNamespace(StreamWriter ostream, CSNamespaceNode node)
        { }

        public virtual void OnExitNamespace(StreamWriter ostream, CSNamespaceNode node)
        { }


        public virtual void OnEnterUsing(StreamWriter ostream, CSUsingNode node)
        { }

        public virtual void OnExitUsing(StreamWriter ostream, CSUsingNode node)
        { }


        public virtual void OnEnterClass(StreamWriter ostream, CSClassNode node)
        { }

        public virtual void OnExitClass(StreamWriter ostream, CSClassNode node)
        { }


        public virtual void OnEnterMethod(StreamWriter ostream, CSMethodNode node)
        { }

        public virtual void OnExitMethod(StreamWriter ostream, CSMethodNode node)
        { }
    }
}
