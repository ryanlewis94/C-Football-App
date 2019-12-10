using FootballApp.Api;
using FootballApp.Classes;
using FootballApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public StandingsViewModel()
        {
            repository = new Football();
            LoadStandings();
        }

        private void LoadStandings()
        {
            Messenger.Default.Register<Country>(this, OnLeagueReceived);
        }

        /// <summary>
        /// When a country is selected check for league table
        /// </summary>
        /// <param name="country"></param>
        private void OnLeagueReceived(Country country)
        {
            try
            {
                if (country != null)
                {
                    var countryBefore = (CurrentCountry != null) ? CurrentCountry : null;
                    if (!string.IsNullOrEmpty(country.competition_id))
                    {
                        CurrentCountry = country;

                        if (countryBefore == null)
                        {
                            SortStandings(country);
                        }
                        else
                        {
                            if (CurrentCountry.matchList != null)
                            {
                                if (CurrentCountry.matchList?.id != countryBefore.matchList?.id)
                                {
                                    SortStandings(country);
                                }
                            }
                            if (CurrentCountry.fixtureList != null)
                            {
                                if (CurrentCountry.fixtureList?.id != countryBefore.fixtureList?.id)
                                {
                                    SortStandings(country);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message != "BadRequest")
                {
                    errorHandler.CheckErrorMessage(ex);
                }
                else
                {
                    Messenger.Default.Send("leagueUnavailable");
                }
            }
        }

        private async void SortStandings(Country country)
        {
            StandingsList = await repository.LoadStandings(country.competition_id);
            if (StandingsList != null)
            {
                string leagueCheck = "";
                bool groupedLeague = false;
                foreach (Table team in StandingsList)
                {
                    if (string.IsNullOrWhiteSpace(leagueCheck))
                    {
                        leagueCheck = team.league_id;
                    }

                    if (leagueCheck == team.league_id)
                    {
                        groupedLeague = false;
                    }
                    else
                    {
                        groupedLeague = true;
                        break;
                    }
                }

                StandingsList = (groupedLeague) ?
                StandingsList.Where(t => t.league_id == country.league_id).ToList() :
                StandingsList;

                HighlightCurrentTeams(country);

                if (StandingsList.Count != 0)
                {
                    Messenger.Default.Send("leagueAvailable");
                }
                else
                {
                    Messenger.Default.Send("leagueUnavailable");
                }
            }
        }

        /// <summary>
        /// highlights the teams playing the match or fixture selected on the league table
        /// </summary>
        /// <param name="country"></param>
        private void HighlightCurrentTeams(Country country)
        {
            try
            {
                if (StandingsList != null)
                {
                    foreach (Table table in StandingsList)
                    {
                        table.name = table.name.Replace("amp;", "");
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
            catch (Exception ex)
            {
                errorHandler.CheckErrorMessage(ex);
            }
        }
    }
}
