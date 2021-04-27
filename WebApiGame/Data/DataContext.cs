using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGame.Models;

namespace WebApiGame.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {

        }
        public DbSet<Character> Characters { get; set;}
        public DbSet<User> Users { get; set;}
        public DbSet<Weapon> Weapons { get; set;}
        public DbSet<Skill> Skills { get; set;}
        public DbSet<CharacterSkill> CharacterSkills { get; set;}

        //this method defines the shape of the entity there relations and how the map to the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // creating a new primary key for CharacterSkill table which is a composite key of character and skill
            modelBuilder.Entity<CharacterSkill>().HasKey(x => new { x.CharacterId, x.SkillId }); // note:this works because of the naming conventiom of CharacterId and SkillId, if not so we have to use fluent api
        }

    }
}
