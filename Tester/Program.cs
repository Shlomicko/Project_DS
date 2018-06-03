using Common;
using GiftShop_DS.Structure;
using GiftShop_DS.Utils;
using System;
using System.Collections.Generic;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {

            IStore storageManager = StorageManager.Instance;
            for (int i = 0; i < 12; i++)
            {
                storageManager.AddPackage(4, 10);
            }
            for (int i = 10; i < 23; i++)
            {
                storageManager.AddPackage(i / 2, i);
            }
            //tree.Insert(10);
            //tree.Insert(21);
            //tree.Insert(43);
            //Console.WriteLine("1,2: " + 1.CompareTo(2));
            IEnumerable<Package> nodes = storageManager.GetPackages();
            var hCount = storageManager.CountPackages(4, 10);
            for (int i = 0; i < hCount; i++)
            {
                storageManager.IssuePackage(4, 10);
            }
            hCount = storageManager.CountPackages(4, 10);                
            var hasHeight = storageManager.IssuePackage(4, 10);
            Console.WriteLine(hCount);
            foreach (var item in nodes)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}
