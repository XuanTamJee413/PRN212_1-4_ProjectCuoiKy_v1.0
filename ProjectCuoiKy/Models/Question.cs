using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectCuoiKy.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public string QuestionText { get; set; } = null!;

    public int TestId { get; set; }

    public string CorrectAnswer { get; set; } = null!;

    public string WrongAnswer1 { get; set; } = null!;

    public string WrongAnswer2 { get; set; } = null!;

    public string WrongAnswer3 { get; set; } = null!;

    [NotMapped]
    public string A { get; set; } = string.Empty;
    [NotMapped]
    public string B { get; set; } = string.Empty;
    [NotMapped]
    public string C { get; set; } = string.Empty;
    [NotMapped]
    public string D { get; set; } = string.Empty;

    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();

    public virtual Test Test { get; set; } = null!;
}
