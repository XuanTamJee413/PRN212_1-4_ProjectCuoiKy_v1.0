using System;
using System.Collections.Generic;

namespace ProjectCuoiKy.Models;

public partial class TestResult
{
    public int ResultId { get; set; }

    public int TestId { get; set; }

    public int StudentId { get; set; }

    public int? Score { get; set; }

    public DateTime? CompletionTime { get; set; }

    public virtual User Student { get; set; } = null!;

    public virtual Test Test { get; set; } = null!;
}
