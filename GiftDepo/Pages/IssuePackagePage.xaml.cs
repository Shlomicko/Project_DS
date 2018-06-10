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

namespace GiftDepo.Pages
{
    /// <summary>
    /// Interaction logic for IssuePackagePage.xaml
    /// </summary>
    public partial class IssuePackagePage : UserControl
    {
        private IStore _store;

        public IssuePackagePage(Common.IStore store)
        {
            InitializeComponent();
            _store = store;
            DataContext = new PackageFormValitationModel();
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

        private void OnIssuePackageClick(object sender, RoutedEventArgs e)
        {
            var model = DataContext as PackageFormValitationModel;
            _store.IssuePackage(model.Width, model.Height, model.Quantity);
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            DataContext = new PackageFormValitationModel();
        }
    }
}
