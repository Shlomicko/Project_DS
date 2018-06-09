using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftDepo.ModelView
{
    public class InventoryModel
    {

        public InventoryModel(IStore store)
        {
            Packages = store.GetPackages();
            store.OnInventoryChange += Store_OnInventoryChange;
        }

        private void Store_OnInventoryChange(object sender, IInventoryChangeEventArgs e)
        {
            if(Packages.TryGetValue(e.Package.Width, out Package package))
            {
                if(e.Package.Count == 0)
                {
                    Packages.Remove(e.Package.Width);
                }
                else
                {
                    Copy(e.Package, package);
                }
            }
        }

        private void Copy(Package from, Package to)
        {
            to.Count = from.Count;
            to.DateAdded = from.DateAdded;            
        }
        public Dictionary<int, Package> Packages { get; set; }
    }
}
