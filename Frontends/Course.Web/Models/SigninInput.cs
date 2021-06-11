using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models
{
    public class SigninInput
    {
        [Display(Name = "Email Adresiniz")]
        public string Email { get; set; }
        [Display(Name = "Şifreniz")]
        public string Password { get; set; }
        [Display(Name = "Beni Hatırla")]
        public bool IsRemember { get; set; }

    }
}
