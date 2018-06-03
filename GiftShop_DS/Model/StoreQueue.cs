using System;

namespace GiftShop_DS.Structure
{
    internal class StoreQueue
    {
        private QueueNode<DataQueue> _first, _last;
        public int Size { get; private set; }

        public StoreQueue()
        {
            _first = _last = null;
        }


        public void Enqueue(HeightData data)
        {
            /*var qNode = new QueueNode<DataQueue>(new DataQueue(data))
            {
                Next = _first
            };

            if (_first == null)
            {
                _first = _last = qNode;
                _first.Previous = null;
            }
            else
            {
                _first = qNode;
                _last.Next = qNode;
                qNode.Previous = _last;
                _last = qNode;
            }
            _first = qNode;*/

            var dq = new DataQueue(data);
            
            var newFirst = new QueueNode<DataQueue>(dq)
            {
                Next = null,                    
            };
            dq.Data.QueueNode = newFirst;
            if (_first != null)
            {
                newFirst.Next = _first;
                _first.Previous = newFirst;
            }

            _first = newFirst;
            if (_last == null)
            {
                _last = _first;
            }
            Size++;
        }

        public DataQueue Dequeue()
        {
            /*var val = _last.Data;
            _last = _last.Previous;

            if (_first == null)
            {
                return null;
            }
            QueueNode<DataQueue> tmp = _first.Next;
            _first.Next = tmp.Next;
            tmp.Next.Previous = _first;*/

            var oldFirst = _first;
            _first = _first.Next;

            if (_first == null)
            {
                _last = null;
            }
            else
            {
                _first.Previous = null;
            }


            Size--;
            return oldFirst.Data;
        }

        public QueueNode<DataQueue> Remove(QueueNode<DataQueue> node)
        {
            if (node.Previous != null)
            {
                node.Previous.Next = node.Next;
            }
            return node;
        }

        public QueueNode<DataQueue> Peek()
        {
            return _first;
        }

        public bool IsEmpty()
        {
            return _first == null;
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
        public DataQueue(HeightData data)
        {
            Data = data;            
            BaseNode = Data.Base;
        }
        public TreeWithSubTrees<HeightData> ParentTree
        {
            get
            {
                return BaseNode.Data.HeightTree;
            }

        }

    }
}
