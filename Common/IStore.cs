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
        bool IssuePackage(int width, int height);
        void AddPackage(int width, int height, int quantity = 1);
        int CountPackages(int width, int height);
        bool RemovePackage(int width);
    }
}
