using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGame.Dtos.Weapon;
using WebApiGame.Models;

namespace WebApiGame.Profiles
{
    public class WeaponProfile : Profile
    {
        public WeaponProfile()
        {
            CreateMap<WeaponWriteDto, Weapon>();
            CreateMap<Weapon, WeaponReadDto>();
        }

    }
}
