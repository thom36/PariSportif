using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PariSportif.Models;

namespace PariSportif.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Matches.Any()) return;

            var matches = new List<Match>
            {
                new Match
                {
                    HomeTeam = "PSG",
                    AwayTeam = "OM",
                    Odds = new List<Odd>
                    {
                        new Odd { OddDescription = "PSG gagne", Value = 1.7 },
                        new Odd { OddDescription = "Match nul", Value = 3.5 },
                        new Odd { OddDescription = "OM gagne", Value = 4.2 }
                    }
                },
                new Match
                {
                    HomeTeam = "Real Madrid",
                    AwayTeam = "Barcelona",
                    Odds = new List<Odd>
                    {
                        new Odd { OddDescription = "Real gagne", Value = 2.0 },
                        new Odd { OddDescription = "Match nul", Value = 3.4 },
                        new Odd { OddDescription = "Barça gagne", Value = 2.3 }
                    }
                }
            };

            context.Matches.AddRange(matches);

            var users = new List<User>
            {
                new User
                {
                    Name = "Dorian",
                    Email = "Dorian@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("pieddansletombeau"),
                    Cash = 0,
                    Freebet = 10
                }
            };
            context.Users.AddRange(users);

            context.SaveChanges();
        }

    }
}