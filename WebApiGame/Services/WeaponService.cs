using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiGame.Data;
using WebApiGame.Dtos.Character;
using WebApiGame.Dtos.Weapon;
using WebApiGame.Models;

namespace WebApiGame.Services
{
    public class WeaponService : IWeaponService
    {
        private DataContext _dataContext;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;


        public WeaponService(DataContext dataContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async  Task<ServiceResponse<CharacterReadDto>> AddWeapon(WeaponWriteDto weaponWriteDto)
        {
            ServiceResponse<CharacterReadDto> response = new ServiceResponse<CharacterReadDto>();
            try
            {
                Character character = _dataContext.Characters.FirstOrDefault(x => x.User.Id == GetId() && x.Id == weaponWriteDto.CharacterId);

                if (character != null)
                {
                    Weapon newWeapon = _mapper.Map<Weapon>(weaponWriteDto);
                    _dataContext.Weapons.Add(newWeapon);
                    await _dataContext.SaveChangesAsync();

                    response.data = _mapper.Map<CharacterReadDto>(character);
                }
                else
                {
                    response.data = null;
                    response.success = false;
                    response.message ="Character for corresponding user not found";
                }

            }

            catch(Exception ex)
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
