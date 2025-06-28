using System;
using System.Collections.Generic;

namespace AssignmentCourseManagementV1.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseCode { get; set; }

    public string? CourseDescription { get; set; }

    public int SubjectId { get; set; }

    public int? InstructorId { get; set; }

    public int? TermId { get; set; }

    public int? CampusId { get; set; }

    public virtual Campus? Campus { get; set; }

    public virtual ICollection<CourseSchedule> CourseSchedules { get; set; } = new List<CourseSchedule>();

    public virtual Instructor? Instructor { get; set; }

    public virtual Subject Subject { get; set; } = null!;

    public virtual Term? Term { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
