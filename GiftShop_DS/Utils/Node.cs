using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop_DS.Utils
{
    internal class Node<T> : INodeWithTree<T> where T : IComparable<T>
    {
        // Data is base of box
        public T Data { get; }
        public INodeWithTree<T> Left { get; internal set; }
        public INodeWithTree<T> Right { get; internal set; }


        public Node(T data)
        {
            Data = data;
        }
        // A tree of heights of boxes.
        public Tree<T> NodeTree
        {
            get;
            internal set;
        }
    }
}
