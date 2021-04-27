using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiGame.Data;
using WebApiGame.Dtos.Character;
using WebApiGame.Dtos.CharacterSkill;
using WebApiGame.Models;

namespace WebApiGame.Services
{
    public class CharacterSkillService : ICharacterSkillService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterSkillService(DataContext dataContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<CharacterReadDto>> AddCharacterSkill(CharacterSkillWriteDto characterSkillWriteDto)
        {
            ServiceResponse<CharacterReadDto> response = new ServiceResponse<CharacterReadDto>();
            try
            {
                Character character = await _dataContext.Characters
                                                            .Include(x=>x.Weapon)
                                                            .Include(x=>x.CharacterSkill).ThenInclude(x=>x.Skill)
                                                            .FirstOrDefaultAsync(x => x.User.Id == GetId()
                                                            && x.Id == characterSkillWriteDto.CharacterId);

                if(character != null)
                {

                    Skill skill = await _dataContext.Skills.FirstOrDefaultAsync(x => x.Id == characterSkillWriteDto.SkillId);
                    if(skill == null )
                    {
                        response.data = null;
                        response.success = false;
                        response.message = "Invalid Skill";
                    }
                    else
                    {
                        //NOTE:  Q)how is the CharacterId and SkillId is asigned for CharacterSkill class?,
                        //Ans These properties are automatically assigned by entity framework BUT only because of the naming convention "Chatacter"+"Id", CharacterId
                        // the Corresponding Id property of Character Class is assigned to CharacterID
                        // similar for SkillId in the CharacterSkill class.
                        CharacterSkill characterSkill = new CharacterSkill()
                        {
                            Character = character,
                            Skill = skill                                             
                        };

                        await _dataContext.CharacterSkills.AddAsync(characterSkill);
                        await _dataContext.SaveChangesAsync();

                        response.data = _mapper.Map<CharacterReadDto>(character);
                    }

                }
                else
                {
                    response.data = null;
                    response.success = false;
                    response.message = "Invalid User or Character";
                }
            }
            catch (Exception ex)
            {
                response.data = null;
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }

        private int GetId()
        {
            return int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
