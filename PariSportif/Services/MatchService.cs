using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PariSportif.Models;
using PariSportif.Dto;
using PariSportif.Repositories;
using PariSportif.Exceptions;

namespace PariSportif.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepo;
        public MatchService(IMatchRepository matchRepo)
        {
            _matchRepo = matchRepo;
        }
        public async Task AddMatches(Match match)
        {
            await _matchRepo.Add(match);
        }

        public async Task<List<MatchDto>> GetAllMatchesAsync()
        {
            var res = await _matchRepo.GetAll();
            return res.Select(m => new MatchDto
            {
                HomeTeam = m.HomeTeam ?? string.Empty,
                AwayTeam = m.AwayTeam ?? string.Empty,
                Odds = m.Odds
            }).ToList();
        }

        public async Task<List<MatchDto>> GetMatchesByTeam(string team)
        {
            var res = await _matchRepo.GetByTeam(team);

            if (res == null || !res.Any())
                throw new NotFoundException($"Any matches found for the team {team}");

            return res.Select(m => new MatchDto
            {
                HomeTeam = m.HomeTeam ?? string.Empty,
                AwayTeam = m.AwayTeam ?? string.Empty,
                Odds = m.Odds
            }).ToList();
        }

        public async Task<List<OddDto>> GetOddsByMatchId(int id)
        {
            var res = await _matchRepo.GetByMatchId(id);

            if (res == null || !res.Any())
                throw new NotFoundException($"Any odds found for the match {id}");

            return res.Select(o => new OddDto
            {
                OddDescription = o.OddDescription ?? string.Empty,
                Value = o.Value
            }).ToList();
        }
    }
}