using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.Views.Dialogs;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HospitalManagementSystem.ViewModels
{
    public class PatientsViewModel : BaseViewModel
    {
        private readonly PatientsService _patientsService;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                SearchPatients(value);
            }
        }

        public ICommand AddCommand { get; }

        private List<Patient> patientsList;
        public ObservableCollection<Patient> Patients { get; }

        public PatientsViewModel()
        {
            _patientsService = new();
            Patients = [];
            patientsList = [];

            AddCommand = new Command(OnAdd);

            Load();
        }

        private void Load()
        {
            var patients = _patientsService.GetPatients();

            foreach(var patient in patients)
            {
                Patients.Add(patient);
                patientsList.Add(patient);
            }
        }

        private void SearchPatients(string searchText)
        {
            var patients = _patientsService.GetPatients(searchText);

            Patients.Clear();
            foreach (var patient in patients)
            {
                Patients.Add(patient);
            }
        }

        private void OnAdd()
        {
            var dialog = new PatientDialog();
            dialog.ShowDialog();
        }
    }
}
