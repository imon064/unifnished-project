using System;
using System.ComponentModel.DataAnnotations;

namespace BelanjaYukAPI.Models
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public DateTime Dob { get; set; }

        [Required]
        public string IdGender { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
