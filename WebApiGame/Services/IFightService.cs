

using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiGame.Dtos.Fight;

namespace WebApiGame.Services
{
    public interface IFightService
    {
        public Task<ServiceResponse<AttackResultDto>> FightWeapon(WeaponAttackWriteDto weaponAttackDto);
        public Task<ServiceResponse<AttackResultDto>> FightSkill(SkillAttackWriteDto skillAttackDto);
        public Task<ServiceResponse<FightResultReadDto>> Fight(FightRequestWriteDto fightRequestDto);
        public Task<ServiceResponse<List<HighScoreReadDto>>> GetHighScore();

    }
}
