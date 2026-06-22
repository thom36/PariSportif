using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PariSportif.Services;
using PariSportif.Models;
using PariSportif.Exceptions;

namespace PariSportif.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _service;
        public MatchController(IMatchService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMatches()
        {
            var matches = await _service.GetAllMatchesAsync();
            return Ok(matches);
        }

        [HttpPost]
        public async Task<IActionResult> AddMatch([FromBody] Match match)
        {
            if (match == null) return BadRequest();
            var created = _service.AddMatches(match);
            return CreatedAtAction("match created", new { id = created.Id }, created);
        }

        [HttpGet("byTeam{team}")]
        public async Task<IActionResult> GetAllMatchesByTeam(string team)
        {
            try
            {
                var matches = await _service.GetMatchesByTeam(team);
                return Ok(matches);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("odds/{matchId}")] // nom dans la route doit être le même qu'en argument
        public async Task<IActionResult> GetOddsByMatchId(int matchId)
        {
            try
            {
                var matches = await _service.GetOddsByMatchId(matchId);
                return Ok(matches);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}