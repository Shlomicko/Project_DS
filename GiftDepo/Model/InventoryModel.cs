using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GiftDepo.Model
{
    public class InventoryModel : BaseViewModel
    {
        private ObservableCollection<Package> _packages;
        public event EventHandler<ObservableCollection<Package>> OnDataRefresh;

        public InventoryModel(IStore store)
        {
            _packages = new ObservableCollection<Package>(store.GetPackages());            
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
                    OnDataRefresh?.Invoke(this, _packages);
                }
            }
            else
            {
                _packages.Add(new Package(e.Package.Width, e.Package.Height, e.Package.DateAdded, e.Package.Count));                
            }
            Packages = _packages;
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
        public ObservableCollection<Package> Packages
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
