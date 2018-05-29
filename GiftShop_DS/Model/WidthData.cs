using System;

namespace GiftShop_DS.Structure
{
    internal class WidthData : IComparable<WidthData>
    {
        // Data is base of box
        public int Width { get; set; }
        public TreeWithSubTrees<HeightData> HeightTree { get; private set; } = new TreeWithSubTrees<HeightData>();        
        public WidthData(int width)
        {
            Width = width;            
        }        


        public int CompareTo(WidthData other) => Width.CompareTo(other.Width);
    }
}
