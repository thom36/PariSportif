using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PariSportif.Models;
using PariSportif.Dto;
using Microsoft.EntityFrameworkCore;

namespace PariSportif.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly AppDbContext _context;
        public MatchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(Match match)
        {
            _context.Add(match);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Match>> GetAll()
        {
            return await _context.Matches
                            .Include(m => m.Odds)
                            .ToListAsync();
        }

        public async Task<List<Match>> GetByTeam(string team)
        {
            return await _context.Matches
                    .Include(m => m.Odds)
                    .Where(m => m.HomeTeam == team || m.AwayTeam == team)
                    .ToListAsync();
        }

        public async Task<List<Odd>> GetByMatchId(int id)
        {
            return await _context.Odds.Where(m => m.MatchId == id).ToListAsync();
        }
    }
}