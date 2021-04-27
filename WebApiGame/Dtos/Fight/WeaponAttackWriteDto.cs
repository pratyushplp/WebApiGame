using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiGame.Dtos.Fight
{
    public class WeaponAttackWriteDto
    {
        public int AttackerId { get; set; }// We have 1 to 1 relation between character and weapon so we dont need a weapon ID
        public int OpponentId { get; set; }
    }
}
