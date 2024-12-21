using System;
using System.Collections.Generic;

namespace ProjectCuoiKy.Models;

public partial class StudentAnswer
{
    public int AnswerId { get; set; }

    public int TestId { get; set; }

    public int StudentId { get; set; }

    public int QuestionId { get; set; }

    public string ChosenAnswer { get; set; } = null!;

    public bool? IsCorrect { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual User Student { get; set; } = null!;

    public virtual Test Test { get; set; } = null!;
}
