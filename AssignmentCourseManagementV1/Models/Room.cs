using System;
using System.Collections.Generic;

namespace AssignmentCourseManagementV1.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string? RoomCode { get; set; }

    public int? CampusId { get; set; }

    public int? Capacity { get; set; }

    public string? Type { get; set; }

    public string? Description { get; set; }

    public virtual Campus? Campus { get; set; }

    public virtual ICollection<CourseSchedule> CourseSchedules { get; set; } = new List<CourseSchedule>();
}
