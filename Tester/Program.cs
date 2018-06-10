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
        }
    }
}
