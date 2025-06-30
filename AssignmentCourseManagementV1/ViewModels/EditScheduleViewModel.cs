using AssignmentCourseManagementV1.Command;
using AssignmentCourseManagementV1.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AssignmentCourseManagementV1.ViewModels
{
    class EditScheduleViewModel : BaseViewModel
    {
        public CourseSchedule ScheduleToEdit { get; set; }
        public bool IsSaved { get; private set; }
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }
        public ICommand SaveCommand { get; set; }

        private Course _selectedCourse;
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set { _selectedCourse = value; OnPropertyChanged(nameof(SelectedCourse)); }
        }

        private Room _selectedRoom;
        public Room SelectedRoom
        {
            get => _selectedRoom;
            set { _selectedRoom = value; OnPropertyChanged(nameof(SelectedRoom)); }
        }

        private DateTime? _date;
        public DateTime? Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(nameof(Date)); }
        }

        private int? _slot;
        public int? Slot
        {
            get => _slot;
            set { _slot = value; OnPropertyChanged(nameof(Slot)); }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        public EditScheduleViewModel(CourseSchedule scheduleToEdit)
        {
            ScheduleToEdit = scheduleToEdit;

            using (var db = new APContext())
            {
                Courses = new ObservableCollection<Course>(db.Courses.ToList());
                Rooms = new ObservableCollection<Room>(db.Rooms.ToList());
            }

            SelectedCourse = Courses.FirstOrDefault(c => c.CourseId == ScheduleToEdit.CourseId);
            SelectedRoom = Rooms.FirstOrDefault(r => r.RoomId == ScheduleToEdit.RoomId);
            Date = ScheduleToEdit.TeachingDate;
            Slot = ScheduleToEdit.Slot;
            Description = ScheduleToEdit.Description;
            IsSaved = false;
            SaveCommand = new RelayCommand(SaveSchedule);
        }

        private void SaveSchedule(object obj)
        {
            if (SelectedCourse == null || SelectedRoom == null || Date == null || Slot == null)
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new APContext())
            {
                var scheduleToUpdate = db.CourseSchedules.Find(ScheduleToEdit.TeachingScheduleId);
                if (scheduleToUpdate != null)
                {
                    scheduleToUpdate.CourseId = SelectedCourse.CourseId;
                    scheduleToUpdate.RoomId = SelectedRoom.RoomId;
                    scheduleToUpdate.TeachingDate = Date;
                    scheduleToUpdate.Slot = Slot;
                    scheduleToUpdate.Description = Description;
                    db.SaveChanges();
                }
            }

            MessageBox.Show("Schedule updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            IsSaved = true;

            if (obj is Window window)
            {
                window.Close();
            }
        }
    }
}
