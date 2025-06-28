using System;
using System.Collections.Generic;

namespace AssignmentCourseManagementV1.Models;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string? SubjectCode { get; set; }

    public string? SubjectName { get; set; }

    public int? DepartmentId { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual Department? Department { get; set; }
}
