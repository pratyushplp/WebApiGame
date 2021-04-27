using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiGame.Dtos.CharacterSkill
{
    public class CharacterSkillWriteDto
    {
        public int CharacterId { get; set; }
        public int SkillId { get; set; }
    }
}
