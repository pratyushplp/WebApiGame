using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGame.Dtos.CharacterSkill;
using WebApiGame.Dtos.Weapon;
using WebApiGame.Services;

namespace WebApiGame.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CharacterSkillController : ControllerBase
    {

        private ICharacterSkillService _characterSkillService;
        public CharacterSkillController(ICharacterSkillService characterSkillService)
        {
            _characterSkillService = characterSkillService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacterSkill(CharacterSkillWriteDto characterSkillWriteDto)
        {
            return Ok(await _characterSkillService.AddCharacterSkill(characterSkillWriteDto));
        }








    }
}
