using Common;
using GiftDepo.ModelView;
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
        public InventoryPage(IStore store)
        {
            InitializeComponent();
            this.DataContext = new InventoryModel(store);
        }

        private void OnClickToManagePackage(object sender, RoutedEventArgs e)
        {

        }
    }
}
