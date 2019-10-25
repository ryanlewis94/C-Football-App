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
    public class StandingsViewModel : ViewModelBase
    {
        private IFootball repository;

        /// <summary>
        /// stores the league standings
        /// </summary>
        private List<Table> _standingsList;

        public List<Table> StandingsList
        {
            get { return _standingsList; }
            set { SetProperty(ref _standingsList, value); }
        }

        private Country _currentCountry;

        public Country CurrentCountry
        {
            get { return _currentCountry; }
            set { SetProperty(ref _currentCountry, value); }
        }

        public StandingsViewModel()
        {
            repository = new Football();
            LoadStandings();
        }

        private void LoadStandings()
        {
            Messenger.Default.Register<Country>(this, OnLeagueReceived);
        }

        private async void OnLeagueReceived(Country country)
        {
            if (!string.IsNullOrEmpty(country.league_id))
            {
                if (CurrentCountry != country)
                {
                    CurrentCountry = country;
                    Messenger.Default.Send("unloaded");
                    StandingsList = await repository.LoadStandings(country.league_id);

                    HighlightCurrentTeams(country);

                    if (StandingsList != null)
                    {
                        Messenger.Default.Send("leagueAvailable");
                    }
                    else
                    {
                        Messenger.Default.Send("leagueUnavailable");
                    }
                }
                
            }
            
            //Hides the loading overlay
            Messenger.Default.Send("loaded");
        }

        /// <summary>
        /// highlights the teams playing the match or fixture selected on the league table
        /// </summary>
        /// <param name="country"></param>
        private void HighlightCurrentTeams(Country country)
        {
            if (StandingsList != null)
            {
                foreach (Table table in StandingsList)
                {
                    if (country.fixtureList != null)
                    {
                        if (table.name == country.fixtureList.home_name || table.name == country.fixtureList.away_name)
                        {
                            table.State = true;
                        }
                    }
                    else if (country.matchList != null)
                    {
                        if (table.name == country.matchList.home_name || table.name == country.matchList.away_name)
                        {
                            table.State = true;
                        }
                    }
                }
            }
        }
    }
}
