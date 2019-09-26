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

namespace FootballApp.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        private IFootball repository;

        #region Properties

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
        }

        private void LoadEvents()
        {
            Messenger.Default.Register<Match>(this, OnMatchReceived);
        }

        private async void OnMatchReceived(Match match)
        {
            if(match != null)
            {
                CurrentMatch = match;
                EventsList = await repository.LoadEvents(match.id);
                SortEvents();
            }
            
        }

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
    }
}
