using System;

namespace GiftShop_DS.Structure
{
    internal class BaseNode<T> : INode<T> where T : IComparable<T>
    {
        // Data is base of box
        public T Data { get; set; }
        public INode<T> Right { get; set; }
        public INode<T> Left { get; set; }

        public int Count { get; set; }

        public BaseNode(T data)
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
