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
        public async Task<List<Country>> LoadCountry()
        {

            string url = "http://livescore-api.com/api-client/countries/list.json?key=8uoqtmuaQ1s4bRe4&secret=M2baUvmhpyZunhzvLYVekqpbrRgCJuHv";

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
            string url = $"http://livescore-api.com/api-client/leagues/list.json?key=8uoqtmuaQ1s4bRe4&secret=M2baUvmhpyZunhzvLYVekqpbrRgCJuHv";

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

        public async Task<List<Match>> LoadLive(string leagueId)
        {
            string url = $"http://livescore-api.com/api-client/scores/live.json?key=8uoqtmuaQ1s4bRe4&secret=M2baUvmhpyZunhzvLYVekqpbrRgCJuHv&league={leagueId}";

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
            string url = $"http://livescore-api.com/api-client/leagues/table.json?key=8uoqtmuaQ1s4bRe4&secret=M2baUvmhpyZunhzvLYVekqpbrRgCJuHv&league={leagueId}";

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



        public async Task<List<Fixture>> LoadFixture()
        {

            string url = $"http://livescore-api.com/api-client/fixtures/matches.json?key=8uoqtmuaQ1s4bRe4&secret=M2baUvmhpyZunhzvLYVekqpbrRgCJuHv&date=2019-09-21&league=25";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    Model fixture = await response.Content.ReadAsAsync<Model>();

                    return fixture.data.fixture;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
