using System.ComponentModel.DataAnnotations;

namespace VermittlungsPlattform.ViewModels
{
    public class RecoveryPasswordViewModel
    {

        [Required]
        public string Email { get; set; }
    }
}
