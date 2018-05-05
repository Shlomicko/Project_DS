using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop_DS.Model
{
    internal class Node<T> where T : IComparable<T>
    {
        // Data is base of box
        public T Data;
        public Node<T> Left;
        public Node<T> Right;
        public int Count { get; internal set; }

        public Node(T data)
        {
            Data = data;
            NodeTree.BaseNode = this;
        }
        // A tree of heights of boxes.
        public TreeWithSubTrees<T> NodeTree
        {
            get;
            internal set;
        } = new TreeWithSubTrees<T>();
    }
}
