using AssignmentCourseManagementV1.Models;

namespace AssignmentCourseManagementV1.ViewModels
{
    class MainWindowViewModel
    {
        public APContext _db { get; set; }
        public List<Course> Courses { get; set; }
        public List<CourseSchedule> CourseSchedues { get; set; }

        public MainWindowViewModel()
        {
            _db = new APContext();
            this.Courses = _db.Courses.ToList();
            this.CourseSchedues = _db.CourseSchedules.ToList();
        }

    }
}
