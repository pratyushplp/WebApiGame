using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiGame.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int HitPoints { get; set; }

        public int Strength { get; set; }

        public int Defence { get; set; }

        public int Intelligence { get; set; }

        public RpgClass Rpgclass { get; set; } = RpgClass.Knight;

        public User User { get; set; }

        public Weapon Weapon { get; set; }

        public List<CharacterSkill> CharacterSkill { get; set; }
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }

        public Character()
        {        }
        public Character(int id, string name, int hitPoints, int strength, int defence, int intelligence, RpgClass rpg)
        {
            this.Id = id;
            this.Name = name;
            this.HitPoints = hitPoints;
            this.Strength = strength;
            this.Defence = defence;
            this.Intelligence = intelligence;
            this.Rpgclass = rpg;
        }


    }
}
