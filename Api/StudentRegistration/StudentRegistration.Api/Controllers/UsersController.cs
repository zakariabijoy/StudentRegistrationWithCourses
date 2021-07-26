using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Api.DTOs;
using StudentRegistration.Api.Services;
using StudentRegistration.DataAccess.Repository.Interfaces;
using StudentRegistration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public UsersController(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            using var hmac = new HMACSHA512();

            var user = new User
            {
                Name = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

           var u= await _unitOfWork.Users.AddUserAsync(user);
            return new UserDto
            {
                UserName = u.Name,
                Token = _tokenService.CreateToken(u)
            };
           
        }

        //[HttpPost("login")]
        //public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        //{
        //    var user = await _unitOfWork.Users.GetByNameAsync(loginDto.UserName.ToLower());

        //    if (user == null) return Unauthorized("Invalid UserName");

        //    using var hmac = new HMACSHA512(user.PasswordSalt);

        //    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        //    for (int i = 0; i < computedHash.Length; i++)
        //    {
        //        if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        //    }

        //    return new UserDto
        //    {
        //        UserName = user.Name,
        //        Token = _tokenService.CreateToken(user)
        //    };
        //}



         [HttpPost("Auth")]
        public async Task<IActionResult> Auth([FromBody] TokenRequestDto model) // granttype = "refresh_token"
        {
            // We will return Generic 500 HTTP Server Status Error
            // If we receive an invalid payload
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            switch(model.GrantType)
                {
                case "password":
                    return await GenerateNewToken(model);
                case "refresh_token":
                    return await RefreshToken(model);
                default:
                    // not supported - return a HTTP 401 (Unauthorized)
                    return new UnauthorizedResult();
            }

        }


        // Method to Create New JWT and Refresh Token
        private async Task<IActionResult> GenerateNewToken(TokenRequestDto model) 
        {
            // check if there's an user with the given username
            var user = await _unitOfWork.Users.GetByNameAsync(model.UserName.ToLower());

            if (user == null) return BadRequest("Invalid UserName");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return BadRequest("Invalid Password");
            }



            // username & password matches: create the refresh token
            var newRtoken = _tokenService.CreateRefreshToken(user.UserId);

            // first we delete any existing old refreshtokens
            await _unitOfWork.Tokens.DeleteAsync(user.UserId);


            // Add new refresh token to Database
            await _unitOfWork.Tokens.AddAsync(newRtoken);

            // Create & Return the access token which contains JWT and Refresh Token

            var accessToken = await _tokenService.CreateAccessToken(user, newRtoken.Value);


            return Ok(new {authToken = accessToken });



        }







        // Method to Refresh JWT and Refresh Token
        private async Task<IActionResult> RefreshToken(TokenRequestDto model) 
        {
            try
            {
                // check if the received refreshToken exists for the given clientId
                var rt = await _tokenService.IfRefreshTokenExists(model);
                    


                if (rt == null)
                {
                    // refresh token not found or invalid (or invalid clientId)
                    return new UnauthorizedResult();
                }

                // check if refresh token is expired
                if (rt.ExpiryTime < DateTime.Now)
                {
                    return new UnauthorizedResult();
                }

                // check if there's an user with the refresh token's userId
                var user = await _unitOfWork.Users.GetByIdAsync(rt.UserId);


                if (user == null)
                {
                    // UserId not found or invalid
                    return new UnauthorizedResult();
                }

                // generate a new refresh token 

                var rtNew = _tokenService.CreateRefreshToken(user.UserId);

                // invalidate the old refresh token (by deleting it)
                 await _unitOfWork.Tokens.DeleteAsync(rt.Id);

                // add the new refresh token
                await _unitOfWork.Tokens.AddAsync(rtNew);
           


                var response = await _tokenService.CreateAccessToken(user, rtNew.Value);

                return Ok(new { authToken = response });

            }
            catch (Exception ex)
            {
               
                return new UnauthorizedResult();
            }
        }

    }


}

