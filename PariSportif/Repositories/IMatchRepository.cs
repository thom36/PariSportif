using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PariSportif.Dto;
using PariSportif.Models;

namespace PariSportif.Repositories
{
    public interface IMatchRepository
    {
        Task Add(Match match);
        Task<List<Match>> GetAll();
        Task<List<Match>> GetByTeam(string team);
        Task<List<Odd>> GetByMatchId(int id);
    }
}