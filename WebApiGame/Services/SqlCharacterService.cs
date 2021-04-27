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
using WebApiGame.Models;

namespace WebApiGame.Services
{
    public class SqlCharacterService : ICharacterServiceAsync
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SqlCharacterService(IMapper mapper, DataContext dBcontext, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _dbContext = dBcontext;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<ServiceResponse<List<CharacterReadDto>>> AddCharacter(CharacterWriteDto characterWrite)
        {
            ServiceResponse<List<CharacterReadDto>> serviceResponse = new ServiceResponse<List<CharacterReadDto>>();
            Character character = _mapper.Map<Character>(characterWrite);
            character.User = _dbContext.Users.FirstOrDefault(x => x.Id == getUserId());

            await _dbContext.Characters.AddAsync(character);
            _dbContext.SaveChanges();

            serviceResponse.data = _mapper.Map<List<CharacterReadDto>>(_dbContext.Characters.Where(x=>x.User.Id == getUserId()));
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<CharacterReadDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<CharacterReadDto>> serviceResponse = new ServiceResponse<List<CharacterReadDto>>();
            try
            {
                Character character = await _dbContext.Characters.FirstOrDefaultAsync(x => x.Id == id && x.User.Id == getUserId());
                if (character != null)
                {
                    _dbContext.Characters.Remove(character);
                    _dbContext.SaveChanges();
                    serviceResponse.data = _mapper.Map<List<CharacterReadDto>>(_dbContext.Characters.Where(x => x.User.Id == getUserId()).ToList());
                    serviceResponse.message = "Character deleted successfully";
                }
                else
                {
                    serviceResponse.data = null;
                    serviceResponse.success = false;
                    serviceResponse.message = "Character Not Found.";
                }

            }

            catch (Exception ex)
            {
                serviceResponse.data = null;
                serviceResponse.success = false;
                serviceResponse.message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<CharacterReadDto>>> GetAllCharacters()
        {
            ServiceResponse<List<CharacterReadDto>> serviceResponse = new ServiceResponse<List<CharacterReadDto>>();
            List<Character> characterList = await _dbContext.Characters.Include(x => x.Weapon).Where(x=>x.User.Id == getUserId()).ToListAsync();
            serviceResponse.data = _mapper.Map<List<CharacterReadDto>>(characterList);
            return serviceResponse;
        }

        public async Task<ServiceResponse<CharacterReadDto>> GetCharacterById(int id)
        {
            ServiceResponse<CharacterReadDto> serviceResponse = new ServiceResponse<CharacterReadDto>();
            try
            {
                Character character = await _dbContext.Characters.FirstAsync(x => x.Id == id && x.User.Id == getUserId());
                serviceResponse.data = _mapper.Map<CharacterReadDto>(character);
            }
            catch(Exception ex)
            {
                serviceResponse.data = null;
                serviceResponse.message = ex.Message;
                serviceResponse.success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<CharacterReadDto>> UpdateCharacter(CharacterUpdateDto characterUpdate)
        {
            ServiceResponse<CharacterReadDto> serviceResponse = new ServiceResponse<CharacterReadDto>();
            try
            {
                Character updateCharacter = await _dbContext.Characters.FirstOrDefaultAsync(x=>x.Id == characterUpdate.Id && x.User.Id == getUserId());
                //alternative
                //Character updateCharacter2 = await _dbContext.Characters.Include(x=>x.User).FirstOrDefaultAsync(x => x.Id == characterUpdate.Id);
                //// Note: notice the include above linq method, if we dont include the user(i.eInclude(x=>x.User)) we dont get the user value from character table, 
                //      as we have maintained a relation of user in character table but not stored the complete user value inside character table.
                //if(updateCharacter2.User.Id == getUserId())
                //{
                //    //update here
                //}

                if (updateCharacter!= null)
                {
                    updateCharacter.Name = characterUpdate.Name;
                    updateCharacter.HitPoints = characterUpdate.HitPoints;
                    updateCharacter.Strength = characterUpdate.Strength;
                    updateCharacter.Defence = characterUpdate.Defence;
                    updateCharacter.Intelligence = characterUpdate.Intelligence;
                    updateCharacter.Rpgclass = characterUpdate.Rpgclass;

                    _dbContext.Characters.Update(updateCharacter);
                    _dbContext.SaveChanges();

                    serviceResponse.data = _mapper.Map<CharacterReadDto>(updateCharacter);
                    serviceResponse.message = "Updated character successfully";
                }
                else
                {
                    serviceResponse.data = null;
                    serviceResponse.success = false;
                    serviceResponse.message = "Character Not Found.";
                }



            }

            catch (Exception ex)
            {
                serviceResponse.data = null;
                serviceResponse.success = false;
                serviceResponse.message = ex.Message;
            }
            return serviceResponse;
        }

        private int getUserId()
        {
            return int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value ?? "0");
        }




    }
}
