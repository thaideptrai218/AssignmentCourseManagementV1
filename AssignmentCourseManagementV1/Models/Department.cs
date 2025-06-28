using System;
using System.Collections.Generic;

namespace AssignmentCourseManagementV1.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
