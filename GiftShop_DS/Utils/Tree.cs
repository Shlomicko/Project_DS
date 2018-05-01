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
        private ICollection<T> _InOrderNodes = new List<T>();


        public void Insert(T data)
        {
            if (_Root == null)
            {
                _Root = new Node<T>(data);
            }
            else
            {
                Insert(data, ref _Root);
            }
        }

        private void Insert(T newData, ref INodeWithTree<T> node)
        {
            if (node == null)
            {
                node = new Node<T>(newData);
            }

            if (node.Data.CompareTo(newData) > 0)
            {
                Insert(newData, ref node.Left);
            }
            else if (node.Data.CompareTo(newData) < 0)
            {
                Insert(newData, ref node);
            }

        }

        public IEnumerable<T> Inorder()
        {
            return Inorder(_Root);
        }

        private IEnumerable<T> Inorder(INodeWithTree<T> node)
        {
            if (node == null)
            {
                return null;
            }
            else
            {

                Inorder(node.Left);

                _InOrderNodes.Add(node.Data);

                Inorder(node.Right);

            }
            return _InOrderNodes;
        }
    }
}
