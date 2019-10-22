using FootballApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FootballApp.Api
{
    public class Football : IFootball
    {
        private ConfigSettings Api = new ConfigSettings();
        public async Task<List<Country>> LoadCountry()
        {

            string url = $"http://livescore-api.com/api-client/countries/list.json?key={Api.Key}&secret={Api.Secret}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    Model country = await response.Content.ReadAsAsync<Model>();

                    return country.data.country;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<League>> LoadLeague()
        {
            string url = $"http://livescore-api.com/api-client/leagues/list.json?key={Api.Key}&secret={Api.Secret}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    Model league = await response.Content.ReadAsAsync<Model>();

                    return league.data.league;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<Match>> LoadLive()
        {
            string url = $"http://livescore-api.com/api-client/scores/live.json?key={Api.Key}&secret={Api.Secret}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    Model match = await response.Content.ReadAsAsync<Model>();
                    
                    return match.data.match;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<Table>> LoadStandings(string leagueId)
        {
            string url = $"http://livescore-api.com/api-client/leagues/table.json?key={Api.Key}&secret={Api.Secret}&league={leagueId}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    Model table = await response.Content.ReadAsAsync<Model>();

                    return table.data.table;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<Event>> LoadEvents(string matchId)
        {
            string url = $"http://livescore-api.com/api-client/scores/events.json?key={Api.Key}&secret={Api.Secret}&id={matchId}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    Model @event = await response.Content.ReadAsAsync<Model>();

                    return @event.data.@event;
                }
                else
                {
                    return null;
                }
            }
        }


        public async Task<List<Fixture>> LoadFixture(DateTime? date, int pageNo)
        {
            string[] currentDate = date.ToString().Split(' ');
            string[] convertedDate = currentDate[0].Split('/');
            string fixtureDate = $"{convertedDate[2]}-{convertedDate[1]}-{convertedDate[0]}";

            string url = $"http://livescore-api.com/api-client/fixtures/matches.json?key={Api.Key}&secret={Api.Secret}&date={fixtureDate}&page={pageNo}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    Model fixture = await response.Content.ReadAsAsync<Model>();

                    return fixture.data.fixtures;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
