using AssignmentCourseManagementV1.Command;
using AssignmentCourseManagementV1.Models;
using AssignmentCourseManagementV1.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace AssignmentCourseManagementV1.ViewModels
{
    class StudentViewModel : BaseViewModel
    {
        private APContext _db;
        private ObservableCollection<Student> _students;
        public ICollectionView Students { get; set; }

        public RelayCommand AddCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand ResetCommand { get; set; }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                Students.Refresh(); // This will re-trigger the filter
            }
        }

        public StudentViewModel()
        {
            LoadStudents();
            AddCommand = new RelayCommand(AddStudent);
            EditCommand = new RelayCommand(EditStudent, (o) => SelectedStudent != null);
            DeleteCommand = new RelayCommand(DeleteStudent, (o) => SelectedStudent != null);
            ResetCommand = new RelayCommand(Reset);
        }

        private void LoadStudents()
        {
            _db = new APContext();
            _students = new ObservableCollection<Student>(_db.Students.ToList());
            Students = CollectionViewSource.GetDefaultView(_students);
            Students.Filter = FilterStudents;
        }

        private bool FilterStudents(object obj)
        {
            if (obj is Student student)
            {
                if (string.IsNullOrEmpty(SearchText))
                {
                    return true; // No filter, show all
                }
                // Filter by name or roll number (case-insensitive)
                return (student.FullName != null && student.FullName.ToLower().Contains(SearchText.ToLower())) ||
                       (student.Roll != null && student.Roll.ToLower().Contains(SearchText.ToLower()));
            }
            return false;
        }

        private void Reset(object obj)
        {
            SearchText = string.Empty;
        }

        private void AddStudent(object obj)
        {
            var viewModel = new AddStudentViewModel();
            var window = new AddStudentWindow
            {
                DataContext = viewModel
            };
            window.ShowDialog();
            if (viewModel.IsSaved)
            {
                LoadStudents();
            }
        }

        private void EditStudent(object obj)
        {
            if (SelectedStudent == null) return;

            var viewModel = new EditStudentViewModel(SelectedStudent);
            var window = new EditStudentWindow
            {
                DataContext = viewModel
            };
            window.ShowDialog();
            if (viewModel.IsSaved)
            {
                LoadStudents();
            }
        }

        private void DeleteStudent(object obj)
        {
            if (SelectedStudent == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete {SelectedStudent.FullName}?",
                                           "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var db = new APContext())
                {
                    var studentToDelete = db.Students.Find(SelectedStudent.StudentId);
                    if (studentToDelete != null)
                    {
                        db.Students.Remove(studentToDelete);
                        db.SaveChanges();
                    }
                }
                LoadStudents();
            }
        }
    }
}
