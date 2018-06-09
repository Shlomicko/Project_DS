using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IStore
    {
        Dictionary<int, Package> GetPackages();
        bool IssuePackage(int width, int height, int quantity = 1);
        void AddPackage(int width, int height, int quantity = 1);
        int CountPackages(int width, int height);
        void SubscribeToAlertLowQuantityMessages(Action<string, Package> action);
        void SubscribeToQuantityOverheadMessages(Action<string, Package> action);
        IStore SetMinimumStock(int min);
        IStore SetMaximumStock(int min);
        IStore SetExpirationTime(int min);
        void Init();
        event EventHandler<IInventoryChangeEventArgs> OnInventoryChange;
    }
    public interface IInventoryChangeEventArgs
    {
        Package Package { get; }        
    }
}
