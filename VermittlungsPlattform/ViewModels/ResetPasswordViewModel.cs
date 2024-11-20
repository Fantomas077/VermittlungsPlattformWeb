using System.ComponentModel.DataAnnotations;

namespace VermittlungsPlattform.ViewModels
{
    public class ResetPasswordViewModel
    {
        
        public string Email { get; set; }
        [Required]
        public int? RecoveryCode { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit contenir au moins {2} caractères.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Le mot de passe doit contenir au moins une majuscule, une minuscule, un chiffre et un caractère spécial.")]
        public string NewPassword { get; set; }

    }
}
