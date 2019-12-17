using FootballApp.Utility;
using System;
using System.Windows.Threading;

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

        private int _requestCount = 0;

        public int RequestCount
        {
            get { return _requestCount; }
            set { SetProperty(ref _requestCount, value); }
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
                case "1":
                    RequestCount++;
                    if (RequestCount > 30)
                    {
                        Messenger.Default.Send("TooManyRequests");
                    }
                    break;
                case "0":
                    RequestCount = 0;
                    break;
                default:
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
