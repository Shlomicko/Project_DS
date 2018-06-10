using Common;
using GiftDepo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GiftDepo.Dialogs
{
    /// <summary>
    /// Interaction logic for ManagePackageControl.xaml
    /// </summary>
    public partial class ManagePackageControl : UserControl
    {
        private IStore _store;
        public ManagePackageControl(IStore store)
        {
            InitializeComponent();
            _store = store;          
        }

        public ObservableCollection<int> ValidationErrors { get; private set; } = new ObservableCollection<int>();
        public void OnValidationError(object sender, ValidationErrorEventArgs e)
        {

            if (e.Action == ValidationErrorEventAction.Added)
            {
                ValidationErrors.Add(1);
            }
            else
            {
                ValidationErrors.Remove(1);
            }
            ((PackageFormValitationModel)DataContext).HasNoErrors = (ValidationErrors.Count == 0);
        }

        private void OnRemovePackage(object sender, RoutedEventArgs e)
        {
            var p = DataContext as PackageFormValitationModel;
            _store.RemovePackage(p.Width, p.Height);
        }

        private void OnAddAmount(object sender, RoutedEventArgs e)
        {
            var p = DataContext as PackageFormValitationModel;
            _store.AddPackage(p.Width, p.Height, p.Quantity);
        }
    }
}

