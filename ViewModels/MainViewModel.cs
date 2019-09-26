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
        #region

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
            if(dataLoaded == "loaded")
            {
                LoadingData = false;
            }
            else if( dataLoaded == "unloaded")
            {
                LoadingData = true;
            }
            else if(dataLoaded == "selected")
            {
                LeagueSelectedBool = true;
            }
            else if(dataLoaded == "unselected")
            {
                LeagueSelectedBool = false;
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
