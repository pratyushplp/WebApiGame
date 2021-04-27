using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGame.Dtos.Skill;
using WebApiGame.Models;

namespace WebApiGame.Profiles
{
    public class SkillProfile : Profile
    {
        public SkillProfile()
        {
            CreateMap<Skill, SkillReadDto>();
        }

    }
}
