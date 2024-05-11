using HospitalManagementSystem.ViewModels;
using System.Windows.Controls;

namespace HospitalManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for PatientsView.xaml
    /// </summary>
    public partial class PatientsView : UserControl
    {
        public PatientsView()
        {
            InitializeComponent();

            DataContext = new PatientsViewModel();
        }
    }
}
