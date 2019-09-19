using FootballApp.Api;
using FootballApp.Classes;
using FootballApp.Commands;
using FootballApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FootballApp.ViewModels
{
    public class MatchViewModel : ViewModelBase
    {

        private IFootball repository;
        public ICommand BackToLeagueCommand { get; set; }
        public ICommand GoToLiveGamesCommand { get; set; }

        private List<Match> _liveMatchesList;

        public List<Match> LiveMatchesList
        {
            get { return _liveMatchesList; }
            set { SetProperty(ref _liveMatchesList, value); }
        }

        private List<Match> _liveList;

        public List<Match> LiveList
        {
            get { return _liveList; }
            set { SetProperty(ref _liveList, value); }
        }

        private string _leagueTabName;

        public string LeagueTabName
        {
            get { return _leagueTabName; }
            set { SetProperty(ref _leagueTabName, value); }
        }

        private string _noLeagueMessage;

        public string NoLeagueMessage
        {
            get { return _noLeagueMessage; }
            set { SetProperty(ref _noLeagueMessage, value); }
        }

        private bool _listOfLiveMatches;
        public bool ListOfLiveMatches
        {
            get { return _listOfLiveMatches; }
            set { SetProperty(ref _listOfLiveMatches, value); }
        }

        private bool _noLiveMatches;
        public bool NoLiveMatches
        {
            get { return _noLiveMatches; }
            set { SetProperty(ref _noLiveMatches, value); }
        }

        private bool _listOfLive;
        public bool ListOfLive
        {
            get { return _listOfLive; }
            set { SetProperty(ref _listOfLive, value); }
        }

        private bool _noLive;
        public bool NoLive
        {
            get { return _noLive; }
            set { SetProperty(ref _noLive, value); }
        }

        private int _matchesTabIndex = 1;

        public int MatchesTabIndex
        {
            get { return _matchesTabIndex; }
            set { SetProperty(ref _matchesTabIndex, value); }
        }

        private int _tabIndex;

        public int TabIndex
        {
            get { return _tabIndex; }
            set { SetProperty(ref _tabIndex, value); }
        }


        public MatchViewModel()
        {
            repository = new Football();
            LoadMatches();
            LoadCommands();
        }

        private void LoadCommands()
        {
            BackToLeagueCommand = new CustomCommand(BackToLeague, CanBackToLeague);
            GoToLiveGamesCommand = new CustomCommand(GoToLiveGames, CanBackToLeague);
        }

        private void GoToLiveGames(object obj)
        {
            MatchesTabIndex = 1;
        }

        private void BackToLeague(object obj)
        {
            TabIndex = 1;
            Messenger.Default.Send<int>(TabIndex);
        }

        private bool CanBackToLeague(object obj)
        {
            return LiveMatchesList != null;
        }

        private async void LoadMatches()
        {
            Messenger.Default.Register<League>(this, OnLeagueReceived);
            LiveList = await repository.LoadLive("0");

            if (LiveList.Count == 0)
            {
                ListOfLive = false;
                NoLive = true;
            }
            else
            {
                ListOfLive = true;
                NoLive = false;
            }
        }

        private async void OnLeagueReceived(League league)
        {
            LiveMatchesList = await repository.LoadLive(league.id.ToString());
            MatchesTabIndex = 0;
            LeagueTabName = $"{league.name} Live Matches";
            NoLeagueMessage = $"No Matches in {league.name}";

            if (LiveMatchesList.Count == 0)
            {
                ListOfLiveMatches = false;
                NoLiveMatches = true;
            }
            else
            {
                ListOfLiveMatches = true;
                NoLiveMatches = false;
            }
        }
    }
}
