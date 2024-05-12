using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.Views.Dialogs;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows;
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

        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set => SetProperty(ref _selectedPatient, value);
        }

        public ICommand AddCommand { get; }
        public ICommand ShowDetailsCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        private List<Patient> patientsList;
        public ObservableCollection<Patient> Patients { get; }

        public PatientsViewModel()
        {
            _patientsService = new();
            Patients = [];
            patientsList = [];

            AddCommand = new Command(OnAdd);
            ShowDetailsCommand = new Command(OnShowDetails);
            EditCommand = new Command<Patient>(OnEdit);
            DeleteCommand = new Command<Patient>(OnDelete);

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

        private void OnShowDetails()
        {
            if (SelectedPatient is null)
            {
                return;
            }

            var dialog = new PatientDetailsDialog(SelectedPatient);
            dialog.ShowDialog();
        }

        private void OnEdit(Patient patient)
        {

        }

        private void OnDelete(Patient patient)
        {
            var result = MessageBox.Show(
                $"Are you sure you want to delete: {patient.FirstName} {patient.LastName}?",
                "Confirm your action.",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            _patientsService.Delete(patient);
            MessageBox.Show(
                $"Patient: {patient.FirstName} {patient.LastName} successfully deleted.",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
