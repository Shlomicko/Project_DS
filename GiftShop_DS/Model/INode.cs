using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop_DS.Structure
{
    interface INode<T> where T : IComparable<T>
    {

        T Data { get; set; }
        INode<T> Right { get; set; }
        INode<T> Left { get; set; }
        int Count { get; set; }
    }
}

