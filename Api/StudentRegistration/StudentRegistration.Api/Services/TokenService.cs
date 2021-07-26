using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentRegistration.Api.DTOs;
using StudentRegistration.DataAccess.Repository.Interfaces;
using StudentRegistration.Model;

namespace StudentRegistration.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IConfiguration _config;

        private readonly IUnitOfWork _unitOfWork;

        public TokenService(IConfiguration config,  IUnitOfWork unitOfWork)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokenkey"]));
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public async Task<TokenResponseDto> CreateAccessToken(User user, string refreshToken)
        {
            double tokenExpiryTime = double.Parse(_config["TokenLifeTimeInMinute"]);

            var key = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);


            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                         new Claim(JwtRegisteredClaimNames.NameId, user.Name)

                     }),

                SigningCredentials = key,
                Expires = DateTime.Now.AddMinutes(tokenExpiryTime)
            };

            // Generate token

            var newtoken = tokenHandler.CreateToken(tokenDescriptor);

            var encodedToken = tokenHandler.WriteToken(newtoken);

            return new TokenResponseDto()
            {
                Token = encodedToken,
                Expiration = newtoken.ValidTo,
                RefreshToken = refreshToken,
                UserName = user.Name
            };
        }

        public Token CreateRefreshToken( int userId)
        {
            return new Token()
            {
                ClientId = _config["ClientId"],
                UserId = userId,
                Value = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.Now,
                ExpiryTime = DateTime.Now.AddMinutes(90)
            };
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Name)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(double.Parse(_config["TokenLifeTimeInMinute"])),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

        public async  Task<Token> IfRefreshTokenExists(TokenRequestDto tokenRequestDto)
        {
           return await _unitOfWork.Tokens.GetByClientIdAndRefreshtokenAsync(_config["ClientId"], tokenRequestDto.RefreshToken);
        }
    
    }
}
