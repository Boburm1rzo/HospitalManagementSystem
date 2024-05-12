using HospitalManagementSystem.ViewModels;
using System.Windows.Controls;

namespace HospitalManagementSystem.Views
{
    public partial class PatientsView : UserControl
    {
        public PatientsView()
        {
            InitializeComponent();

            DataContext = new PatientsViewModel();
        }
    }
}
