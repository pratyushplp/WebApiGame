using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApiGame.Dtos.Character;
using WebApiGame.Models;

namespace WebApiGame.Profiles
{
    public class CharacterProfile : Profile
    {
        // create a profile for each of the domain object(ie model)
        // a profile links the object and the object to be mapped
        public CharacterProfile()
        {
            CreateMap<CharacterWriteDto,Character>();
            CreateMap<Character,CharacterReadDto>();

        }
    }
}
