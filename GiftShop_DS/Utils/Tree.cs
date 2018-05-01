using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop_DS.Utils
{
    public class Tree<T> where T : IComparable<T>
    {

        private INodeWithTree<T> _Root;

        public void Insert(T data)
        {
            if (_Root == null)
            {
                _Root = new Node<T>(data);
            }
            else
            {
                Insert(data, _Root);
            }
        }

        private void Insert(T newData, INodeWithTree<T> node)
        {
            if (node == null)
            {
                node = new Node<T>(newData);
            }

            if (node.Data.CompareTo(newData) > 0)
            {
                Insert(newData, node.Left);
            }
            else if (node.Data.CompareTo(newData) < -0)
            {
                Insert(newData, node);
            }

        }
    }
}
