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
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime DateAdded { get; set; }
        public int Count { get; set; } = 1;

        public override string ToString() => $"Package, {Width}x{Height}, Quantity:{Count}, Date added:{DateAdded}";
    }
}
