using AssignmentCourseManagementV1.Command;
using AssignmentCourseManagementV1.Models;
using AssignmentCourseManagementV1.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace AssignmentCourseManagementV1.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        public APContext _db { get; set; }
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<CourseSchedule> CourseSchedues { get; set; }
        public ObservableCollection<CourseSchedule> FilteredCourseSchedules { get; set; }

        public RelayCommand EditCommand;
        public RelayCommand DeleteCommand;
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand FilterCommand { get; set; }

        private Course _selectedCourse;
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                if (_selectedCourse != value)
                {
                    _selectedCourse = value;
                    OnPropertyChanged(nameof(SelectedCourse));
                    FilterCommand.Execute(null);
                }
            }
        }


        public MainWindowViewModel()
        {
            _db = new APContext();
            this.Courses = new ObservableCollection<Course>(_db.Courses.ToList());
            this.CourseSchedues = new ObservableCollection<CourseSchedule>(_db.CourseSchedules.ToList());
            AddCommand = new RelayCommand(addSchedule);
            FilterCommand = new RelayCommand(filterSchedule);
            ResetCommand = new RelayCommand(resetSchedule);

        }

        private void resetSchedule(object obj)
        {
            SelectedCourse = null;
        }

        private void filterSchedule(object obj)
        {
            if (SelectedCourse == null)
            {
                FilteredCourseSchedules = new ObservableCollection<CourseSchedule>(CourseSchedues);
            }
            else
            {
                FilteredCourseSchedules = new ObservableCollection<CourseSchedule>(
                    CourseSchedues.Where(cs => cs.CourseId == SelectedCourse.CourseId));
            }
            OnPropertyChanged(nameof(FilteredCourseSchedules));
        }

        private void addSchedule(object obj)
        {
            AddScheduleWindow addScheduleWindow = new AddScheduleWindow();
            addScheduleWindow.Show();
        }
    }
}
