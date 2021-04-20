using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiGame.Dtos.Character;
using WebApiGame.Models;
using WebApiGame.Services;

namespace WebApiGame.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase // NOTE: inherit from Controller if u want to enable view support, controllerBase enough for api 
    {
        private readonly ICharacterServiceAsync _characterService;


        //Dependency Injection for characterService 
        public CharacterController(ICharacterServiceAsync characterService)
        {
            _characterService = characterService;
        }

        //[AllowAnonymous] // if we use allow anonymous nonauthentication is required to call this method
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            // User object is present inside controllerBase class, which contains the data about the claims present in the token
            //We login using authcontroller which inturn provides us a token for authentication, and to call the character controller we require that token 
            //int userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}", Name = "GetCharacterById")]
        public async Task<IActionResult> GetCharacterById(int id)
        {
            var value = await _characterService.GetCharacterById(id);
            if (value.data == null) return NotFound(value);
            return Ok(value);
        }

        [HttpPost("AddCharacter")]
        public async Task<IActionResult> AddCharacter(CharacterWriteDto characterWrite)
        {
            return Ok(await _characterService.AddCharacter(characterWrite));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCharacter(CharacterUpdateDto characterUpdateDto)
        {
            ServiceResponse <CharacterReadDto> serviceResponse = await _characterService.UpdateCharacter(characterUpdateDto);
            if (serviceResponse.data == null) return NotFound(serviceResponse);
            return Ok(serviceResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            ServiceResponse<List<CharacterReadDto>> serviceResponse = await _characterService.DeleteCharacter(id);
            if(serviceResponse.data == null) return NotFound(serviceResponse);
            return Ok(serviceResponse);
        }

    }
}
