using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiGame.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public Character Character { get; set; }


        // In this case, the Weapon class is the dependent side, there cannot be a weapon without a character,
        //so to define this relation of dependent side we create a Id property, with the name of which class it is dependent on as the prefix 
        // i.e Character + Id, now CharacterId will be assigned as the foreign key
        public int CharacterId { get; set; } 
    }
}
