using FootballApp.Api;
using FootballApp.Classes;
using FootballApp.Commands;
using FootballApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<Country> _originalList;

        public List<Country> OriginalList
        {
            get { return _originalList; }
            set { SetProperty(ref _originalList, value); }
        }

        private List<Country> _searchCountryList;

        public List<Country> SearchCountryList
        {
            get { return _searchCountryList; }
            set { SetProperty(ref _searchCountryList, value); }
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

        private List<Country> _federationList;

        public List<Country> FederationList
        {
            get { return _federationList; }
            set { SetProperty(ref _federationList, value); }
        }

        private List<Competition> _competitionList;

        public List<Competition> CompetitionList
        {
            get { return _competitionList; }
            set { SetProperty(ref _competitionList, value); }
        }

        private List<Match> _matchList;
        public List<Match> MatchList
        {
            get { return _matchList; }
            set { SetProperty(ref _matchList, value); }
        }

        private List<Match> _pastMatchList;
        public List<Match> PastMatchList
        {
            get { return _pastMatchList; }
            set { SetProperty(ref _pastMatchList, value); }
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
        /// checks if the countries and leagues have already been loaded to reduce calls to api
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

        /// <summary>
        /// checks if still processing request
        /// </summary>
        private bool _isProcessing = false;
        public bool IsProcessing
        {
            get { return _isProcessing; }
            set
            {
                SetProperty(ref _isProcessing, value);
            }
        }

        /// <summary>
        /// checks if country list is empty
        /// </summary>
        private bool _noCountries = false;
        public bool NoCountries
        {
            get { return _noCountries; }
            set
            {
                SetProperty(ref _noCountries, value);
            }
        }

        /// <summary>
        /// Message when no results are found
        /// </summary>
        private string _noResults;
        public string NoResults
        {
            get { return _noResults; }
            set
            {
                SetProperty(ref _noResults, value);
            }
        }

        /// <summary>
        /// Search String
        /// </summary>
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetProperty(ref _searchText, value);
                Search(_searchText);
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
            Messenger.Default.Register<string>(this, FinishedProcessing);
            MatchSelectedCommand = new CustomCommand(SelectMatch, CanSelectMatch);
            MatchClickedCommand = new CustomCommand(MatchClicked, CanClickMatch);
        }

        /// <summary>
        /// checks if match is still processing
        /// </summary>
        /// <param name="obj"></param>
        private void FinishedProcessing(string obj)
        {
            if (obj == "loaded") IsProcessing = false;
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
            Messenger.Default.Send("unloaded");
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
            Messenger.Default.Send("unloaded");
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
                FederationList = await repository.LoadFederation();
                CompetitionList = await repository.LoadCompetition();
                CompetitionList = CompetitionList.OrderBy(c => c.id).ToList();
                CountryList = await repository.LoadCountry();
                CheckDate();
                CountriesLoaded = true;
            }
            catch (Exception ex)
            {
                errorHandler.CheckErrorMessage(ex);
            }
        }

        /// <summary>
        /// if the date selected is todays date then load the live matches and at to the matches already played
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
                MatchList = (PastMatchList?.Count != 0) ?
                    MatchList.Concat(PastMatchList).ToList() :
                    MatchList;
                CreateGroupedList();
            }
            catch (Exception ex)
            {
                errorHandler.CheckErrorMessage(ex);
            }
        }

        /// <summary>
        /// gets all the fixtures for the date selected
        /// </summary>
        private async void GetFixtures()
        {
            try
            {
                //Load the fixtures from multiple pages
                int i = 0;
                FixturePageList = new List<Fixture>();
                do
                {
                    i = i + 1;

                    FixtureList = await repository.LoadFixture(DateSelected, i);
                    FixturePageList = FixturePageList.Concat(FixtureList).ToList();

                } while (FixtureList.Count == 30);
                FixtureList = FixturePageList;

                //Load the past matches from multiple pages
                i = 0;
                var prevMatches = new List<Match>();
                do
                {
                    i = i + 1;

                    PastMatchList = await repository.LoadPast(DateSelected, i);
                    prevMatches = prevMatches.Concat(PastMatchList).ToList();

                } while (PastMatchList.Count == 30);
                PastMatchList = prevMatches;

                //if countries haven't been loaded before
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
                errorHandler.CheckErrorMessage(ex);
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
                    foreach (Competition competition in CompetitionList)
                    {
                        if (competition.countries.Count != 0)
                        {
                            if (competition.countries[0].id == country.id)
                            {
                                CheckForFixturesAndMatches(country, competition, null);
                            }
                        }
                        else// if (competition.federations.Count != 0)
                        {
                            foreach (Country federation in FederationList)
                            {
                                if (competition.federations[0].id == federation.id)
                                {
                                    CheckForFixturesAndMatches(null, competition, federation);
                                }
                            }
                        }

                    }
                }
                //removes any duplicates 
                SortCountryList = SortCountryList.GroupBy(f => f.index).Select(c => c.First()).ToList();
                //order alphabetically by country name 
                //SortCountryList = SortCountryList.OrderBy(c => c.name).ToList();
                OriginalList = SortCountryList;
                MainList = SortCountryList;
                if (SearchText != null) Search(SearchText);
            }
            catch (Exception ex)
            {
                errorHandler.CheckErrorMessage(ex);
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
                        //Checks if the fixture has kicked off, and select it if it is
                        if (CurrentCountry.fixtureList != null && country.matchList != null)
                        {
                            if ((CurrentCountry.fixtureList.home_name == country.matchList.home_name) || 
                                (CurrentCountry.fixtureList.away_name == country.matchList.away_name))
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
                errorHandler.CheckErrorMessage(ex);
            }
        }

        /// <summary>
        /// sorts the matches and fixtures into the countries 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="competition"></param>
        /// <param name="federation"></param>
        private void CheckForFixturesAndMatches(Country country, Competition competition, Country federation)
        {
            try
            {
                foreach (Match match in MatchList)
                {
                    if (match.competition_id == competition.id.ToString())
                    {
                        if (!match.score.Contains("?"))
                        {
                            match.home_name = match.home_name.Replace("amp;", "");
                            match.away_name = match.away_name.Replace("amp;", "");

                            var countryToAdd = new Country
                            {
                                index = match.id,
                                id = (country != null) ? country.id : federation.id,
                                league_id =match.league_id,
                                competition_id = competition.id.ToString(),
                                name = (country != null) ? country.name : federation.name,
                                leagueName = competition.name,
                                matchList = match,
                                fixtureList = null,
                                flag = (competition.countries.Count != 0) ? 
                                    competition.countries[0].flag : null
                            };

                            SortCountryList.Add(countryToAdd);
                        }
                    }
                }
                foreach (Fixture fixture in FixtureList)
                {
                    if (fixture.competition_id == competition.id.ToString())
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
                            home_id = fixture.home_id,
                            away_id = fixture.away_id,
                            home_name = fixture.home_name.Replace("amp;", ""),
                            away_name = fixture.away_name.Replace("amp;", ""),
                            league_id = fixture.league_id,
                            competition_id = fixture.competition_id
                        };

                        var countryToAdd = new Country
                        {
                            index = fixture.id,
                            id = (country != null) ? country.id : federation.id,
                            league_id = fixture.league_id,
                            competition_id = competition.id.ToString(),
                            name = (country != null) ? country.name : federation.name,
                            leagueName = competition.name,
                            matchList = null,
                            fixtureList = fixtureToAdd,
                            flag = (competition.countries.Count != 0) ?
                                competition.countries[0].flag : null
                        };

                        SortCountryList.Add(countryToAdd);
                    }

                }
            }
            catch (Exception ex)
            {
                errorHandler.CheckErrorMessage(ex);
            }
        }

        /// <summary>
        /// When a user selects a match
        /// </summary>
        /// <param name="obj"></param>
        private void SelectMatch(object obj)
        {
            if (IsProcessing) return;
            try
            {
                if (SelectedCountry != null)
                {
                    if (SelectedCountry.matchList != null || SelectedCountry.fixtureList != null)
                    {
                        if (SelectedCountry != CurrentCountry)
                        {
                            IsProcessing = true;
                            Messenger.Default.Send("unloaded");
                            Messenger.Default.Send(SelectedCountry);
                            CurrentCountry = SelectedCountry;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorHandler.CheckErrorMessage(ex);
            }
            finally
            {
                Messenger.Default.Send(0); //TabIndex
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
            return MainList.Count != 0;
        }
        private bool CanClickMatch(object obj)
        {
            return MainList.Count != 0;
        }

        /// <summary>
        /// sorts the list by the users search (includes abreviations of club names)
        /// </summary>
        /// <param name="searchText"></param>
        private void Search(string searchText)
        {
            try
            {
                if (searchText != null)
                {
                    SearchCountryList = new List<Country>();
                    System.Text.RegularExpressions.Regex initials = new System.Text.RegularExpressions.Regex(@"(\b[a-zA-Z])[a-zA-Z]* ?");

                    foreach (Country country in OriginalList)
                    {
                        if (country.matchList?.id != null)
                        {
                            string hometeam = initials.Replace(country.matchList.home_name, "$1");
                            string awayteam = initials.Replace(country.matchList.away_name, "$1");

                            if (country.name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                            country.leagueName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                            country.matchList.home_name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                            country.matchList.away_name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                            hometeam.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                            awayteam.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                            {
                                SearchCountryList.Add(country);
                            }
                        }
                        if (country.fixtureList?.id != null)
                        {
                            string hometeam = initials.Replace(country.fixtureList.home_name, "$1");
                            string awayteam = initials.Replace(country.fixtureList.away_name, "$1");

                            if (country.name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                            country.leagueName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                            country.fixtureList.home_name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                            country.fixtureList.away_name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                            hometeam.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                            awayteam.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                            {
                                SearchCountryList.Add(country);
                            }
                        }
                    }
                    MainList = SearchCountryList;
                    NoResults = $"Sorry we couldn't find anything matching \"{searchText}\"";
                    NoCountries = (MainList.Count != 0) ? false : true;
                }
            }
            catch (Exception ex)
            {
                errorHandler.CheckErrorMessage(ex);
            }
        }
    }
}
