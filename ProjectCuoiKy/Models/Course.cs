using System;
using System.Collections.Generic;

namespace ProjectCuoiKy.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int CreatorId { get; set; }

    public virtual User Creator { get; set; } = null!;

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
