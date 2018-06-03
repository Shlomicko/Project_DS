using Common;
using GiftShop_DS.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GiftShop_DS.InventoryMessageBroadcaster;

namespace GiftShop_DS.Utils
{
    internal class StorageManager : IStore
    {
        private readonly TreeWithSubTrees<WidthData> tree;
        private readonly StoreQueue _storeQueue;
        private readonly int MinimumPackageQuantity;
        private readonly int MaximumPackageQuantity;
        private readonly int ExpiretionThreshHoldInMilliseconds = 10000;
        private ExpirationJobRunner _jobRunner;

        public static IStore Instance { get; private set; } = new StorageManager();
        private StorageManager()
        {
            tree = new TreeWithSubTrees<WidthData>();
            _storeQueue = new StoreQueue();
            MinimumPackageQuantity = 5;
            MaximumPackageQuantity = 100;
        }

        private void SetJob()
        {
            _jobRunner = new ExpirationJobRunner(_storeQueue)
            {
                ExpiretionThreshHoldInMilliseconds = ExpiretionThreshHoldInMilliseconds
            };
            _jobRunner.OnJobDone += OnExpired;
            _jobRunner.Start();
        }               

        private void OnExpired(object sender, JobDoneArgs e)
        {
            _jobRunner.Stop();
            var dq = e.DataQueue;
            var parentTree = dq.ParentTree;
            parentTree.RemoveNode(dq.Data);
            var hCount = parentTree.Count;
            if (hCount <= 0)
            {
                RemovePackage(dq.BaseNode.Data.Width);
            }
            _jobRunner.Start();
        }

        public bool IssuePackage(int width, int height, int quantity = 1)
        {
            var widthNode = new Node<WidthData>(new WidthData(width));
            if (tree.TryGetNode(ref widthNode))
            {
                var heightNode = new Node<HeightData>(new HeightData(height));
                if (widthNode.Data.HeightTree.TryGetNode(ref heightNode, true))
                {
                    var hCount = --heightNode.Data.Count;
                    if (hCount == 0)
                    {
                        widthNode.Data.HeightTree.RemoveNode(heightNode.Data);
                        _storeQueue.Remove(heightNode.Data.QueueNode);
                    }
                    else if(hCount <= MinimumPackageQuantity)
                    {
                        Publish(MessageType.PackageQuanityLow, new Package(width, height));
                    }

                    if (widthNode.Data.HeightTree.Count == 0)
                    {
                        widthNode.Data.HeightTree.RemoveNode(heightNode.Data);
                        if (widthNode.Data.HeightTree.Count == 0)
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
                    var hCount = heightNode.Data.Count;
                    var total = hCount + quantity;
                    if (total > MaximumPackageQuantity)
                    {
                        quantity = total - MaximumPackageQuantity;
                        Publish(MessageType.QuantityOverhead, new Package(width, height), numOffPackagesTooMuch: quantity);
                    }
                    heightNode.Data.Count += quantity;
                    _storeQueue.Enqueue(_storeQueue.Dequeue().Data);
                }
                else
                {                    
                    if (quantity > MaximumPackageQuantity)
                    {
                        
                        Publish(MessageType.QuantityOverhead, new Package(width, height), numOffPackagesTooMuch: quantity - MaximumPackageQuantity);
                        quantity = MaximumPackageQuantity;
                    }
                    hData.Count = quantity;
                    _storeQueue.Enqueue(hData);
                    widthNode.Data.HeightTree.Insert(data: hData);
                }
            }
            else
            {
                hData.Base = widthNode;
                _storeQueue.Enqueue(hData);
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

        public void SubscribeToAlertLowQuantityMessages(Action<string, Package> action)
        {
            InventoryMessageBroadcaster.SubscribeToQuantityToLow(action);
        }

        public void SubscribeToQuantityOverheadMessages(Action<string, Package> action)
        {
            InventoryMessageBroadcaster.SubscribeToQuantityOverhead(action);
        }

        private bool RemovePackage(int width)
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
