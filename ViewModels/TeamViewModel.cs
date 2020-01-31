using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FootballApp.Api;
using FootballApp.Classes;
using FootballApp.Commands;
using FootballApp.Utility;

namespace FootballApp.ViewModels
{
    public class TeamViewModel : ViewModelBase
    {
        private IFootball repository;

        public ICommand GameEnteredCommand { get; set; }
        public ICommand LiveClickedCommand { get; set; }
        public ICommand MatchClickedCommand { get; set; }
        public ICommand MatchSelectedCommand { get; set; }
        public ICommand FixtureSelectedCommand { get; set; }

        #region Properties

        /// <summary>
        /// stores team name and logo for displaying
        /// </summary>
        private string _teamName;
        public string TeamName
        {
            get { return _teamName; }
            set { SetProperty(ref _teamName, value); }
        }

        private string _teamLogo;

        public string TeamLogo
        {
            get { return _teamLogo; }
            set { SetProperty(ref _teamLogo, value); }
        }

        /// <summary>
        /// bool to check if the team selected is currently playing a match
        /// </summary>
        private bool _currentlyLive;

        public bool CurrentlyLive
        {
            get { return _currentlyLive; }
            set { SetProperty(ref _currentlyLive, value); }
        }

        /// <summary>
        /// stores the match details if the team is currently playing
        /// </summary>
        private Match _liveMatch;
        public Match LiveMatch
        {
            get { return _liveMatch; }
            set { SetProperty(ref _liveMatch, value); }
        }

        /// <summary>
        /// lists for storing the past matches and upcoming fixtures
        /// </summary>
        private List<Match> _pastMatchList;
        public List<Match> PastMatchList
        {
            get { return _pastMatchList; }
            set { SetProperty(ref _pastMatchList, value); }
        }

        private List<Fixture> _upcomingFixtureList;
        public List<Fixture> UpcomingFixtureList
        {
            get { return _upcomingFixtureList; }
            set { SetProperty(ref _upcomingFixtureList, value); }
        }

        /// <summary>
        /// stores the match or fixture when one is selected by double clicking or entering
        /// </summary>
        private Fixture _selectedFixture;
        public Fixture SelectedFixture
        {
            get { return _selectedFixture; }
            set { SetProperty(ref _selectedFixture, value); }
        }

        private Match _selectedMatch;
        public Match SelectedMatch
        {
            get { return _selectedMatch; }
            set { SetProperty(ref _selectedMatch, value); }
        }

        /// <summary>
        /// stores the currently selected team name
        /// </summary>
        private string _currentTeam = "";
        public string CurrentTeam
        {
            get { return _currentTeam; }
            set { SetProperty(ref _currentTeam, value); }
        }

        #endregion

        public TeamViewModel()
        {
            Messenger.Default.Register<string>(this, TeamSelected);
            repository = new Football();
            GameEnteredCommand = new RelayCommand(GameEntered);
            LiveClickedCommand = new RelayCommand(LiveClicked);
            MatchClickedCommand = new RelayCommand(MatchClicked);
            MatchSelectedCommand = new RelayCommand(MatchSelected);
            FixtureSelectedCommand = new RelayCommand(FixtureSelected);
        }

        /// <summary>
        /// When receiving the team
        /// </summary>
        /// <param name="teamId"></param>
        private async void TeamSelected(string teamId)
        {
            if (teamId.Contains("teamId"))
            {
                var TeamId = teamId.Split('=')[1];
                //get team id and load all it's matches and fixtures
                GetMatchesAndFixtures(TeamId);

                //loop through the live matches to find if the team is currently playing
                foreach (Match match in await repository.LoadLive())
                {
                    if (TeamId == match.home_id || TeamId == match.away_id)
                    {
                        CurrentlyLive = true;
                        LiveMatch = match;
                        LiveMatch.date = "LIVE";
                        return;
                    }
                    else
                    {
                        CurrentlyLive = false;
                    }
                }
            }

            if (teamId.Contains("teamName"))
            {
                TeamName = teamId.Split('=')[1];

                //load the team logo
                foreach (Logo logo in repository.LoadLogos())
                {
                    if (TeamName.ToLower() == logo.team_name.ToLower())
                    {
                        TeamLogo = logo.logo;
                        return;
                    }
                    if (TeamName.Contains(logo.team_name) ||
                        $"FC {TeamName}".Contains(logo.team_name) ||
                        $"{TeamName} FC".Contains(logo.team_name))
                    {
                        TeamLogo = logo.logo;
                        return;
                    }
                    TeamLogo = "";
                }
            }
        }

        /// <summary>
        /// loads all the teams upcoming fixtures and past matches by the teamId
        /// </summary>
        /// <param name="teamId"></param>
        private async void GetMatchesAndFixtures(string teamId)
        {
            try
            {
                Messenger.Default.Send("unloaded");
                if (CurrentTeam == teamId) return; //if same team is already selected 
                CurrentTeam = teamId;
                int i = 0;

                var FixturePageList = new List<Fixture>();
                var futureFixtures = new List<Fixture>();
                do //load all fixtures from multiple pages
                {
                    i = i + 1;
                    futureFixtures = await repository.LoadFixture(null, teamId, i);
                    FixturePageList = FixturePageList.Concat(futureFixtures).ToList();

                } while (futureFixtures.Count == 30);

                FixturePageList.Reverse(); //reorder the list
                //loop through fixtures and format the date and time
                foreach (Fixture fixture in FixturePageList)
                {
                    if (fixture.time.Length > 3)
                        fixture.time = fixture.time.Substring(0, fixture.time.Length - 3);

                    string[] date = fixture.date.Split('-');
                    fixture.date = $"{date[2]}/{date[1]}/{date[0]}";
                    fixture.round = "";
                }
                UpcomingFixtureList = FixturePageList;

                i = 0;
                var MatchPageList = new List<Match>();
                var prevMatches = new List<Match>();
                do //load all matches from multiple pages
                {
                    i = i + 1;
                    prevMatches = await repository.LoadPast(null, teamId, i);
                    MatchPageList = MatchPageList.Concat(prevMatches).ToList();

                } while (prevMatches.Count == 30);

                MatchPageList.Reverse(); //reorder the list
                //loop through the match to format the date
                foreach (Match match in MatchPageList)
                {
                    string[] date = match.date.Split('-');
                    match.date = $"{date[2]}/{date[1]}/{date[0]}";
                }
                //if team is playing then add that match to the beginning of the list
                if (CurrentlyLive) MatchPageList.Insert(0, LiveMatch);
                PastMatchList = MatchPageList;
            }
            catch (Exception ex)
            {
                errorHandler.CheckErrorMessage(ex);
            }
            finally
            {
                Messenger.Default.Send("loaded");
            }
        }

        /// <summary>
        /// when user presses enter on a game
        /// </summary>
        /// <param name="obj"></param>
        private void GameEntered(object obj)
        {
            if (SelectedFixture != null) FixtureSelected(null);
            if (SelectedMatch != null) MatchSelected(null);
        }

        /// <summary>
        /// when user presses the live button
        /// </summary>
        /// <param name="obj"></param>
        private void LiveClicked(object obj)
        {
            Messenger.Default.Send($"matchId={LiveMatch.id}");
            SendCountry("live");
        }

        /// <summary>
        /// when user clicks on a match deselect all the other selected matches and fixtures
        /// </summary>
        /// <param name="obj"></param>
        private void MatchClicked(object obj)
        {
            SelectedFixture = null;
            SelectedMatch = null;
        }

        /// <summary>
        /// when a fixture is selected
        /// </summary>
        /// <param name="fixtureId"></param>
        private void FixtureSelected(object obj)
        {
            Messenger.Default.Send($"fixtureId={SelectedFixture.id}");
            SendCountry("fixture");
        }

        /// <summary>
        /// when a match is selected
        /// </summary>
        /// <param name="matchId"></param>
        private void MatchSelected(object obj)
        {
            Messenger.Default.Send($"matchId={SelectedMatch.id}");
            SendCountry("match");
        }

        /// <summary>
        /// When any game is selected create a country and send it
        /// </summary>
        /// <param name="matchType"></param>
        private void SendCountry(string matchType)
        {
            Match match = new Match();
            Fixture fixture = new Fixture();
            if (matchType == "match")
            {
                match = SelectedMatch;
                fixture = null;
            }
            if (matchType == "live")
            {
                match = LiveMatch;
                fixture = null;
            }
            if (matchType == "fixture")
            {
                match = null;
                fixture = SelectedFixture;
            }

            Country country = new Country()
            {
                index = "",
                id = "",
                league_id = "",
                competition_id = (match != null) ? match.competition_id : fixture.competition_id,
                name = "",
                leagueName = (match != null) ? match.competition_name : fixture.competition.name,
                matchList = (match != null) ? match : null,
                fixtureList = (match != null) ? null : fixture,
                logo = (match != null) ? match.competition_name : fixture.competition.name
            };

            Messenger.Default.Send("unloaded");
            Messenger.Default.Send(0); //tab index
            Messenger.Default.Send(country);
        }
    }
}
