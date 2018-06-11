using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Common
{
    public interface IStore
    {
        List<Package> GetPackages();
        bool IssuePackage(int width, int height, int quantity = 1);
        void AddPackage(int width, int height, int quantity = 1);
        int CountPackages(int width, int height);
        void SubscribeToAlertLowQuantityMessages(Action<Package> action);
        void SubscribeToQuantityOverheadMessages(Action<Package> action);
        IStore SetMinimumStock(int min);
        IStore SetMaximumStock(int min);
        IStore SetExpirationTime(int min);
        void Init();
        bool RemovePackage(int width);
        bool RemovePackage(int width, int height);
        event EventHandler<IInventoryChangeEventArgs> OnInventoryChange;
    }
    public interface IInventoryChangeEventArgs
    {
        Package Package { get; }        
    }
}
