using FootballApp.Api;
using FootballApp.Classes;
using FootballApp.Commands;
using FootballApp.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace FootballApp.ViewModels
{
    public class CountryViewModel: ViewModelBase
    {
        private IFootball repository;

        public ICommand CountrySelectedCommand { get; set; }
        public ICommand SortCountriesCommand { get; set; }
        public ICommand ClearSelectionCommand { get; set; }

        public ICommand MatchSelectedCommand { get; set; }

        #region Properties

        /// <summary>
        /// Lists for displaying Countries and sorting them when user searches
        /// </summary>

        private List<Country> _countryList;

        public List<Country> CountryList
        {
            get { return _countryList; }
            set { SetProperty(ref _countryList, value); }
        }

        private List<Country> _sortCountryList;
        public List<Country> SortCountryList
        {
            get { return _sortCountryList; }
            set { SetProperty(ref _sortCountryList, value); }
        }

        private List<League> _allLeagueList;
        public List<League> AllLeagueList
        {
            get { return _allLeagueList; }
            set { SetProperty(ref _allLeagueList, value); }
        }

        private List<Match> _matchList;
        public List<Match> MatchList
        {
            get { return _matchList; }
            set { SetProperty(ref _matchList, value); }
        }

        private List<Fixture> _fixtureList;
        public List<Fixture> FixtureList
        {
            get { return _fixtureList; }
            set { SetProperty(ref _fixtureList, value); }
        }

        private List<Fixture> _fixturePageList;
        public List<Fixture> FixturePageList
        {
            get { return _fixturePageList; }
            set { SetProperty(ref _fixturePageList, value); }
        }

        /// <summary>
        /// Country that was selected by the user
        /// </summary>
        private Country _selectedCountry;

        public Country SelectedCountry
        {
            get { return _selectedCountry; }
            set 
            { 
                SetProperty(ref _selectedCountry, value);
                CountrySelected(_selectedCountry);
                //Console.WriteLine(_selectedCountry.league_id);
            }
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

        private string _loadingData;

        public string LoadingData
        {
            get { return _loadingData; }
            set { SetProperty(ref _loadingData, value); }
        }

        private DateTime? _dateSelected;
        public DateTime? DateSelected
        {
            get { return _dateSelected; }
            set
            {
                SetProperty(ref _dateSelected, value);
                GetFixtures(_dateSelected);
                Messenger.Default.Send("unloaded");
            }
        }

        #endregion

        public CountryViewModel()
        {
            repository = new Football();
            //LoadCountries();
            LoadCommands();
        }

        private void LoadCommands()
        {
            ClearSelectionCommand = new CustomCommand(ClearSelection, CanClearSelection);
            MatchSelectedCommand = new DelegateCommand<Country>(LiveMatchSelected, () => true);
        }

        private void LiveMatchSelected(Country countrySelected)
        {
            Console.WriteLine(countrySelected.name);
        }

        private async void LoadCountries()
        {
            CountryList = await repository.LoadCountry();
            AllLeagueList = await repository.LoadLeague();
            AllLeagueList = AllLeagueList.OrderBy(l => l.id).ToList();
            string[] dateNow = DateTime.Now.ToString().Split(' ');
            string currentDate = $"{dateNow[0]} 00:00:00";
            if (DateSelected.ToString() == currentDate)
            {
                MatchList = await repository.LoadLive();
            }
            else
            {
                MatchList = new List<Match>();
            }

            CreateGroupedList();

            CountryCountCheck();

            SelectedCountry = new Country();
            Messenger.Default.Send(SelectedCountry);
        }

        private async void GetFixtures(DateTime? dateSelected)
        {
            int i = 0;
            FixturePageList = new List<Fixture>();
            do
            {
                i = i + 1;

                FixtureList = await repository.LoadFixture(dateSelected, i);
                FixturePageList = FixturePageList.Concat(FixtureList).ToList();

            } while (FixtureList.Count == 30);
            FixtureList = FixturePageList;

            LoadCountries();
        }

        private void CreateGroupedList()
        {
            SortCountryList = new List<Country>();

            foreach (Country country in CountryList)
            {
                foreach (League league in AllLeagueList)
                {
                    if (league.country_id == country.id)
                    {
                        foreach (Match match in MatchList)
                        {
                            if (match.league_id == league.id.ToString())
                            {
                                if (!match.score.Contains("?"))
                                {
                                    var countryToAdd = new Country
                                    {
                                        index = match.id,
                                        id = country.id,
                                        league_id = league.id.ToString(),
                                        name = country.name,
                                        leagueName = league.name,
                                        matchList = match,
                                        fixtureList = null
                                    };

                                    SortCountryList.Add(countryToAdd);
                                }
                            }
                        }
                        foreach (Fixture fixture in FixtureList)
                        {
                            if (fixture.league_id == league.id.ToString())
                            {
                                string[] splitTime = fixture.time.Split(':');
                                splitTime[0] = (int.Parse(splitTime[0]) + 1).ToString();

                                if (splitTime[0].Length < 2)
                                {
                                    splitTime[0] = $"0{splitTime[0]}";
                                }

                                if (splitTime[0] == "24")
                                {
                                    splitTime[0] = "00";
                                }

                                var fixtureToAdd = new Fixture
                                {
                                    id = fixture.id,
                                    date = fixture.date,
                                    time = $"{splitTime[0]}:{splitTime[1]}",
                                    home_name = fixture.home_name,
                                    away_name = fixture.away_name,
                                    league_id = fixture.league_id,
                                    competition_id = fixture.competition_id
                                };

                                var countryToAdd = new Country
                                {
                                    index = fixture.id,
                                    id = country.id,
                                    league_id = league.id.ToString(),
                                    name = country.name,
                                    leagueName = league.name,
                                    matchList = null,
                                    fixtureList = fixtureToAdd
                                };

                                SortCountryList.Add(countryToAdd);
                            }

                        }
                    }
                }
            }
            SortCountryList = SortCountryList.GroupBy(f => f.index).Select(c => c.First()).ToList();
            CountryList = SortCountryList;
            Messenger.Default.Send("loaded");
        }

        private void CountryCountCheck()
        {
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
        }

        private void CountrySelected(Country selectedCountry)
        {
            if(SelectedCountry != null)
            {
                //Display the loading overlay
                if (selectedCountry.matchList != null || selectedCountry.fixtureList != null)
                {
                    Messenger.Default.Send("unloaded");
                    Messenger.Default.Send(selectedCountry);
                }
            }
        }


        /// <summary>
        /// clear button resets the app and clears all of the users selections
        /// </summary>
        /// <param name="obj"></param>
        private void ClearSelection(object obj)
        {
            //Tab Index
            Messenger.Default.Send(0);

            SelectedCountry = null;
            SelectedCountry = new Country();
            Messenger.Default.Send(SelectedCountry);

            //closes the match window if opened
            Messenger.Default.Send("matchClosed");
            ClearButton = false;
        }

        private bool CanClearSelection(object obj)
        {
            return SelectedCountry != null;
        }
    }
}
