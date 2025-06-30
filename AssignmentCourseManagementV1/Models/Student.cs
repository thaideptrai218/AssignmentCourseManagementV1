using System;
using System.Collections.Generic;

namespace AssignmentCourseManagementV1.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string? Roll { get; set; }

    public string? FirstName { get; set; }

    public string? MidName { get; set; }

    public string? LastName { get; set; }

    public string FullName => $"{LastName} {MidName} {FirstName}";

    public virtual ICollection<RollCallBook> RollCallBooks { get; set; } = new List<RollCallBook>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
