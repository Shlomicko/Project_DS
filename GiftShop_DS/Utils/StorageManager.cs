using Common;
using GiftShop_DS.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop_DS.Utils
{
    internal class StorageManager : IStore
    {
        private readonly TreeWithSubTrees<WidthData> tree;
        private readonly StoreQueue<HeightData> storeQueue;
        private readonly int MinimumPackageQuantity;
        private readonly int MaximumPackageQuantity;
        public StorageManager()
        {
            tree = new TreeWithSubTrees<WidthData>();
            storeQueue = new StoreQueue<HeightData>();
            MinimumPackageQuantity = 5;
            MaximumPackageQuantity = 100;
        }

        public bool IssuePackage(int width, int height)
        {
            var widthNode = new Node<WidthData>(new WidthData(width));
            if (tree.TryGetNode(ref widthNode))
            {
                var heightNode = new Node<HeightData>(new HeightData(height));
                if (widthNode.Data.HeightTree.TryGetNode(ref heightNode, true))
                {
                    var hCount = --heightNode.Data.Count;
                    if(hCount == 0)
                    {
                        widthNode.Data.HeightTree.RemoveNode(heightNode.Data);
                        storeQueue.Remove(heightNode.Data.QueueNode);
                    }
                    if(widthNode.Data.HeightTree.Count == 0)
                    {
                        widthNode.Data.HeightTree.RemoveNode(heightNode.Data);
                        if(widthNode.Data.HeightTree.Count == 0)
                        {
                            RemovePackage(width: width);                            
                        }
                    }                    
                    return true;
                }
            }
            return false;
        }

        public void AddPackage(int width, int height, int quantity = 1)
        {
            var widthNode = new Node<WidthData>(new WidthData(width));
            var heightNode = new Node<HeightData>(new HeightData(height));
            var hData = heightNode.Data;
            if (tree.TryGetNode(ref widthNode))
            {                
                if (widthNode.Data.HeightTree.TryGetNode(node: ref heightNode))
                {
                    heightNode.Data.Count++;
                    storeQueue.Enqueue(storeQueue.Dequeue().Data);
                }
                else
                {
                    storeQueue.Enqueue(hData);
                    widthNode.Data.HeightTree.Insert(data: hData);                    
                }                
            }
            else
            {                
                hData.Base = widthNode;
                storeQueue.Enqueue(hData);
                widthNode.Data.HeightTree.Insert(hData);                
                tree.Insert(widthNode.Data);
            }            
        }

        public int CountPackages(int width, int height)
        {
            var widthNode = new Node<WidthData>(new WidthData(width));
            if (tree.TryGetNode(ref widthNode))
            {
                var heightNode = new Node<HeightData>(new HeightData(height));
                if (widthNode.Data.HeightTree.TryGetNode(ref heightNode))
                {
                    return heightNode.Data.Count;
                }
            }
            return 0;
        }

        public List<Package> GetPackages()
        {
            List<Package> packages = new List<Package>();
            var bases = tree.Inorder();
            foreach (var baseItem in bases)
            {
                var heights = baseItem.HeightTree.Inorder();
                foreach (var heightItem in heights)
                {
                    var package = new Package
                    {
                        Width = baseItem.Width,
                        Height = heightItem.Height,
                        DateAdded = heightItem.InsertionDate,
                        Count = heightItem.Count
                    };
                    packages.Add(package);
                }
            }

            return packages;
        }

        public bool RemovePackage(int width)
        {
            var widthNode = new Node<WidthData>(new WidthData(width));
            if (tree.TryGetNode(ref widthNode))
            {
                tree.RemoveNode(data: widthNode.Data);
                return true;
            }
            return false;
        }                 
    }
}
