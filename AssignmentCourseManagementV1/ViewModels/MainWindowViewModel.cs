using AssignmentCourseManagementV1.Command;
using AssignmentCourseManagementV1.Models;
using AssignmentCourseManagementV1.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace AssignmentCourseManagementV1.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private APContext _db;

        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<CourseSchedule> CourseSchedules { get; set; }
        public ObservableCollection<CourseSchedule> FilteredCourseSchedules { get; set; }

        public RelayCommand EditCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand FilterCommand { get; set; }

        private Course _selectedCourse;
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                OnPropertyChanged(nameof(SelectedCourse));
                FilterSchedules();
            }
        }

        private CourseSchedule _selectedCourseSchedule;
        public CourseSchedule SelectedCourseSchedule
        {
            get => _selectedCourseSchedule;
            set
            {
                _selectedCourseSchedule = value;
                OnPropertyChanged(nameof(SelectedCourseSchedule));
            }
        }

        public MainWindowViewModel()
        {
            LoadData();
            AddCommand = new RelayCommand(AddSchedule);
            FilterCommand = new RelayCommand((o) => FilterSchedules());
            ResetCommand = new RelayCommand(ResetFilter);
            DeleteCommand = new RelayCommand(DeleteSchedule, (o) => SelectedCourseSchedule != null);
            EditCommand = new RelayCommand(EditSchedule, (o) => SelectedCourseSchedule != null);
        }

        private void LoadData()
        {
            _db = new APContext();
            Courses = new ObservableCollection<Course>(_db.Courses.ToList());
            CourseSchedules = new ObservableCollection<CourseSchedule>(_db.CourseSchedules.ToList());
            FilterSchedules();
        }

        private void ResetFilter(object obj)
        {
            SelectedCourse = null;
            SelectedCourseSchedule = null;
        }

        private void FilterSchedules()
        {
            if (SelectedCourse == null)
            {
                FilteredCourseSchedules = new ObservableCollection<CourseSchedule>(CourseSchedules);
            }
            else
            {
                FilteredCourseSchedules = new ObservableCollection<CourseSchedule>(
                    CourseSchedules.Where(cs => cs.CourseId == SelectedCourse.CourseId));
            }
            OnPropertyChanged(nameof(FilteredCourseSchedules));
        }

        private void AddSchedule(object obj)
        {
            var viewModel = new AddScheduleViewModel();
            var window = new AddScheduleWindow
            {
                DataContext = viewModel
            };
            window.ShowDialog();
            if (viewModel.IsSaved)
            {
                LoadData();
            }
        }

        private void DeleteSchedule(object obj)
        {
            if (SelectedCourseSchedule == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete the schedule for {SelectedCourseSchedule.Course.CourseCode}?",
                                           "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var db = new APContext())
                {
                    var scheduleToDelete = db.CourseSchedules.Find(SelectedCourseSchedule.TeachingScheduleId);
                    if (scheduleToDelete != null)
                    {
                        db.CourseSchedules.Remove(scheduleToDelete);
                        db.SaveChanges();
                    }
                }
                LoadData();
            }
        }

        private void EditSchedule(object obj)
        {
            if (SelectedCourseSchedule == null) return;

            var viewModel = new EditScheduleViewModel(SelectedCourseSchedule);
            var window = new EditScheduleWindow
            {
                DataContext = viewModel
            };
            window.ShowDialog();
            if (viewModel.IsSaved)
            {
                LoadData();
            }
        }
    }
}
