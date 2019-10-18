using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballApp.Classes
{
    public class Country
    {
        public string index { get; set; }
        public string id { get; set; }
        public string league_id { get; set; }
        public string name { get; set; }
        //public League leagueList { get; set; }
        public string leagueName { get; set; }
        public Match matchList { get; set; }
        public Fixture fixtureList { get; set; }
    }
    public class League
    {
        public int id { get; set; }
        public string name { get; set; }
        public string country_id { get; set; }
        public string scores { get; set; }
        public Match matchList { get; set; }
        public Fixture fixtureList { get; set; }
    }

    public class Competition
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Country> countries { get; set; }
        public List<Country> federations { get; set; }
    }


    public class Match
    {
        public string id { get; set; }
        public string home_name { get; set; }
        public string away_name { get; set; }
        public string score { get; set; }
        public string ht_score { get; set; }
        public string ft_score { get; set; }
        public string et_score { get; set; }
        public string time { get; set; }
        public string league_id { get; set; }
        public string status { get; set; }
        public string added { get; set; }
        public string last_changed { get; set; }
        public string home_id { get; set; }
        public string away_id { get; set; }
        public string competition_id { get; set; }
        public string location { get; set; }
        public string fixture_id { get; set; }
        public string scheduled { get; set; }
        public string events { get; set; }
        public string league_name { get; set; }
        public string competition_name { get; set; }
    }

    public class Table
    {
        public string league_id { get; set; }
        public string season_id { get; set; }
        public string name { get; set; }
        public int rank { get; set; }
        public int points { get; set; }
        public int matches { get; set; }
        public int goal_diff { get; set; }
        public int goals_scored { get; set; }
        public int goals_conceded { get; set; }
        public int lost { get; set; }
        public int drawn { get; set; }
        public int won { get; set; }
        public string team_id { get; set; }
        public string competition_id { get; set; }
        public bool State { get; set; }
    }

    public class Event
    {
        public string id { get; set; }
        public string match_id { get; set; }
        public string player { get; set; }
        public string time { get; set; }
        public string @event { get; set; }
        public int sort { get; set; }
        public string home_away { get; set; }
    }

    public class Fixture
    {
        public string id { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string home_name { get; set; }
        public string away_name { get; set; }
        public string league_id { get; set; }
        public string competition_id { get; set; }
    }
}
