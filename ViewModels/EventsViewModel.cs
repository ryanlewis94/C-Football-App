using FootballApp.Api;
using FootballApp.Classes;
using FootballApp.Utility;
using System;
using System.Collections.Generic;
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
                    if (country.matchList != null)
                    {
                        CurrentCountry = country;

                        MatchSelected = true;
                        FixtureSelected = false;

                        if (country.matchList.time != "FT")
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
                    }
                }
                Messenger.Default.Send("matchOpened");
                Messenger.Default.Send(0); //TabIndex
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Adds images for the different events and sorts the home and away into different lists
        /// </summary>
        private void SortEvents()
        {
            try
            {
                HomeEventsList = new List<Event>();
                AwayEventsList = new List<Event>();
                BlankEvent = new Event();

                foreach (Event @event in EventsList)
                {
                    int idx = @event.player.LastIndexOf(" ");
                    string playerToAdd;
                    if (idx != -1)
                    {
                        playerToAdd = $"{@event.player.Substring(idx+1)} {@event.player.Substring(0, idx)}";
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
                    }
                    else
                    {
                        AwayEventsList.Insert(0, EventToAdd);
                        HomeEventsList.Insert(0, BlankEvent);
                    }
                }

                NoEvents = (EventsList.Count != 0) ? false : true;

                Console.WriteLine(HomeEventsList.Count);
                Console.WriteLine(AwayEventsList.Count);

                TimeUpdated = (!FullTime) ?
                    $"Last Updated: {DateTime.Now.ToString("HH:mm:ss")}" :
                    "";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
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
            TimeSpan ts = FixtureKickOffTime.Subtract(DateTime.Now);
            CountdownTime = ts.ToString("d' days 'h' hrs 'm' min 's' sec'");
            CountdownTimer.Start();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = FixtureKickOffTime.Subtract(DateTime.Now);
            CountdownTime = ts.ToString("d' days 'h' hrs 'm' min 's' sec'");
        }
    }
}
