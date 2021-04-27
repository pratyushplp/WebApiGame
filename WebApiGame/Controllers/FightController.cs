using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGame.Dtos.Fight;
using WebApiGame.Services;

namespace WebApiGame.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class FightController: ControllerBase
    {
        private readonly IFightService _fightService;

        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("weapon")]
        public async Task<IActionResult> FightCharactersWeapon(WeaponAttackWriteDto weaponAttackDto)
        {
            return Ok(await _fightService.FightWeapon(weaponAttackDto));
        }

        [HttpPost("skill")]
        public async Task<IActionResult> FightCharactersSkill(SkillAttackWriteDto skillAttackDto)
        {
            return Ok(await _fightService.FightSkill(skillAttackDto));
        }

        [HttpPost]
        public async Task<IActionResult> Fight(FightRequestWriteDto fightRequest)
        {
            return Ok(await _fightService.Fight(fightRequest));
        }

        [HttpGet("highscore")]
        public async Task<IActionResult> GetHighScore()
        {
            return Ok(await _fightService.GetHighScore());
        }


    }
}
