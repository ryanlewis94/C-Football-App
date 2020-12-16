using FootballApp.Api;
using FootballApp.Classes;
using FootballApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FootballApp.Commands;

namespace FootballApp.ViewModels
{
    public class StandingsViewModel : ViewModelBase
    {
        private IFootball repository;
        public ICommand TeamClickedCommand { get; set; }

        /// <summary>
        /// stores the league standings
        /// </summary>
        private List<Table> _standingsList;

        public List<Table> StandingsList
        {
            get { return _standingsList; }
            set { SetProperty(ref _standingsList, value); }
        }

        /// <summary>
        /// stores the top goalscorer standings
        /// </summary>
        private List<Goalscorer> _goalscorerList;

        public List<Goalscorer> GoalscorerList
        {
            get { return _goalscorerList; }
            set { SetProperty(ref _goalscorerList, value); }
        }

        private Table _selectedTeam;

        public Table SelectedTeam
        {
            get { return _selectedTeam; }
            set { SetProperty(ref _selectedTeam, value); }
        }

        /// <summary>
        /// bools for visibility of the leage name and logo
        /// </summary>
        private bool _leagueLogo;
        public bool LeagueLogo
        {
            get { return _leagueLogo; }
            set { SetProperty(ref _leagueLogo, value); }
        }

        private bool _leagueName;
        public bool LeagueName
        {
            get { return _leagueName; }
            set { SetProperty(ref _leagueName, value); }
        }

        public StandingsViewModel()
        {
            repository = new Football();
            TeamClickedCommand = new RelayCommand(TeamClicked);
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
                    //if there was previously a country selected keep it in memory to compare against the newly selected country
                    var countryBefore = (CurrentCountry != null) ? CurrentCountry : null;
                    if (!string.IsNullOrEmpty(country.competition_id))
                    {
                        CurrentCountry = country;

                        if (CurrentCountry.logo.Contains("https"))
                        {
                            LeagueLogo = true;
                            LeagueName = false;
                        }
                        else
                        {
                            LeagueLogo = false;
                            LeagueName = true;
                        }

                        //if first time selecting a country load the league standings
                        if (countryBefore == null)
                        {
                            SortStandings(country);
                        }
                        else
                        {
                            //if the new country is different from the last load the league standings
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
                errorHandler.CheckErrorMessage(ex);
            }
        }

        /// <summary>
        /// Load the league standings
        /// </summary>
        /// <param name="country"></param>
        private async void SortStandings(Country country)
        {
            try
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

                        //if teams are all from the same league 
                        if (leagueCheck == team.league_id)
                        {
                            groupedLeague = false;
                        }
                        //if there are teams from different leagues e.g. group stage of a cup
                        else
                        {
                            groupedLeague = true;
                            break;
                        }
                        //load the team logos
                        foreach (Logo logo in repository.LoadLogos())
                        {
                            if (team.name.ToLower() == logo.team_name.ToLower())
                            {
                                team.logo = logo.logo;
                                break;
                            }
                            if (team.name.Contains(logo.team_name) ||
                                $"FC {team.name}".Contains(logo.team_name) ||
                                $"{team.name} FC".Contains(logo.team_name))
                            {
                                team.logo = logo.logo;
                            }
                        }
                    }

                    //if standings for a cup group then sort and group them by their group
                    StandingsList = (groupedLeague) ?
                    StandingsList.Where(t => t.league_id == country.league_id).ToList() :
                    StandingsList;

                    HighlightCurrentTeams(country);

                    //display standings tab if league standings are available anf hide if not
                    if (StandingsList.Count != 0)
                    {
                        Messenger.Default.Send("leagueAvailable");
                    }
                    else
                    {
                        Messenger.Default.Send("leagueUnavailable");
                    }
                }

                GoalscorerList = await repository.LoadTopGoalscorers(country.competition_id);
                if (GoalscorerList != null)
                {
                    var i = 0;
                    foreach (Goalscorer goalscorer in GoalscorerList)
                    {
                        i++;
                        goalscorer.rank = i.ToString();
                        foreach (Logo logo in repository.LoadLogos())
                        {
                            if (goalscorer.team.name.ToLower() == logo.team_name.ToLower())
                            {
                                goalscorer.logo = logo.logo;
                                break;
                            }
                            if (goalscorer.team.name.Contains(logo.team_name) ||
                                $"FC {goalscorer.team.name}".Contains(logo.team_name) ||
                                $"{goalscorer.team.name} FC".Contains(logo.team_name))
                            {
                                goalscorer.logo = logo.logo;
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
                        //get rid of any errors in the team name
                        table.name = table.name.Replace("amp;", "");

                        //if fixture is selected
                        if (country.fixtureList != null)
                        {
                            //highlight the team in the standings if found
                            if (table.name == country.fixtureList.home_name || table.name == country.fixtureList.away_name)
                            {
                                table.State = true;
                            }
                        }
                        //if match is selected
                        else if (country.matchList != null)
                        {
                            //highlight the team in the standings if found
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

        private void TeamClicked(object teamId)
        {
            foreach (Table team in StandingsList)
            {
                if (teamId.ToString() == team.team_id)
                {
                    Messenger.Default.Send($"teamName={team.name}");
                    Messenger.Default.Send($"teamId={teamId}");
                    SelectedTeam = null;
                    break;
                }
            }
        }
    }
}
