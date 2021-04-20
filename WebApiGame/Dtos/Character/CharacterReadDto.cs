using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGame.Models;

namespace WebApiGame.Dtos.Character
{
    public class CharacterReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int HitPoints { get; set; }

        public int Strength { get; set; }

        public int Defence { get; set; }

        public int Intelligence { get; set; }

        public RpgClass Rpgclass { get; set; } = RpgClass.Knight;
    }
}
