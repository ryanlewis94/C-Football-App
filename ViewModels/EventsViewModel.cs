using FootballApp.Api;
using FootballApp.Classes;
using FootballApp.Utility;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Wpf;

namespace FootballApp.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        private IFootball repository;

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

        private Data _stats;
        public Data Stats
        {
            get { return _stats; }
            set { SetProperty(ref _stats, value); }
        }

        /// <summary>
        /// Lists for storing events
        /// </summary>
        private List<Event> _eventsList;

        public List<Event> EventsList
        {
            get { return _eventsList; }
            set { SetProperty(ref _eventsList, value); }
        }

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

        private List<Event> _homeList;

        public List<Event> HomeList
        {
            get { return _homeList; }
            set { SetProperty(ref _homeList, value); }
        }


        private List<Event> _awayList;

        public List<Event> AwayList
        {
            get { return _awayList; }
            set { SetProperty(ref _awayList, value); }
        }

        private List<Event> _timeList;

        public List<Event> TimeList
        {
            get { return _timeList; }
            set { SetProperty(ref _timeList, value); }
        }

        /// <summary>
        /// blank event gets inserted to the home or away list
        /// </summary>
        private Event _blankEvent;

        public Event BlankEvent
        {
            get { return _blankEvent; }
            set { SetProperty(ref _blankEvent, value); }
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
        /// gets the time the game is updated
        /// </summary>
        private string _timeUpdated;
        public string TimeUpdated
        {
            get { return _timeUpdated; }
            set { SetProperty(ref _timeUpdated, value); }
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

        private bool _oddsAvailable;
        public bool OddsAvailable
        {
            get { return _oddsAvailable; }
            set { SetProperty(ref _oddsAvailable, value); }
        }

        private float homeWins;
        private float homeDraws;
        private float homeLosses;
        private float homeMatches;

        private float awayWins;
        private float awayDraws;
        private float awayLosses;
        private float awayMatches;


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

        private List<Form> _homeFormList;
        public List<Form> HomeFormList
        {
            get { return _homeFormList; }
            set { SetProperty(ref _homeFormList, value); }
        }

        private List<Form> _awayFormList;
        public List<Form> AwayFormList
        {
            get { return _awayFormList; }
            set { SetProperty(ref _awayFormList, value); }
        }

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

        private List<LastMatch> _homeMatchList;
        public List<LastMatch> HomeMatchList
        {
            get { return _homeMatchList; }
            set { SetProperty(ref _homeMatchList, value); }
        }

        private List<LastMatch> _awayMatchList;
        public List<LastMatch> AwayMatchList
        {
            get { return _awayMatchList; }
            set { SetProperty(ref _awayMatchList, value); }
        }

        private SeriesCollection _statsCollection;
        public SeriesCollection StatsCollection
        {
            get { return _statsCollection; }
            set { SetProperty(ref _statsCollection, value); }
        }

        private List<string> _labels;
        public List<string> Labels
        {
            get { return _labels; }
            set { SetProperty(ref _labels, value); }
        }

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

        #endregion

        public EventsViewModel()
        {
            repository = new Football();
            LoadEvents();
            LoadTimers();
        }

        private void LoadTimers()
        {
            CountdownTimer = new DispatcherTimer();
        }

        private void LoadEvents()
        {
            if (DateTime.Now < DateTime.Parse("12:00:00")) { Greeting = "Good Morning!"; }
            else if (DateTime.Now < DateTime.Parse("17:00:00")) { Greeting = "Good Afternoon!"; }
            else { Greeting = "Good Ebening!"; }

            Messenger.Default.Register<Country>(this, OnCountryReceived);
        }

        private void GetOdds(List<Table> leagueTable)
        {
            try
            {
                if (leagueTable.Count != 0)
                {
                    foreach (Table team in leagueTable)
                    {
                        if (team.name == CurrentCountry.fixtureList.home_name)
                        {
                            homeWins = team.won;
                            homeDraws = team.drawn;
                            homeLosses = team.lost;
                            homeMatches = team.matches;
                        }
                        if (team.name == CurrentCountry.fixtureList?.away_name)
                        {
                            awayWins = team.won;
                            awayDraws = team.drawn;
                            awayLosses = team.lost;
                            awayMatches = team.matches;
                        }
                    }
                    HomeOdds = (100 / (100 * ((homeWins + awayLosses) / homeMatches))).ToString("#.##");
                    AwayOdds = (100 / (100 * ((awayWins + homeLosses) / awayMatches))).ToString("#.##");
                    DrawOdds = (100 / (100 * ((homeDraws + awayDraws) / homeMatches))).ToString("#.##");

                    HomeOdds = (HomeOdds[0].ToString() == ".") ? $"0{HomeOdds}" : HomeOdds;
                    AwayOdds = (AwayOdds[0].ToString() == ".") ? $"0{AwayOdds}" : AwayOdds;
                    DrawOdds = (DrawOdds[0].ToString() == ".") ? $"0{DrawOdds}" : DrawOdds;

                    HomeOdds = (!HomeOdds.Contains(".", StringComparison.OrdinalIgnoreCase)) ?
                        $"{HomeOdds}.0" : HomeOdds;
                    AwayOdds = (!AwayOdds.Contains(".", StringComparison.OrdinalIgnoreCase)) ?
                        $"{AwayOdds}.0" : AwayOdds;
                    DrawOdds = (!DrawOdds.Contains(".", StringComparison.OrdinalIgnoreCase)) ?
                        $"{DrawOdds}.0" : DrawOdds;

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
                    HomeFormList = new List<Form>();
                    AwayFormList = new List<Form>();
                    HomeMatchList = new List<LastMatch>();
                    AwayMatchList = new List<LastMatch>();
                    NoMatchSelected = false;
                    HomeEventsList?.Clear();
                    AwayEventsList?.Clear();
                    if (country.matchList != null)
                    {
                        CurrentCountry = country;
                        
                        MatchSelected = true;
                        FixtureSelected = false;

                        if (country.matchList.time != "FT" && country.matchList.time != "AET")
                        {
                            FullTime = false;
                        }
                        else
                        {
                            FullTime = true;
                            TimeUpdated = "";
                        }
                        Head2Head = await repository.LoadTeamsH2H(country.matchList.home_id, country.matchList.away_id);
                        if (Head2Head != null)
                        {
                            CheckForm(Head2Head.team1.overall_form, Head2Head.team2.overall_form);
                            CheckLastSix(Head2Head.team1_last_6, Head2Head.team2_last_6, Head2Head.team1.name, Head2Head.team2.name);
                            NoMatchHistory = true;
                        }
                        else
                        {
                            HomeForm = new List<Form>();
                            AwayForm = new List<Form>();
                            HomeMatches = new List<LastMatch>();
                            AwayMatches = new List<LastMatch>();
                            NoMatchHistory = false;
                        }

                        try
                        {
                            Stats = await repository.LoadStats(country.matchList.id);
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

                        EventsList = await repository.LoadEvents(country.matchList.id);
                        SortEvents();
                    }
                    else if (country.fixtureList != null)
                    {
                        CurrentCountry = country;
                        
                        MatchSelected = false;
                        FixtureSelected = true;

                        FixtureKickOffTime = DateTime.Parse($"{country.fixtureList.date} {country.fixtureList.time}");
                        if (country.fixtureList.time == "00:00" || country.fixtureList.time == "00:30" || country.fixtureList.time == "01:00")
                        {
                            FixtureKickOffTime = FixtureKickOffTime.AddDays(1);
                        }

                        LoadCountdown();

                        Head2Head = await repository.LoadTeamsH2H(country.fixtureList.home_id, country.fixtureList.away_id);
                        if (Head2Head != null)
                        {
                            CheckForm(Head2Head.team1.overall_form, Head2Head.team2.overall_form);
                            CheckLastSix(Head2Head.team1_last_6, Head2Head.team2_last_6, Head2Head.team1.name, Head2Head.team2.name);
                            NoMatchHistory = true;
                        }
                        else
                        {
                            HomeForm = new List<Form>();
                            AwayForm = new List<Form>();
                            HomeMatches = new List<LastMatch>();
                            AwayMatches = new List<LastMatch>();
                            NoMatchHistory = false;
                        }

                        try
                        {
                            GetOdds(await repository.LoadStandings(country.competition_id));
                        }
                        catch (Exception ex)
                        {
                            if  (ex.Message == "BadRequest")
                            {
                                GetOdds(null);
                            }
                            else
                            {
                                errorHandler.CheckErrorMessage(ex);
                            }
                        }
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
        /// Adds images for the different events and sorts the home and away into different lists
        /// </summary>
        private void SortEvents()
        {
            try
            {
                HomeList = new List<Event>();
                AwayList = new List<Event>();
                TimeList = new List<Event>();
                BlankEvent = new Event();

                if (EventsList != null)
                {
                    if (EventsList?.Count != 0)
                    {
                        NoEvents = false;

                        foreach (Event @event in EventsList)
                        {
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
                                TimeList.Insert(0, BlankEvent);
                            }
                            else
                            {
                                AwayList.Insert(0, EventToAdd);
                                HomeList.Insert(0, BlankEvent);
                                TimeList.Insert(0, BlankEvent);
                            }

                            //if (!string.IsNullOrWhiteSpace(CurrentCountry.matchList.ht_score))
                            //{
                            //    var htEvent = new Event
                            //    {
                            //        id = "",
                            //        match_id = @event.match_id,
                            //        player = CurrentCountry.matchList.ht_score,
                            //        time = "HT",
                            //        @event = "",
                            //        sort = @event.sort,
                            //        home_away = ""
                            //    };

                            //    HomeEventsList.Insert(0, BlankEvent);
                            //    AwayEventsList.Insert(0, BlankEvent);
                            //    TimeList.Insert(0, htEvent);
                            //} 
                        }
                        HomeEventsList = HomeList;
                        AwayEventsList = AwayList;
                    }
                    else
                    {
                        NoEvents = true;
                        HomeEventsList = new List<Event>();
                        AwayEventsList = new List<Event>();
                    }
                }
                else
                {
                    NoEvents = true;
                    HomeEventsList = new List<Event>();
                    AwayEventsList = new List<Event>();
                }

                TimeUpdated = (!FullTime) ?
                    $"Last Updated: {DateTime.Now.ToString("HH:mm:ss")}" :
                    "";
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

        private void CheckForm(List<string> overall_form1, List<string> overall_form2)
        {

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

        private void CheckLastSix(List<LastMatch> lastMatches1, List<LastMatch> lastMatches2, string homeName, string awayName)
        {
            foreach (LastMatch match in lastMatches1)
            {
                var homeGoals = int.Parse(match.score.Split('-')[0]);
                var awayGoals = int.Parse(match.score.Split('-')[1]);
                var matchColour = new SolidColorBrush(Colors.AliceBlue);

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

                HomeMatchList.Add(new LastMatch
                {
                    id = match.id,
                    date = match.date,
                    home_name = match.home_name,
                    away_name = match.away_name,
                    score = match.score,
                    scheduled = match.scheduled,
                    color = matchColour
                });
            }

            foreach (LastMatch match in lastMatches2)
            {
                var homeGoals = int.Parse(match.score.Split('-')[0]);
                var awayGoals = int.Parse(match.score.Split('-')[1]);
                var matchColour = new SolidColorBrush(Colors.AliceBlue);

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

                AwayMatchList.Add(new LastMatch
                {
                    id = match.id,
                    date = match.date,
                    home_name = match.home_name,
                    away_name = match.away_name,
                    score = match.score,
                    scheduled = match.scheduled,
                    color = matchColour
                });
            }
            HomeMatches = HomeMatchList;
            AwayMatches = AwayMatchList;
        }

        private void SortStats(Data stats)
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
                    collection[0].Values.Insert(0, double.Parse(stats.possesion.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.possesion.Split(':')[1]));

                    labels.Insert(0, "Possession");
                }
            }

            if (stats.attempts_on_goal != null)
            {
                if (stats.attempts_on_goal != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.attempts_on_goal.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.attempts_on_goal.Split(':')[1]));

                    labels.Insert(0, "Total Shots");
                }
            }

            if (stats.shots_on_target != null)
            {
                if (stats.shots_on_target != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.shots_on_target.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.shots_on_target.Split(':')[1]));

                    labels.Insert(0, "Shots on Target");
                }
            }

            if (stats.shots_off_target != null)
            {
                if (stats.shots_off_target != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.shots_off_target.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.shots_off_target.Split(':')[1]));

                    labels.Insert(0, "Shots off Target");
                }
            }

            if (stats.shots_blocked != null)
            {
                if (stats.shots_blocked != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.shots_blocked.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.shots_blocked.Split(':')[1]));

                    labels.Insert(0, "Shots Blocked");
                }
            }

            if (stats.attacks != null)
            {
                if (stats.attacks != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.attacks.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.attacks.Split(':')[1]));

                    labels.Insert(0, "Attacks");
                }
            }

            if (stats.dangerous_attacks != null)
            {
                if (stats.dangerous_attacks != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.dangerous_attacks.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.dangerous_attacks.Split(':')[1]));

                    labels.Insert(0, "Dangerous Attacks");
                }
            }

            if (stats.penalties != null)
            {
                if (stats.penalties != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.penalties.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.penalties.Split(':')[1]));

                    labels.Insert(0, "Penalties");
                }
            }

            if (stats.corners != null)
            {
                if (stats.corners != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.corners.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.corners.Split(':')[1]));

                    labels.Insert(0, "Corner Kicks");
                }
            }

            if (stats.free_kicks != null)
            {
                if (stats.free_kicks != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.free_kicks.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.free_kicks.Split(':')[1]));

                    labels.Insert(0, "Free Kicks");
                }
            }

            if (stats.goal_kicks != null)
            {
                if (stats.goal_kicks != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.goal_kicks.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.goal_kicks.Split(':')[1]));

                    labels.Insert(0, "Goal Kicks");
                }
            }

            if (stats.throw_ins != null)
            {
                if (stats.throw_ins != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.throw_ins.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.throw_ins.Split(':')[1]));

                    labels.Insert(0, "Throw Ins");
                }
            }

            if (stats.offsides != null)
            {
                if (stats.offsides != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.offsides.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.offsides.Split(':')[1]));

                    labels.Insert(0, "Offsides");
                }
            }

            if (stats.fauls != null)
            {
                if (stats.fauls != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.fauls.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.fauls.Split(':')[1]));

                    labels.Insert(0, "Fouls");
                }
            }

            if (stats.yellow_cards != null)
            {
                if (stats.yellow_cards != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.yellow_cards.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.yellow_cards.Split(':')[1]));

                    labels.Insert(0, "Yellow Cards");
                }
            }

            if (stats.red_cards != null)
            {
                if (stats.red_cards != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.red_cards.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.red_cards.Split(':')[1]));

                    labels.Insert(0, "Red Cards");
                }
            }

            if (stats.saves != null)
            {
                if (stats.saves != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.saves.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.saves.Split(':')[1]));

                    labels.Insert(0, "Saves");
                }
            }

            if (stats.substitutions != null)
            {
                if (stats.substitutions != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.substitutions.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.substitutions.Split(':')[1]));

                    labels.Insert(0, "Substitutions");
                }
            }

            if (stats.treatments != null)
            {
                if (stats.treatments != "0:0")
                {
                    collection[0].Values.Insert(0, double.Parse(stats.treatments.Split(':')[0]));
                    collection[1].Values.Insert(0, double.Parse(stats.treatments.Split(':')[1]));

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
