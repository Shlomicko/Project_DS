﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop_DS.Structure
{
    internal class TreeWithSubTrees<T> where T : IComparable<T>
    {

        private INode<T> _root;
        private ICollection<T> _inOrderNodes = new List<T>();
        public readonly bool ShouldNodeTreeKeepCount;
        public BaseNode<T> BaseNode { get; set; }

        public TreeWithSubTrees(bool shouldNodeTreeKeepCount)
        {
            ShouldNodeTreeKeepCount = shouldNodeTreeKeepCount;
        }

        public TreeWithSubTrees() : this(true)
        {

        }

        /*public void InsertRef(T data)
        {
            if (_root == null)
            {
                _root = new BaseNode<T>(data);
            }
            else
            {
                InsertRef(data, ref _root);
            }
        }

        private void InsertRef(T newData, ref INode<T> node)
        {
            if (node == null)
            {
                node = new BaseNode<T>(newData);
            }

            if (node.Data.CompareTo(newData) > 0)
            {
                InsertRef(newData, ref node.Left);
            }
            else if (node.Data.CompareTo(newData) < 0)
            {
                InsertRef(newData, ref node.Right);
            }
            else
            {
                if (ShouldNodeTreeKeepCount)
                {
                    node.Count++;
                }
            }
        }

        public void Insert(T data)
        {
            if (_root == null)
            {
                _root = new BaseNode<T>(data);
            }
            else
            {
                Insert(data, _root);
            }
        }

        private INode<T> Insert(T newData, INode<T> node)
        {
            if (node == null)
            {
                node = new BaseNode<T>(newData);
            }

            if (node.Data.CompareTo(newData) > 0)
            {
                return Insert(newData, node.Left);
            }
            else if (node.Data.CompareTo(newData) < 0)
            {
                return Insert(newData, node.Right);
            }
            else
            {
                if (ShouldNodeTreeKeepCount)
                {
                    node.Count++;                    
                }
                return node;
            }
        }*/

        public void Insert(T data)
        {
            if (_root == null)
            {
                _root = new BaseNode<T>(data);
            }
            else
            {
                Insert(data, _root);
            }
        }

        private void Insert(T data, INode<T> node)
        {

            INode<T> newNode = new BaseNode<T>(data);
            INode<T> current = node;
            INode<T> previous = current;

            while (current != null)
            {
                if (data.CompareTo(current.Data) < 0)
                {
                    previous = current;
                    current = current.Left;
                }
                else if (data.CompareTo(current.Data) > 0)
                {
                    previous = current;
                    current = current.Right;
                }
            }

            if (data.CompareTo(previous.Data) < 0)
            {
                previous.Left = newNode;
            }
            else
            {
                previous.Right = newNode;
            }
        }

        public INode<T> GetNodeParent(INode<T> childNode)
        {
            return GetNodeParent(_root, childNode);
        }
        private INode<T> GetNodeParent(INode<T> currentRoot, INode<T> childNode)
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

        private int CountLeaves(INode<T> node)
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


        public bool TryGetNode<INode>(T data, out INode<T> node, bool findClosest = false)
        {
            node = null;
            var nodeToFind = _root;
            if (nodeToFind == null)
            {
                return false;
            }

            int compareResult;
            while (nodeToFind != null && (compareResult = nodeToFind.Data.CompareTo(data)) != 0)
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

            return nodeToFind != null;
        }

        public void RemoveNode(ref INode<T> node)
        {
            if (node == null) return;


            if (node.Left == null)
            {
                node = node.Right;
            }
            else if (node.Right == null)
            {
                node = node.Left;
            }
            else
            {
                T min = GetMin(node.Right);
                node.Data = min;
                RemoveNode(ref node);
            }
        }

        public T GetMin()
        {
            return GetMin(_root);
        }


        private T GetMin(INode<T> node)
        {
            if (node == null)
            {
                return default(T);
            }

            while (node.Left != null)
            {
                node = node.Left;
            }
            return node.Data;
        }

        public IEnumerable<T> Inorder()
        {
            return Inorder(_root);
        }

        private IEnumerable<T> Inorder(INode<T> node)
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
