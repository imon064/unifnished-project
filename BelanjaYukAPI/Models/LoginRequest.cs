using System.ComponentModel.DataAnnotations;

namespace BelanjaYukAPI.Models 
    public class LoginRequest
    {
        [Required]
        public string EmailOrPhone { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

