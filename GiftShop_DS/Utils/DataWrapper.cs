﻿using GiftShop_DS.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop_DS.Utils
{
    internal class DataWrapper<T> where T : IComparable<T>
    {
        TreeWithSubTrees<T> tree;
        public DataWrapper()
        {
            tree = new TreeWithSubTrees<T>();
        }

        public bool GetItemOfSize(T @base, T height)
        {
            if (tree.TryGetNode<BaseNode<T>>(@base, out INode<T> node))
            {
                if (((BaseNode<T>)node).NodeTree.TryGetNode<HeightNode<T>>(height, out INode<T> innerNode, true))
                {

                    return true;
                }
            }
            return false;
        }

        public void AddItemOfSize(T @base, T height)
        {
            if (tree.TryGetNode<BaseNode<T>>(@base, out INode<T> node))
            {
                ((BaseNode<T>)node).NodeTree.Insert(height);
            }
        }

        public int CountItemOffSize(T @base, T height)
        {
            if (tree.TryGetNode<BaseNode<T>>(@base, out INode<T> node))
            {
                if (((BaseNode<T>)node).NodeTree.TryGetNode<HeightNode<T>>(height, out INode<T> innerNode))
                {
                    return ((BaseNode<T>)innerNode).NodeTree.Count;
                }
            }
            return 0;
        }

        public bool RemoveBase(T @base)
        {
            if (tree.TryGetNode<BaseNode<T>>(@base, out INode<T> node))
            {
                tree.RemoveNode(ref node);
                return true;
            }
            return false;
        }
    }
}
