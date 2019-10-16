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
    public class StandingsViewModel : ViewModelBase
    {
        private IFootball repository;
        //public ICommand BackToLeagueCommand { get; set; }

        #region Properties

        /// <summary>
        /// stores the league standings
        /// </summary>
        private List<Table> _standingsList;

        public List<Table> StandingsList
        {
            get { return _standingsList; }
            set { SetProperty(ref _standingsList, value); }
        }

        /// <summary>
        /// bools for visibility converter
        /// </summary>
        private bool _listOfStandings;
        public bool ListOfStandings
        {
            get { return _listOfStandings; }
            set { SetProperty(ref _listOfStandings, value); }
        }

        private bool _noStandings;
        public bool NoStandings
        {
            get { return _noStandings; }
            set { SetProperty(ref _noStandings, value); }
        }

        /// <summary>
        /// stores the message if the league selected doesn't contain a table
        /// </summary>
        private string _noLeagueMessage;

        public string NoLeagueMessage
        {
            get { return _noLeagueMessage; }
            set { SetProperty(ref _noLeagueMessage, value); }
        }     

        #endregion

        public StandingsViewModel()
        {
            repository = new Football();
            LoadStandings();
        }

        private void LoadStandings()
        {
            Messenger.Default.Register<Country>(this, OnLeagueReceived);
        }

        private async void OnLeagueReceived(Country country)
        {
            if (!string.IsNullOrEmpty(country.league_id))
            {
                Messenger.Default.Send("unloaded");
                StandingsList = await repository.LoadStandings(country.league_id);

                HighlightCurrentTeams(country);

                if (StandingsList != null)
                {
                    Messenger.Default.Send("leagueAvailable");
                }
                else
                {
                    Messenger.Default.Send("leagueUnavailable");
                }
            }
            
            //Hides the loading overlay
            Messenger.Default.Send("loaded");
        }

        private void HighlightCurrentTeams(Country country)
        {
            foreach (Table table in StandingsList)
            {
                if (table.name == country.fixtureList.home_name || table.name == country.fixtureList.away_name)
                {
                    table.State = "selected";
                }
            }
        }
    }
}
