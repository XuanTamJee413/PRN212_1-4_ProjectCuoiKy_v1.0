using System;
using System.Collections.Generic;

namespace ProjectCuoiKy.Models;

public partial class Test
{
    public int TestId { get; set; }

    public string TestName { get; set; } = null!;

    public int CourseId { get; set; }

    public int CreatorId { get; set; }

    public bool? Status { get; set; }

    public string TestKey { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? DueTime { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User Creator { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();

    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
