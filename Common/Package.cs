using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Package
    {

        public Package()
        {

        }
        public Package(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Package(int width, int height, DateTime time) : this(width, height)
        {            
            DateAdded = time;
        }

        public Package(int width, int height, DateTime time, int quantity) : this(width, height, time)
        {
            Count = quantity; ;
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public int Count { get; set; } = 1;

        public override string ToString() => $"Package, {Width}x{Height}, Quantity:{Count}, Date added:{DateAdded}";
    }
}
