using AssignmentCourseManagementV1.Command;
using AssignmentCourseManagementV1.Models;
using System.Windows;
using System.Windows.Input;

namespace AssignmentCourseManagementV1.ViewModels
{
    class AddStudentViewModel : BaseViewModel
    {
        public string Roll { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public bool IsSaved { get; private set; }

        public ICommand SaveCommand { get; set; }

        public AddStudentViewModel()
        {
            IsSaved = false;
            SaveCommand = new RelayCommand(SaveStudent);
        }

        private void SaveStudent(object obj)
        {
            if (string.IsNullOrEmpty(Roll) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            {
                MessageBox.Show("Roll, First Name, and Last Name are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new APContext())
            {
                var newStudent = new Student
                {
                    Roll = this.Roll,
                    FirstName = this.FirstName,
                    MidName = this.MidName,
                    LastName = this.LastName
                };
                db.Students.Add(newStudent);
                db.SaveChanges();
            }

            MessageBox.Show("Student added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            IsSaved = true;

            if (obj is Window window)
            {
                window.Close();
            }
        }
    }
}
