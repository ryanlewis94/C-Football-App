using System.Collections.Generic;

namespace FootballApp.Classes
{
    public class Data
    {
        public List<Country> country { get; set; }
        public List<League> league { get; set; }
        public List<Match> match { get; set; }
        public List<Table> table { get; set; }
        public List<Fixture> fixtures { get; set; }
        public List<Competition> competition { get; set; }
        public List<Country> federation { get; set; }

        public Team team1 { get; set; }
        public Team team2 { get; set; }
        public List<LastMatch> team1_last_6 { get; set; }
        public List<LastMatch> team2_last_6 { get; set; }

        public string yellow_cards { get; set; }
        public string red_cards { get; set; }
        public string substitutions { get; set; }
        public string possesion { get; set; }
        public string free_kicks { get; set; }
        public string goal_kicks { get; set; }
        public string throw_ins { get; set; }
        public string offsides { get; set; }
        public string corners { get; set; }
        public string shots_on_target { get; set; }
        public string shots_off_target { get; set; }
        public string attempts_on_goal { get; set; }
        public string saves { get; set; }
        public string fauls { get; set; }
        public string treatments { get; set; }
        public string penalties { get; set; }
        public string shots_blocked { get; set; }
        public string dangerous_attacks { get; set; }
        public string attacks { get; set; }

        public Lineup lineup { get; set; }
    }
}
