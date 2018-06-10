using Common;
using GiftDepo.Commands;
using GiftDepo.Dialogs;
using GiftDepo.Model;
using GiftDepo.Pages;
using GiftShop_DS.Utils;
using MaterialDesignThemes.Wpf;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GiftDepo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IStore _StoreManager;
        public ICommand RunExtendedDialogCommand => new ApplicationCommand(ExecuteRunExtendedDialog);
        public MainWindow()
        {
            InitializeComponent();
            _StoreManager = StorageManager.Instance;
            Config();
            DataContext = this;
        }

        private void Config()
        {
            var appSettings = ConfigurationManager.AppSettings;

            if (!int.TryParse(appSettings.Get("MaximumPackageQuantity"), out int MaximumPackageQuantity))
            {
                MaximumPackageQuantity = 100;
            }

            if (!int.TryParse(appSettings.Get("MinimumPackageQuantity"), out int MinimumPackageQuantity))
            {
                MinimumPackageQuantity = 5;
            }

            if (!int.TryParse(appSettings.Get("ExpiretionThreshHoldInMilliseconds"),
                out int ExpiretionThreshHoldInMilliseconds))
            {
                ExpiretionThreshHoldInMilliseconds = 10000;
            }

            _StoreManager.SetMinimumStock(MinimumPackageQuantity)
                         .SetMaximumStock(MaximumPackageQuantity)
                         .SetExpirationTime(ExpiretionThreshHoldInMilliseconds)
                         .Init();
        }

        private void StackPanel_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button btn = e.Source as Button;
            switch (btn.Tag)
            {
                case "about":
                    break;
                case "exit":
                    Application.Current.Shutdown();
                    break;
            }
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LeftNav.IsLeftDrawerOpen = false;
            ListViewItem item = (e.Source as ListView).SelectedItem as ListViewItem;
            switch (item.Name)
            {
                case "ItemManage":
                    PageContainer.Content = new InventoryPage(_StoreManager);
                    break;
                case "ItemIssuePackage":                                       
                    PageContainer.Content = new IssuePackagePage(_StoreManager);
                    break;
            }             
        }

        private async void ExecuteRunExtendedDialog(object o)
        {
            var view = new PackageForm();
            var model = new PackageFormValitationModel();
            view.DataContext = model;

            var result = await DialogHost.Show(view, "MainDialogHost", ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {

        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter == null) return;

            if (eventArgs.Parameter is PackageFormValitationModel model)
            {
                _StoreManager.AddPackage(model.Width, model.Height, model.Quantity);
            }
        }
    }
}
