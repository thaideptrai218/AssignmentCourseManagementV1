using System;
using System.Collections.Generic;

namespace AssignmentCourseManagementV1.Models;

public partial class Campus
{
    public int CampusId { get; set; }

    public string? CampusCode { get; set; }

    public string? CampusName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
