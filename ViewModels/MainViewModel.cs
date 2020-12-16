using FootballApp.Utility;

namespace FootballApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// changes the tab index of the main tab control
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
        private bool _leagueSelectedBool;

        public bool LeagueSelectedBool
        {
            get { return _leagueSelectedBool; }
            set { SetProperty(ref _leagueSelectedBool, value); }
        }

        private bool _teamSelectedBool;

        public bool TeamSelectedBool
        {
            get { return _teamSelectedBool; }
            set { SetProperty(ref _teamSelectedBool, value); }
        }

        private bool _loadingData = true;

        public bool LoadingData
        {
            get { return _loadingData; }
            set { SetProperty(ref _loadingData, value); }
        }

        private bool _matchData;

        public bool MatchData
        {
            get { return _matchData; }
            set { SetProperty(ref _matchData, value); }
        }

        /// <summary>
        /// keeps track of how many api requests the user has sent
        /// </summary>
        private int _requestCount = 0;

        public int RequestCount
        {
            get { return _requestCount; }
            set { SetProperty(ref _requestCount, value); }
        }

        /// <summary>
        /// keeps track of how many matches are loaded
        /// </summary>
        private int _matchCount = 0;

        public int MatchCount
        {
            get { return _matchCount; }
            set { SetProperty(ref _matchCount, value); }
        }

        /// <summary>
        /// gets the time the game is updated
        /// </summary>
        private string _timeUpdated;
        public string TimeUpdated
        {
            get { return _timeUpdated; }
            set { SetProperty(ref _timeUpdated, value); }
        }

        private string _teamName;

        public string TeamName
        {
            get { return _teamName; }
            set { SetProperty(ref _teamName, value); }
        }

        #endregion

        public MainViewModel()
        {
            MainLoad();
        }

        private void MainLoad()
        {
            Messenger.Default.Register<int>(TabIndex, OnIndexReceived);
            Messenger.Default.Register<string>(this, OnDataReceived);
        }

        /// <summary>
        /// depending on what string is received hide or make user controls visible
        /// </summary>
        /// <param name="dataLoaded"></param>
        private void OnDataReceived(string dataLoaded)
        {
            switch (dataLoaded)
            {
                //Show or hide the loading overlay
                case "loaded":
                    LoadingData = false;
                    break;
                case "unloaded":
                    LoadingData = true;
                    break;
                //Show or hide the match event window
                case "matchOpened":
                    MatchData = true;
                    break;
                case "matchClosed":
                    MatchData = false;
                    break;
                //Show or hide the standings tab
                case "leagueAvailable":
                    LeagueSelectedBool = true;
                    break;
                case "leagueUnavailable":
                    LeagueSelectedBool = false;
                    break;
                    //updates the request count, sends message if too many requests have been received
                case "request":
                    RequestCount++;
                    if (RequestCount > 300)
                    {
                        Messenger.Default.Send("TooManyRequests");
                    }
                    break;
                    //resets the request count every 60 seconds
                case "resetRequest":
                    RequestCount = 0;
                    break;
                case "TooManyRequests":
                    break;
                default:
                    int i = 0;
                    if (int.TryParse(dataLoaded, out i))
                    {
                        MatchCount = i;
                    }
                    else if (dataLoaded.Contains("teamId") ||
                             dataLoaded.Contains("fixtureId") ||
                             dataLoaded.Contains("matchId"))
                    {
                        break;
                    }
                    else if (dataLoaded.Contains("teamName"))
                    {
                        TeamName = dataLoaded.Split('=')[1];
                        TeamSelectedBool = true;
                        TabIndex = 2;
                    }
                    else
                    {
                        TimeUpdated = dataLoaded;
                    }
                    break;
            }
        }

        /// <summary>
        /// change the tab index of the main tab control
        /// </summary>
        /// <param name="index"></param>
        private void OnIndexReceived(int index)
        {
            TabIndex = index;
        }
    }
}
