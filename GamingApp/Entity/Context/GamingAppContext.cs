using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingApp.Entity.Context
{
    public class GamingAppContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySql(@"server=localhost;port=3306;database=gamingappdb;user=root;password=can1912*;CharSet=utf8;", ServerVersion.AutoDetect("server=localhost;port=3306;database=gamingappdb;user=root;password=can1912*;CharSet=utf8;"));


            optionsBuilder.UseMySql(@"server=213.14.179.76;port=3306;database=gamingappdb;user=ceysin;password=can1912;CharSet=utf8;", ServerVersion.AutoDetect("server=213.14.179.76;port=3306;database=gamingappdb;user=ceysin;password=can1912;CharSet=utf8;"));
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameUser>().HasKey(x => new { x.UserId, x.GameId });
        }

        public DbSet<User> User { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<GameUser> GameUser { get; set; }
        public DbSet<SessionUser> SessionUser { get; set; }
        public  DbSet<Logs> Logs { get; set; }
        public DbSet<OyunTuru> OyunTuru { get; set; }

    }
}
