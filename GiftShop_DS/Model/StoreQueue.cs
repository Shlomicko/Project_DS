using GiftShop_DS.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop_DS.Model
{
    class StoreQueue<T>/* where T : IComparable<T>*/
    {        
        private QueueNode<T> Head, Tail, Prev;
        public StoreQueue()
        {
            Head = Tail = null;
        }
        public void Enque(T node)
        {
            var qNode = new QueueNode<T>(node);
            if (Prev == null)
            {
                Head = Tail = qNode;
            }
            else if (Head == Tail)
            {
                Tail.Previous = qNode;
                Head = Tail.Previous;
            }
            else
            {
                Head.Previous = qNode;
                Head = Head.Previous;
            }
        }
        public T Deque()
        {
            var val = Tail.Data;
            Tail = Tail.Previous;
            return val;
        }



        private class QueueNode<TNode> /*where TNode : IComparable<TNode>*/
        {
            public QueueNode<TNode> Previous;
            public QueueNode<TNode> Next;
            public TNode Data;
            public QueueNode(TNode data)
            {
                Data = data;
            }
        }
    }
}
