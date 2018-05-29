using System;

namespace GiftShop_DS.Structure
{
    internal class StoreQueue<T> where T : IComparable<T>
    {
        private QueueNode<DataQueue> _head, _tail;
        public int Size { get; private set; }

        public StoreQueue()
        {
            _head = _tail = null;
        }


        public void Enqueue(HeightData data)
        {
            var qNode = new QueueNode<DataQueue>(new DataQueue(data))
            {                
                Next = null
            };
            
            if (_head == null)
            {
                _head = _tail = qNode;
                _head.Previous = null;
            }
            else
            {
                _tail.Next = qNode;
                qNode.Previous = _tail;
                _tail = qNode;
            }
            Size++;
        }

        public DataQueue Dequeue()
        {
            var val = _tail.Data;
            _tail = _tail.Previous;

            if (_head == null)
            {
                return null;
            }
            QueueNode<DataQueue> tmp = _head.Next;
            _head.Next = tmp.Next;
            tmp.Next.Previous = _head;
            Size--;
            return _head.Data;
        }

        public QueueNode<DataQueue> Remove(QueueNode<DataQueue> node)
        {
            if(node.Previous != null)
            {
                node.Previous.Next = node.Next;
            }
            return node;
        }
   
        public QueueNode<DataQueue> Peek()
        {
            return _head;
        }
    }
    internal class QueueNode<T>
    {
        public QueueNode<T> Previous;
        public QueueNode<T> Next;
        public T Data;
        public QueueNode(T data)
        {
            Data = data;
        }
    }

    internal class DataQueue
    {
        public Node<WidthData> BaseNode { get; private set; }
        public HeightData Data { get; private set; }
        public DateTime InsertionDate { get; private set; }
        public DataQueue(HeightData data)
        {
            Data = data;
            BaseNode = Data.Base;
            InsertionDate = DateTime.Now;
        }
    }
}
