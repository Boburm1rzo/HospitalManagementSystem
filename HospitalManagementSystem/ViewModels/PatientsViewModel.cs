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

        #region Pagination

        private int pagesCount;
        private int firstPageNumber = 1;
        private int lastPageNumber = 3;

        private int _currentPage = 1;
        public int CurrentPage 
        {
            get => _currentPage;
            set
            {
                SetProperty(ref _currentPage, value);
                HasPreviousPage = CurrentPage > 3;
                HasNextPage = CurrentPage < pagesCount;
            }

            // [1], 2, 3
            // 1, [2], 3
            // 1, 2, [3]
            // 2, 3, [4]
        }

        private bool _hasNextPage;
        public bool HasNextPage
        {
            get => _hasNextPage;
            set => SetProperty(ref _hasNextPage, value);
        }

        private bool _hasPreviousPage = false;
        public bool HasPreviousPage
        {
            get => _hasPreviousPage;
            set => SetProperty(ref _hasPreviousPage, value);
        }

        private int _firstButtonContent = 1;
        public int FirstButtonContent 
        { 
            get => _firstButtonContent; 
            set => SetProperty(ref _firstButtonContent, value); 
        }
        
        private int _secondButtonContent = 2;
        public int SecondButtonContent 
        {
            get => _secondButtonContent; 
            set => SetProperty(ref _secondButtonContent, value); 
        }

        private int _thirdButtonContent = 3;
        public int ThirdButtonContent 
        { 
            get => _thirdButtonContent; 
            set => SetProperty(ref _thirdButtonContent, value); 
        }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        #endregion

        public ICommand AddCommand { get; }
        public ICommand ShowDetailsCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        private readonly List<Patient> patientsList;
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

            NextPageCommand = new Command(OnNextPage);
            PreviousPageCommand = new Command(OnPreviousPage);

            Load();
        }

        private void OnNextPage()
        {
            if (_currentPage >= pagesCount)
            {
                return;
            }

            CurrentPage += 3;
            FirstButtonContent += 3;
            SecondButtonContent += 3;
            ThirdButtonContent += 3;
        }

        private void OnPreviousPage()
        {
            if (_currentPage <= 1)
            {
                return;
            }

            CurrentPage -= 3;
            FirstButtonContent -= 3;
            SecondButtonContent -= 3;
            ThirdButtonContent -= 3;
        }

        private void Load()
        {
            var patients = _patientsService.GetPatients();
            var totalCount = _patientsService.GetTotalCount();
            pagesCount = (int)Math.Ceiling((double)totalCount / 20);
            HasNextPage = pagesCount > 3;

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
