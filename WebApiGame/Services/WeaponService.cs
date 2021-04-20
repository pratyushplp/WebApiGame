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

        public ServiceResponse<CharacterReadDto> AddWeapon(WeaponWriteDto weaponWriteDto)
        {
            ServiceResponse<CharacterReadDto> response = new ServiceResponse<CharacterReadDto>();
            try
            {
               //return int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value ?? "0");

                if (_dataContext.Users.Where(x=>x.Id == ))
                {
                    Weapon newWeapon = _mapper.Map<Weapon>(weaponWriteDto);
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
