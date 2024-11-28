using System;
using System.Collections.Generic;

namespace VermittlungsPlattform.Models.Db;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Vorname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsAdmin { get; set; }

    public bool IsStudent { get; set; }

    public bool IsCompany { get; set; }

    public DateTime? RegisterDate { get; set; }

    public int? RecoveryCode { get; set; }
}
