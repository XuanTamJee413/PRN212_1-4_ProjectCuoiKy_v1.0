using System;
using System.Collections.Generic;

namespace ProjectCuoiKy.Models;

public partial class Term
{
    public int TermId { get; set; }

    public int CourseId { get; set; }

    public string TermText { get; set; } = null!;

    public string CorrectAnswer { get; set; } = null!;

    public string WrongAnswer1 { get; set; } = null!;

    public string WrongAnswer2 { get; set; } = null!;

    public string WrongAnswer3 { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}
