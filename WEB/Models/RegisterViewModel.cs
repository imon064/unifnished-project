using System;
using System.ComponentModel.DataAnnotations;

namespace belanjaYuk.WEB.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username wajib diisi.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username minimal 3 karakter.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email wajib diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nomor HP wajib diisi.")]
        [Phone(ErrorMessage = "Format nomor HP tidak valid.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Nama depan wajib diisi.")]
        public string FirstName { get; set; }

        public string? LastName { get; set; } 

        [Required(ErrorMessage = "Tanggal lahir wajib diisi.")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Jenis kelamin wajib diisi.")]
        public string IdGender { get; set; }

        [Required(ErrorMessage = "Password wajib diisi.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password minimal 6 karakter.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Konfirmasi password wajib diisi.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password dan konfirmasi password tidak cocok.")]
        public string ConfirmPassword { get; set; }
    }
}
