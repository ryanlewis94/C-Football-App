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

        public MainViewModel()
        {
            LoadTabIndex();
        }

        private void LoadTabIndex()
        {
            Messenger.Default.Register<int>(TabIndex, OnIndexReceived);
            Messenger.Default.Register<bool>(LeagueSelectedBool, OnStandingsBoolReceived);
        }

        private void OnStandingsBoolReceived(bool leagueBool)
        {
            LeagueSelectedBool = leagueBool;
        }

        private void OnIndexReceived(int index)
        {
            TabIndex = index;
        }
    }
}
