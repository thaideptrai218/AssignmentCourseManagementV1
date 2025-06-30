using AssignmentCourseManagementV1.Command;
using AssignmentCourseManagementV1.Models;
using System.Windows;
using System.Windows.Input;

namespace AssignmentCourseManagementV1.ViewModels
{
    class EditStudentViewModel : BaseViewModel
    {
        public Student StudentToEdit { get; set; }
        public bool IsSaved { get; private set; }

        public string Roll { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }

        public ICommand SaveCommand { get; set; }

        public EditStudentViewModel(Student student)
        {
            StudentToEdit = student;
            IsSaved = false;

            // Initialize properties with the student's current values
            Roll = student.Roll;
            FirstName = student.FirstName;
            MidName = student.MidName;
            LastName = student.LastName;

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
                var studentToUpdate = db.Students.Find(StudentToEdit.StudentId);
                if (studentToUpdate != null)
                {
                    studentToUpdate.Roll = this.Roll;
                    studentToUpdate.FirstName = this.FirstName;
                    studentToUpdate.MidName = this.MidName;
                    studentToUpdate.LastName = this.LastName;
                    db.SaveChanges();
                }
            }

            MessageBox.Show("Student updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            IsSaved = true;

            if (obj is Window window)
            {
                window.Close();
            }
        }
    }
}
