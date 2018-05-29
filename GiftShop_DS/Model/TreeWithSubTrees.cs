using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop_DS.Structure
{
    internal class TreeWithSubTrees<T> where T : IComparable<T>
    {

        private Node<T> _root;
        private ICollection<T> _inOrderNodes = new List<T>();
       
        public TreeWithSubTrees()
        {
            
        }        
        
        public void Insert(T data)
        {
            if (_root == null)
            {
                _root = new Node<T>(data);
            }
            else
            {
                Insert(data, ref _root);
            }
        }

        private void Insert(T data, ref Node<T> node)
        {

            if (node == null)
            {
                node = new Node<T>(data);
                return;
            }
            if (node.Data.CompareTo(data) > 0)
            {
                Insert(data, ref node.Left);
            }
            else if (node.Data.CompareTo(data) < 0)
            {
                Insert(data, ref node.Right);
            }
        }

        public Node<T> GetNodeParent(Node<T> childNode)
        {
            return GetNodeParent(_root, childNode);
        }
        private Node<T> GetNodeParent(Node<T> currentRoot, Node<T> childNode)
        {
            if (childNode == _root || currentRoot == null)
            {
                return null;
            }
            else
            {
                if (currentRoot.Left == childNode || currentRoot.Left == childNode)
                    return currentRoot;
                else
                {
                    if (currentRoot.Data.CompareTo(childNode.Data) < 0)
                    {
                        return GetNodeParent(currentRoot.Right, childNode);
                    }
                    else
                    {
                        return GetNodeParent(currentRoot.Left, childNode);
                    }
                }
            }
        }
        public int Count
        {
            get
            {
                return CountLeaves(_root);
            }
        }
        private int CountLeaves(Node<T> node)
        {
            if (node == null)
            {
                return 0;
            }

            if (node.Left == null && node.Right == null)
            {
                return 1;
            }
            else
            {
                return CountLeaves(node.Left) + CountLeaves(node.Right);
            }
        }


        public bool TryGetNode(ref Node<T> node, bool findClosest = false)
        {                        
            var nodeToFind = _root;
            if (nodeToFind == null)
            {
                return false;
            }

            int compareResult;
            while (nodeToFind != null && (compareResult = nodeToFind.Data.CompareTo(node.Data)) != 0)
            {
                nodeToFind = compareResult < 0 ? nodeToFind.Left : nodeToFind.Right;
                if (findClosest)
                {
                    if (nodeToFind != null)
                    {
                        if (nodeToFind.Left.Data.CompareTo(nodeToFind.Left.Data) >= 0)
                        {
                            nodeToFind = nodeToFind.Left;
                            break;
                        }
                    }
                }
            }

            node = nodeToFind ?? node;
            
            return nodeToFind != null;
        }

        public void RemoveNode(T data)
        {
            RemoveNode(ref _root, data);
        }

        private Node<T> RemoveNode(ref Node<T> node, T data)
        {
            if (node == null)
            {
                return null;
            }
            else if (node.Data.CompareTo(data) > 0)
            {
                node.Left = RemoveNode(ref node.Left, data);
            }
            else if (node.Data.CompareTo(data) < 0)
            {
                node.Right = RemoveNode(ref node.Right, data);
            }
            else
            {
                if (node.Right == null && node.Left == null)
                {
                    node = null;
                }
                else if (node.Left == null)
                {
                    node = node.Right;
                }
                else if (node.Right == null)
                {
                    node = node.Left;
                }
                else
                {
                    Node<T> temp = GetMin(node.Right);
                    node.Data = temp.Data;
                    node.Right = RemoveNode(ref node.Right, temp.Data);
                }
                
            }
            return node;
        }

        public Node<T> GetMin()
        {
            return GetMin(_root);
        }


        private Node <T> GetMin(Node<T> node)
        {
            if (node == null)
            {
                return null;
            }

            while (node.Left != null)
            {
                node = node.Left;
            }
            return node;
        }

        public IEnumerable<T> Inorder()
        {
            return Inorder(_root);
        }

        private IEnumerable<T> Inorder(Node<T> node)
        {
            if (node == null)
            {
                return null;
            }
            else
            {

                Inorder(node.Left);

                _inOrderNodes.Add(node.Data);

                Inorder(node.Right);

            }
            return _inOrderNodes;
        }
    }
}
