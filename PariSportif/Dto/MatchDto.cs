using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PariSportif.Models;

namespace PariSportif.Dto
{
    public class MatchDto
    {
        public string HomeTeam { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;
        public List<Odd> Odds { get; set; } = new List<Odd>();
    }
}