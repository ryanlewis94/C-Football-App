using FootballApp.Api;
using FootballApp.Classes;
using FootballApp.Commands;
using FootballApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace FootballApp.ViewModels
{
    public class CountryViewModel: ViewModelBase
    {
        private IFootball repository;

        public ICommand MatchSelectedCommand { get; set; }
        public ICommand MatchClickedCommand { get; set; }

        private DispatcherTimer MatchTimer { get; set; }
        private DispatcherTimer FixtureTimer { get; set; }

        #region Properties

        /// <summary>
        /// Lists for displaying Countries and sorting them when user searches
        /// </summary>

        private List<Country> _mainList;

        public List<Country> MainList
        {
            get { return _mainList; }
            set { SetProperty(ref _mainList, value); }
        }

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
            }
        }

        /// <summary>
        /// Date selected from date picker
        /// </summary>
        private DateTime? _dateSelected;
        public DateTime? DateSelected
        {
            get { return _dateSelected; }
            set
            {
                SetProperty(ref _dateSelected, value);
                GetFixtures();
                Messenger.Default.Send("unloaded");
            }
        }

        /// <summary>
        /// checks if the countries and leagues have already beenloaded to reduce calls to api
        /// </summary>
        private bool _countriesLoaded = false;
        public bool CountriesLoaded
        {
            get { return _countriesLoaded; }
            set
            {
                SetProperty(ref _countriesLoaded, value);
            }
        }

        /// <summary>
        /// if invoked by timer dont make a matchlist call to reduce api usage
        /// </summary>
        private bool _invokedByFixtureTimer = false;
        public bool InvokedByFixtureTimer
        {
            get { return _invokedByFixtureTimer; }
            set
            {
                SetProperty(ref _invokedByFixtureTimer, value);
            }
        }

        #endregion

        public CountryViewModel()
        {
            repository = new Football();
            LoadCommands();
            LoadTimers();
        }

        private void LoadCommands()
        {
            MatchSelectedCommand = new CustomCommand(SelectMatch, CanSelectMatch);
            MatchClickedCommand = new CustomCommand(MatchClicked, CanClickMatch);
        }

        /// <summary>
        /// starts the timers to update the list of matches and fixtures
        /// </summary>
        private void LoadTimers()
        {
            MatchTimer = new DispatcherTimer();
            FixtureTimer = new DispatcherTimer();
            LoadMatchTimer();
            LoadFixtureTimer();
        }

        /// <summary>
        /// sets the match timer to update every 60 seconds
        /// </summary>
        private void LoadMatchTimer()
        {
            if (MatchTimer.IsEnabled)
            {
                MatchTimer.Stop();
            }
            MatchTimer = new DispatcherTimer();
            MatchTimer.Interval = TimeSpan.FromSeconds(60);
            MatchTimer.Tick += MatchTimer_Tick;
            MatchTimer.Start();
        }

        private void MatchTimer_Tick(object sender, EventArgs e)
        {
            CheckDate();
        }

        /// <summary>
        /// sets the fixture timer to update every 5 minutes
        /// </summary>
        private void LoadFixtureTimer()
        {
            if (FixtureTimer.IsEnabled)
            {
                FixtureTimer.Stop();
            }
            FixtureTimer = new DispatcherTimer();
            FixtureTimer.Interval = TimeSpan.FromSeconds(300);
            FixtureTimer.Tick += FixtureTimer_Tick;
            FixtureTimer.Start();
        }

        private void FixtureTimer_Tick(object sender, EventArgs e)
        {
            InvokedByFixtureTimer = true;
            GetFixtures();
        }

        /// <summary>
        /// loads all the countries leagues and matches ready to sort into the grouped list
        /// </summary>
        private async void LoadCountries()
        {
            try
            {
                CountryList = await repository.LoadCountry();
                AllLeagueList = await repository.LoadLeague();
                AllLeagueList = AllLeagueList.OrderBy(l => l.id).ToList();
                CheckDate();
                CountriesLoaded = true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// if the date selected is todays date then load the live matches
        /// </summary>
        private async void CheckDate()
        {
            try
            { 
                string[] dateNow = DateTime.Now.ToString().Split(' ');
                string currentDate = $"{dateNow[0]} 00:00:00";

                MatchList = (DateSelected.ToString() == currentDate) ? 
                    await repository.LoadLive() : 
                    new List<Match>();

                CreateGroupedList();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// gets all the fixtures for the date selected
        /// </summary>
        private async void GetFixtures()
        {
            try
            {
                int i = 0;
                FixturePageList = new List<Fixture>();
                do
                {
                    i = i + 1;

                    FixtureList = await repository.LoadFixture(DateSelected, i);
                    FixturePageList = FixturePageList.Concat(FixtureList).ToList();

                } while (FixtureList.Count == 30);
                FixtureList = FixturePageList;

                if (!CountriesLoaded)
                {
                    LoadCountries();
                }
                else
                {
                    //date changed
                    if (!InvokedByFixtureTimer)
                    {
                        CheckDate();
                    }
                    //fixture timer
                    else
                    {
                        CreateGroupedList();
                    }
                }
                InvokedByFixtureTimer = false;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// creates a list only with countries that contain games 
        /// </summary>
        private void CreateGroupedList()
        {
            SortCountryList = new List<Country>();

            try
            {
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
                                    //splitTime[0] = (int.Parse(splitTime[0]) + 1).ToString();

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
                //removes any duplicates 
                SortCountryList = SortCountryList.GroupBy(f => f.index).Select(c => c.First()).ToList();
                MainList = SortCountryList;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }

            try
            {
                //When recreating the list check if the user had selected a match and keep it selected
                if (CurrentCountry != null)
                {
                    foreach (Country country in MainList)
                    {
                        if (CurrentCountry.matchList != null && country.matchList != null)
                        {
                            if (CurrentCountry.matchList.id == country.matchList.id)
                            {
                                SelectedCountry = country;
                                CurrentCountry = country;
                                Messenger.Default.Send(SelectedCountry);
                                break;
                            }
                        }
                        if (CurrentCountry.fixtureList != null && country.fixtureList != null)
                        {
                            if (CurrentCountry.fixtureList.id == country.fixtureList.id)
                            {
                                SelectedCountry = country;
                                CurrentCountry = country;
                                Messenger.Default.Send(SelectedCountry);
                                break;
                            }
                        }
                    }
                }
                Messenger.Default.Send("loaded");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// When a user selects a match
        /// </summary>
        /// <param name="obj"></param>
        private void SelectMatch(object obj)
        {
            try
            {
                if (SelectedCountry != null)
                {
                    if (SelectedCountry.matchList != null || SelectedCountry.fixtureList != null)
                    {
                        if (SelectedCountry != CurrentCountry)
                        {
                            Messenger.Default.Send("unloaded");
                            Messenger.Default.Send(SelectedCountry);
                            CurrentCountry = SelectedCountry;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }
        
        /// <summary>
        /// This is necessary to cut out multiple selection
        /// </summary>
        /// <param name="obj"></param>
        private void MatchClicked(object obj)
        {
            SelectedCountry = null;
        }

        private bool CanSelectMatch(object obj)
        {
            return CountryList.Count != 0;
        }
        private bool CanClickMatch(object obj)
        {
            return CountryList.Count != 0;
        }
    }
}
