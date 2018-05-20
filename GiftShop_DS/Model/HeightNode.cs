using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop_DS.Structure
{
    internal class HeightNode<T> : INode<T> where T : IComparable<T>
    {

        public T Data { get; set; }
        public INode<T> Right { get; set; }
        public INode<T> Left { get; set; }

        public int Count { get; set; }

        public HeightNode(T data)
        {
            Data = data;            
        }

    }
}
