using AssignmentCourseManagementV1.Command;
using AssignmentCourseManagementV1.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AssignmentCourseManagementV1.ViewModels
{
    internal class AddScheduleViewModel : BaseViewModel
    {
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }
        public ICommand AddCommand { get; set; }

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

        public DateTime? date { get; set; }
        public int? slot { get; set; }
        public string description { get; set; }

        public AddScheduleViewModel()
        {
            using (var db = new APContext())
            {
                Courses = new ObservableCollection<Course>(db.Courses.ToList());
                Rooms = new ObservableCollection<Room>(db.Rooms.ToList());
            }
            AddCommand = new RelayCommand(AddSchedule);
        }

        private void AddSchedule(object obj)
        {
            if (SelectedCourse == null || SelectedRoom == null || date == null || slot == null)
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new APContext())
            {
                var newSchedule = new CourseSchedule
                {
                    CourseId = SelectedCourse.CourseId,
                    TeachingDate = date,
                    Slot = slot,
                    RoomId = SelectedRoom.RoomId,
                    Description = description
                };

                db.CourseSchedules.Add(newSchedule);
                db.SaveChanges();
            }

            MessageBox.Show("Schedule added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // The window will be closed by the user or by setting its DialogResult.
            // For simplicity, we let the user close it. The MainWindow will reload data anyway.
        }
    }
}
