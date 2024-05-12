using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.Views.Dialogs;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HospitalManagementSystem.ViewModels
{
	public class DoctorsViewModel : BaseViewModel
	{
		private readonly DoctorsService _doctorsService;

		private string _searchText;

		public string SearchText
		{
			get => _searchText;
			set
			{
				SetProperty(ref _searchText, value);
				SearchDoctors(value);
			}
		}

		public ICommand AddCommand { get; }

		private readonly List<Doctor> doctorsList;
		public ObservableCollection<Doctor> Doctors { get; }

		public DoctorsViewModel()
		{
			_doctorsService = new();
			doctorsList = [];
			Doctors = [];

			AddCommand = new Command(OnAdd);

			Load();
		}

		private void Load()
		{
			var doctors = _doctorsService.GetDoctors();

			foreach (var doctor in doctors)
			{
				Doctors.Add(doctor);
				doctorsList.Add(doctor);
			}
		}

		private void SearchDoctors(string searchText)
		{
			var doctors = _doctorsService.GetDoctors(searchText);

			Doctors.Clear();

			foreach (var doctor in doctors)
			{
				Doctors.Add(doctor);
			}
		}

		private void OnAdd()
		{
			var dialog = new DoctorDialog();
			dialog.ShowDialog();
		}
	}
}
