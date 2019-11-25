using FootballApp.Classes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FootballApp.Api
{
    public class Football : IFootball
    {
        private ConfigSettings Api = new ConfigSettings();

        public async Task<List<Country>> LoadCountry()
        {
            string url = $"https://livescore-api.com/api-client/countries/list.json?key={Api.Key}&secret={Api.Secret}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Model country = stream.ReadAndDeserializeFromJson<Model>();

                    return country.data.country;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }

        public async Task<List<Country>> LoadFederation()
        {
            string url = $"https://livescore-api.com/api-client/federations/list.json?key={Api.Key}&secret={Api.Secret}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Model federation = stream.ReadAndDeserializeFromJson<Model>();

                    return federation.data.federation;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }

        public async Task<List<League>> LoadLeague()
        {
            string url = $"https://livescore-api.com/api-client/leagues/list.json?key={Api.Key}&secret={Api.Secret}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Model league = stream.ReadAndDeserializeFromJson<Model>();

                    return league.data.league;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }
        public async Task<List<Competition>> LoadCompetition()
        {
            string url = $"https://livescore-api.com/api-client/competitions/list.json?key={Api.Key}&secret={Api.Secret}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Model competition = stream.ReadAndDeserializeFromJson<Model>();

                    return competition.data.competition;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }

        public async Task<List<Match>> LoadLive()
        {
            string url = $"https://livescore-api.com/api-client/scores/live.json?key={Api.Key}&secret={Api.Secret}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Model match = stream.ReadAndDeserializeFromJson<Model>();
                    
                    return match.data.match;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }

        public async Task<List<Table>> LoadStandings(string leagueId)
        {
            string url = $"https://livescore-api.com/api-client/leagues/table.json?key={Api.Key}&secret={Api.Secret}&league={leagueId}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Model table = stream.ReadAndDeserializeFromJson<Model>();

                    return table.data.table;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }

        public async Task<List<Event>> LoadEvents(string matchId)
        {
            string url = $"https://livescore-api.com/api-client/scores/events.json?key={Api.Key}&secret={Api.Secret}&id={matchId}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Model @event = stream.ReadAndDeserializeFromJson<Model>();
                    
                    return @event.data.@event;
                    
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }


        public async Task<List<Fixture>> LoadFixture(DateTime? date, int pageNo)
        {
            string[] currentDate = date.ToString().Split(' ');
            string[] convertedDate = currentDate[0].Split('/');
            string fixtureDate = $"{convertedDate[2]}-{convertedDate[1]}-{convertedDate[0]}";

            string url = $"https://livescore-api.com/api-client/fixtures/matches.json?key={Api.Key}&secret={Api.Secret}&date={fixtureDate}&page={pageNo}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Model fixture = stream.ReadAndDeserializeFromJson<Model>();

                    return fixture.data.fixtures;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }
    }
}
