using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGame.Dtos.Character;
using WebApiGame.Models;

namespace WebApiGame.Services
{
    public class MockCharacterService : ICharacterServiceAsync
    {
        private static List<Character> characterList = new List<Character>()
        {
            new Character{ Id = 1, Name = "Levi", User = new User {Id = 1, UserName="Sam" } },
            new Character{ Id = 1, Name = "Eren", Rpgclass= RpgClass.Mage , User = new User {Id = 2, UserName="Pam" } }

            //new Character(1,"Aaron", 100,100,100,100,RpgClass.Mage),
            //new Character(2,"Boron", 100,100,100,100,RpgClass.Knight),
            //new Character(3,"Carbon", 100,100,100,100,RpgClass.Healer),
            //new Character(4,"Drone", 100,100,100,100,RpgClass.Knight),
        };

        private readonly IMapper _mapper;

        public MockCharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<CharacterReadDto>>> AddCharacter(CharacterWriteDto characterWrite)
        {
            ServiceResponse<List<CharacterReadDto>> serviceResponse = new ServiceResponse<List<CharacterReadDto>>();
            Character character = _mapper.Map<Character>(characterWrite);
            character.Id = characterList.Max(x => x.Id) + 1;
            characterList.Add(character);
            serviceResponse.data = _mapper.Map<List<CharacterReadDto>>(characterList);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<CharacterReadDto>>> GetAllCharacters()
        {
            ServiceResponse<List<CharacterReadDto>> serviceResponse = new ServiceResponse<List<CharacterReadDto>>();
            serviceResponse.data = _mapper.Map<List<CharacterReadDto>>(characterList); 
            return serviceResponse;
        }

        public async Task<ServiceResponse<CharacterReadDto>> GetCharacterById(int Id)
        {
            ServiceResponse<CharacterReadDto> serviceResponse = new ServiceResponse<CharacterReadDto>();
            serviceResponse.data =  _mapper.Map<CharacterReadDto>(characterList.Where(x => x.Id == Id).FirstOrDefault());
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<CharacterReadDto>>> DeleteCharacter(int Id)
        {
            ServiceResponse<List<CharacterReadDto>> serviceResponse = new ServiceResponse<List<CharacterReadDto>>();
            try
            {
                characterList.Remove(characterList.First(x => x.Id == Id));
                serviceResponse.data = _mapper.Map<List<CharacterReadDto>>(characterList);
                serviceResponse.message = "Character deleted successfully";
            }

            catch (Exception ex)
            {
                serviceResponse.data = null;
                serviceResponse.success = false;
                serviceResponse.message = ex.Message;
            }
            return serviceResponse;
        }



        public async Task<ServiceResponse<CharacterReadDto>> UpdateCharacter(CharacterUpdateDto characterUpdate)
        {
            ServiceResponse<CharacterReadDto> serviceResponse = new ServiceResponse<CharacterReadDto>();
            try
            {
                Character updateCharacter = characterList.First(x => x.Id == characterUpdate.Id);
                updateCharacter.Name = characterUpdate.Name;
                updateCharacter.HitPoints = characterUpdate.HitPoints;
                updateCharacter.Strength = characterUpdate.Strength;
                updateCharacter.Defence = characterUpdate.Defence;
                updateCharacter.Intelligence = characterUpdate.Intelligence;
                updateCharacter.Rpgclass = characterUpdate.Rpgclass;

                serviceResponse.data = _mapper.Map<CharacterReadDto>(updateCharacter);
                serviceResponse.message = "Updated character successfully";
            }

            catch (Exception ex)
            {
                serviceResponse.data = null;
                serviceResponse.success = false;
                serviceResponse.message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
