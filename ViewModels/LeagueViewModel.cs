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
    public class LeagueViewModel : ViewModelBase
    {
        private IFootball repository;

        public ICommand LeagueSelectedCommand { get; set; }
        public ICommand SortLeaguesCommand { get; set; }

        #region Properties

        /// <summary>
        /// All lists for sorting and filtering the leagues
        /// </summary>
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

        private List<League> _searchList;

        public List<League> SearchList
        {
            get { return _searchList; }
            set { SetProperty(ref _searchList, value); }
        }

        /// <summary>
        /// Stores the league selected by the user
        /// </summary>
        private League _selectedLeague;

        public League SelectedLeague
        {
            get { return _selectedLeague; }
            set { SetProperty(ref _selectedLeague, value); }
        }

        /// <summary>
        /// changes the index of the main tab control
        /// </summary>
        private int _tabIndex;

        public int TabIndex
        {
            get { return _tabIndex; }
            set { SetProperty(ref _tabIndex, value); }
        }

        /// <summary>
        /// stores the users search input
        /// </summary>
        private string _leagueSearch;

        public string LeagueSearch
        {
            get { return _leagueSearch; }
            set { SetProperty(ref _leagueSearch, value); }
        }

        /// <summary>
        /// stores the countries name that is selected
        /// </summary>
        private string _countryName;

        public string CountryName
        {
            get { return _countryName; }
            set { SetProperty(ref _countryName, value); }
        }

        private string _searchCriteria;

        public string SearchCriteria
        {
            get { return _searchCriteria; }
            set { SetProperty(ref _searchCriteria, value); }
        }

        /// <summary>
        /// bools for the visibility converter
        /// </summary>
        private bool _leagueSearchBox;
        public bool LeagueSearchBox
        {
            get { return _leagueSearchBox; }
            set { SetProperty(ref _leagueSearchBox, value); }
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

        private bool _noSearchResults;
        public bool NoSearchResults
        {
            get { return _noSearchResults; }
            set { SetProperty(ref _noSearchResults, value); }
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

        #endregion

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

        private async void LoadLeagues()
        {
            MainList = await repository.LoadLeague();
            LeagueList = MainList;
            MemoryList = MainList;
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
                FilterLeagueByCountry(country.id);
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

        /// <summary>
        /// filters the league list based on the country that was selected
        /// </summary>
        /// <param name="countryid"></param>
        private void FilterLeagueByCountry(string countryid)
        {
            LeagueList = new List<League>();

            foreach (League league in MainList)
            {
                if (league.country_id == countryid)
                {
                    LeagueList.Add(league);
                }
            }

            LeagueList = LeagueList.OrderBy(l => l.id).ToList();
            MemoryList = LeagueList;
        }

        private void LeagueListCountCheck()
        {
            if (LeagueList.Count == 0)
            {
                LeagueSearchBox = false;
                ListOfLeagues = false;
                NoLeagues = true;
            }
            else
            {
                LeagueSearchBox = true;
                ListOfLeagues = true;
                NoLeagues = false;
            }
        }

        /// <summary>
        /// sorts the league list based on the users search input
        /// </summary>
        /// <param name="leagueSearch"></param>
        private void SortLeagueList(string leagueSearch)
        {
            SearchList = new List<League>();

            foreach (League league in MemoryList)
            {
                if (league.name.Contains(leagueSearch, StringComparison.OrdinalIgnoreCase))
                {
                    SearchList.Add(league);
                }
            }

            LeagueList = SearchList;

            if (LeagueList.Count == 0)
            {
                SearchCriteria = $"No results found with criteria of '{leagueSearch}'";
                ListOfLeagues = false;
                NoSearchResults = true;
            }
            else
            {
                ListOfLeagues = true;
                NoSearchResults = false;
            }
        }

        /// <summary>
        /// When the user selects a league
        /// </summary>
        /// <param name="obj"></param>
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
    }
}
