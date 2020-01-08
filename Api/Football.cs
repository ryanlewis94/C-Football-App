using FootballApp.Classes;
using FootballApp.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace FootballApp.Api
{
    public class Football : IFootball
    {
        private ConfigSettings Api = new ConfigSettings();

        public List<LeagueLogo> LoadLeagueLogos()
        {
            using (StreamReader r = new StreamReader(@"..\..\Resources\league_list.json"))
            {
                string json = r.ReadToEnd();
                List<LeagueLogo> leagues = JsonConvert.DeserializeObject<List<LeagueLogo>>(json);
                return leagues;
            }
        }

        public List<Logo> LoadLogos()
        {
            using (StreamReader r = new StreamReader(@"..\..\Resources\team_list.json"))
            {
                string json = r.ReadToEnd();
                List<Logo> teams = JsonConvert.DeserializeObject<List<Logo>>(json);
                return teams;
            }
        }



        public async Task<List<Country>> LoadCountry()
        {
            string url = $"https://livescore-api.com/api-client/countries/list.json?key={Api.Key}&secret={Api.Secret}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Model country = stream.ReadAndDeserializeFromJson<Model>();
                    Messenger.Default.Send("request");

                    return country?.data?.country;
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
                    Messenger.Default.Send("request");

                    return federation?.data?.federation;
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
                    Messenger.Default.Send("request");

                    return competition?.data?.competition;
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
                    Messenger.Default.Send("request");

                    return match?.data?.match;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }

        public async Task<List<Table>> LoadStandings(string leagueId)
        {
            string url = $"https://livescore-api.com/api-client/leagues/table.json?key={Api.Key}&secret={Api.Secret}&competition_id={leagueId}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Model table = stream.ReadAndDeserializeFromJson<Model>();
                    Messenger.Default.Send("request");

                    return table?.data?.table;
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
                    Messenger.Default.Send("request");

                    return @event?.data?.@event;
                    
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
                    Messenger.Default.Send("request");

                    return fixture?.data?.fixtures;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }

        public async Task<List<Match>> LoadPast(DateTime? date, int pageNo)
        {
            string[] currentDate = date.ToString().Split(' ');
            string[] convertedDate = currentDate[0].Split('/');
            string pastDate = $"{convertedDate[2]}-{convertedDate[1]}-{convertedDate[0]}";

            string url = $"http://livescore-api.com/api-client/scores/history.json?key={Api.Key}&secret={Api.Secret}&from={pastDate}&to={pastDate}&page={pageNo}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Model match = stream.ReadAndDeserializeFromJson<Model>();
                    Messenger.Default.Send("request");

                    return match?.data?.match;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }

        public async Task<List<Match>> LoadPastForTeam(string teamId)
        {
            string[] date = ((DateTime.Today.AddMonths(-4).ToString()).Split(' ')[0]).Split('/');
            string dateFrom = $"{date[2]}-{date[1]}-{date[0]}";

            string url = $"http://livescore-api.com/api-client/scores/history.json?key={Api.Key}&secret={Api.Secret}&team={teamId}&from={dateFrom}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Model match = stream.ReadAndDeserializeFromJson<Model>();
                    Messenger.Default.Send("request");

                    return match?.data?.match;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }

        public async Task<Data> LoadTeamsH2H(string homeId, string awayId)
        {
            string url = $"https://live-score-api.com/api-client/teams/head2head.json?team1_id={homeId}&team2_id={awayId}&key={Api.Key}&secret={Api.Secret}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Model h2h = stream.ReadAndDeserializeFromJson<Model>();
                    Messenger.Default.Send("request");

                    return h2h?.data;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }

        public async Task<Data> LoadStats(string matchId)
        {
            string url = $"https://live-score-api.com/api-client/matches/stats.json?match_id={matchId}&key=J0C7r92vwFrHvG86&secret=mdxZfbxWVCgDpi28iEfJOiflYH3unbAK";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Model h2h = stream.ReadAndDeserializeFromJson<Model>();
                    Messenger.Default.Send("request");

                    return h2h?.data;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }
    }
}
