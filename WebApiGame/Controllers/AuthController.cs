using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGame.Dtos.User;
using WebApiGame.Models;
using WebApiGame.Services;

namespace WebApiGame.Controllers

{   
    
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationRepo _authenticationRepo;
        public AuthController(IAuthenticationRepo authenticationRepo)
        {
            _authenticationRepo = authenticationRepo;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegister)
        {
            User user = new User()
            {
                UserName = userRegister.UserName
            };
            ServiceResponse<int> serviceResponse = new ServiceResponse<int>();
            serviceResponse = await _authenticationRepo.Register(user, userRegister.Password);
            if(!serviceResponse.success) return BadRequest(serviceResponse);
            return Ok(serviceResponse);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            ServiceResponse<string> response = await _authenticationRepo.Login(userLogin.UserName, userLogin.Password);
            if (!response.success) return NotFound(response);
            return Ok(response);           
        }




    }
}
