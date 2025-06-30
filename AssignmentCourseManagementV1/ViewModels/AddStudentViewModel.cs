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
        private APContext _db;

        public ICommand SaveCommand { get; set; }

        public AddStudentViewModel(APContext db)
        {
            _db = db;
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

            // Data type and length validation
            if (Roll.Length > 20)
            {
                MessageBox.Show("Roll number cannot exceed 20 characters.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (FirstName.Length > 50 || (MidName != null && MidName.Length > 50) || LastName.Length > 50)
            {
                MessageBox.Show("Names cannot exceed 50 characters.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            // Check for duplicate Roll number
            if (_db.Students.Any(s => s.Roll == this.Roll))
            {
                MessageBox.Show("A student with this Roll number already exists.", "Duplicate Roll Number", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var maxId = _db.Students.Max(s => (int?)s.StudentId) ?? 0;
            var newStudent = new Student
            {
                StudentId = maxId + 1,
                Roll = this.Roll,
                FirstName = this.FirstName,
                MidName = this.MidName,
                LastName = this.LastName
            };
            _db.Students.Add(newStudent);
            _db.SaveChanges();

            MessageBox.Show("Student added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            IsSaved = true;

            if (obj is Window window)
            {
                window.Close();
            }
        }
    }
}
