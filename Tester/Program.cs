using GiftShop_DS.Structure;
using System;
using System.Collections.Generic;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {

           TreeWithSubTrees<int> tree = new TreeWithSubTrees<int>();

            for (int i = 10; i < 23; i++)
            {
                tree.Insert(i);
            }
            //tree.Insert(10);
            //tree.Insert(21);
            //tree.Insert(43);
            //Console.WriteLine("1,2: " + 1.CompareTo(2));
            IEnumerable<int> nodes = tree.Inorder();

            foreach (var item in nodes)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}
