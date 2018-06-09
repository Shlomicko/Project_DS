using Common;
using GiftShop_DS.Structure;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                int hCount = heightNode.Data.Count;
                if (widthNode.Data.HeightTree.TryGetNode(ref heightNode, true))
                {
                    --hCount;
                    if (hCount == 0)
                    {
                        widthNode.Data.HeightTree.RemoveNode(heightNode.Data);
                        _storeQueue.Remove(heightNode.Data.QueueNode);
                    }
                    else if (hCount <= MinimumPackageQuantity)
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
                    int hCount = heightNode.Data.Count;
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

            var pkg = new Package()
            {
                Count = heightNode.Data.Count,
                Width = width,
                Height = height
            };

            OnInventoryChange?.Invoke(this, new InventoryChangeEventArgs(pkg));
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

        public Dictionary<int, Package> GetPackages()
        {
            Dictionary<int, Package> packages = new Dictionary<int, Package>();
            packages.Add(2, new Package(2, 10, DateTime.Now.AddMonths(4)));
            packages.Add(6, new Package(6, 2, DateTime.Now.AddMonths(-8)));
            packages.Add(3, new Package(3, 11, DateTime.Now.AddYears(3)));
            packages.Add(18, new Package(18, 77, DateTime.Now.AddYears(-4)));
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
                        packages.Add(baseItem.Width, package);
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
