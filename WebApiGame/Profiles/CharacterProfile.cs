using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApiGame.Dtos.Character;
using WebApiGame.Dtos.Fight;
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

            // NOTE: what we doing above is, mapping the skill property to CharacterReadDto withoud the need of characterSKill ,
            // If u see the class CharacterReadDto there is no property relating to character skill, that is done so that we can provide direct mapping of skill property
            // Thus by using ForMember and MapFrom we are mapping the skill property of CharacterReadDto directly from CharacterSkill of CharacterClass
            CreateMap<Character,CharacterReadDto>()
                .ForMember(dto => dto.Skills, x=>x.MapFrom(x=>x.CharacterSkill.Select(cs=> cs.Skill)));
            CreateMap<Character, HighScoreReadDto>(); 
        }
    }
}
