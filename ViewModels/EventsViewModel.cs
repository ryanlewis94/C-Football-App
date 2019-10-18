using FootballApp.Api;
using FootballApp.Classes;
using FootballApp.Commands;
using FootballApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FootballApp.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        private IFootball repository;

        #region Properties

        private DispatcherTimer Timer { get; set; }
        private DispatcherTimer CountdownTimer { get; set; }

    /// <summary>
    /// Store the selected Match
    /// </summary>
    private Match _currentMatch;

        public Match CurrentMatch
        {
            get { return _currentMatch; }
            set { SetProperty(ref _currentMatch, value); }
        }

        /// <summary>
        /// Store the selected Fixture
        /// </summary>
        private Fixture _currentFixture;

        public Fixture CurrentFixture
        {
            get { return _currentFixture; }
            set { SetProperty(ref _currentFixture, value); }
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

        private DateTime _fixtureKickOffTime;
        public DateTime FixtureKickOffTime
        {
            get { return _fixtureKickOffTime; }
            set { SetProperty(ref _fixtureKickOffTime, value); }
        }

        private string _countdownTime;
        public string CountdownTime
        {
            get { return _countdownTime; }
            set { SetProperty(ref _countdownTime, value); }
        }

        #endregion

        public EventsViewModel()
        {
            repository = new Football();
            LoadEvents();
        }

        /// <summary>
        /// sets the timer
        /// </summary>
        private void LoadTimer()
        {
            Timer = new DispatcherTimer();
            Timer.Stop();
            Timer.Interval = TimeSpan.FromSeconds(60);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        /// <summary>
        /// updates the live game and events every 60 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (CurrentMatch != null)
            {
                UpdateMatchList(await repository.LoadLive());
                EventsList = await repository.LoadEvents(CurrentMatch.id);
                SortEvents();
            }
        }

        private void LoadEvents()
        {
            Messenger.Default.Register<Country>(this, OnCountryReceived);
        }

        /// <summary>
        /// updates the user with the latest events and score
        /// </summary>
        /// <param name="matchList"></param>
        private void UpdateMatchList(List<Match> matchList)
        {
            foreach (Match match in matchList)
            {
                if (match.id == CurrentMatch.id)
                {
                    CurrentMatch = match;
                    Messenger.Default.Send(matchList);
                }
            }
        }

        /// <summary>
        /// When a match or fixture gets selected
        /// </summary>
        /// <param name="country"></param>
        private async void OnCountryReceived(Country country)
        {
            if (country.matchList != null)
            {
                MatchSelected = true;
                FixtureSelected = false;

                LoadTimer();
                CurrentMatch = country.matchList;
                EventsList = await repository.LoadEvents(country.matchList.id);
                SortEvents();

                Messenger.Default.Send("matchOpened");
            }
            else if (country.fixtureList != null)
            {
                MatchSelected = false;
                FixtureSelected = true;

                FixtureKickOffTime = DateTime.Parse($"{country.fixtureList.date} {country.fixtureList.time}");
                if (country.fixtureList.time == "00:00" || country.fixtureList.time == "00:30" || country.fixtureList.time == "01:00")
                {
                    FixtureKickOffTime = FixtureKickOffTime.AddDays(1);
                }

                CurrentFixture = country.fixtureList;
                
                LoadCountdown();

                Messenger.Default.Send("matchOpened");
            }
            Messenger.Default.Send(0);
        }

        /// <summary>
        /// Adds images for the different events and sorts the home and away into different lists
        /// </summary>
        private void SortEvents()
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
        }

        private void LoadCountdown()
        {
            CountdownTimer = new DispatcherTimer();
            CountdownTimer.Stop();
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
