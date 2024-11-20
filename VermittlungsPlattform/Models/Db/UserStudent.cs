using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VermittlungsPlattform.Models.Db;

public partial class UserStudent
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Vorname { get; set; } = null!;

    public string Email { get; set; } = null!;
    [StringLength(100, ErrorMessage = "Le {0} doit contenir au moins {2} caractères.", MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Le mot de passe doit contenir au moins une majuscule, une minuscule, un chiffre et un caractère spécial.")]
    public string Password { get; set; } = null!;

    public bool IsAdmin { get; set; }

    public DateTime? RegisterDate { get; set; }

    public int? RecoveryCode { get; set; }
}
