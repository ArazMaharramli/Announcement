using System;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Auth.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Phone]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class ConfirmEmailViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Confirmation Code")]
        [StringLength(maximumLength: 6, MinimumLength = 6)]
        public string Code { get; set; }
    }
}
