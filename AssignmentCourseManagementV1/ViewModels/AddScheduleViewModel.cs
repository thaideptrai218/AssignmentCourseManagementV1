using AssignmentCourseManagementV1.Command;
using AssignmentCourseManagementV1.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AssignmentCourseManagementV1.ViewModels
{
    internal class AddScheduleViewModel: BaseViewModel
    {
        public APContext _db { get; set; }
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<CourseSchedule> CourseSchedues { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }

        public ICommand AddCommand {  get; set; }

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
                }
            }
        }

        private Room _selectedRoom;
        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                if (_selectedRoom != value)
                {
                    _selectedRoom = value;
                    OnPropertyChanged(nameof(SelectedRoom));
                }
            }
        }

        public DateTime? date { get; set; }
        public int? slot { get; set; }
        
        public string? description { get; set; }

        public AddScheduleViewModel()
        {
            _db = new APContext();
            this.Courses = new ObservableCollection<Course>(_db.Courses.ToList());
            this.CourseSchedues = new ObservableCollection<CourseSchedule>(_db.CourseSchedules.ToList());
            this.Rooms = new ObservableCollection<Room>(_db.Rooms.ToList());
            this.AddCommand = new RelayCommand(AddSchedule);
        }

        private void AddSchedule(object obj)
        {
            // Basic validation
            //if (SelectedCourse == null || SelectedRoom == null || date == null || slot == null)
            //{
            //    MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            var newSchedule = new CourseSchedule
            {
                CourseId = SelectedCourse.CourseId,
                TeachingDate = date,
                Slot = slot,
                RoomId = SelectedRoom.RoomId,
                Description = description
            };

            _db.CourseSchedules.Add(newSchedule);
            _db.SaveChanges();

            // Update local collection
            CourseSchedues.Add(newSchedule);

            MessageBox.Show("Schedule added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Optionally close the window if obj is the window
            if (obj is Window window)
            {
                window.Close();
            }
        }
    }
}
