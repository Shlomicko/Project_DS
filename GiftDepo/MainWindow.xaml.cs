using Common;
using GiftDepo.Commands;
using GiftDepo.Dialogs;
using GiftDepo.Model;
using GiftDepo.Pages;
using GiftShop_DS.Utils;
using MaterialDesignThemes.Wpf;
using System.Collections.Specialized;
using System.Configuration;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GiftDepo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IStore StoreManager { get; private set; }
        public ICommand RunExtendedDialogCommand => new ApplicationCommand(ExecuteRunExtendedDialog);

        public readonly NameValueCollection AppSettings = ConfigurationManager.AppSettings;

        public readonly int MaximumPackageQuantity;
        public readonly int MinimumPackageQuantity;
        public readonly int ExpiretionThreshHoldInMilliseconds;
        private const string OverheadMessage = "There are {0} packages too much. Package dimentions:{1}x{2}";
        private const string TooLowMessage = "We are running low of {0} packages.";
        public MainWindow()
        {
            InitializeComponent();
            StoreManager = StorageManager.Instance;
            if (!int.TryParse(AppSettings.Get("MaximumPackageQuantity"), out MaximumPackageQuantity))
            {
                MaximumPackageQuantity = 100;
            }

            if (!int.TryParse(AppSettings.Get("MinimumPackageQuantity"), out MinimumPackageQuantity))
            {
                MinimumPackageQuantity = 5;
            }

            if (!int.TryParse(AppSettings.Get("ExpiretionThreshHoldInMilliseconds"),
                out ExpiretionThreshHoldInMilliseconds))
            {
                ExpiretionThreshHoldInMilliseconds = 10000;
            }

            DataContext = this;
            Initialize();
        }

        private void Initialize()
        {
            StoreManager.SetMinimumStock(MinimumPackageQuantity)
                         .SetMaximumStock(MaximumPackageQuantity)
                         .SetExpirationTime(ExpiretionThreshHoldInMilliseconds)
                         .Init();

            MainSnackbar.MessageQueue = new SnackbarMessageQueue(new System.TimeSpan(0, 0, 8));

            StoreManager.SubscribeToAlertLowQuantityMessages(package =>
            {
                string message = string.Format(TooLowMessage, package.ToStringNoDate());
                int restock = MaximumPackageQuantity - package.Count;
                MainSnackbar.MessageQueue.Enqueue(message, "Restock", () =>
                {
                    StoreManager.AddPackage(package.Width, package.Height, restock);
                });
            });

            StoreManager.SubscribeToQuantityOverheadMessages(package =>
            {
                int change = package.Count;
                string message = string.Format(OverheadMessage, change, package.Width, package.Height);

                MainSnackbar.MessageQueue.Enqueue(message, "Take action", () =>
                {
                    DialogHost.Show(new JustKidding());
                });
            });
        }

        private void StackPanel_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button btn = e.Source as Button;
            AddPackageButton.Visibility = Visibility.Collapsed;
            switch (btn.Tag)
            {
                case "about":                    
                    PageContainer.Content = new AboutPage();
                    break;
                case "help":
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                    string[] names = GetType().Assembly.GetManifestResourceNames();
                    System.IO.Stream helpBeatles = assembly.GetManifestResourceStream("GiftDepo.Assets.Beatles_help.wav");
                    System.IO.Stream bg = assembly.GetManifestResourceStream("GiftDepo.Assets.beatles.jpg");

                    var player = new SoundPlayer(helpBeatles);
                    var bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.StreamSource = bg;
                    bmp.EndInit();
                    var img = new Image
                    {
                        Source = bmp
                    };

                    PageContainer.Content = img;                   
                    player.Play();
                    
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
            AddPackageButton.Visibility = Visibility.Visible;
            switch (item.Tag)
            {
                case "item_manage":
                    PageContainer.Content = new InventoryPage(StoreManager);
                    break;
                case "item_issue_package":
                    PageContainer.Content = new IssuePackagePage(StoreManager);
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
                StoreManager.AddPackage(model.Width, model.Height, model.Quantity);
            }
        }
    }
}
