using Common;
using GiftDepo.Dialogs;
using GiftDepo.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : UserControl
    {
        public IStore _store { get; private set; }

        InventoryModel _model;
        public InventoryPage(IStore store)
        {
            InitializeComponent();
            _store = store;
            _model = new InventoryModel(store);
            _model.OnDataRefresh += OnDataRefresh;
            this.DataContext = _model;
        }


        private void OnDataRefresh(object sender, List<Package> e)
        {
            this.Dispatcher.Invoke(() =>
            {
                PackagesGridView.ItemsSource = null;
                PackagesGridView.ItemsSource = e;
            });

        }


        private void OnClickToManagePackage(object sender, RoutedEventArgs e)
        {
            var btn = ((Button)sender);
            var data = btn.DataContext as Package;
            MainWindow mn = (MainWindow)Application.Current.MainWindow;
            
            var view = new ManagePackageControl(_store)
            {
                DataContext = new PackageFormValitationModel()
                {
                    Width = data.Width,
                    Height = data.Height,
                    Quantity = data.Count,
                    NeedsRestock = data.Count < mn.MaximumPackageQuantity
                }
            };

            DialogHost.Show(view);
        }
    }
}
