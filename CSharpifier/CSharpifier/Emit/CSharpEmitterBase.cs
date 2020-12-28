using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CSharpifier
{
    public abstract class CSharpEmitterBase : CSNodeVisitorBase
    {
        public CSharpEmitterBase()
        {}

        public StreamWriter OutputStream
        {
            get
            {
                return _outputStream;
            }

            set
            {
                _outputStream = value;
                OnOutputStreamChanged?.Invoke(_outputStream);
            }
        }

        public Action<StreamWriter> OnOutputStreamChanged;


        // File
        public override void OnEnterFile(CSFileNode node){ base.OnEnterFile(node); }
        public override void OnExitFile(CSFileNode node){ base.OnExitFile(node); }

        // Namespace
        public override void OnEnterNamespace(CSNamespaceNode node){ base.OnEnterNamespace(node); }
        public override void OnExitNamespace(CSNamespaceNode node){ base.OnExitNamespace(node); }

        // Using
        public override void OnEnterUsing(CSUsingNode node){ base.OnEnterUsing(node); }
        public override void OnExitUsing(CSUsingNode node){ base.OnExitUsing(node); }

        // Class(Struct)
        public override void OnEnterClass(CSClassNode node){ base.OnEnterClass(node); }
        public override void OnExitClass(CSClassNode node){ base.OnExitClass(node); }

        // Method
        public override void OnEnterMethod(CSMethodNode node){ base.OnEnterMethod(node); }
        public override void OnExitMethod(CSMethodNode node) { base.OnExitMethod(node); }

        // Field
        public override void OnEnterField(CSFieldNode node){ base.OnEnterField(node); }
        public override void OnExitField(CSFieldNode node){ base.OnExitField(node); }

        private StreamWriter _outputStream;
    }
}
