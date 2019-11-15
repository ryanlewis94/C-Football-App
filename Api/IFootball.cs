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
        Task<List<Country>> LoadCountry();
        Task<List<Country>> LoadFederation();
        Task<List<League>> LoadLeague();
        Task<List<Competition>> LoadCompetition();
        Task<List<Match>> LoadLive();
        Task<List<Table>> LoadStandings(string leagueId);
        Task<List<Event>> LoadEvents(string matchId);

        Task<List<Fixture>> LoadFixture(DateTime? date, int pageNo);

    }
}
