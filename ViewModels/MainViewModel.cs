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

        private bool _mainView = true;

        public bool MainView
        {
            get { return _mainView; }
            set { SetProperty(ref _mainView, value); }
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
                //Show or hide the league standings tab
                case "selected":
                    LeagueSelectedBool = true;
                    break;
                case "unselected":
                    LeagueSelectedBool = false;
                    break;
                //Show or hide the match event window
                case "matchOpened":
                    MatchData = true;
                    LoadingData = false;
                    MainView = false;
                    break;
                case "matchClosed":
                    MatchData = false;
                    MainView = true;
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
