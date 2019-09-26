using FootballApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballApp
{
    public class ViewModelLocator
    {
        private static MainViewModel mainViewModel = new MainViewModel();
        private static CountryViewModel countryViewModel = new CountryViewModel();
        private static LeagueViewModel leagueViewModel = new LeagueViewModel();
        private static MatchViewModel matchViewModel = new MatchViewModel();
        private static StandingsViewModel standingsViewModel = new StandingsViewModel();
        private static EventsViewModel eventsViewModel = new EventsViewModel();

        public static MainViewModel MainViewModel
        {
            get
            {
                return mainViewModel;
            }
        }

        public static CountryViewModel CountryViewModel
        {
            get
            {
                return countryViewModel;
            }
        }

        public static LeagueViewModel LeagueViewModel
        {
            get
            {
                return leagueViewModel;
            }
        }

        public static MatchViewModel MatchViewModel
        {
            get
            {
                return matchViewModel;
            }
        }

        public static StandingsViewModel StandingsViewModel
        {
            get
            {
                return standingsViewModel;
            }
        }

        public static EventsViewModel EventsViewModel
        {
            get
            {
                return eventsViewModel;
            }
        }
    }
}
