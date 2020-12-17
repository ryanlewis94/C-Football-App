using FootballApp.Api;
using FootballApp.Classes;
using FootballApp.Utility;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using FootballApp.Commands;
using LiveCharts;
using LiveCharts.Wpf;
using System.Linq;

namespace FootballApp.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        private IFootball repository;
        public ICommand TeamClickedCommand { get; set; }

        private DispatcherTimer CountdownTimer { get; set; }

        #region Properties

        /// <summary>
        /// Stores all the H2H info
        /// </summary>
        private Data _head2Head;
        public Data Head2Head
        {
            get { return _head2Head; }
            set { SetProperty(ref _head2Head, value); }
        }

        /// <summary>
        /// Lists for storing the live events for the home and away team
        /// </summary>
        private List<Event> _homeEventsList;

        public List<Event> HomeEventsList
        {
            get { return _homeEventsList; }
            set { SetProperty(ref _homeEventsList, value); }
        }


        private List<Event> _awayEventsList;

        public List<Event> AwayEventsList
        {
            get { return _awayEventsList; }
            set { SetProperty(ref _awayEventsList, value); }
        }

        /// <summary>
        /// bools for hiding the fixture or match depending on what is selected
        /// </summary>
        private bool _matchSelected;
        public bool MatchSelected
        {
            get { return _matchSelected; }
            set { SetProperty(ref _matchSelected, value); }
        }

        private bool _fixtureSelected;
        public bool FixtureSelected
        {
            get { return _fixtureSelected; }
            set { SetProperty(ref _fixtureSelected, value); }
        }

        private bool _noMatchSelected = true;
        public bool NoMatchSelected
        {
            get { return _noMatchSelected; }
            set { SetProperty(ref _noMatchSelected, value); }
        }

        /// <summary>
        /// when there are no events to show equal false
        /// </summary>
        private bool _noEvents;
        public bool NoEvents
        {
            get { return _noEvents; }
            set { SetProperty(ref _noEvents, value); }
        }

        /// <summary>
        /// Time of Kick off
        /// </summary>
        private DateTime _fixtureKickOffTime;
        public DateTime FixtureKickOffTime
        {
            get { return _fixtureKickOffTime; }
            set { SetProperty(ref _fixtureKickOffTime, value); }
        }

        /// <summary>
        /// Countdown to Kick off
        /// </summary>
        private string _countdownTime;
        public string CountdownTime
        {
            get { return _countdownTime; }
            set { SetProperty(ref _countdownTime, value); }
        }

        /// <summary>
        /// checks if the game is at full time
        /// </summary>
        private bool _fullTime;
        public bool FullTime
        {
            get { return _fullTime; }
            set { SetProperty(ref _fullTime, value); }
        }

        /// <summary>
        /// Sets the greeting depending on the time of day
        /// </summary>
        private string _greeting;
        public string Greeting
        {
            get { return _greeting; }
            set { SetProperty(ref _greeting, value); }
        }

        /// <summary>
        /// Odds for home win away win and a draw
        /// </summary>
        private string _homeOdds;
        public string HomeOdds
        {
            get { return _homeOdds; }
            set { SetProperty(ref _homeOdds, value); }
        }

        private string _drawOdds;
        public string DrawOdds
        {
            get { return _drawOdds; }
            set { SetProperty(ref _drawOdds, value); }
        }

        private string _awayOdds;
        public string AwayOdds
        {
            get { return _awayOdds; }
            set { SetProperty(ref _awayOdds, value); }
        }

        private string _homePercentage;
        public string HomePercentage
        {
            get { return _homePercentage; }
            set { SetProperty(ref _homePercentage, value); }
        }

        private string _drawPercentage;
        public string DrawPercentage
        {
            get { return _drawPercentage; }
            set { SetProperty(ref _drawPercentage, value); }
        }

        private string _awayPercentage;
        public string AwayPercentage
        {
            get { return _awayPercentage; }
            set { SetProperty(ref _awayPercentage, value); }
        }

        private List<ScoreOdds> _matchScoreList;
        public List<ScoreOdds> MatchScoreList
        {
            get { return _matchScoreList; }
            set { SetProperty(ref _matchScoreList, value); }
        }

        /// <summary>
        /// if no odds available hide
        /// </summary>
        private bool _oddsAvailable;
        public bool OddsAvailable
        {
            get { return _oddsAvailable; }
            set { SetProperty(ref _oddsAvailable, value); }
        }

        /// <summary>
        /// Lists for storing the form of the home and away teams
        /// </summary>
        private List<Form> _homeForm;
        public List<Form> HomeForm
        {
            get { return _homeForm; }
            set { SetProperty(ref _homeForm, value); }
        }

        private List<Form> _awayForm;
        public List<Form> AwayForm
        {
            get { return _awayForm; }
            set { SetProperty(ref _awayForm, value); }
        }

        /// <summary>
        /// Lists for storing the last 6 matches of the home and away team
        /// </summary>
        private List<LastMatch> _homeMatches;
        public List<LastMatch> HomeMatches
        {
            get { return _homeMatches; }
            set { SetProperty(ref _homeMatches, value); }
        }

        private List<LastMatch> _awayMatches;
        public List<LastMatch> AwayMatches
        {
            get { return _awayMatches; }
            set { SetProperty(ref _awayMatches, value); }
        }

        /// <summary>
        /// stores the match Stats
        /// </summary>
        private Data _stats;
        public Data Stats
        {
            get { return _stats; }
            set { SetProperty(ref _stats, value); }
        }

        /// <summary>
        /// stores the stats for display in chart
        /// </summary>
        private SeriesCollection _statsCollection;
        public SeriesCollection StatsCollection
        {
            get { return _statsCollection; }
            set { SetProperty(ref _statsCollection, value); }
        }

        /// <summary>
        /// labels for the stats chart
        /// </summary>
        private List<string> _labels;
        public List<string> Labels
        {
            get { return _labels; }
            set { SetProperty(ref _labels, value); }
        }

        /// <summary>
        /// max value and height of the stats chart
        /// </summary>
        private int _maxValue;
        public int MaxValue
        {
            get { return _maxValue; }
            set { SetProperty(ref _maxValue, value); }
        }

        private int _height;
        public int Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        /// <summary>
        /// bools for visibility of the last 6 matches and the stats chart 
        /// </summary>
        private bool _noStats;
        public bool NoStats
        {
            get { return _noStats; }
            set { SetProperty(ref _noStats, value); }
        }

        private bool _noMatchHistory;
        public bool NoMatchHistory
        {
            get { return _noMatchHistory; }
            set { SetProperty(ref _noMatchHistory, value); }
        }

        /// <summary>
        /// bools for visibility of the leage name and logo
        /// </summary>
        private bool _leagueLogo;
        public bool LeagueLogo
        {
            get { return _leagueLogo; }
            set { SetProperty(ref _leagueLogo, value); }
        }

        private bool _leagueName;
        public bool LeagueName
        {
            get { return _leagueName; }
            set { SetProperty(ref _leagueName, value); }
        }

        /// <summary>
        /// List for storing the competition past 12 months of matches for betting calculations
        /// </summary>
        private List<Match> _competitionMatches;
        public List<Match> CompetitionMatches
        {
            get { return _competitionMatches; }
            set { SetProperty(ref _competitionMatches, value); }
        }

        private List<Player> _homeEleven;
        public List<Player> HomeEleven
        {
            get { return _homeEleven; }
            set { SetProperty(ref _homeEleven, value); }
        }

        private List<Player> _homeBench;
        public List<Player> HomeBench
        {
            get { return _homeBench; }
            set { SetProperty(ref _homeBench, value); }
        }

        private List<Player> _awayEleven;
        public List<Player> AwayEleven
        {
            get { return _awayEleven; }
            set { SetProperty(ref _awayEleven, value); }
        }

        private List<Player> _awayBench;
        public List<Player> AwayBench
        {
            get { return _awayBench; }
            set { SetProperty(ref _awayBench, value); }
        }

        private bool _lineupsAvailable;
        public bool LineupsAvailable
        {
            get { return _lineupsAvailable; }
            set { SetProperty(ref _lineupsAvailable, value); }
        }

        #endregion

        public EventsViewModel()
        {
            repository = new Football();
            LoadEvents();
            TeamClickedCommand = new RelayCommand(TeamClicked);
            CountdownTimer = new DispatcherTimer();
        }

        /// <summary>
        /// when a team is selected send the team id and team name
        /// </summary>
        /// <param name="teamId"></param>
        private void TeamClicked(object teamId)
        {
            Messenger.Default.Send($"teamId={teamId}");
            string teamName = "Team Name";
            if (teamId.ToString() == CurrentCountry.matchList?.home_id) teamName = CurrentCountry.matchList.home_name;
            if (teamId.ToString() == CurrentCountry.matchList?.away_id) teamName = CurrentCountry.matchList.away_name;
            if (teamId.ToString() == CurrentCountry.fixtureList?.home_id) teamName = CurrentCountry.fixtureList.home_name;
            if (teamId.ToString() == CurrentCountry.fixtureList?.away_id) teamName = CurrentCountry.fixtureList.away_name;
            Messenger.Default.Send($"teamName={teamName}");
        }

        /// <summary>
        /// Sets the Greeting message depending on the current time of day
        /// </summary>
        private void LoadEvents()
        {
            if (DateTime.Now < DateTime.Parse("12:00:00")) { Greeting = "Good Morning!"; }
            else if (DateTime.Now < DateTime.Parse("16:00:00")) { Greeting = "Good Afternoon!"; }
            else { Greeting = "Good Evening!"; }

            Messenger.Default.Register<Country>(this, OnCountryReceived);
        }

        /// <summary>
        /// When a match or fixture gets selected
        /// </summary>
        /// <param name="country"></param>
        private async void OnCountryReceived(Country country)
        {
            try
            {
                if (country != null)
                {
                    NoMatchSelected = false;
                    //if there was previously a country selected keep it in memory to compare against the newly selected country
                    var countryBefore = (CurrentCountry != null) ? CurrentCountry : null;
                    CurrentCountry = null;
                    //if a match was selected
                    if (country.matchList != null)
                    {
                        if (country.logo.Contains("https"))
                        {
                            LeagueLogo = true;
                            LeagueName = false;
                        }
                        else
                        {
                            LeagueLogo = false;
                            LeagueName = true;
                        }

                        MatchSelected = true;
                        FixtureSelected = false;

                        //if the match is still live
                        if (country.matchList.time != "FT" && country.matchList.time != "AET")
                        {
                            FullTime = false;
                        }
                        else
                        {
                            FullTime = true;
                        }
                        GetLogos(repository.LoadLogos(), "match", country);
                        //if first time selecting load all the data about the teams and the game
                        if (countryBefore == null)
                        {
                            LoadForm(country.matchList.home_id, country.matchList.away_id);
                            LoadInGame(country.matchList.id);
                        }
                        else
                        {
                            //if the country being sent is a new country load all the info on the teams
                            if (country.matchList?.id != countryBefore.matchList?.id)
                            {
                                LoadForm(country.matchList.home_id, country.matchList.away_id);
                                LoadInGame(country.matchList.id);
                            }
                            else
                            {
                                //if the match is still live load the live events and stats
                                if (!FullTime)
                                {
                                    LoadInGame(country.matchList.id);
                                }
                                else
                                {
                                    Messenger.Default.Send("loaded");
                                }
                            }
                        }
                        CurrentCountry = country;
                    }
                    //if a fixture was selected
                    else if (country.fixtureList != null)
                    {
                        if (country.logo.Contains("https"))
                        {
                            LeagueLogo = true;
                            LeagueName = false;
                        }
                        else
                        {
                            LeagueLogo = false;
                            LeagueName = true;
                        }

                        MatchSelected = false;
                        FixtureSelected = true;

                        FixtureKickOffTime = DateTime.Parse($"{country.fixtureList.date} {country.fixtureList.time}");
                        if (country.fixtureList.time == "00:00" || country.fixtureList.time == "00:30" || country.fixtureList.time == "01:00")
                        {
                            FixtureKickOffTime = FixtureKickOffTime.AddDays(1);
                        }
                        LoadCountdown();
                        GetLogos(repository.LoadLogos(), "fixture", country);
                        //if first time selecting load all the data about the teams and the game
                        if (countryBefore == null)
                        {
                            LoadForm(country.fixtureList.home_id, country.fixtureList.away_id);

                            int i = 0;
                            CompetitionMatches = new List<Match>();
                            var SortCompetitionMatches = new List<Match>();
                            do //load all matches from multiple pages
                            {
                                i = i + 1;
                                SortCompetitionMatches = await repository.LoadPastForCompetition(country.competition_id, i);
                                CompetitionMatches = CompetitionMatches.Concat(SortCompetitionMatches).ToList();

                            } while (SortCompetitionMatches.Count == 30);
                            GetOdds(country);
                        }
                        else
                        {
                            //if the country being sent is a new country load all the info on the teams
                            if (country.fixtureList?.id != countryBefore.fixtureList?.id)
                            {
                                LoadForm(country.fixtureList.home_id, country.fixtureList.away_id);
                                
                                if (country.competition_id != countryBefore.competition_id)
                                {
                                    int i = 0;
                                    CompetitionMatches = new List<Match>();
                                    var SortCompetitionMatches = new List<Match>();
                                    do //load all matches from multiple pages
                                    {
                                        i = i + 1;
                                        SortCompetitionMatches = await repository.LoadPastForCompetition(country.competition_id, i);
                                        CompetitionMatches = CompetitionMatches.Concat(SortCompetitionMatches).ToList();

                                    } while (SortCompetitionMatches.Count == 30);
                                }
                                GetOdds(country);
                            }
                            else
                            {
                                Messenger.Default.Send("loaded");
                            }
                        }
                        CurrentCountry = country;
                    }
                }
            }
            catch (Exception ex)
            {
                errorHandler.CheckErrorMessage(ex);
            }
            finally
            {
                Messenger.Default.Send("matchOpened");
            }
        }

        /// <summary>
        /// loads all the head to head data ready for sorting
        /// </summary>
        /// <param name="home_id"></param>
        /// <param name="away_id"></param>
        private async void LoadForm(string home_id, string away_id)
        {
            try
            {
                Head2Head = await repository.LoadTeamsH2H(home_id, away_id);
                if (Head2Head != null)
                {
                    CheckForm(Head2Head.team1.overall_form, Head2Head.team2.overall_form);
                    CheckLastSix(Head2Head.team1_last_6, Head2Head.team2_last_6, Head2Head.team1.name, Head2Head.team2.name);
                }
                else
                {
                    HomeForm = new List<Form>();
                    AwayForm = new List<Form>();
                    HomeMatches = new List<LastMatch>();
                    AwayMatches = new List<LastMatch>();
                    NoMatchHistory = false;
                }
            }
            catch (Exception ex)
            {
                errorHandler.CheckErrorMessage(ex);
            }
        }

        /// <summary>
        /// loads all the stats and events ready for sorting
        /// </summary>
        /// <param name="id"></param>
        private async void LoadInGame(string id)
        {
            try
            {
                Stats = await repository.LoadStats(id);
            }
            catch (Exception ex)
            {
                Stats = new Data();

                if (!ex.Message.Contains("Cannot deserialize the current JSON array", StringComparison.OrdinalIgnoreCase))
                {
                    errorHandler.CheckErrorMessage(ex);
                }
            }
            finally
            {
                SortStats(Stats);
            }

            try 
            {
                SortLineups(await repository.LoadLineups(id));
                SortEvents(await repository.LoadEvents(id)); 
            }
            catch (Exception ex) 
            { 
                errorHandler.CheckErrorMessage(ex); 
            }
        }

        /// <summary>
        /// puts the players into different lists depending on team and if they are a sub or not
        /// </summary>
        /// <param name="teamLineups"></param>
        private void SortLineups(Lineup teamLineups)
        {
            HomeEleven = new List<Player>();
            HomeBench = new List<Player>();
            AwayEleven = new List<Player>();
            AwayBench = new List<Player>();

            var homeLineup = teamLineups.home.players;
            var awayLineup = teamLineups.away.players;
            if (homeLineup?.Count > 0 || awayLineup?.Count > 0)
            {
                LineupsAvailable = true;

                foreach (Player homePlayer in homeLineup)
                {
                    if (homePlayer.substitution == "0")
                    {
                        HomeEleven.Add(homePlayer);
                    }
                    else
                    {
                        HomeBench.Add(homePlayer);
                    }
                }

                foreach (Player awayPlayer in awayLineup)
                {
                    if (awayPlayer.substitution == "0")
                    {
                        AwayEleven.Add(awayPlayer);
                    }
                    else
                    {
                        AwayBench.Add(awayPlayer);
                    }
                }
            }
            else
            {
                LineupsAvailable = false;
            }
        }

        /// <summary>
        /// Adds images for the different events and sorts the home and away into different lists
        /// </summary>
        private void SortEvents(List<Event> eventsList)
        {
            try
            {
                var HomeList = new List<Event>();
                var AwayList = new List<Event>();
                var BlankEvent = new Event();

                if (eventsList != null)
                {
                    if (eventsList?.Count != 0)
                    {

                        foreach (Event @event in eventsList)
                        {
                            //format the player name so the first name is displayed before the last name
                            int idx = @event.player.LastIndexOf(" ");
                            string playerToAdd;
                            if (idx != -1)
                            {
                                playerToAdd = $"{@event.player.Substring(idx + 1)} {@event.player.Substring(0, idx)}";
                            }
                            else
                            {
                                playerToAdd = @event.player;
                            }

                            string eventImage;
                            switch (@event.@event)
                            {
                                case "GOAL":
                                    eventImage = "/Resources/events/Goal.png";
                                    break;
                                case "GOAL_PENALTY":
                                    eventImage = "/Resources/events/Penalty.png";
                                    break;
                                case "OWN_GOAL":
                                    eventImage = "/Resources/events/ownGoal.png";
                                    break;
                                case "YELLOW_CARD":
                                    eventImage = "/Resources/events/Yellow.png";
                                    break;
                                case "RED_CARD":
                                    eventImage = "/Resources/events/Red.png";
                                    break;
                                case "YELLOW_RED_CARD":
                                    eventImage = "/Resources/events/YellowRed.png";
                                    break;
                                default:
                                    eventImage = @event.@event;
                                    break;
                            }

                            var EventToAdd = new Event
                            {
                                id = @event.id,
                                match_id = @event.match_id,
                                player = playerToAdd,
                                time = @event.time,
                                @event = eventImage,
                                sort = @event.sort,
                                home_away = @event.home_away
                            };

                            if (@event.home_away == "h")
                            {
                                HomeList.Insert(0, EventToAdd);
                                AwayList.Insert(0, BlankEvent);
                            }
                            else
                            {
                                AwayList.Insert(0, EventToAdd);
                                HomeList.Insert(0, BlankEvent);
                            }
                        }
                        HomeEventsList = HomeList;
                        AwayEventsList = AwayList;
                        NoEvents = false;
                    }
                    else
                    {
                        HomeEventsList = new List<Event>();
                        AwayEventsList = new List<Event>();
                        NoEvents = true;
                    }
                }
                else
                {
                    HomeEventsList = new List<Event>();
                    AwayEventsList = new List<Event>();
                    NoEvents = true;
                }
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
        /// Calculate the Odds from the past matches of the last 12 months
        /// </summary>
        /// <param name="homeList"></param>
        /// <param name="awayList"></param>
        private async void GetOdds(Country country)
        {
            var homeList = new List<Match>();
            var awayList = new List<Match>();

            int i = 0;
            var HomeMatchPageList = new List<Match>();
            var HomePrevMatches = new List<Match>();
            do //load all matches from multiple pages
            {
                i = i + 1;
                HomePrevMatches = await repository.LoadPastForTeam(country.fixtureList.home_id, i);
                HomeMatchPageList = HomeMatchPageList.Concat(HomePrevMatches).ToList();

            } while (HomePrevMatches.Count == 30);
            homeList = HomeMatchPageList.Where(x => x.competition_id.Equals(country.competition_id)).ToList();

            i = 0;
            var AwayMatchPageList = new List<Match>();
            var AwayPrevMatches = new List<Match>();
            do //load all matches from multiple pages
            {
                i = i + 1;
                AwayPrevMatches = await repository.LoadPastForTeam(country.fixtureList.away_id, i);
                AwayMatchPageList = AwayMatchPageList.Concat(AwayPrevMatches).ToList();

            } while (AwayPrevMatches.Count == 30);
            awayList = AwayMatchPageList.Where(x => x.competition_id.Equals(country.competition_id)).ToList();
            

            try
            {
                decimal competitionHomeGoalsPerMatch = 0;
                decimal competitionAwayGoalsPerMatch = 0;

                decimal homeGoalsScoredPerMatch = 0;
                decimal homeGoalsConcededPerMatch = 0;

                decimal awayGoalsScoredPerMatch = 0;
                decimal awayGoalsConcededPerMatch = 0;

                if (homeList.Count != 0 || awayList.Count != 0)
                {
                    decimal MatchCount = 0;

                    decimal competitionHomeGoalsCount = 0;
                    decimal competitionAwayGoalsCount = 0;
                    foreach (Match match in CompetitionMatches)
                    {
                        MatchCount++;
                        competitionHomeGoalsCount = competitionHomeGoalsCount + int.Parse(match.score.Split('-')[0]);
                        competitionAwayGoalsCount = competitionAwayGoalsCount + int.Parse(match.score.Split('-')[1]);
                    }
                    competitionHomeGoalsPerMatch = competitionHomeGoalsCount / MatchCount;
                    competitionAwayGoalsPerMatch = competitionAwayGoalsCount / MatchCount;

                    MatchCount = 0;
                    decimal HomeGoalsScoredCount = 0;
                    decimal HomeGoalsConcededCount = 0;
                    foreach (Match match in homeList)
                    {
                        if (match.home_id == country.fixtureList.home_id)
                        {
                            MatchCount++;
                            HomeGoalsScoredCount = HomeGoalsScoredCount + int.Parse(match.score.Split('-')[0]);
                            HomeGoalsConcededCount = HomeGoalsConcededCount + int.Parse(match.score.Split('-')[1]);
                        }
                    }
                    homeGoalsScoredPerMatch = HomeGoalsScoredCount / MatchCount;
                    homeGoalsConcededPerMatch = HomeGoalsConcededCount / MatchCount;

                    MatchCount = 0;
                    decimal AwayGoalsScoredCount = 0;
                    decimal AwayGoalsConcededCount = 0;
                    foreach (Match match in awayList)
                    {
                        if (match.away_id == country.fixtureList.away_id)
                        {
                            MatchCount++;
                            AwayGoalsScoredCount = AwayGoalsScoredCount + int.Parse(match.score.Split('-')[1]);
                            AwayGoalsConcededCount = AwayGoalsConcededCount + int.Parse(match.score.Split('-')[0]);
                        }
                    }
                    awayGoalsScoredPerMatch = AwayGoalsScoredCount / MatchCount;
                    awayGoalsConcededPerMatch = AwayGoalsConcededCount / MatchCount;

                    decimal attackStrengthHomeTeam = homeGoalsScoredPerMatch / competitionHomeGoalsPerMatch;
                    decimal attackStrengthAwayTeam = awayGoalsScoredPerMatch / competitionAwayGoalsPerMatch;

                    decimal DefenceStrengthHomeTeam = homeGoalsConcededPerMatch / competitionAwayGoalsPerMatch;
                    decimal DefenceStrengthAwayTeam = awayGoalsConcededPerMatch / competitionHomeGoalsPerMatch;

                    decimal homeScorePrediction = attackStrengthHomeTeam * DefenceStrengthAwayTeam * competitionHomeGoalsPerMatch;
                    decimal awayScorePrediction = attackStrengthAwayTeam * DefenceStrengthHomeTeam * competitionAwayGoalsPerMatch;

                    decimal Home0 = CummulitiveDistributionFunction(0, homeScorePrediction);
                    decimal Home1 = CummulitiveDistributionFunction(1, homeScorePrediction);
                    decimal Home2 = CummulitiveDistributionFunction(2, homeScorePrediction);
                    decimal Home3 = CummulitiveDistributionFunction(3, homeScorePrediction);
                    decimal Home4 = CummulitiveDistributionFunction(4, homeScorePrediction);
                    decimal Home5 = CummulitiveDistributionFunction(5, homeScorePrediction);

                    decimal Away0 = CummulitiveDistributionFunction(0, awayScorePrediction);
                    decimal Away1 = CummulitiveDistributionFunction(1, awayScorePrediction);
                    decimal Away2 = CummulitiveDistributionFunction(2, awayScorePrediction);
                    decimal Away3 = CummulitiveDistributionFunction(3, awayScorePrediction);
                    decimal Away4 = CummulitiveDistributionFunction(4, awayScorePrediction);
                    decimal Away5 = CummulitiveDistributionFunction(5, awayScorePrediction);

                    decimal home1v0 = Home1 * Away0;
                    decimal home2v0 = Home2 * Away0;
                    decimal home3v0 = Home3 * Away0;
                    decimal home4v0 = Home4 * Away0;
                    decimal home5v0 = Home5 * Away0;
                    decimal home2v1 = Home2 * Away1;
                    decimal home3v1 = Home3 * Away1;
                    decimal home4v1 = Home4 * Away1;
                    decimal home5v1 = Home5 * Away1;
                    decimal home3v2 = Home3 * Away2;
                    decimal home4v2 = Home4 * Away2;
                    decimal home5v2 = Home5 * Away2;
                    decimal home4v3 = Home4 * Away3;
                    decimal home5v3 = Home5 * Away3;
                    decimal home5v4 = Home5 * Away4;

                    decimal HomeProbability = home1v0 + home2v0 + home3v0 + home4v0 + home5v0
                        + home2v1 + home3v1 + home4v1 + home5v1
                        + home3v2 + home4v2 + home5v2
                        + home4v3 + home5v3
                        + home5v4;
                    decimal HomeWin = 1 / HomeProbability;

                    decimal away1v0 = Away1 * Home0;
                    decimal away2v0 = Away2 * Home0;
                    decimal away3v0 = Away3 * Home0;
                    decimal away4v0 = Away4 * Home0;
                    decimal away5v0 = Away5 * Home0;
                    decimal away2v1 = Away2 * Home1;
                    decimal away3v1 = Away3 * Home1;
                    decimal away4v1 = Away4 * Home1;
                    decimal away5v1 = Away5 * Home1;
                    decimal away3v2 = Away3 * Home2;
                    decimal away4v2 = Away4 * Home2;
                    decimal away5v2 = Away5 * Home2;
                    decimal away4v3 = Away4 * Home3;
                    decimal away5v3 = Away5 * Home3;
                    decimal away5v4 = Away5 * Home4;

                    decimal AwayProbability = away1v0 + away2v0 + away3v0 + away4v0 + away5v0
                        + away2v1 + away3v1 + away4v1 + away5v1
                        + away3v2 + away4v2 + away5v2
                        + away4v3 + away5v3
                        + away5v4;
                    decimal AwayWin = 1 / AwayProbability;

                    decimal draw0v0 = Home0 * Away0;
                    decimal draw1v1 = Home1 * Away1;
                    decimal draw2v2 = Home2 * Away2;
                    decimal draw3v3 = Home3 * Away3;
                    decimal draw4v4 = Home4 * Away4;
                    decimal draw5v5 = Home5 * Away5;

                    decimal DrawProbability = draw0v0 + draw1v1 + draw2v2 + draw3v3 + draw4v4 + draw5v5;
                    decimal Draw = 1 / DrawProbability;

                    HomeOdds = HomeWin.ToString("#.##");
                    AwayOdds = AwayWin.ToString("#.##");
                    DrawOdds = Draw.ToString("#.##");

                    HomeProbability = decimal.Round((HomeProbability * 100), 1, MidpointRounding.AwayFromZero);
                    AwayProbability = decimal.Round((AwayProbability * 100), 1, MidpointRounding.AwayFromZero);
                    DrawProbability = decimal.Round((DrawProbability * 100), 1, MidpointRounding.AwayFromZero);

                    HomePercentage = $"{decimal.Round(HomeProbability, 0, MidpointRounding.AwayFromZero).ToString()}%";
                    AwayPercentage = $"{decimal.Round(AwayProbability, 0, MidpointRounding.AwayFromZero).ToString()}%";
                    DrawPercentage = $"{decimal.Round(DrawProbability, 0, MidpointRounding.AwayFromZero).ToString()}%";

                    MatchScoreList = new List<ScoreOdds>()
                    {
                        new ScoreOdds() { score = "0 - 0", odds = decimal.Round((1 / draw0v0), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * draw0v0), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "0 - 1", odds = decimal.Round((1 / away1v0), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away1v0), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "0 - 2", odds = decimal.Round((1 / away2v0), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away2v0), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "0 - 3", odds = decimal.Round((1 / away3v0), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away3v0), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "0 - 4", odds = decimal.Round((1 / away4v0), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away4v0), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "0 - 5", odds = decimal.Round((1 / away5v0), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away5v0), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "1 - 0", odds = decimal.Round((1 / home1v0), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home1v0), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "1 - 1", odds = decimal.Round((1 / draw1v1), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * draw1v1), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "1 - 2", odds = decimal.Round((1 / away2v1), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away2v1), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "1 - 3", odds = decimal.Round((1 / away3v1), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away3v1), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "1 - 4", odds = decimal.Round((1 / away4v1), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away4v1), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "1 - 5", odds = decimal.Round((1 / away5v1), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away5v1), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "2 - 0", odds = decimal.Round((1 / home2v0), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home2v0), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "2 - 1", odds = decimal.Round((1 / home2v1), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home2v1), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "2 - 2", odds = decimal.Round((1 / draw2v2), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * draw2v2), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "2 - 3", odds = decimal.Round((1 / away3v2), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away3v2), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "2 - 4", odds = decimal.Round((1 / away4v2), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away4v2), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "2 - 5", odds = decimal.Round((1 / away5v2), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away5v2), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "3 - 0", odds = decimal.Round((1 / home3v0), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home3v0), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "3 - 1", odds = decimal.Round((1 / home3v1), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home3v1), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "3 - 2", odds = decimal.Round((1 / home3v2), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home3v2), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "3 - 3", odds = decimal.Round((1 / draw3v3), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * draw3v3), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "3 - 4", odds = decimal.Round((1 / away4v3), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away4v3), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "3 - 5", odds = decimal.Round((1 / away5v3), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away5v3), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "4 - 0", odds = decimal.Round((1 / home4v0), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home4v0), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "4 - 1", odds = decimal.Round((1 / home4v1), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home4v1), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "4 - 2", odds = decimal.Round((1 / home4v2), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home4v2), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "4 - 3", odds = decimal.Round((1 / home4v3), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home4v3), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "4 - 4", odds = decimal.Round((1 / draw4v4), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * draw4v4), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "4 - 5", odds = decimal.Round((1 / away5v4), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * away5v4), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "5 - 0", odds = decimal.Round((1 / home5v0), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home5v0), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "5 - 1", odds = decimal.Round((1 / home5v1), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home5v1), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "5 - 2", odds = decimal.Round((1 / home5v2), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home5v2), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "5 - 3", odds = decimal.Round((1 / home5v3), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home5v3), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "5 - 4", odds = decimal.Round((1 / home5v4), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * home5v4), 4, MidpointRounding.AwayFromZero) },
                        new ScoreOdds() { score = "5 - 5", odds = decimal.Round((1 / draw5v5), 2, MidpointRounding.AwayFromZero), percentage = decimal.Round((100 * draw5v5), 4, MidpointRounding.AwayFromZero) }
                    };

                    //MatchScoreList = scoreList;

                    OddsAvailable = true;
                }
                else
                {
                    HomeOdds = null;
                    AwayOdds = null;
                    DrawOdds = null;
                    OddsAvailable = false;
                }
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

        public decimal CummulitiveDistributionFunction(int k, decimal lambda)
        {
            double e = Math.Pow(Math.E, (double)-lambda);
            double Lambda = Math.Pow((double)lambda, k);
            decimal sum = (decimal)(e * Lambda) / Factorial(k);
            return sum;
        }

        private int Factorial(int k)
        {
            int count = k;
            int factorial = 1;
            while (count >= 1)
            {
                factorial = factorial * count;
                count--;
            }
            return factorial;
        }

        /// <summary>
        /// sorts the match form to add a colour depending on win loss or draw
        /// </summary>
        /// <param name="overall_form1"></param>
        /// <param name="overall_form2"></param>
        private void CheckForm(List<string> overall_form1, List<string> overall_form2)
        {
            try
            {
                var HomeFormList = new List<Form>();
                var AwayFormList = new List<Form>();

                foreach (string form in overall_form1)
                {
                    var formColour = new SolidColorBrush(Colors.AliceBlue);

                    if (form == "W")
                    {
                        formColour = new SolidColorBrush(Color.FromArgb(85, 0, 205, 0));

                    }
                    else if (form == "L")
                    {
                        formColour = new SolidColorBrush(Color.FromArgb(85, 255, 0, 0));

                    }

                    HomeFormList.Insert(0, new Form
                    {
                        form = form,
                        color = formColour
                    });
                }

                foreach (string form in overall_form2)
                {
                    var formColour = new SolidColorBrush(Colors.AliceBlue);

                    if (form == "W")
                    {
                        formColour = new SolidColorBrush(Color.FromArgb(85, 0, 205, 0));
                    }
                    else if (form == "L")
                    {
                        formColour = new SolidColorBrush(Color.FromArgb(85, 255, 0, 0));
                    }

                    AwayFormList.Insert(0, new Form
                    {
                        form = form,
                        color = formColour
                    });
                }

                HomeForm = HomeFormList;
                AwayForm = AwayFormList;
            }
            catch (Exception ex)
            {
                errorHandler.CheckErrorMessage(ex);
            }
        }

        /// <summary>
        /// sorts the last 6 matches to add colour depending on win loss or draw
        /// </summary>
        /// <param name="lastMatches1"></param>
        /// <param name="lastMatches2"></param>
        /// <param name="homeName"></param>
        /// <param name="awayName"></param>
        private void CheckLastSix(List<LastMatch> lastMatches1, List<LastMatch> lastMatches2, string homeName, string awayName)
        {
            try
            {
                var HomeMatchList = new List<LastMatch>();
                var AwayMatchList = new List<LastMatch>();

                if (lastMatches1.Count != 0)
                {
                    foreach (LastMatch match in lastMatches1)
                    {
                        var matchColour = new SolidColorBrush(Colors.AliceBlue);

                        if (match.score != "? - ?")
                        {
                            var homeGoals = int.Parse(match.score.Split('-')[0]);
                            var awayGoals = int.Parse(match.score.Split('-')[1]);

                            if (match.home_name == homeName)
                            {
                                if (homeGoals > awayGoals)
                                {
                                    matchColour = new SolidColorBrush(Color.FromArgb(85, 0, 205, 0));
                                }
                                else if (homeGoals < awayGoals)
                                {
                                    matchColour = new SolidColorBrush(Color.FromArgb(85, 255, 0, 0));
                                }
                            }
                            else if (match.away_name == homeName)
                            {
                                if (homeGoals > awayGoals)
                                {
                                    matchColour = new SolidColorBrush(Color.FromArgb(85, 255, 0, 0));
                                }
                                else if (homeGoals < awayGoals)
                                {
                                    matchColour = new SolidColorBrush(Color.FromArgb(85, 0, 205, 0));
                                }
                            }
                        }

                        string[] date = match.date.Split('-');
                        string matchDate = $"{date[2]}/{date[1]}/{date[0]}";

                        HomeMatchList.Add(new LastMatch
                        {
                            id = match.id,
                            date = matchDate,
                            home_name = match.home_name,
                            away_name = match.away_name,
                            score = match.score,
                            scheduled = match.scheduled,
                            color = matchColour
                        });
                    }
                    NoMatchHistory = true;
                }
                else
                {
                    NoMatchHistory = false;
                }
                
                if (lastMatches2.Count != 0)
                {
                    foreach (LastMatch match in lastMatches2)
                    {
                        var matchColour = new SolidColorBrush(Colors.AliceBlue);

                        if (match.score != "? - ?")
                        {
                            var homeGoals = int.Parse(match.score.Split('-')[0]);
                            var awayGoals = int.Parse(match.score.Split('-')[1]);

                            if (match.home_name == awayName)
                            {
                                if (homeGoals > awayGoals)
                                {
                                    matchColour = new SolidColorBrush(Color.FromArgb(85, 0, 205, 0));
                                }
                                else if (homeGoals < awayGoals)
                                {
                                    matchColour = new SolidColorBrush(Color.FromArgb(85, 255, 0, 0));
                                }
                            }
                            else if (match.away_name == awayName)
                            {
                                if (homeGoals > awayGoals)
                                {
                                    matchColour = new SolidColorBrush(Color.FromArgb(85, 255, 0, 0));
                                }
                                else if (homeGoals < awayGoals)
                                {
                                    matchColour = new SolidColorBrush(Color.FromArgb(85, 0, 205, 0));
                                }
                            }
                        }

                        string[] date = match.date.Split('-');
                        string matchDate = $"{date[2]}/{date[1]}/{date[0]}";

                        AwayMatchList.Add(new LastMatch
                        {
                            id = match.id,
                            date = matchDate,
                            home_name = match.home_name,
                            away_name = match.away_name,
                            score = match.score,
                            scheduled = match.scheduled,
                            color = matchColour
                        });
                    }
                    NoMatchHistory = true;
                }
                else
                {
                    NoMatchHistory = false;
                }

                HomeMatches = HomeMatchList;
                AwayMatches = AwayMatchList;
            }
            catch (Exception ex)
            {
                errorHandler.CheckErrorMessage(ex);
            }
        }

        /// <summary>
        /// sorts the stats into a seriescollection so that it can be displayed in a graph
        /// </summary>
        /// <param name="stats"></param>
        private void SortStats(Data stats)
        {
            try
            {
                var collection = new SeriesCollection();
                var labels = new List<string>();

                collection.Add(new StackedRowSeries
                {
                    Values = new ChartValues<double> { },
                    StackMode = StackMode.Percentage,
                    DataLabels = true
                });
                collection.Add(new StackedRowSeries
                {
                    Values = new ChartValues<double> { },
                    StackMode = StackMode.Percentage,
                    DataLabels = true
                });

                if (stats.possesion != null)
                {
                    if (stats.possesion != "0:0")
                    {
                        addStats(collection, stats.possesion);

                        labels.Insert(0, "Possession");
                    }
                }

                if (stats.attempts_on_goal != null)
                {
                    if (stats.attempts_on_goal != "0:0")
                    {
                        addStats(collection, stats.attempts_on_goal);

                        labels.Insert(0, "Total Shots");
                    }
                }

                if (stats.shots_on_target != null)
                {
                    if (stats.shots_on_target != "0:0")
                    {
                        addStats(collection, stats.shots_on_target);

                        labels.Insert(0, "Shots on Target");
                    }
                }

                if (stats.shots_off_target != null)
                {
                    if (stats.shots_off_target != "0:0")
                    {
                        addStats(collection, stats.shots_off_target);

                        labels.Insert(0, "Shots off Target");
                    }
                }

                if (stats.shots_blocked != null)
                {
                    if (stats.shots_blocked != "0:0")
                    {
                        addStats(collection, stats.shots_blocked);

                        labels.Insert(0, "Shots Blocked");
                    }
                }

                if (stats.attacks != null)
                {
                    if (stats.attacks != "0:0")
                    {
                        addStats(collection, stats.attacks);

                        labels.Insert(0, "Attacks");
                    }
                }

                if (stats.dangerous_attacks != null)
                {
                    if (stats.dangerous_attacks != "0:0")
                    {
                        addStats(collection, stats.dangerous_attacks);

                        labels.Insert(0, "Dangerous Attacks");
                    }
                }

                if (stats.penalties != null)
                {
                    if (stats.penalties != "0:0")
                    {
                        addStats(collection, stats.penalties);

                        labels.Insert(0, "Penalties");
                    }
                }

                if (stats.corners != null)
                {
                    if (stats.corners != "0:0")
                    {
                        addStats(collection, stats.corners);

                        labels.Insert(0, "Corners");
                    }
                }

                if (stats.free_kicks != null)
                {
                    if (stats.free_kicks != "0:0")
                    {
                        addStats(collection, stats.free_kicks);

                        labels.Insert(0, "Free Kicks");
                    }
                }

                if (stats.goal_kicks != null)
                {
                    if (stats.goal_kicks != "0:0")
                    {
                        addStats(collection, stats.goal_kicks);

                        labels.Insert(0, "Goal Kicks");
                    }
                }

                if (stats.throw_ins != null)
                {
                    if (stats.throw_ins != "0:0")
                    {
                        addStats(collection, stats.throw_ins);

                        labels.Insert(0, "Throw Ins");
                    }
                }

                if (stats.offsides != null)
                {
                    if (stats.offsides != "0:0")
                    {
                        addStats(collection, stats.offsides);

                        labels.Insert(0, "Offsides");
                    }
                }

                if (stats.fauls != null)
                {
                    if (stats.fauls != "0:0")
                    {
                        addStats(collection, stats.fauls);

                        labels.Insert(0, "Fouls");
                    }
                }

                if (stats.yellow_cards != null)
                {
                    if (stats.yellow_cards != "0:0")
                    {
                        addStats(collection, stats.yellow_cards);

                        labels.Insert(0, "Yellow Cards");
                    }
                }

                if (stats.red_cards != null)
                {
                    if (stats.red_cards != "0:0")
                    {
                        addStats(collection, stats.red_cards);

                        labels.Insert(0, "Red Cards");
                    }
                }

                if (stats.saves != null)
                {
                    if (stats.saves != "0:0")
                    {
                        addStats(collection, stats.saves);

                        labels.Insert(0, "Saves");
                    }
                }

                if (stats.substitutions != null)
                {
                    if (stats.substitutions != "0:0")
                    {
                        addStats(collection, stats.substitutions);

                        labels.Insert(0, "Substitutions");
                    }
                }

                if (stats.treatments != null)
                {
                    if (stats.treatments != "0:0")
                    {
                        addStats(collection, stats.treatments);

                        labels.Insert(0, "Treatments");
                    }
                }
                if (labels.Count == 0)
                {
                    NoStats = false;
                }
                else
                {
                    NoStats = true;

                    MaxValue = labels.Count;
                    Height = (labels.Count * 50);

                    Labels = labels;
                    StatsCollection = collection;
                }
            }
            catch (Exception ex)
            {
                errorHandler.CheckErrorMessage(ex);
            }
        }

        private void addStats(SeriesCollection collection, string stat)
        {
            //adds the home stat
            collection[0].Values.Insert(0, double.Parse(stat.Split(':')[0]));
            //adds the away stat
            collection[1].Values.Insert(0, double.Parse(stat.Split(':')[1]));
        }

        private void GetLogos(List<Logo> Logos, string matchOrFixture, Country country)
        {
            if (matchOrFixture == "match")
            {
                //looks for team logos for matches
                foreach (var logo in Logos)
                {
                    if (country.matchList.home_name.ToLower() == logo.team_name.ToLower() ||
                        country.matchList.home_name.Contains(logo.team_name) ||
                        $"FC {country.matchList.home_name}".Contains(logo.team_name) ||
                        $"{country.matchList.home_name} FC".Contains(logo.team_name))
                    {
                        country.matchList.home_logo = logo.logo;
                    }
                    if (country.matchList.away_name.ToLower() == logo.team_name.ToLower() ||
                        country.matchList.away_name.Contains(logo.team_name) ||
                        $"FC {country.matchList.away_name}".Contains(logo.team_name) ||
                        $"{country.matchList.away_name} FC".Contains(logo.team_name))
                    {
                        country.matchList.away_logo = logo.logo;
                    }
                }
            }
            else
            {
                //looks for team logos for fixtures
                foreach (var logo in Logos)
                {
                    if (country.fixtureList.home_name.Replace("amp;", "").ToLower() == logo.team_name.ToLower() ||
                        country.fixtureList.home_name.Replace("amp;", "").Contains(logo.team_name) ||
                        $"FC {country.fixtureList.home_name.Replace("amp;", "")}".Contains(logo.team_name) ||
                        $"{country.fixtureList.home_name.Replace("amp;", "")} FC".Contains(logo.team_name))
                    {
                        country.fixtureList.home_logo = logo.logo;
                    }
                    if (country.fixtureList.away_name.Replace("amp;", "").ToLower() == logo.team_name.ToLower() ||
                        country.fixtureList.away_name.Replace("amp;", "").Contains(logo.team_name) ||
                        $"FC {country.fixtureList.away_name.Replace("amp;", "")}".Contains(logo.team_name) ||
                        $"{country.fixtureList.away_name.Replace("amp;", "")} FC".Contains(logo.team_name))
                    {
                        country.fixtureList.away_logo = logo.logo;
                    }
                }
            }
        }

        /// <summary>
        /// Countdown until Fixture Kick Off
        /// </summary>
        private void LoadCountdown()
        {
            if (CountdownTimer.IsEnabled)
            {
                CountdownTimer.Stop();
            }
            CountdownTimer = new DispatcherTimer();
            CountdownTimer.Interval = TimeSpan.FromSeconds(1);
            CountdownTimer.Tick += CountdownTimer_Tick;
            if (FixtureKickOffTime > DateTime.Now)
            {
                TimeSpan ts = FixtureKickOffTime.Subtract(DateTime.Now);
                CountdownTime = ts.ToString("d' days 'h' hrs 'm' min 's' sec'");
                CountdownTimer.Start();
            }
            else
            {
                CountdownTime = "0 days 0 hrs 0 min 0 sec";
            }
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = FixtureKickOffTime.Subtract(DateTime.Now);
            string[] timeLeft = ts.ToString().Split('.');
            CountdownTime = ts.ToString("d' days 'h' hrs 'm' min 's' sec'");
            if (timeLeft[0] == "00:00:00")
            {
                CountdownTimer.Stop();
            }
        }
    }
}
