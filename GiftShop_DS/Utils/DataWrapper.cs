using GiftShop_DS.Model;
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
            if (tree.TryGetNode(@base, out Node<T> node))
            {
                if (node.NodeTree.TryGetNode(height, out Node<T> innerNode, true))
                {

                    return true;
                }
            }
            return false;
        }

        public void AddItemOfSize(T @base, T height)
        {
            if (tree.TryGetNode(@base, out Node<T> node))
            {
                node.NodeTree.Insert(height);
            }
        }

        public int CountItemOffSize(T @base, T height)
        {
            if (tree.TryGetNode(@base, out Node<T> node))
            {
                if (node.NodeTree.TryGetNode(height, out Node<T> innerNode))
                {
                    return innerNode.NodeTree.Count;
                }
            }
            return 0;
        }

        public bool RemoveBase(T @base)
        {
            if (tree.TryGetNode(@base, out Node<T> node))
            {
                tree.RemoveNode(ref node);
                return true;
            }
            return false;
        }
    }
}
