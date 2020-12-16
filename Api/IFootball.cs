using FootballApp.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FootballApp.Api
{
    public interface IFootball
    {
        List<LeagueLogo> LoadLeagueLogos();
        List<Logo> LoadLogos();
        Task<List<Country>> LoadCountry();
        Task<List<Country>> LoadFederation();
        Task<List<Competition>> LoadCompetition();
        Task<List<Match>> LoadLive();
        Task<List<Match>> LoadPast(DateTime? date, string teamId, int pageNo);
        Task<List<Match>> LoadPastForTeam(string teamId, int pageNo);
        Task<List<Match>> LoadPastForCompetition(string competitionId, int pageNo);
        Task<List<Table>> LoadStandings(string leagueId);
        Task<List<Event>> LoadEvents(string matchId);

        Task<List<Fixture>> LoadFixture(DateTime? date, string teamId, int pageNo);
        Task<Data> LoadTeamsH2H(string homeId, string awayId);
        Task<Data> LoadStats(string matchId);

        Task<List<Goalscorer>> LoadTopGoalscorers(string competitionId);
        Task<Lineup> LoadLineups(string matchId);
    }
}
