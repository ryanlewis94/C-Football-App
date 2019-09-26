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
    public class CountryViewModel: ViewModelBase
    {
        private IFootball repository;

        public ICommand CountrySelectedCommand { get; set; }
        public ICommand SortCountriesCommand { get; set; }
        public ICommand ClearSelectionCommand { get; set; }

        #region Properties

        /// <summary>
        /// Lists for displaying Countries and sorting them when user searches
        /// </summary>
        private List<Country> _memoryList;

        public List<Country> MemoryList
        {
            get { return _memoryList; }
            set { SetProperty(ref _memoryList, value); }
        }

        private List<Country> _countryList;

        public List<Country> CountryList
        {
            get { return _countryList; }
            set { SetProperty(ref _countryList, value); }
        }

        private List<Country> _searchCountryList;

        public List<Country> SearchCountryList
        {
            get { return _searchCountryList; }
            set { SetProperty(ref _searchCountryList, value); }
        }

        /// <summary>
        /// Country that was selected by the user
        /// </summary>
        private Country _selectedCountry;

        public Country SelectedCountry
        {
            get { return _selectedCountry; }
            set { SetProperty(ref _selectedCountry, value); }
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
        /// bools for visibility converter 
        /// </summary>
        private bool _listOfCountries;
        public bool ListOfCountries
        {
            get { return _listOfCountries; }
            set { SetProperty(ref _listOfCountries, value); }
        }

        private bool _noCountries;
        public bool NoCountries
        {
            get { return _noCountries; }
            set { SetProperty(ref _noCountries, value); }
        }

        private bool _clearButton;

        public bool ClearButton
        {
            get { return _clearButton; }
            set { SetProperty(ref _clearButton, value); }
        }

        /// <summary>
        /// the users search input
        /// </summary>
        private string _countrySearch;

        public string CountrySearch
        {
            get { return _countrySearch; }
            set { SetProperty(ref _countrySearch, value); }
        }


        private List<League> _leagueList;

        public List<League> LeagueList
        {
            get { return _leagueList; }
            set { SetProperty(ref _leagueList, value); }
        }

        //private List<Match> _liveMatchesList;

        //public List<Match> LiveMatchesList
        //{
        //    get { return _liveMatchesList; }
        //    set { SetProperty(ref _liveMatchesList, value); }
        //}

        private string _loadingData;

        public string LoadingData
        {
            get { return _loadingData; }
            set { SetProperty(ref _loadingData, value); }
        }

        #endregion

        public CountryViewModel()
        {
            repository = new Football();
            LoadCountries();
            LoadCommands();
        }

        private void LoadCommands()
        {
            CountrySelectedCommand = new CustomCommand(SelectCountry, CanSelectCountry);
            SortCountriesCommand = new DelegateCommand<string>(SortCountryList, () => true);
            ClearSelectionCommand = new CustomCommand(ClearSelection, CanClearSelection);
        }

        private async void LoadCountries()
        {
            //LiveMatchesList = await repository.LoadLive("0");
            //LeagueList = await repository.LoadLeague();
            //Messenger.Default.Send<List<League>>(LeagueList);
            MemoryList = await repository.LoadCountry();
            CountryList = MemoryList;

            if (CountryList.Count == 0)
            {
                ListOfCountries = false;
                NoCountries = true;
            }
            else
            {
                ListOfCountries = true;
                NoCountries = false;
            }

            SelectedCountry = new Country();
            Messenger.Default.Send(SelectedCountry);

            //SortCountries();
            //LoadingData = "loaded";
            //Messenger.Default.Send(LoadingData);
        }

        //private void SortCountries()
        //{
        //    SearchCountryList = new List<Country>();

        //    foreach (Country country in CountryList)
        //    {
        //        foreach (League league in LeagueList)
        //        {
        //            if (country.id == league.country_id)
        //            {
        //                foreach (Match match in LiveMatchesList)
        //                {
        //                    if (league.id.ToString() == match.league_id)
        //                    {

        //                    }
        //                }
        //            }
        //        }
        //    }

        //    CountryList = SearchCountryList;
        //    CountryList = CountryList.Distinct().ToList();
        //    MemoryList = CountryList;
        //}

        /// <summary>
        /// When user searches for a country look for countries that contain the users input
        /// </summary>
        /// <param name="countrySearch"></param>
        private void SortCountryList(string countrySearch)
        {
            CountryList = MemoryList;
            SearchCountryList = new List<Country>();

            foreach (Country country in CountryList)
            {
                if (country.name.Contains(countrySearch, StringComparison.OrdinalIgnoreCase))
                {
                    SearchCountryList.Add(country);
                }
            }

            CountryList = SearchCountryList;

            if (CountryList.Count == 0)
            {

            }
        }

        /// <summary>
        /// When user selects a Country
        /// </summary>
        /// <param name="obj"></param>
        private void SelectCountry(object obj)
        {
            if (SelectedCountry != null)
            {
                Messenger.Default.Send(SelectedCountry);
                TabIndex = 1;
                Messenger.Default.Send<int>(TabIndex);
                ClearButton = true;
            }
        }

        private bool CanSelectCountry(object obj)
        {
            return CountryList.Count > 0;
        }

        

        /// <summary>
        /// clear button resets the app and clears all of the users selections
        /// </summary>
        /// <param name="obj"></param>
        private void ClearSelection(object obj)
        {
            CountrySearch = "";
            CountryList = MemoryList;

            TabIndex = 0;
            Messenger.Default.Send(TabIndex);

            SelectedCountry = null;
            SelectedCountry = new Country();
            Messenger.Default.Send(SelectedCountry);

            ClearButton = false;
        }

        private bool CanClearSelection(object obj)
        {
            return SelectedCountry != null;
        }
    }
}
