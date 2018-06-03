using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IStore
    {
        List<Package> GetPackages();
        bool IssuePackage(int width, int height, int quantity = 1);
        void AddPackage(int width, int height, int quantity = 1);
        int CountPackages(int width, int height);
        void SubscribeToAlertLowQuantityMessages(Action<string, Package> action);
        void SubscribeToQuantityOverheadMessages(Action<string, Package> action);        
    }
}
