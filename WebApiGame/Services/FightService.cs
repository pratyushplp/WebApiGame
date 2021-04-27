using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGame.Data;
using WebApiGame.Dtos.Fight;
using WebApiGame.Models;

namespace WebApiGame.Services
{
    public class FightService : IFightService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public FightService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }


        public async Task<ServiceResponse<AttackResultDto>> FightSkill(SkillAttackWriteDto skillAttackDto)
        {
            ServiceResponse<AttackResultDto> response = new ServiceResponse<AttackResultDto>();
            try
            {
                AttackResultDto attackResult = new AttackResultDto();
                Character Attacker = await _dataContext.Characters.Include(x => x.CharacterSkill).ThenInclude(x=>x.Skill).FirstOrDefaultAsync(x => x.Id == skillAttackDto.AttackerId);
                Character Opponent = await _dataContext.Characters.FirstOrDefaultAsync(x => x.Id == skillAttackDto.OpponentId);

                if (Attacker == null || Opponent == null)
                {
                    response.data = null;
                    response.success = false;
                    response.message = "Invalid Attacker or Opponent";
                }
                else
                {

                    CharacterSkill characterSkill = Attacker.CharacterSkill.FirstOrDefault(x => x.Skill.Id == skillAttackDto.SkillId);
                    if (characterSkill == null)
                    {
                        response.data = null;
                        response.success = false;
                        response.message = $"{Attacker.Name} does not know that skill";
                    }
                    else
                    {
                        int damageVal = SkillDamage(Attacker, Opponent, characterSkill);

                        attackResult.Attacker = Attacker.Name ?? "";
                        attackResult.Opponent = Opponent.Name ?? "";
                        attackResult.AttackerHP = Attacker.HitPoints;
                        attackResult.Damage = damageVal > 0 ? damageVal : 0;
                        attackResult.OpponentHP = attackResult.Damage > 0 ? (Opponent.HitPoints - attackResult.Damage) : Opponent.HitPoints;

                        Opponent.HitPoints = attackResult.OpponentHP;
                        _dataContext.Characters.Update(Opponent);
                        await _dataContext.SaveChangesAsync();
                        response.data = attackResult;
                        if (Opponent.HitPoints <= 0)
                        {
                            response.message = "Opponent Has Been Defeated";
                        }

                    }

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



        public async Task<ServiceResponse<AttackResultDto>> FightWeapon(WeaponAttackWriteDto weaponAttackDto)
        {
            ServiceResponse<AttackResultDto> response = new ServiceResponse<AttackResultDto>();
            try
            {
                AttackResultDto attackResult = new AttackResultDto();
                Character Attacker = await _dataContext.Characters.Include(x=>x.Weapon).FirstOrDefaultAsync(x => x.Id == weaponAttackDto.AttackerId);
                Character Opponent = await _dataContext.Characters.FirstOrDefaultAsync(x => x.Id == weaponAttackDto.OpponentId);

                if(Attacker == null || Opponent == null)
                {
                    response.data = null;
                    response.success = false;
                    response.message = "Invalid AttackerId or OpponentId";
                }
                else
                {
                    int damageVal = WeaponAttack(Attacker, Opponent);

                    attackResult.Attacker = Attacker.Name ?? "";
                    attackResult.Opponent = Opponent.Name ?? "";
                    attackResult.AttackerHP = Attacker.HitPoints;
                    attackResult.Damage = damageVal;
                    attackResult.OpponentHP =  Opponent.HitPoints - damageVal;

                    Opponent.HitPoints = attackResult.OpponentHP;
                    _dataContext.Characters.Update(Opponent);
                    await _dataContext.SaveChangesAsync();
                    response.data = attackResult;
                    if (Opponent.HitPoints <= 0)
                    {
                        response.message = "Opponent Has Been Defeated";
                    }

                }
            }

            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;

            }
            return response;
        }

        public async Task<ServiceResponse<FightResultReadDto>> Fight(FightRequestWriteDto fightRequestDto)
        {
            ServiceResponse<FightResultReadDto> response = new ServiceResponse<FightResultReadDto>();
            FightResultReadDto fightResult = new FightResultReadDto();
            try
            {
                List<Character> characters = await _dataContext.Characters.Include(x => x.Weapon)
                                              .Include(x => x.CharacterSkill).ThenInclude(x => x.Skill)
                                              .Where(x => fightRequestDto.characterList.Contains(x.Id)).ToListAsync();

                if(characters == null || characters.Count <= 0 || fightRequestDto.characterList.Count != characters.Count)
                {
                    response.success = false;
                    response.message = "Invalid Characters";
                    return response;
                }

                bool isDefeated = false;
                List<Character> opponents = new List<Character>();
                Character opponent = new Character();
                int damage = 0;

                HashSet<int> involvedCharacterId = new HashSet<int>();

                while(!isDefeated)
                {
                    foreach (Character attacker in characters)
                    {
                        if(!involvedCharacterId.Contains(attacker.Id))
                        {
                            involvedCharacterId.Add(attacker.Id);
                        }
                        opponents = characters.Where(x => x.Id != attacker.Id).ToList();
                        opponent = opponents[new Random().Next(opponents.Count)];

                        bool isWeaponAttack = new Random().Next(2) == 0 ? true : false;
                        if(isWeaponAttack)
                        {
                            damage = WeaponAttack(attacker, opponent);
                            opponent.HitPoints = opponent.HitPoints - damage;

                            fightResult.log.Add($"{attacker.Name} attacked {opponent.Name} with {attacker.Weapon.Name}, dealing {damage} damage");
                        }
                        else
                        {
                            CharacterSkill characterSkill = attacker.CharacterSkill[new Random().Next(attacker.CharacterSkill.Count)];
                            damage = SkillDamage(attacker, opponent, characterSkill);
                            opponent.HitPoints = opponent.HitPoints - damage;

                            fightResult.log.Add($"{attacker.Name} attacked {opponent.Name} with {characterSkill.Skill.Name}, dealing {damage} damage");

                        }


                        if (opponent.HitPoints <= 0)
                        {
                            isDefeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            fightResult.log.Add($"{opponent.Name} has been defeated");
                            fightResult.log.Add($"{attacker.Name} won the match with {attacker.HitPoints} hp left");
                            break;
                        }



                        //[new Random().Next(characters.Count)]

                    }
                }

                foreach(Character c in characters.Where(x => involvedCharacterId.Contains(x.Id)))
                {
                    c.Fights++;
                    c.HitPoints = 1000;
                }

                _dataContext.Characters.UpdateRange(characters);
                _dataContext.SaveChanges();

                response.data = fightResult;
                response.message = "Fights completed!";


            }

            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<HighScoreReadDto>>> GetHighScore()
        {
            ServiceResponse<List<HighScoreReadDto>> highScore = new ServiceResponse<List<HighScoreReadDto>>();
            List<Character> characterList =  await _dataContext.Characters.Where(x => x.Fights > 0).OrderByDescending(x => x.Victories).ThenBy(x => x.Defeats).Take(10).ToListAsync();

            highScore.data = _mapper.Map<List<HighScoreReadDto>>(characterList);
            return highScore;
        }

        #region private functions
        private static int WeaponAttack(Character Attacker, Character Opponent)
        {
            int damageVal = Attacker.Weapon?.Damage ?? 0;
            damageVal = damageVal + new Random().Next(Attacker.Strength) - new Random().Next(Opponent.Defence);
            return (damageVal > 0 ? damageVal : 0);
        }


        private static int SkillDamage(Character Attacker, Character Opponent, CharacterSkill characterSkill)
        {
            int damageVal = characterSkill.Skill.Damage;
            damageVal = damageVal + new Random().Next(Attacker.Intelligence) - new Random().Next(Opponent.Defence);
            return (damageVal > 0 ? damageVal : 0);
        }
        #endregion


    }
}
