using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.IdentityServer.Dto
{
    public class SignupDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string City { get; set; }

    }
}
