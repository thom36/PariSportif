using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PariSportif.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string? HomeTeam { get; set; }
        public string? AwayTeam { get; set; }
        public List<Odd> Odds { get; set; } = new List<Odd>();

    }
}