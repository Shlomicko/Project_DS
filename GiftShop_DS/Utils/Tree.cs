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

        private ICollection<T> _InOrderNodes = new List<T>();

        public IEnumerable<T> Inorder(INodeWithTree<T> _Root)
        {

            if (_Root != null)
            {

                Inorder(_Root.Left);
                
                _InOrderNodes.Add(_Root.Data);
                
               return Inorder(_Root.Right);

            }
            else
            {
                return _InOrderNodes;
            }

        }





    }
}
