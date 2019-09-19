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

        private Country _selectedCountry;

        public Country SelectedCountry
        {
            get { return _selectedCountry; }
            set { SetProperty(ref _selectedCountry, value); }
        }

        private int _tabIndex;

        public int TabIndex
        {
            get { return _tabIndex; }
            set { SetProperty(ref _tabIndex, value); }
        }

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
        }

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

        private void SelectCountry(object obj)
        {
            if (SelectedCountry != null)
            {
                Messenger.Default.Send(SelectedCountry);
                TabIndex = 1;
                Messenger.Default.Send<int>(TabIndex);
            }
        }

        private bool CanSelectCountry(object obj)
        {
            return CountryList.Count > 0;
        }

        private async void LoadCountries()
        {
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
        }
    }
}
