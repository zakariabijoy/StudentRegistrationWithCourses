using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.Api.DTOs
{
    public class TokenRequestDto
    {
        public string GrantType { get; set; }
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
        public string Password { get; set; }
    }
}
