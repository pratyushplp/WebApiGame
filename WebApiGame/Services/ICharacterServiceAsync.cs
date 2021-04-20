using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGame.Dtos.Character;
using WebApiGame.Models;

namespace WebApiGame.Services
{
    public interface ICharacterServiceAsync
    {
        public Task<ServiceResponse<List<CharacterReadDto>>> AddCharacter(CharacterWriteDto characterWrite);
        public Task<ServiceResponse<List<CharacterReadDto>>> GetAllCharacters();
        public Task<ServiceResponse<CharacterReadDto>> GetCharacterById(int id);
        public Task<ServiceResponse<CharacterReadDto>> UpdateCharacter(CharacterUpdateDto characterUpdate);
        public Task<ServiceResponse<List<CharacterReadDto>>> DeleteCharacter(int id);

    }
}
