namespace PariSportif.tests;

using Moq;
using PariSportif.Models;
using PariSportif.Repositories;
using PariSportif.Services;
using FluentAssertions;
using PariSportif.Exceptions;

public class MatchServiceTest
{
    private readonly List<PariSportif.Models.Match> matches = new()
    {
        new PariSportif.Models.Match
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
        new PariSportif.Models.Match
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

    // -----------------------------
    // GET ALL MATCHES
    // -----------------------------
    [Fact]
    public async Task TestGetAllMatchesAsync()
    {
        var repoMock = new Mock<IMatchRepository>();

        repoMock.Setup(r => r.GetAll())
                .ReturnsAsync(matches);

        var service = new MatchService(repoMock.Object);

        var result = await service.GetAllMatchesAsync();

        result.Should().HaveCount(2);
        result.First().HomeTeam.Should().Be("PSG");
    }

    // -----------------------------
    // GET MATCHES BY TEAM
    // -----------------------------
    [Fact]
    public async Task TestGetMatchesByTeam()
    {
        var repoMock = new Mock<IMatchRepository>();
        var team = "PSG";

        repoMock.Setup(r => r.GetByTeam(team))
                .ReturnsAsync(matches
                    .Where(m => m.HomeTeam == team || m.AwayTeam == team)
                    .ToList());

        var service = new MatchService(repoMock.Object);

        var result = await service.GetMatchesByTeam(team);

        result.Should().HaveCount(1);
        result.First().HomeTeam.Should().Be("PSG");
    }

    [Fact]
    public async Task TestGetMatchesByTeamNotExist()
    {
        var repoMock = new Mock<IMatchRepository>();
        var team = "OL";

        repoMock.Setup(r => r.GetByTeam(team))
                .ReturnsAsync(new List<PariSportif.Models.Match>());

        var service = new MatchService(repoMock.Object);

        var ex = await Assert.ThrowsAsync<NotFoundException>(
            () => service.GetMatchesByTeam(team)
        );

        Assert.Equal($"Any matches found for the team {team}", ex.Message);
    }

    // -----------------------------
    // GET ODDS BY MATCH ID
    // -----------------------------
    [Fact]
    public async Task TestGetOddsByMatchId()
    {
        var repoMock = new Mock<IMatchRepository>();
        var id = 1;

        repoMock.Setup(r => r.GetByMatchId(id))
                .ReturnsAsync(matches[0].Odds);

        var service = new MatchService(repoMock.Object);

        var result = await service.GetOddsByMatchId(id);

        result.Should().HaveCount(3);
        result.First().OddDescription.Should().Be("PSG gagne");
        result.First().Value.Should().Be(1.7);
    }

    [Fact]
    public async Task TestGetOddsByMatchIdInvalid()
    {
        var repoMock = new Mock<IMatchRepository>();
        var id = 5;

        repoMock.Setup(r => r.GetByMatchId(id))
                .ReturnsAsync(new List<Odd>());

        var service = new MatchService(repoMock.Object);

        var ex = await Assert.ThrowsAsync<NotFoundException>(
            () => service.GetOddsByMatchId(id)
        );

        Assert.Equal($"Any odds found for the match {id}", ex.Message);
    }
}