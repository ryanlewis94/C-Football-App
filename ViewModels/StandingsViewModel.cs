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
        public ICommand BackToLeagueCommand { get; set; }

        private List<Table> _standingsList;

        public List<Table> StandingsList
        {
            get { return _standingsList; }
            set { SetProperty(ref _standingsList, value); }
        }

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

        private string _noLeagueMessage;

        public string NoLeagueMessage
        {
            get { return _noLeagueMessage; }
            set { SetProperty(ref _noLeagueMessage, value); }
        }

        private int _tabIndex;

        public int TabIndex
        {
            get { return _tabIndex; }
            set { SetProperty(ref _tabIndex, value); }
        }


        public StandingsViewModel()
        {
            repository = new Football();
            LoadStandings();
            LoadCommands();
        }

        private void LoadCommands()
        {
            BackToLeagueCommand = new CustomCommand(BackToLeague, CanBackToLeague);
        }

        private void BackToLeague(object obj)
        {
            TabIndex = 1;
            Messenger.Default.Send<int>(TabIndex);
        }

        private bool CanBackToLeague(object obj)
        {
            return StandingsList == null;
        }

        private void LoadStandings()
        {
            Messenger.Default.Register<League>(this, OnLeagueReceived);
        }

        private async void OnLeagueReceived(League league)
        {
            StandingsList = await repository.LoadStandings(league.id.ToString());
            NoLeagueMessage = $"{league.name} is not a league!";
            if (StandingsList != null)
            {
                ListOfStandings = true;
                NoStandings = false;
            }
            else
            {
                ListOfStandings = false;
                NoStandings = true;
            }
        }
    }
}
