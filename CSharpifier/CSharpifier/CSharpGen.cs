using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CSharpifier
{
    public class CSharpGen
    {
        public void Generate(string filename, CSFileNode fileNode, CSharpEmitterBase emitter)
        {
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write))
            {
                StreamWriter sw = new StreamWriter(fs);
                Generate(sw, fileNode, emitter);
                fs.Close();
            }
        }

        public void Generate(StreamWriter ostream, CSFileNode fileNode, CSharpEmitterBase emitter)
        {
            _curTarget = ostream;
            _curEmitter = emitter;
            Travel(fileNode);
            ostream.Flush();
        }

        private void OnEnterNode(CSNode node)
        {
            switch(node.NodeType())
            {
                case CSNodeType.File:
                    _curEmitter.OnEnterFile(_curTarget, node as CSFileNode);
                    break;
                case CSNodeType.Namespace:
                    _curEmitter.OnEnterNamespace(_curTarget, node as CSNamespaceNode);
                    break;
                case CSNodeType.Using:
                    _curEmitter.OnEnterUsing(_curTarget, node as CSUsingNode);
                    break;
                case CSNodeType.Class:
                    _curEmitter.OnEnterClass(_curTarget, node as CSClassNode);
                    break;
                case CSNodeType.Method:
                    _curEmitter.OnEnterMethod(_curTarget, node as CSMethodNode);
                    break;
            }
        }

        private void OnExitNode(CSNode node)
        {
            switch(node.NodeType())
            {
                case CSNodeType.File:
                    _curEmitter.OnExitFile(_curTarget, node as CSFileNode);
                    break;
                case CSNodeType.Namespace:
                    _curEmitter.OnExitNamespace(_curTarget, node as CSNamespaceNode);
                    break;
                case CSNodeType.Using:
                    _curEmitter.OnExitUsing(_curTarget, node as CSUsingNode);
                    break;
                case CSNodeType.Class:
                    _curEmitter.OnExitClass(_curTarget, node as CSClassNode);
                    break;
                case CSNodeType.Method:
                    _curEmitter.OnExitMethod(_curTarget, node as CSMethodNode);
                    break;
            }
        }

        private void Travel(CSNode root)
        {
            OnEnterNode(root);

            foreach(var child in root.Children)
            {
                Travel(child);
            }

            OnExitNode(root);
        }

        private StreamWriter _curTarget;
        private CSharpEmitterBase _curEmitter;
    }
}
