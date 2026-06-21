using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PariSportif.Models;
using PariSportif.Dto;

namespace PariSportif.Services
{
    public interface IMatchService
    {
        public Task<List<MatchDto>> GetAllMatchesAsync();
        public Task<List<MatchDto>> GetMatchesByTeam(string team);
        public Task<List<OddDto>> GetOddsByMatchId(int id);
        public Task AddMatches(Match match);
    }
}