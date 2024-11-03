using System;
using System.Collections.Generic;

namespace ConsoleAppTestDb.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public DateTime? CreatedAt { get; set; }
}
