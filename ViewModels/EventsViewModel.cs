using FootballApp.Api;
using FootballApp.Classes;
using FootballApp.Utility;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;

namespace FootballApp.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        private IFootball repository;

        private DispatcherTimer CountdownTimer { get; set; }

        #region Properties

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
                Thread thread = new Thread(new ThreadStart(delegate ()
                {
                    Thread.Sleep(0);
                    
                    HomeEventsList = new List<Event>();
                    AwayEventsList = new List<Event>();
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
                                    HomeEventsList.Insert(0, EventToAdd);
                                    AwayEventsList.Insert(0, BlankEvent);
                                    TimeList.Insert(0, BlankEvent);
                                }
                                else
                                {
                                    AwayEventsList.Insert(0, EventToAdd);
                                    HomeEventsList.Insert(0, BlankEvent);
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
                        }
                        else
                        {
                            NoEvents = true;
                        }
                    }
                    else
                    {
                        NoEvents = true;
                    }

                    TimeUpdated = (!FullTime) ?
                        $"Last Updated: {DateTime.Now.ToString("HH:mm:ss")}" :
                        "";
                }));
                thread.Start();
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
