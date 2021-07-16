using StudentRegistration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.Api.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
