using FootballApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FootballApp.Api
{
    public interface IFootball
    {
        int CountApiCalls();

        Task<List<Country>> LoadCountry();
        Task<List<Country>> LoadFederation();
        Task<List<League>> LoadLeague();
        Task<List<Competition>> LoadCompetition();
        Task<List<Match>> LoadLive();
        Task<List<Match>> LoadPast(DateTime? date, int pageNo);
        Task<List<Match>> LoadPastForTeam(string teamId);
        Task<List<Table>> LoadStandings(string leagueId);
        Task<List<Event>> LoadEvents(string matchId);

        Task<List<Fixture>> LoadFixture(DateTime? date, int pageNo);
        Task<Data> LoadTeamsH2H(string homeId, string awayId);
        Task<Data> LoadStats(string matchId);
    }
}
