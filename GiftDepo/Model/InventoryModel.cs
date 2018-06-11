using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GiftDepo.Model
{
    public class InventoryModel : BaseViewModel
    {
        private List<Package> _packages;
        public event EventHandler<List<Package>> OnDataRefresh;

        public InventoryModel(IStore store)
        {
            _packages = store.GetPackages();            
            store.OnInventoryChange += Store_OnInventoryChange;
        }

        private void Store_OnInventoryChange(object sender, IInventoryChangeEventArgs e)
        {             
            if (TryGetPackage(e.Package.Width, e.Package.Height, out Package package))
            {
                if (e.Package.Count == 0)
                {
                    var index = IndexOf(e.Package);
                    _packages.RemoveAt(index);                    
                }
                else
                {
                    Copy(e.Package, package);                    
                }
            }
            else
            {
                _packages.Add(new Package(e.Package.Width, e.Package.Height, e.Package.DateAdded, e.Package.Count));                
            }
            OnDataRefresh?.Invoke(this, _packages);
        }
        private bool TryGetPackage(int width, int height, out Package package)
        {
            package = null;
            int length = _packages.Count;
            for (int i = 0; i < length; i++)
            {
                Package p = _packages[i];
                if(p.Width == width && p.Height == height)
                {
                    package = p;
                    return true;
                }
            }
            return false;
        }

        private int IndexOf(Package package)
        {
            int length = _packages.Count;
            for (int i = 0; i < length; i++)
            {
                Package p = _packages[i];
                if (p.Width == package.Width && p.Height == package.Height)
                {                    
                    return i;
                }
            }
            return -1;
        }
        private void Copy(Package from, Package to)
        {
            to.Count = from.Count;
            to.DateAdded = from.DateAdded;
        }
        public List<Package> Packages
        {
            get
            {
                return _packages;
            }
            set
            {
                _packages = value;                
                RaisePropertyChanged("Packages");
            }
        }
    }
}
