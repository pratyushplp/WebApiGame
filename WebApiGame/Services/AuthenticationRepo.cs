using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiGame.Data;
using WebApiGame.Models;

namespace WebApiGame.Services
{
    public class AuthenticationRepo : IAuthenticationRepo
    {
        private readonly DataContext _dbContext;
        private readonly IConfiguration _configuration ;

        public AuthenticationRepo(DataContext dataContext, IConfiguration configuration)
        {
            _dbContext = dataContext;
            _configuration = configuration;
        }
        public async Task<ServiceResponse<string>> Login(string userName, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(userName.ToLower()));
            if(user == null)
            {
                response.success = false;
                response.message = "Invalid username or password";
                return response;
            }
            else
            {
               if(!ValidatePassword(password, user.PasswordHash, user.PasswordSalt))
               {
                 response.success = false;
                 response.message = "Invalid username or password";
               }
                else
                {
                    //response.data = user.Id.ToString();
                    response.data = CreateToken(user);
                }
                return response;
            }
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> result = new ServiceResponse<int>();
            try
            {
                if(await UserExists(user.UserName))
                {
                    result.success = false;
                    result.message = "Username already taken";
                    return result;
                }

                CreatePasswordHash(password, out byte[] hashPassword, out byte[] saltPassword);
                user.PasswordHash = hashPassword;
                user.PasswordSalt = saltPassword;
                await _dbContext.Users.AddAsync(user);
                _dbContext.SaveChanges();
                result.data = user.Id;
            }

            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;            
            }

            return result;
        }

        public async Task<bool> UserExists(string userName)
        {
            if (await _dbContext.Users.AnyAsync(x => !string.IsNullOrWhiteSpace(userName) && x.UserName.ToLower().Equals(userName.ToLower())))  return true;
            return false;
        }


        private void CreatePasswordHash(string password, out byte[] hashValue, out byte[] saltValue)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                saltValue = hmac.Key;
                hashValue = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));// converts password to array of bytes, and computes the hash of those bytes
            }
        }

        private bool ValidatePassword(string password,byte[] hashValue, byte[] saltvalue)
        {

            var result = new System.Security.Cryptography.HMACSHA512(saltvalue);
            var newHash = result.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i< hashValue.Length; i++)
            {
                if(newHash[i] != hashValue[i]) return false;
            }
            return true;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            //Symmetric key uses the same key to encrypt and decrypt, assymetric keys uses a key to encrypt and a diferent key to decrypt
            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            //gives the description of the token, like what is the expiry date of the token, what is the signing credential of the token
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor 
            { 
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(3),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
