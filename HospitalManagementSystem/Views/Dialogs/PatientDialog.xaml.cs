using HospitalManagementSystem.Helpers;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.ViewModels.Dialogs;
using System.Windows;

namespace HospitalManagementSystem.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for PatientDialog.xaml
    /// </summary>
    public partial class PatientDialog : Window
    {
        public PatientDialog()
        {
            InitializeComponent();

            List<Gender> genders = new List<Gender>
            {
                Gender.Male,
                Gender.Female,
            };
            inputGender.ItemsSource= genders;
        }
        
        private void CancelButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void AddNewPatientButton(object sender, RoutedEventArgs e)
        {
            try
            {
                Patient patient = new Patient();

                patient.FirstName = inputFirstName.Text;
                patient.LastName = inputLastName.Text;
                if (string.IsNullOrEmpty(patient.FirstName) || string.IsNullOrEmpty(patient.LastName))
                {
                    MessageBox.Show("You must fill all information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
               

                if (!PhoneRgex.IsValidPhoneNumber  (inputPhoneNumber.Text))
                {
                    MessageBox.Show("Invalid phone number format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; 
                }

                patient.PhoneNumber = (inputPhoneNumber.Text);

               
                if (!string.IsNullOrEmpty(inputBirthdate.Text))
                {
                    
                    if (DateOnly.TryParse(inputBirthdate.Text, out DateOnly birthdate))
                    {
                        patient.Birthdate = birthdate;
                    }
                    else
                    {
                        MessageBox.Show("Invalid birthdate format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; 
                    }
                }
                else
                {
                    MessageBox.Show("Birthdate is required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; 
                }

                
                if (inputGender.SelectedItem == null)
                {
                    MessageBox.Show("Gender is required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; 
                }

                patient.Gender = (Gender)inputGender.SelectedItem;                
                
                    PatientsService service = new PatientsService();
                    service.Create(patient);
                    MessageBox.Show("New Patient successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.None);
                    Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
