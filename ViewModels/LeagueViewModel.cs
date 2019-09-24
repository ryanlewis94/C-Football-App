﻿using FootballApp.Api;
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
    public class LeagueViewModel : ViewModelBase
    {
        private IFootball repository;

        public ICommand LeagueSelectedCommand { get; set; }
        public ICommand SortLeaguesCommand { get; set; }

        private List<League> _mainList;

        public List<League> MainList
        {
            get { return _mainList; }
            set { SetProperty(ref _mainList, value); }
        }

        private List<League> _memoryList;

        public List<League> MemoryList
        {
            get { return _memoryList; }
            set { SetProperty(ref _memoryList, value); }
        }

        private List<League> _leagueList;

        public List<League> LeagueList
        {
            get { return _leagueList; }
            set { SetProperty(ref _leagueList, value); }
        }

        private List<League> _searchLeagueList;

        public List<League> SearchLeagueList
        {
            get { return _searchLeagueList; }
            set { SetProperty(ref _searchLeagueList, value); }
        }

        private League _selectedLeague;

        public League SelectedLeague
        {
            get { return _selectedLeague; }
            set { SetProperty(ref _selectedLeague, value); }
        }

        private int _tabIndex;

        public int TabIndex
        {
            get { return _tabIndex; }
            set { SetProperty(ref _tabIndex, value); }
        }

        private string _countryName;

        public string CountryName
        {
            get { return _countryName; }
            set { SetProperty(ref _countryName, value); }
        }

        private bool _listOfLeagues;
        public bool ListOfLeagues
        {
            get { return _listOfLeagues; }
            set { SetProperty(ref _listOfLeagues, value); }
        }

        private bool _noLeagues;
        public bool NoLeagues
        {
            get { return _noLeagues; }
            set { SetProperty(ref _noLeagues, value); }
        }

        private string _leagueSearch;

        public string LeagueSearch
        {
            get { return _leagueSearch; }
            set { SetProperty(ref _leagueSearch, value); }
        }

        private string _leagueSelectedBool;

        public string LeagueSelectedBool
        {
            get { return _leagueSelectedBool; }
            set { SetProperty(ref _leagueSelectedBool, value); }
        }

        private string _loadingData;

        public string LoadingData
        {
            get { return _loadingData; }
            set { SetProperty(ref _loadingData, value); }
        }

        public LeagueViewModel()
        {
            repository = new Football();
            LoadLeagues();
            LoadCommands();
        }

        private void LoadCommands()
        {
            LeagueSelectedCommand = new CustomCommand(SelectLeague, CanSelectLeague);
            SortLeaguesCommand = new DelegateCommand<string>(SortLeagueList, () => true);
        }

        private void SortLeagueList(string leagueSearch)
        {
            LeagueList = MemoryList;
            SearchLeagueList = new List<League>();

            foreach (League league in LeagueList)
            {
                if (league.name.Contains(leagueSearch, StringComparison.OrdinalIgnoreCase))
                {
                    SearchLeagueList.Add(league);
                }
            }

            LeagueList = SearchLeagueList;

            if(LeagueList.Count == 0)
            {
                
            }
        }

        private void SelectLeague(object obj)
        {
            if (SelectedLeague != null)
            {
                Messenger.Default.Send(SelectedLeague);
                LeagueSelectedBool = "selected";
                Messenger.Default.Send(LeagueSelectedBool);
                TabIndex = 2;
                Messenger.Default.Send<int>(TabIndex);
                LoadingData = "unloaded";
                Messenger.Default.Send(LoadingData);
            }
        }

        private bool CanSelectLeague(object obj)
        {
            return LeagueList.Count != 0;
        }

        private async void LoadLeagues()
        {
            MainList = await repository.LoadLeague();
            MemoryList = MainList;
            LeagueList = MainList;
            LeagueListCountCheck();

            Messenger.Default.Register<Country>(this, OnCountryReceived);

            LoadingData = "loaded";
            Messenger.Default.Send(LoadingData);
        }

        private void OnCountryReceived(Country country)
        {
            LeagueSearch = "";
            CountryName = $"No Leagues in {country.name}";

            if (!string.IsNullOrEmpty(country.id))
            {
                LeagueList = MainList;
                FilterLeagueByCountry(country.id);
                MemoryList = LeagueList;
            }
            else
            {
                LeagueList = MainList;
                MemoryList = MainList;
                LeagueSelectedBool = "unselected";
                Messenger.Default.Send(LeagueSelectedBool);
                SelectedLeague = null;
                SelectedLeague = new League();
                Messenger.Default.Send(SelectedLeague);
            }
            LeagueListCountCheck();
        }

        private void LeagueListCountCheck()
        {
            if (LeagueList.Count == 0)
            {
                ListOfLeagues = false;
                NoLeagues = true;
            }
            else
            {
                ListOfLeagues = true;
                NoLeagues = false;
            }
        }

        private void FilterLeagueByCountry(string countryid)
        {
            SearchLeagueList = new List<League>();

            foreach (League league in LeagueList)
            {
                if (league.country_id == countryid)
                {
                    SearchLeagueList.Add(league);
                }
            }

            LeagueList = SearchLeagueList.OrderBy(l => l.id).ToList();
        }
    }
}
