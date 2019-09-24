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
        private int _tabIndex;

        public int TabIndex
        {
            get { return _tabIndex; }
            set { SetProperty(ref _tabIndex, value); }
        }

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

        private bool _clearButton = true;

        public bool ClearButton
        {
            get { return _clearButton; }
            set { SetProperty(ref _clearButton, value); }
        }


        public MainViewModel()
        {
            LoadTabIndex();
        }

        private void LoadTabIndex()
        {
            Messenger.Default.Register<int>(TabIndex, OnIndexReceived);
            Messenger.Default.Register<string>(this, OnDataReceived);
        }

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
            //else if(dataLoaded == "cleared")
            //{
            //    ClearButton = false;
            //}
            //else if(dataLoaded == "uncleared")
            //{
            //    ClearButton = true;
            //}
        }

        private void OnIndexReceived(int index)
        {
            TabIndex = index;
        }
    }
}
