using System.ComponentModel.DataAnnotations;

namespace belanjaYuk.WEB.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email atau nomor HP wajib diisi.")]
        [Display(Name = "Email/No. HP")]
        public string EmailOrPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kata sandi wajib diisi.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Ingat saya")]
        public bool RememberMe { get; set; }
    }
}

