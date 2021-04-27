using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGame.Dtos.Character;
using WebApiGame.Dtos.CharacterSkill;

namespace WebApiGame.Services
{
    public interface ICharacterSkillService
    {
        public Task<ServiceResponse<CharacterReadDto>> AddCharacterSkill(CharacterSkillWriteDto characterSkillWriteDto);
    }
}
