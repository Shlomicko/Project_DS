using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop_DS.Structure
{
    internal class HeightData : IComparable<HeightData>
    {

        public int Height { get; set; }
        public int Count { get; set; } = 1;
        public QueueNode<DataQueue> QueueNode { get; set; }
        public Node<WidthData> Base { get; set; }
        public DateTime InsertionDate { get; set; }
        public HeightData(int data)
        {
            Height = data;
            InsertionDate = DateTime.Now;
        }

        public int CompareTo(HeightData other) => Height.CompareTo(other.Height);        
    }
}
