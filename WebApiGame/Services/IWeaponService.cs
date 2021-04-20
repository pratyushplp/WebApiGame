using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGame.Dtos.Character;
using WebApiGame.Dtos.Weapon;

namespace WebApiGame.Services
{
    public interface IWeaponService
    {
        public ServiceResponse<CharacterReadDto> AddWeapon(WeaponWriteDto weaponWriteDto);

    }
}
