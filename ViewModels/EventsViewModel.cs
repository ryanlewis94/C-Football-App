using FootballApp.Api;
using FootballApp.Classes;
using FootballApp.Commands;
using FootballApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FootballApp.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        private IFootball repository;

        public ICommand CloseMatchCommand { get; set; }

        #region Properties

        private DispatcherTimer Timer { get; set; }

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

        #endregion

        public EventsViewModel()
        {
            repository = new Football();
            LoadEvents();
            LoadCommands();
            LoadTimers();
        }

        /// <summary>
        /// sets the timer
        /// </summary>
        private void LoadTimers()
        {
            Timer = new DispatcherTimer();
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
            UpdateMatchList(await repository.LoadLive("0"));
            EventsList = await repository.LoadEvents(CurrentMatch.id);
            SortEvents();
        }

        private void LoadCommands()
        {
            CloseMatchCommand = new CustomCommand(CloseMatch, CanCloseMatch);
        }

        private void LoadEvents()
        {
            Messenger.Default.Register<Match>(this, OnMatchReceived);
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
                }
            }
        }

        /// <summary>
        /// When a match gets selected
        /// </summary>
        /// <param name="match"></param>
        private async void OnMatchReceived(Match match)
        {
            if (match != null)
            {
                LoadTimers();
                CurrentMatch = match;
                EventsList = await repository.LoadEvents(match.id);
                SortEvents();
                Messenger.Default.Send("matchOpened");
            }

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
                switch (@event.@event)
                {
                    case "GOAL":
                        @event.@event = "/Resources/events/Goal.png";
                        break;
                    case "GOAL_PENALTY":
                        @event.@event = "/Resources/events/Penalty.png";
                        break;
                    case "OWN_GOAL":
                        @event.@event = "/Resources/events/ownGoal.png";
                        break;
                    case "YELLOW_CARD":
                        @event.@event = "/Resources/events/Yellow.png";
                        break;
                    case "RED_CARD":
                        @event.@event = "/Resources/events/Red.png";
                        break;
                    case "YELLOW_RED_CARD":
                        @event.@event = "/Resources/events/YellowRed.png";
                        break;
                    default:
                        break;
                }

                if (@event.home_away == "h")
                {
                    HomeEventsList.Insert(0, @event);
                    AwayEventsList.Insert(0, BlankEvent);
                }
                else
                {
                    AwayEventsList.Insert(0, @event);
                    HomeEventsList.Insert(0, BlankEvent);
                }
            }
        }

        /// <summary>
        ///Close button to close the live events screen 
        /// </summary>
        /// <param name="obj"></param>
        private void CloseMatch(object obj)
        {
            MessageBox.Show(HomeEventsList.Count.ToString());
            MessageBox.Show(AwayEventsList.Count.ToString());
            Messenger.Default.Send("matchClosed");
            Timer.Stop();
        }

        private bool CanCloseMatch(object obj)
        {
            return CurrentMatch != null;
        }
    }
}
