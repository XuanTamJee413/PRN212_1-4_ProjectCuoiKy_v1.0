using System;
using System.Collections.Generic;

namespace ProjectCuoiKy.Models;

public partial class Test
{
    public int TestId { get; set; }

    public int CourseId { get; set; }

    public int CreatorId { get; set; }

    public bool? TimerEnabled { get; set; }

    public string TestKey { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual User Creator { get; set; } = null!;

    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();

    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
