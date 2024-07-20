using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class gameManager : MonoBehaviour
{

    public playerScript player;
    public UIController UI;
    public teamScript team;
    public skillScript skill;
    public saveScript save;
    public GameObject firstStadium;
    public GameObject secondStadium;
    public GameObject thirdStadium;
    public GameObject fourthStadium;
    public GameObject fithStadium;
    public GameObject stadiumStore;
    public List<teamScript> teamList;
    public List<skillScript> skillList;
    public stadiumScript stadium;
    public List<teamScript> firstDivisionTeams;
    public List<teamScript> secondDivisionTeams;
    public (int,int)[,] fixtures;
    public (int,int)[] playerFixtures;
    public List<Dictionary<string,int>> freeAgents;
    public List<Dictionary<string, int>> stadiumUpgrades;
    public List<Dictionary<string, int>> skillUpgrades;
    string[] skillTypes, skillNames, skillDescriptions, latestResults;
    string[] stadiumLevelDescriptors = {"Sunday League", "Semi-Pro Team", "Professional Team", "Top Division Team", "World Class Team"};
    string[] secondDivisionTeamNames = {"My Team","Stevenage", "Eagles", "Comets", "Snakes", "Verona", "Seoul", "Spartans", "Trains", "Coders"};
    string[] firstDivisionTeamNames = {"Liverpool", "Milan", "Barcelona", "Madrid", "Gremio", "Eagles", "Cards", "Dave's", "Lister", "Warriors"};
    string continueMode;
    string loadType = "new";



    int gameWeek;
    int seasonLength;



    // Start is called before the first frame update
    void Start()
    {
      DontDestroyOnLoad(this);
    }

    public void initialiseGM(){
      // Find the game objects refernced in the code
      player = GameObject.Find("Player").GetComponent<playerScript>();
      UI = GameObject.Find("UIDocument").GetComponent<UIController>();
      team = GameObject.Find("teamInfo").GetComponent<teamScript>();
      skill = GameObject.Find("skillInfo").GetComponent<skillScript>();
      stadium = GameObject.Find("playerStadium").GetComponent<stadiumScript>();
      save = GameObject.Find("saveData").GetComponent<saveScript>();

      // Intialise the variables stored up above.
      teamList = new List<teamScript>();
      firstDivisionTeams = new List<teamScript>();
      secondDivisionTeams = new List<teamScript>();
      List<Dictionary<string,int>> freeAgents = new List<Dictionary<string,int>>();
      List<Dictionary<string,int>> stadiumUpgrades = new List<Dictionary<string, int>>();
      List<Dictionary<string,int>> skillUpgrades = new List<Dictionary<string, int>>();
      skillTypes = new string[]{"Attack", "Defence", "Keeper", "Money"};
      skillNames = new string[]{"Boost the abilities of your attack!", "Boost the abilities of your defence!", "Boost the abilities of your goalkeeper!", "Increase the ticket prices for your stadium!"};
      skillDescriptions = new string[]{"Team Attack + 1", "Team Defence + 1", "Team GoalKeeper + 1", "Increase your ticket prices by £5"};
      continueMode = "CPU";
      latestResults = new string[4];

      // Check if loading data
      if(loadType == "loadGame"){
        save.Load();
      } else {
        // Generate a new team, add to second divison
            teamScript newTeam = Instantiate(team);
            newTeam.initialiseTeam("My Team", 8, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0);
            player.setTeam(newTeam);
            teamList.Add(newTeam);
            secondDivisionTeams.Add(newTeam);

            // Create random teams
            for(int i = 0; i < 9; i++){
              int randAtt = Random.Range(1, 11);
              int randDef = Random.Range(1, 11);
              int randGk = Random.Range(1, 11);
              string newName = secondDivisionTeamNames[i+1];
              teamScript randTeam = Instantiate(team);
              randTeam.initialiseTeam(newName, randAtt, randDef, randGk, 0, 0, 0, 0, 0, 0, 0, 0);
              teamList.Add(randTeam);
              secondDivisionTeams.Add(randTeam);
            }

            for(int i = 0; i < 10; i++){
              int randAtt = Random.Range(10, 21);
              int randDef = Random.Range(10, 21);
              int randGk = Random.Range(10, 21);
              string newName = firstDivisionTeamNames[i];
              teamScript randTeam = Instantiate(team);
              randTeam.initialiseTeam(newName, randAtt, randDef, randGk, 0, 0, 0, 0, 0, 0, 0, 0);
              teamList.Add(randTeam);
              firstDivisionTeams.Add(randTeam);
            }
            gameWeek = 0;

      }




      // Populate the variables with values and dispaly all basic information to the player
      displayStadium(player.getStadiumLevel());
      seasonLength = (firstDivisionTeams.Count - 1) * 2;
      string weekIndicator = "Match Day: " + (gameWeek+1).ToString() + "/" + seasonLength.ToString();
      UI.setGameWeekLabel(weekIndicator);
      UI.setStadiumLabel(stadiumLevelDescriptors[player.getStadiumLevel()]);
      UI.setMoneyLabel(player.getMoney());
      generateFixtures();
      generateFreeAgents();
      generateStadiumUpgrades();
      generateSkillUpgrades();




    }


    // Getter functions for private variables
    public string[] getSkillNames(){
      return skillNames;
    }

    public string[] getSkillDescriptions(){
      return skillDescriptions;
    }

    public int getGameWeek(){
      return gameWeek;
    }

    public void setGameWeek(int week){
      gameWeek = week;
    }

    public void setLoadType(string load){
      loadType = load;
    }

    // For visualising the leagues in the console
    public string[] teamToArray(){
      string[] teamArray = new string[teamList.Count];
      for(int i = 0; i < teamList.Count; i++){
        teamArray[i] = teamList[i].getTeamName() + " " + teamList[i].getAttackValue().ToString() + " " + teamList[i].getDefenceValue().ToString() + " " + teamList[i].getKeeperValue().ToString();
      }
      return teamArray;
    }


    // Update the win counter for the team
    public void win(teamScript team){
      int wins = team.getSeasonWins();
      team.setSeasonWins(wins + 1);
    }

    // Update the draw counter for the team
    public void draw(teamScript team){
      int draws = team.getSeasonDraws();
      team.setSeasonDraws(draws + 1);
    }

    // Update the loss counter for the team
    public void loss(teamScript team){
      int losses = team.getSeasonLosses();
      team.setSeasonLosses(losses + 1);
    }

    // Pass in the team and the result and score
    public void teamResult(teamScript team, string outcome, int goals, int conceded){
      if(outcome == "win"){
        win(team);
      } else if (outcome == "draw"){
        draw(team);
      } else {
        loss(team);
      }

      // Update the goals scored and goals conceded for use with goal difference calculations
      int seasonGoals = team.getGoalsScored();
      team.setGoalsScored(seasonGoals + goals);

      int seasonConceded = team.getGoalsConceded();
      team.setGoalsConceded(seasonConceded + conceded);
    }

    // Sort the teams into decending order by points and by goal difference on ties.
    // Then placed into an array for easier looping
    // Sorted using the System.Linq library
    public teamScript[] sortedTeams(List<teamScript> league){
      teamScript[] sortedTeams = new teamScript[10];
      league = league.OrderByDescending(x => x.getSeasonPoints()).ThenByDescending(x => x.getSeasonGoalDifference()).ToList();
      int counter = 0;
      foreach(teamScript team in league){
        sortedTeams[counter] = team;
        counter++;
      }

      return sortedTeams;
    }




    // Create new free agents here. Prices are corresponding to the rating they have
    public void generateFreeAgents(){
      int[] prices = {0,10, 20, 30, 40, 50, 100, 200, 400, 1000, 2000, 5000, 10000, 20000, 50000, 75000, 100000, 200000, 500000, 1000000, 10000000};
      List<Dictionary<string,int>> newFreeAgents = new List<Dictionary<string,int>>();
      for(int i = 0; i < 7; i++){
        Dictionary<string,int> freeAgent = new Dictionary<string,int>{};
        int randomRange = Random.Range(1,21);
        int randomPos = Random.Range(0,3);
        freeAgent.Add("position", randomPos);
        freeAgent.Add("rating", randomRange);
        freeAgent.Add("price", prices[randomRange]);
        newFreeAgents.Add(freeAgent);
      }
      freeAgents = newFreeAgents;
    }

    // Create a youth intake
    public void generateYouthIntake(){
      // Create an empty List and get the player statistics
      List<Dictionary<string,int>> youthIntake = new List<Dictionary<string,int>>();
      int attack = player.getPlayerTeam().getAttackValue();
      int def = player.getPlayerTeam().getDefenceValue();
      int keeper = player.getPlayerTeam().getKeeperValue();

      // Create a new player
      for(int i = 0; i < 4; i++){
        Dictionary<string,int> youthPlayer = new Dictionary<string,int>{};
        int chance = Random.Range(1, 101);
        // Generate a random position for the player
        int randomPos = Random.Range(0,3);
        int rating;
        // If chance is 1 create a really good player.
        if(chance == 1){
          rating = Random.Range(15,21);
        } else if(chance <= 50) {
          if(randomPos == 0){
            rating = Random.Range(attack-2, attack +2);
          } else if (randomPos == 1){
            rating = Random.Range(def-2, def +2);
          } else {
            rating = Random.Range(keeper-2, keeper + 2);
          }
        } else {
          rating = Random.Range(1, 6);
        }

        // Add to the list the new players
        youthPlayer.Add("position", randomPos);
        youthPlayer.Add("rating", rating);
        youthPlayer.Add("price", 0);
        youthIntake.Add(youthPlayer);
        freeAgents = youthIntake;
      }
      // Pop up the information on the screen
      UI.setFreeAgents(youthIntake, player);
    }


    // Display the information panels for the button pressed
    public void showFreeAgentView(){
      UI.setFreeAgents(freeAgents, player);
    }

    // Display the stadium view
    public void showStadiumView(){
      UI.setStadiumInformation(stadiumUpgrades, player);
    }

    // Display the skill view
    public void showSkillView(){
      UI.setSkillView(skillUpgrades, player);
    }

    // Find the league the player is in, pass the league information over to the UIController
    public void showLeagueTable(){
      int playerLevel = player.getLeagueLevel();
      teamScript[] sortedTeams = new teamScript[10];
      if(playerLevel == 0){
        sortedTeams = this.sortedTeams(secondDivisionTeams);
      } else {
        sortedTeams = this.sortedTeams(firstDivisionTeams);
      }

      UI.setLeagueTable(sortedTeams);

    }

    // Display the team view
    public void viewMyTeam(){
      Debug.Log(player.getPlayerTeam().getBonusDefence());
      UI.setMyTeamView(player);
    }

    // Display the club view
    public void viewMyClub(){
      UI.setMyClubView(player);
    }



    // Upgrade the player team to match the new status from the player they bought
    public void purchasePlayer(int playerPos){
      Debug.Log(playerPos);
      Dictionary<string,int> purchasedPlayer = freeAgents[playerPos];
      if(purchasedPlayer["position"] == 0){
        player.getPlayerTeam().setAttackValue(purchasedPlayer["rating"]);
      } else if (purchasedPlayer["position"] == 1){
        player.getPlayerTeam().setDefenceValue(purchasedPlayer["rating"]);
      } else {
        player.getPlayerTeam().setGoalKeeperValue(purchasedPlayer["rating"]);
      }
      player.setMoney(player.getMoney() - purchasedPlayer["price"]);
      UI.setMoneyLabel(player.getMoney());
      showFreeAgentView();

    }

    //Generate a dictionary containing all the stadium levels. Created here so updates just need to be done here
    public void generateStadiumUpgrades(){
      List<Dictionary<string,int>> stadiums = new List<Dictionary<string,int>>{};
      int[] prices = new int[]{4000, 10000, 40000, 75000, 150000};
      int[] capacities = new int[]{50, 200, 1000, 5000, 40000};

      for(int i =0; i < 5; i++){
        Dictionary<string, int> stadiumUpgrade = new Dictionary<string,int>{};
        stadiumUpgrade.Add("level", i);
        stadiumUpgrade.Add("price", prices[i]);
        stadiumUpgrade.Add("capacity", capacities[i]);
        stadiums.Add(stadiumUpgrade);
      }

      player.setStadiumCapacity(capacities[0]);
      stadiumUpgrades = stadiums;
    }

    // Take the funds from the player and upgrade their stadium information to match the upgraded stadium
    public void purchaseStadium(Dictionary<string,int> pstadium){
      player.setMoney(player.getMoney() - pstadium["price"]);
      UI.setMoneyLabel(player.getMoney());
      player.setStadiumLevel(pstadium["level"]);
      player.setStadiumCapacity(pstadium["capacity"]);
      displayStadium(player.getStadiumLevel());
      UI.setStadiumLabel(stadiumLevelDescriptors[player.getStadiumLevel()]);
      showStadiumView();
    }

    // Show the player stadium, and destroy the old stadium
    public void displayStadium(int stadiumLevel){
        GameObject stadium;
      if(stadiumLevel == 0){
        stadium = Instantiate(firstStadium);
      } else if(stadiumLevel == 1){
        stadium = Instantiate(secondStadium);
      } else if(stadiumLevel == 2){
        stadium = Instantiate(thirdStadium);
      } else if (stadiumLevel == 3){
        stadium = Instantiate(fourthStadium);
      } else {
        stadium = Instantiate(fithStadium);
      }

      stadium.transform.position = new Vector3(-0.2f, 0.5f, 0f);
      Destroy(stadiumStore);
      stadiumStore = stadium;


    }

    public void purchaseSkill(Dictionary<string, int> pSkill){
      player.setMoney(player.getMoney() - (pSkill["price"] * (pSkill["userLevel"] + 1)));
      UI.setMoneyLabel(player.getMoney());
      int matchingSkill = 0;
      for(int i = 0; i < skillUpgrades.Count; i++){
        if(pSkill["Name"] == skillUpgrades[i]["Name"]){
          matchingSkill = i;
        }
      }
      // Upgrade the corresponding skill bonus in the player object
      if(pSkill["Name"] == 0){
        player.getPlayerTeam().setBonusAttack(player.getPlayerTeam().getBonusAttack() + pSkill["upgradeAmount"]);
      } else if (pSkill["Name"] == 1){
        player.getPlayerTeam().setBonusDefence(player.getPlayerTeam().getBonusDefence() + pSkill["upgradeAmount"]);
      } else if (pSkill["Name"] == 2){
        player.getPlayerTeam().setBonusKeeper(player.getPlayerTeam().getBonusKeeper() + pSkill["upgradeAmount"]);
      } else if (pSkill["Name"] == 3){
        player.setTicketPrices(player.getTicketPrices() + pSkill["upgradeAmount"]);
      }

      skillUpgrades[matchingSkill]["userLevel"] = pSkill["userLevel"] + 1;
      player.setSkillLevel(matchingSkill, player.getSkillLevel(matchingSkill) + 1);
    //  Debug.Log("Stadium Level " + player.getStadiumLevel());
      //Refresh the information panel
      showSkillView();
    }


    // Create all the skill upgrades the player can purchase. TO add more create a new dictionary and add to the list later
    public void generateSkillUpgrades(){
      List<Dictionary<string,int>> upgrades = new List<Dictionary<string,int>>{};
      Dictionary<string,int> attackBonus = new Dictionary<string,int>(){
        {"Name", 0},
        {"upgradeType", 0},
        {"price", 10000},
        {"upgradeLevels", 3},
        {"upgradeAmount", 1},
        {"userLevel", 0}
      };
      upgrades.Add(attackBonus);
      Dictionary<string,int> defenceBonus = new Dictionary<string,int>(){
        {"Name", 1},
        {"upgradeType", 1},
        {"price", 10000},
        {"upgradeLevels", 3},
        {"upgradeAmount", 1},
        {"userLevel", 0}
      };
      upgrades.Add(defenceBonus);
      Dictionary<string,int> keeperBonus = new Dictionary<string,int>(){
        {"Name", 2},
        {"upgradeType", 2},
        {"price", 10000},
        {"upgradeLevels", 3},
        {"upgradeAmount", 1},
        {"userLevel", 0}
      };
      upgrades.Add(keeperBonus);
      Dictionary<string,int> ticketBonus = new Dictionary<string,int>(){
        {"Name", 3},
        {"upgradeType", 3},
        {"price", 1000},
        {"upgradeLevels", 5},
        {"upgradeAmount", 5},
        {"userLevel", 0}
      };
      upgrades.Add(ticketBonus);
      skillUpgrades = upgrades;

    }





    // Simulate the match
    public string matchSimulation(teamScript homeTeam, teamScript awayTeam){
      int homeScore = 0;
      int awayScore = 0;
      string matchResult;

      //Home advantage usually exists
      float homeTeamAdvantage = 1.25f;
      // Bounces included for randomness which occurs
      float luckyBounces = Random.Range(0f,7f);
      float unluckyBounces = Random.Range(-7f, 0f);
      // More chances based on how much better the team is
      float attackChances = ((homeTeam.getFinalAttack() + luckyBounces + unluckyBounces) * homeTeamAdvantage) / awayTeam.getFinalDefence();
      float remainder = attackChances % 1;
      int whole = Mathf.RoundToInt(attackChances);
      if(whole < 1){whole = 1;}
      float[] numberOfAttacks = new float[whole];
      for(int i = 0; i < whole-1; i++){
        numberOfAttacks[i] = 1;
      }
      numberOfAttacks[whole-1] = remainder;

      // For each chance check if a goal was scored
      for(int i = 0; i < whole; i++){
        float strikersFinish = Random.Range(-7.5f,7.5f);
        // Work out chance to score
        float goalChance = (numberOfAttacks[i] * ((homeTeam.getFinalAttack() + strikersFinish) * homeTeamAdvantage)) / ((awayTeam.getFinalDefence() * 0.125f) + awayTeam.getFinalKeeper());
        // When chance of goal goes over 100, make it high 90s as there is no such thing as a guaranteed goal.
        if(goalChance > 1){
          goalChance = goalChance / (goalChance + 0.1f );
        }

        // Generate a random number. If it is lower than the chance to score then the goal goes in
        goalChance = goalChance * 100;
        int chance = Mathf.RoundToInt(goalChance);
        int randNumber = Random.Range(1, 100);

        if(randNumber < chance){
          homeScore++;
        }
      }

      // Repeated for the away time with values flipped order of inputs
      float luckyBouncesA = Random.Range(0f, 7f);
      float unluckyBouncesA = Random.Range(-7f, 0f);
      float awayChances = (awayTeam.getFinalAttack() + luckyBouncesA + unluckyBouncesA) / (homeTeam.getFinalDefence());
      float awayRemainder = awayChances % 1;
      int awayWhole = Mathf.RoundToInt(awayChances);
      if(awayWhole < 1){awayWhole = 1;}
      float[] awayAttacks = new float[awayWhole];
      for(int i = 0; i < awayWhole-1; i++){
        awayAttacks[i] = 1;
      }
      awayAttacks[awayWhole-1] = remainder;

      for(int i =0; i < awayWhole; i++){
        float strikersFinish = Random.Range(-7.5f,7.5f);
        float awayGoalChance = (awayAttacks[i] * (awayTeam.getFinalAttack() + strikersFinish)) / (((homeTeam.getFinalDefence() * 0.125f) + homeTeam.getFinalKeeper()) * homeTeamAdvantage);
        if(awayGoalChance > 1){
          awayGoalChance = (awayGoalChance + 0.1f) ;
        }
        awayGoalChance = awayGoalChance * 100;
        int chance = Mathf.RoundToInt(awayGoalChance);

        int randNumber = Random.Range(1, 101);

        if(randNumber< chance){
          awayScore++;
        }
      }

      // Update the team's records based on the match outcome
      if(homeScore > awayScore){
        teamResult(homeTeam, "win", homeScore, awayScore);
        teamResult(awayTeam, "loss", awayScore, homeScore);

      } else if (awayScore > homeScore){
        teamResult(awayTeam, "win", awayScore, homeScore);
        teamResult(homeTeam, "loss", homeScore, awayScore);
;
      } else {
        teamResult(homeTeam, "draw", awayScore, homeScore);
        teamResult(awayTeam, "draw", awayScore, homeScore);

      }

      matchResult = homeTeam.getTeamName() + " " + homeScore.ToString() + ":" + awayScore.ToString() + " " + awayTeam.getTeamName();
      return matchResult;





    }


    // Use the Round Robin way to generate fixtures. Two lists rotate around the player team
    public void generateFixtures(){
      int[] teams = {0,1,2,3,4};
      int[] adjTeams = {9,8,7,6,5};
      (int,int)[,] pairs = new (int,int)[9,5];
      (int,int)[] playerPairs = new (int,int)[9];


      for(int x = 0; x < 9; x++){
        (int, int) pFixture = (teams[0], adjTeams[0]);
        // Seperate player fixtures so they are played seperately
        playerPairs[x] = pFixture;
        // Match up pairs
        for(int i=0; i < 5; i++){
            pairs[x,i] = (teams[i], adjTeams[i]);
          //  Debug.Log(pairs[x,i]);
          }

        // Team numbers changing arrays
        int teamHolder = teams[4];
        int adjTeamHolder = adjTeams[0];

        // Rotate the arrays and then add in the team numbers that change arrays
        for(int y = 0; y < 4; y++){
          adjTeams[y] = adjTeams[y+1];
        }
        adjTeams[4] = teamHolder;

        for(int y=4; y > 1; y--){
          teams[y] = teams[y-1];
        }
        teams[1] = adjTeamHolder;
      }

      fixtures = pairs;
      playerFixtures = playerPairs;

    }


    // Simulate all the CPU matches
    public void simulateRound(){
      string firstDivisionResult, secondDivisionResult;
      int playerLevel = player.getLeagueLevel();

      // Home Games vs Away Games
      if(gameWeek < 9){
        for(int i = 1; i < 5; i++){
          secondDivisionResult = matchSimulation(secondDivisionTeams[fixtures[gameWeek,i].Item1], secondDivisionTeams[fixtures[gameWeek,i].Item2]);
          firstDivisionResult = matchSimulation(firstDivisionTeams[fixtures[gameWeek,i].Item1], firstDivisionTeams[fixtures[gameWeek,i].Item2]);
          if(playerLevel == 0){
            latestResults[i-1] = secondDivisionResult;
          } else {
            latestResults[i-1] = firstDivisionResult;
          }
        }
      } else {
        for(int i = 1; i < 5; i++){
          secondDivisionResult = matchSimulation(secondDivisionTeams[fixtures[gameWeek-9,i].Item2], secondDivisionTeams[fixtures[gameWeek-9,i].Item1]);
          firstDivisionResult = matchSimulation(firstDivisionTeams[fixtures[gameWeek-9,i].Item2], firstDivisionTeams[fixtures[gameWeek-9,i].Item1]);
          if(playerLevel == 0){
            latestResults[i-1] = secondDivisionResult;
          } else {
            latestResults[i-1] = firstDivisionResult;
          }
        }

      }

    }


    public void simulateWeek(){
      string playerResult, div1Result, div2Result;
      int playerWins = player.getPlayerTeam().getSeasonWins();
      int playerDraws = player.getPlayerTeam().getSeasonDraws();
      int money = 0;
      // Home Games vs Away Games
      if(gameWeek < 9){
        div2Result = matchSimulation(secondDivisionTeams[fixtures[gameWeek,0].Item1], secondDivisionTeams[fixtures[gameWeek,0].Item2]);
        div1Result = matchSimulation(firstDivisionTeams[fixtures[gameWeek,0].Item1], firstDivisionTeams[fixtures[gameWeek,0].Item2]);
      } else {
        div2Result = matchSimulation(secondDivisionTeams[fixtures[gameWeek-9,0].Item2], secondDivisionTeams[fixtures[gameWeek-9,0].Item1]);
        div1Result = matchSimulation(firstDivisionTeams[fixtures[gameWeek-9,0].Item2], firstDivisionTeams[fixtures[gameWeek-9,0].Item1]);
      }
      //Check player league and then assign the correct outcome string
      if(player.getLeagueLevel() == 0){
        playerResult = div2Result;
      } else {
        playerResult = div1Result;
      }

    // Check which (win,loss,draw) is different to see what the outcome was
      if(playerWins < player.getPlayerTeam().getSeasonWins()){
        money = money + 50;
      } else if (playerDraws < player.getPlayerTeam().getSeasonDraws()){
        money = money + 25;
      } else {
        money = money + 10;
      }
      if(player.getLeagueLevel() == 1){
        money = money * 100;
      }
      //Add money from ticket sales to money from match result
      money = money + (player.getStadiumCapacity() * player.getTicketPrices());
      player.setMoney(player.getMoney() + money);
      //Update labels and show the weekly recap
      UI.loadWeekRecap(money, playerResult);
      gameWeek++;
      UI.setGameWeekLabel("Match Day: " + (gameWeek+1).ToString() + "/" + seasonLength.ToString());
      UI.setMoneyLabel(player.getMoney());
    }

    // Button to control the progress in the game
    public void continueButton(){
      // If CPU simulate CPU matches then show them on the screen
      if(continueMode == "CPU"){
        simulateRound();
        UI.displayAIResults(latestResults);
        // On even weeks generate new free agents
        if((gameWeek + 1) % 2 == 0){
          generateFreeAgents();
          // On week 16 generate youth intake
        } else if(gameWeek == 16){
          generateYouthIntake();
        }

        // If player's turn, display player result
        continueMode = "Player";
      } else if(continueMode == "Player") {
        simulateWeek();
        if(gameWeek == 18){
          continueMode = "SeasonEnd";
        } else {
          continueMode = "CPU";
        }
        // Save data
        save.Save();
        // If at the end of season, work out promotion and relegation and update leagues. Then reset standings
      } else if(continueMode == "SeasonEnd"){
        continueMode = "CPU";
        seasonEnd();
        save.Save();
      }


    }

    // End of season results
    public void seasonEnd(){
      teamScript[] topSortedTeams = sortedTeams(firstDivisionTeams);
      teamScript[] bottomSortedTeams = sortedTeams(secondDivisionTeams);

      teamScript[] relegatedTeams = new teamScript[]{topSortedTeams[8], topSortedTeams[9]};
      teamScript[] promotedTeams = new teamScript[]{bottomSortedTeams[0], bottomSortedTeams[1]};

      gameWeek = 0;
      UI.setGameWeekLabel("Match Day: " + (gameWeek+1).ToString() + "/" + seasonLength.ToString());
      bool relegation = false;
      bool promotion = false;
      int playerTeam = 0;
      int notPlayer = 1;

      // Check if player was promoted from second division or relegated from first divison
      if(player.getLeagueLevel() == 1){
        if(relegatedTeams[0].getTeamName() == player.getPlayerTeam().getTeamName()){
          relegation = true;
          playerTeam = 0;
          notPlayer = 1;
        } else if(relegatedTeams[1].getTeamName() == player.getPlayerTeam().getTeamName()){
          relegation = true;
          playerTeam = 1;
          notPlayer = 0;
        }
      } else {
        if(promotedTeams[0].getTeamName() == player.getPlayerTeam().getTeamName()){
          promotion = true;
          playerTeam = 0;
          notPlayer = 1;
        } else if(promotedTeams[1].getTeamName() == player.getPlayerTeam().getTeamName()){
          promotion = true;
          playerTeam = 1;
          notPlayer = 0;
        }

      }


        // Make it so the player team is at either end of the division lists
        bottomSortedTeams[0] = relegatedTeams[playerTeam];
        bottomSortedTeams[1] = relegatedTeams[notPlayer];

        topSortedTeams[9] = promotedTeams[playerTeam];
        topSortedTeams[8] = promotedTeams[notPlayer];

        List<teamScript> newSecondDivision = new List<teamScript>();
        //Reset the season statistics for teams
        if(relegation){
          for(int i = 0; i < 10; i++){
            bottomSortedTeams[i].setSeasonWins(0);
            bottomSortedTeams[i].setSeasonDraws(0);
            bottomSortedTeams[i].setSeasonLosses(0);
            bottomSortedTeams[i].setGoalsScored(0);
            bottomSortedTeams[i].setGoalsConceded(0);
            newSecondDivision.Add(bottomSortedTeams[i]);
          }
          secondDivisionTeams = newSecondDivision;
          player.setLeagueLevel(0);
        } else {
          // Check that the player's team is in spot 0.
          for(int i = 0; i < 10; i++){
            if(bottomSortedTeams[i].getTeamName() == player.getPlayerTeam().getTeamName()){
              bottomSortedTeams[i] = bottomSortedTeams[0];
              bottomSortedTeams[0] = player.getPlayerTeam();
            }
          }
          for(int i = 0; i < 10; i++){
            bottomSortedTeams[i].setSeasonWins(0);
            bottomSortedTeams[i].setSeasonDraws(0);
            bottomSortedTeams[i].setSeasonLosses(0);
            bottomSortedTeams[i].setGoalsScored(0);
            bottomSortedTeams[i].setGoalsConceded(0);
            newSecondDivision.Add(bottomSortedTeams[i]);
          }
          secondDivisionTeams = newSecondDivision;
        }

        //Debug.Log(promotion);
        List<teamScript> newFirstDivision = new List<teamScript>();
        // Place the player into slot 9. Loop from 9 to 0 to reverse and place player in slot 0.
        if(promotion){
          for(int i = 9; i >= 0; i--){
            topSortedTeams[i].setSeasonWins(0);
            topSortedTeams[i].setSeasonDraws(0);
            topSortedTeams[i].setSeasonLosses(0);
            topSortedTeams[i].setGoalsScored(0);
            topSortedTeams[i].setGoalsConceded(0);
            newFirstDivision.Add(topSortedTeams[i]);
          }
          player.setLeagueLevel(1);
          firstDivisionTeams = newFirstDivision;
          UI.promotion();
        } else {
          for(int i = 0; i < 10; i++){
            if(topSortedTeams[i].getTeamName() == player.getPlayerTeam().getTeamName()){
              topSortedTeams[i] = topSortedTeams[9];
              topSortedTeams[9] = player.getPlayerTeam();
            }
          }
          for(int i = 9; i >= 0; i--){
            topSortedTeams[i].setSeasonWins(0);
            topSortedTeams[i].setSeasonDraws(0);
            topSortedTeams[i].setSeasonLosses(0);
            topSortedTeams[i].setGoalsScored(0);
            topSortedTeams[i].setGoalsConceded(0);
            newFirstDivision.Add(topSortedTeams[i]);
          }
          firstDivisionTeams = newFirstDivision;
        }

        gameWeek = 0;
        UI.setGameWeekLabel("Match Day: " + (gameWeek+1).ToString() + "/" + seasonLength.ToString());
    }



    // Functions to turn team class into JSON

    public List<string> getTeamNames(){
      List<string> names = new List<string>();
      for(int i = 0; i < 10; i++){
        names.Add(firstDivisionTeams[i].getTeamName());
      }
      for(int i =0; i < 10; i++){
        names.Add(secondDivisionTeams[i].getTeamName());
      }
      return names;
    }

    public List<int> getAttackValues(){
      List<int> values = new List<int>();
      for(int i = 0; i < 10; i++){
        values.Add(firstDivisionTeams[i].getAttackValue());
      }
      for(int i =0; i < 10; i++){
        values.Add(secondDivisionTeams[i].getAttackValue());
      }
      return values;
    }

    public List<int> getDefenceValues(){
      List<int> values = new List<int>();
      for(int i = 0; i < 10; i++){
        values.Add(firstDivisionTeams[i].getDefenceValue());
      }
      for(int i =0; i < 10; i++){
        values.Add(secondDivisionTeams[i].getDefenceValue());
      }
      return values;
    }

    public List<int> getGoalKeeperValues(){
      List<int> values = new List<int>();
      for(int i = 0; i < 10; i++){
        values.Add(firstDivisionTeams[i].getKeeperValue());
      }
      for(int i =0; i < 10; i++){
        values.Add(secondDivisionTeams[i].getKeeperValue());
      }
      return values;
    }

    public List<int> getSeasonWins(){
      List<int> values = new List<int>();
      for(int i = 0; i < 10; i++){
        values.Add(firstDivisionTeams[i].getSeasonWins());
      }
      for(int i =0; i < 10; i++){
        values.Add(secondDivisionTeams[i].getSeasonWins());
      }
      return values;
    }

    public List<int> getSeasonDraws(){
      List<int> values = new List<int>();
      for(int i = 0; i < 10; i++){
        values.Add(firstDivisionTeams[i].getSeasonDraws());
      }
      for(int i =0; i < 10; i++){
        values.Add(secondDivisionTeams[i].getSeasonDraws());
      }
      return values;
    }

    public List<int> getSeasonLosses(){
      List<int> values = new List<int>();
      for(int i = 0; i < 10; i++){
        values.Add(firstDivisionTeams[i].getSeasonLosses());
      }
      for(int i =0; i < 10; i++){
        values.Add(secondDivisionTeams[i].getSeasonLosses());
      }
      return values;
    }

    public List<int> getSeasonGoalsScored(){
      List<int> values = new List<int>();
      for(int i = 0; i < 10; i++){
        values.Add(firstDivisionTeams[i].getGoalsScored());
      }
      for(int i =0; i < 10; i++){
        values.Add(secondDivisionTeams[i].getGoalsScored());
      }
      return values;
    }

    public List<int> getSeasonGoalsConceded(){
      List<int> values = new List<int>();
      for(int i = 0; i < 10; i++){
        values.Add(firstDivisionTeams[i].getGoalsConceded());
      }
      for(int i =0; i < 10; i++){
        values.Add(secondDivisionTeams[i].getGoalsConceded());
      }
      return values;
    }

    public List<int> getAttackBonus(){
      List<int> values = new List<int>();
      for(int i = 0; i < 10; i++){
        values.Add(firstDivisionTeams[i].getBonusAttack());
      }
      for(int i =0; i < 10; i++){
        values.Add(secondDivisionTeams[i].getBonusAttack());
      }
      return values;
    }

    public List<int> getDefenceBonus(){
      List<int> values = new List<int>();
      for(int i = 0; i < 10; i++){
        values.Add(firstDivisionTeams[i].getBonusDefence());
      }
      for(int i =0; i < 10; i++){
        values.Add(secondDivisionTeams[i].getBonusDefence());
      }
      return values;
    }

    public List<int> getKeeperBonus(){
      List<int> values = new List<int>();
      for(int i = 0; i < 10; i++){
        values.Add(firstDivisionTeams[i].getBonusKeeper());
      }
      for(int i =0; i < 10; i++){
        values.Add(secondDivisionTeams[i].getBonusKeeper());
      }
      return values;
    }




















    }
