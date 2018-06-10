using GiftDepo.Model;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace GiftDepo.Dialogs
{
    /// <summary>
    /// Interaction logic for AddPackageDialog.xaml
    /// </summary>
    public partial class PackageForm : UserControl
    {
        public PackageForm()
        {
            InitializeComponent();
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

    }
}
