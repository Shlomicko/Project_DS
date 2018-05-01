using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop_DS.Utils
{
    interface INodeWithTree<T> where T : IComparable<T>
    {
        Tree<T> NodeTree { get; }
        T Data { get; }
        INodeWithTree<T> Left { get; }
        INodeWithTree<T> Right { get; }
    }
}
