using Common;
using GiftShop_DS.Structure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Threading.Tasks;
using static GiftShop_DS.InventoryMessageBroadcaster;
using static GiftShop_DS.Utils.StorageConfiguration;

namespace GiftShop_DS.Utils
{
    public class StorageManager : IStore
    {
        private const string StorageNotInitializedMessage = "Need to config stock quantities and expiration time.";
        private TreeWithSubTrees<WidthData> tree;
        private StoreQueue _storeQueue;
        private ExpirationJobRunner _jobRunner;
        public event EventHandler<IInventoryChangeEventArgs> OnInventoryChange;

        public static IStore Instance { get; private set; } = new StorageManager();

        private StorageManager()
        {

        }

        public void Init()
        {

            if (MinimumPackageQuantity == default(int) ||
                MaximumPackageQuantity == default(int) ||
                ExpiretionThreshHoldInMilliseconds == default(int))
            {
                throw new Exception(StorageNotInitializedMessage);
            }

            tree = new TreeWithSubTrees<WidthData>();
            _storeQueue = new StoreQueue();

            SetJob();
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
            if (hCount == 0)
            {
                var width = dq.BaseNode.Data.Width;
                var height = dq.Data.Height;
                RemovePackage(width);
            }
            _jobRunner.Start();            
        }

        public bool IssuePackage(int width, int height, int quantity = 1)
        {
            var widthNode = new Node<WidthData>(new WidthData(width));
            if (tree.TryGetNode(ref widthNode))
            {
                var heightNode = new Node<HeightData>(new HeightData(height));
                int hCount = heightNode.Data.Count;
                if (widthNode.Data.HeightTree.TryGetNode(ref heightNode, true))
                {
                    --hCount;
                    if (hCount == 0)
                    {
                        RemovePackage(width, height);                        
                        _storeQueue.Remove(heightNode.Data.QueueNode);
                    }
                    else if (hCount <= MinimumPackageQuantity)
                    {
                        Publish(MessageType.PackageQuanityLow, new Package(width, height));
                    }
                    
                    var pkg = new Package()
                    {
                        Count = hCount,
                        Width = width,
                        Height = height
                    };

                    OnInventoryChange?.Invoke(this, new InventoryChangeEventArgs(pkg));
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
                    hData = heightNode.Data;
                    _storeQueue.Enqueue(_storeQueue.Dequeue().Data);
                }
                else
                {
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

            var pkg = new Package()
            {
                Count = heightNode.Data.Count,
                Width = width,
                Height = height
            };

            hData.Count = GetRightAmountAndPublishMessage(currentPackageCount: hData.Count,
                    amountToAdd: quantity,
                    package: pkg);

            pkg.Count = hData.Count;

            OnInventoryChange?.Invoke(this, new InventoryChangeEventArgs(pkg));
        }

        private int GetRightAmountAndPublishMessage(int currentPackageCount, int amountToAdd, Package package)
        {
            int total = currentPackageCount + amountToAdd;
            if (total > MaximumPackageQuantity)
            {
                var change = total - MaximumPackageQuantity;
                total = amountToAdd - change;
                total += currentPackageCount;
                Publish(MessageType.QuantityOverhead, package, numOffPackagesTooMuch: change);
            }
            return total;
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

        public ICollection<Package> GetPackages()
        {
            List<Package> packages = new List<Package>();
            var bases = tree.Inorder();
            if (bases != null)
            {
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
                        if (!packages.Contains(package))
                        {
                            packages.Add(package);
                        }
                    }
                }
            }
            return packages;
        }

        public IStore SetMinimumStock(int min)
        {
            MinimumPackageQuantity = min;
            return this;
        }
        public IStore SetMaximumStock(int max)
        {
            MaximumPackageQuantity = max;
            return this;
        }
        public IStore SetExpirationTime(int milliseconds)
        {
            ExpiretionThreshHoldInMilliseconds = milliseconds;
            return this;
        }

        public void SubscribeToAlertLowQuantityMessages(Action<string, Package> action)
        {
            InventoryMessageBroadcaster.SubscribeToQuantityToLow(action);
        }

        public void SubscribeToQuantityOverheadMessages(Action<string, Package> action)
        {
            InventoryMessageBroadcaster.SubscribeToQuantityOverhead(action);
        }

        public bool RemovePackage(int width, int height)
        {
            var widthNode = new Node<WidthData>(new WidthData(width));
            if (tree.TryGetNode(ref widthNode))
            {
                var heightNode = new Node<HeightData>(new HeightData(height));
                if (widthNode.Data.HeightTree.TryGetNode(ref heightNode))
                {
                    widthNode.Data.HeightTree.RemoveNode(heightNode.Data);
                    heightNode.Data.Count = 0;
                    
                    if (widthNode.Data.HeightTree.Count == 0)
                    {
                        tree.RemoveNode(data: widthNode.Data);
                    }
                    
                    var pkg = new Package()
                    {
                        Count = 0,
                        Width = width,
                        Height = height
                    };

                    OnInventoryChange?.Invoke(this, new InventoryChangeEventArgs(pkg));
                    return true;
                }
            }
            return false;
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

        public class InventoryChangeEventArgs : EventArgs, IInventoryChangeEventArgs
        {

            internal InventoryChangeEventArgs(Package package)
            {
                Package = package;
            }

            public Package Package
            {
                get;
                private set;
            }
        }
    }
}
