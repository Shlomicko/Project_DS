using GiftShop_DS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {

            Tree<int> tree = new Tree<int>();

            for (int i = 10; i < 23; i++)
            {
                tree.Insert(i);
            }


        }
    }
}
