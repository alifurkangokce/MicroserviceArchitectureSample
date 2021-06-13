using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models
{
    public class SigninInput
    {
        [Required]
        [Display(Name = "Email Adresiniz")]
        public string Email { get; set; }
        [Display(Name = "Şifreniz")]
        [Required]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Beni Hatırla")]
        public bool IsRemember { get; set; }

    }
}
