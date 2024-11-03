using System;
using System.Collections.Generic;

namespace ConsoleAppTestDb.Models;

public partial class Vocabulary
{
    public int WordId { get; set; }

    public string EnglishWord { get; set; } = null!;

    public string VietnameseWord { get; set; } = null!;

    public string? WordType { get; set; }

    public DateTime? CreatedAt { get; set; }
}
