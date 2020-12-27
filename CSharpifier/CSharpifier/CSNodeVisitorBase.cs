using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CSharpifier
{
    public class CSNodeVisitorBase
    {
        public void Visit(CSNode node)
        {
            OnEnterNode(node);
            OnExitNode(node);
        }

        public void VisitChildren(CSNode node)
        {
            foreach(var child in node.Children)
            {
                Visit(child);
            }
        }

        // general events
        public virtual void OnEnterNode(CSNode node)
        {
            switch(node.NodeType())
            {
                case CSNodeType.File:
                    OnEnterFile(node as CSFileNode);
                    break;
                case CSNodeType.Namespace:
                    OnEnterNamespace(node as CSNamespaceNode);
                    break;
                case CSNodeType.Using:
                    OnEnterUsing(node as CSUsingNode);
                    break;
                case CSNodeType.Class:
                    OnEnterClass(node as CSClassNode);
                    break;
                case CSNodeType.Method:
                    OnEnterMethod(node as CSMethodNode);
                    break;
                case CSNodeType.Field:
                    OnEnterField(node as CSFieldNode);
                    break;

                default:
                    // some types don't have emitter to take care of.
                    Debug.Assert(false);
                    break;
            }
        }

        public virtual void OnExitNode(CSNode node)
        {
            switch(node.NodeType())
            {
                case CSNodeType.File:
                    OnExitFile(node as CSFileNode);
                    break;
                case CSNodeType.Namespace:
                    OnExitNamespace(node as CSNamespaceNode);
                    break;
                case CSNodeType.Using:
                    OnExitUsing(node as CSUsingNode);
                    break;
                case CSNodeType.Class:
                    OnExitClass(node as CSClassNode);
                    break;
                case CSNodeType.Method:
                    OnExitMethod(node as CSMethodNode);
                    break;
                case CSNodeType.Field:
                    OnExitField(node as CSFieldNode);
                    break;

                default:
                    // some types don't have emitter to take care of.
                    Debug.Assert(false);
                    break;
            }
        }

        // File
        public virtual void OnEnterFile(CSFileNode node){ VisitChildren(node); }
        public virtual void OnExitFile(CSFileNode node){}

        // Namespace
        public virtual void OnEnterNamespace(CSNamespaceNode node){ VisitChildren(node); }
        public virtual void OnExitNamespace(CSNamespaceNode node){}

        // Using
        public virtual void OnEnterUsing(CSUsingNode node){ VisitChildren(node); }
        public virtual void OnExitUsing(CSUsingNode node){}

        // Class(Struct)
        public virtual void OnEnterClass(CSClassNode node){ VisitChildren(node); }
        public virtual void OnExitClass(CSClassNode node){}

        // Method
        public virtual void OnEnterMethod(CSMethodNode node){ VisitChildren(node); }
        public virtual void OnExitMethod(CSMethodNode node){}

        // Field
        public virtual void OnEnterField(CSFieldNode node){ VisitChildren(node); }
        public virtual void OnExitField(CSFieldNode node){}
        
    }
}
