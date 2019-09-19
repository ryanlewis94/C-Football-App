﻿using FootballApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballApp.Api
{
    public interface IFootball
    {
        Task<List<Country>> LoadCountry();
        Task<List<League>> LoadLeague();
        Task<List<Match>> LoadLive(string leagueId);
        Task<List<Table>> LoadStandings(string leagueId);
        Task<List<Fixture>> LoadFixture();
    }
}
