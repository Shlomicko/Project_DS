using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop_DS.Structure
{
    class Node<T> where T :IComparable<T>
    {
        public T Data;
        public Node<T> Right;
        public Node<T> Left;

        public Node(T data)
        {
            Data = data;
        }

    }
}
