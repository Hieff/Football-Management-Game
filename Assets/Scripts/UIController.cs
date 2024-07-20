using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public UIDocument UI;
    public gameManager gm;
    VisualElement root, leagueTable, freeAgentView, skillView, stadiumView, teamInfoView, clubInfoView, weekRecapView;
    Button freeAgentButton, leagueTableButton, myTeamButton, myClubButton, upgradeStadiumButton, skillUpgradeButton, closeButton, continueButton, quitButton;
    Label gameWeekLabel, moneyLabel, stadiumLabel;
    int skillLoads, freeLoads, stadiumLoads;

    // Start is called before the first frame update
    void Start()
    {
      InitialiseComponents();
  //    gm.setLoadType("loadGame");
      gm.initialiseGM();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitialiseComponents()
    {
      // Find other game objects
      gm = GameObject.Find("gameManager").GetComponent<gameManager>();
      UI = GetComponent<UIDocument>();
      root = UI.rootVisualElement;

      // Assign UI elements to variables
      freeAgentButton = root.Q<Button>("freeAgentsButton");
      leagueTableButton = root.Q<Button>("leagueTableButton");
      myTeamButton = root.Q<Button>("myTeamButton");
      myClubButton = root.Q<Button>("myClubButton");
      upgradeStadiumButton = root.Q<Button>("stadiumUpgradeButton");
      skillUpgradeButton = root.Q<Button>("skillButton");
      closeButton = root.Q<Button>("closeButton");
      continueButton = root.Q<Button>("continueButton");
      quitButton = root.Q<Button>("quitToMainMenuButton");

      leagueTable = root.Q<VisualElement>("tableView");
      freeAgentView = root.Q<VisualElement>("freeAgentView");
      skillView = root.Q<VisualElement>("skillUpgradesView");
      stadiumView = root.Q<VisualElement>("stadiumUpgradeView");
      teamInfoView = root.Q<VisualElement>("teamInfoDisplay");
      clubInfoView = root.Q<VisualElement>("clubInfoView");
      weekRecapView = root.Q<VisualElement>("weekRecap");

      gameWeekLabel = root.Q<Label>("gameWeekLabel");
      moneyLabel = root.Q<Label>("moneyLabel");
      stadiumLabel = root.Q<Label>("thirdLabel");

      freeAgentButton.text = "Free Agents";


      // Add listeners to the main screen buttons
      leagueTableButton.clicked += () => gm.showLeagueTable();
      freeAgentButton.clicked += () => gm.showFreeAgentView();
      closeButton.clicked += () => hideButton();
      upgradeStadiumButton.clicked += () => gm.showStadiumView();
      skillUpgradeButton.clicked += () => gm.showSkillView();
      continueButton.clicked += () => gm.continueButton();
      myTeamButton.clicked += () => gm.viewMyTeam();
      myClubButton.clicked += () => gm.viewMyClub();
      quitButton.clicked += () => quit();


      skillLoads = 0;
      freeLoads = 0;
      stadiumLoads = 0;
    }

    // Return to main menu
    public void quit(){
      SceneManager.LoadScene("introScene");
    }

    // Show pop up for the player weekly information
    public void loadWeekRecap(int money, string result){
      hideButton();
      closeButton.style.display = DisplayStyle.Flex;
      weekRecapView.style.display = DisplayStyle.Flex;
      weekRecapView.Q<Label>("weekResult").text = result;
      weekRecapView.Q<Label>("weekMoney").text = "This week, you earnt: £" + money.ToString();
    }

    // Show promotion pop up
    public void promotion(){
      hideButton();
      closeButton.style.display = DisplayStyle.Flex;
      weekRecapView.style.display = DisplayStyle.Flex;
      weekRecapView.Q<Label>("weekResult").text = "Congratulations you got Promoted!";
      weekRecapView.Q<Label>("weekMoney").text = "After every game you will earn more money in this league!";
    }

    public void setLeagueTable(teamScript[] leagueOrder){
      hideButton();
      leagueTable.style.display = DisplayStyle.Flex;
      closeButton.style.display = DisplayStyle.Flex;
      // Fill out the table with correct information
      for(int i = 1; i < 11; i++){
        root.Query<Label>("teamNameColumn").AtIndex(i).text = leagueOrder[i-1].getTeamName();
        root.Query<Label>("winColumn").AtIndex(i).text = leagueOrder[i-1].getSeasonWins().ToString();
        root.Query<Label>("drawColumn").AtIndex(i).text = leagueOrder[i-1].getSeasonDraws().ToString();
        root.Query<Label>("loseColumn").AtIndex(i).text = leagueOrder[i-1].getSeasonLosses().ToString();
        root.Query<Label>("goalDifferenceColumn").AtIndex(i).text = leagueOrder[i-1].getSeasonGoalDifference().ToString();
        root.Query<Label>("pointsColumn").AtIndex(i).text = leagueOrder[i-1].getSeasonPoints().ToString();

      }

    }

    public void setFreeAgents(List<Dictionary<string,int>> freeAgents, playerScript playerTeam){
      hideButton();
      freeAgentView.style.display = DisplayStyle.Flex;
      closeButton.style.display = DisplayStyle.Flex;
      string[] positions = new string[]{"Forward", "Defender", "GoalKeeper"};
      root.Q<Label>("freeAgentPosition").text = "Player Position";
      root.Q<Label>("freeAgentRating").text = "Player Rating";
      root.Q<Label>("freeAgentCost").text = "Player Cost";

      for(int i = freeAgents.Count; i < 8; i++){
        root.Query<VisualElement>("freeAgentRow").AtIndex(i).style.display = DisplayStyle.None;
      }

      for(int i = 1; i < (freeAgents.Count + 1); i++){
        // Fill out the table with the corresponding information
        Dictionary<string,int> player = freeAgents[i-1];
        int rowNumb = i;
        int pos = player["position"];
        bool expensive = false;
        bool worse = false;

        root.Query<Label>("freeAgentPosition").AtIndex(i).text = positions[pos];
        root.Query<Label>("freeAgentRating").AtIndex(i).text =  player["rating"].ToString();
        root.Query<Label>("freeAgentCost").AtIndex(i).text = player["price"].ToString();

        root.Query<Button>("purchasePlayer").AtIndex(i).style.display = DisplayStyle.Flex;
        root.Query<Label>("freeAgentPosition").AtIndex(i).style.color = Color.black;
        root.Query<Label>("freeAgentRating").AtIndex(i).style.color = Color.black;
        root.Query<Label>("freeAgentCost").AtIndex(i).style.color = Color.black;
        root.Query<VisualElement>("freeAgentRow").AtIndex(i).style.display = DisplayStyle.Flex;
        if(freeLoads == 0){
          int playerId = i-1;
          root.Query<Button>("purchasePlayer").AtIndex(i).clicked += () =>gm.purchasePlayer(playerId);
        }

        // Check if purchaseable. Player has enough money or is an improvement for their team.
        if(playerTeam.getMoney() < player["price"]){
          expensive = true;
        }
        if(pos == 0){
          if(playerTeam.getPlayerTeam().getAttackValue() >= player["rating"]){
            worse = true;
          }
        } else if (pos == 1){
          if(playerTeam.getPlayerTeam().getDefenceValue() >= player["rating"]){
            worse = true;
          }
        } else {
          if(playerTeam.getPlayerTeam().getKeeperValue() >= player["rating"]){
            worse = true;
          }
        }

        // If too expensive/worse than player's current team. Remove purchase button and turn text red.
        if(expensive && !worse){
          root.Query<Button>("purchasePlayer").AtIndex(i).style.display = DisplayStyle.None;
          root.Query<Label>("freeAgentPosition").AtIndex(i).style.color = Color.red;
          root.Query<Label>("freeAgentRating").AtIndex(i).style.color = Color.red;
          root.Query<Label>("freeAgentCost").AtIndex(i).style.color = Color.red;
       } else if (worse){
         root.Query<Button>("purchasePlayer").AtIndex(i).style.display = DisplayStyle.None;
         root.Query<Label>("freeAgentPosition").AtIndex(i).style.color = new Color(0.1574f, 0.3113f, 0.0631f, 1);
         root.Query<Label>("freeAgentRating").AtIndex(i).style.color = new Color(0.1574f, 0.3113f, 0.0631f, 1);
         root.Query<Label>("freeAgentCost").AtIndex(i).style.color = new Color(0.1574f, 0.3113f, 0.0631f, 1);
       }
      }

      freeLoads = 1;
    }


    // Show my stadium screen
    public void setStadiumInformation(List<Dictionary<string,int>> stadiums, playerScript playerTeam){
      hideButton();
      stadiumView.style.display = DisplayStyle.Flex;
      closeButton.style.display = DisplayStyle.Flex;

      root.Q<Label>("stadiumLevel").text = "Stadium Level";
      root.Q<Label>("stadiumCapacity").text = "Stadium Capacity";
      root.Q<Label>("stadiumCost").text = "Stadium Cost";

      for(int i =1; i < (stadiums.Count + 1); i++){
        Dictionary<string,int> stadium = stadiums[i-1];


        root.Query<Label>("stadiumLevel").AtIndex(i).text = i.ToString();
        root.Query<Label>("stadiumCapacity").AtIndex(i).text =  stadium["capacity"].ToString();
        root.Query<Label>("stadiumCost").AtIndex(i).text = stadium["price"].ToString();

        root.Query<Button>("purchaseLevel").AtIndex(i).style.display = DisplayStyle.Flex;
        root.Query<Label>("stadiumLevel").AtIndex(i).style.color = Color.black;
        root.Query<Label>("stadiumCapacity").AtIndex(i).style.color = Color.black;
        root.Query<Label>("stadiumCost").AtIndex(i).style.color = Color.black;
        root.Query<VisualElement>("freeAgentRow").AtIndex(i).style.display = DisplayStyle.Flex;
        // If first load assign listener to button
        if(stadiumLoads == 0){
          root.Query<Button>("purchaseLevel").AtIndex(i).clicked += () =>gm.purchaseStadium(stadium);
        }

        // Unpurchasable turn red, worse than current stadium turn green
        if(stadium["price"] > playerTeam.getMoney() && playerTeam.getStadiumLevel() < (i-1)){
          root.Query<Button>("purchaseLevel").AtIndex(i).style.display = DisplayStyle.None;
          root.Query<Label>("stadiumLevel").AtIndex(i).style.color = Color.red;
          root.Query<Label>("stadiumCapacity").AtIndex(i).style.color = Color.red;
          root.Query<Label>("stadiumCost").AtIndex(i).style.color = Color.red;
        } else if ((i-1) <= playerTeam.getStadiumLevel()){
          root.Query<Button>("purchaseLevel").AtIndex(i).style.display = DisplayStyle.None;
          root.Query<Label>("stadiumCost").AtIndex(i).text = "Purchased";
          root.Query<Label>("stadiumLevel").AtIndex(i).style.color = new Color(0.1574f, 0.3113f, 0.0631f, 1);
          root.Query<Label>("stadiumCapacity").AtIndex(i).style.color = new Color(0.1574f, 0.3113f, 0.0631f, 1);
          root.Query<Label>("stadiumCost").AtIndex(i).style.color = new Color(0.1574f, 0.3113f, 0.0631f, 1);
        }

      }
      stadiumLoads = 1;
    }

      // Show skill tree view
      public void setSkillView(List<Dictionary<string,int>> upgrades, playerScript playerTeam){
        hideButton();
        skillView.style.display = DisplayStyle.Flex;
        closeButton.style.display = DisplayStyle.Flex;
        string[] descriptions = gm.getSkillNames();
        string[] upgradeDescriptions = gm.getSkillDescriptions();


        for(int i=1; i < (upgrades.Count + 1); i++){
          Dictionary<string,int> upgrade = upgrades[i-1];
          Debug.Log(upgrade["upgradeLevels"] + " " + upgrade["userLevel"]);
          int cost = upgrade["price"] * (upgrade["userLevel"] + 1);
          root.Query<Label>("skillDescription").AtIndex(i).text = descriptions[upgrade["Name"]];
          root.Query<Label>("upgradeLevel").AtIndex(i).text = (upgrade["upgradeLevels"] - playerTeam.getSkillLevel(i-1)).ToString();
          root.Query<Label>("upgradeCost").AtIndex(i).text = cost.ToString();
          root.Query<Label>("upgradeDetails").AtIndex(i).text = upgradeDescriptions[upgrade["Name"]];
          root.Query<Button>("purchaseButton").AtIndex(i).style.display = DisplayStyle.Flex;

          root.Query<Label>("skillDescription").AtIndex(i).style.color = Color.black;
          root.Query<Label>("upgradeLevel").AtIndex(i).style.color = Color.black;
          root.Query<Label>("upgradeCost").AtIndex(i).style.color = Color.black;
          root.Query<Label>("upgradeDetails").AtIndex(i).style.color = Color.black;



          if(skillLoads == 0){
            root.Query<Button>("purchaseButton").AtIndex(i).clicked += () => gm.purchaseSkill(upgrade);
          }

          // If skill is unpurchasable, turn red. If purchasable or fully purchased turn green
          if(cost > playerTeam.getMoney() && (upgrade["upgradeLevels"] - playerTeam.getSkillLevel(i-1) != 0)){
            root.Query<Button>("purchaseButton").AtIndex(i).style.display = DisplayStyle.None;
            root.Query<Label>("skillDescription").AtIndex(i).style.color = Color.red;
            root.Query<Label>("upgradeLevel").AtIndex(i).style.color = Color.red;
            root.Query<Label>("upgradeCost").AtIndex(i).style.color = Color.red;
            root.Query<Label>("upgradeDetails").AtIndex(i).style.color = Color.red;
          } else if (upgrade["upgradeLevels"] == playerTeam.getSkillLevel(i-1)){
            root.Query<Label>("skillDescription").AtIndex(i).style.color = new Color(0.1574f, 0.3113f, 0.0631f, 1);
            root.Query<Label>("upgradeLevel").AtIndex(i).style.color = new Color(0.1574f, 0.3113f, 0.0631f, 1);
            root.Query<Label>("upgradeCost").AtIndex(i).style.color = new Color(0.1574f, 0.3113f, 0.0631f, 1);
            root.Query<Label>("upgradeCost").AtIndex(i).text = "Max Level";
            root.Query<Label>("upgradeDetails").AtIndex(i).style.color = new Color(0.1574f, 0.3113f, 0.0631f, 1);
            root.Query<Button>("purchaseButton").AtIndex(i).style.display = DisplayStyle.None;
          }

        }
        skillLoads = 1;
      }

      // Show my team screen
      public void setMyTeamView(playerScript player){
        hideButton();
        teamInfoView.style.display = DisplayStyle.Flex;
        closeButton.style.display = DisplayStyle.Flex;

      //  Debug.Log(player.getPlayerTeam().getBonusDefence());
        teamScript playerTeam = player.getPlayerTeam();
        root.Query<Label>("teamInfoLabel").AtIndex(1).text = "Attack:   " + playerTeam.getAttackValue() + "   (" + playerTeam.getBonusAttack() + ")";
        root.Query<Label>("teamInfoLabel").AtIndex(2).text = "Defence:  " + playerTeam.getDefenceValue() + "   (" + playerTeam.getBonusDefence() + ")";
        root.Query<Label>("teamInfoLabel").AtIndex(3).text = "Keeper:   " + playerTeam.getKeeperValue() + "   (" + playerTeam.getBonusKeeper() + ")";
      }

      // Show my club screen
      public void setMyClubView(playerScript player){
        clubInfoView.style.display = DisplayStyle.Flex;
        closeButton.style.display = DisplayStyle.Flex;

        root.Q<Label>("clubTitleLabel").text = "My Club:";
        root.Q<Label>("stadiumCapacityLabel").text = "Stadium Capacity: " + player.getStadiumCapacity();
        root.Q<Label>("ticketPriceLabel").text = "Ticket Price: £" + player.getTicketPrices();
        root.Q<Label>("teamLevelLabel").text = "Team Level: " + player.getStadiumDescription();
        int teamLeague = player.getLeagueLevel();
        if(teamLeague == 0){
          root.Q<Label>("teamLeagueLabel").text = "League: Second Division";
        } else {
          root.Q<Label>("teamLeagueLabel").text = "League: First Division";
        }

      }

      // Loop through the results array and display the results in corresponding text label
      public void displayAIResults(string[] results){
        for(int i=0; i < 4; i++){
          root.Query<Label>("resultLabel").AtIndex(i).style.display = DisplayStyle.Flex;
          root.Query<Label>("resultLabel").AtIndex(i).text = results[i];

        }
      }









    // Hide the information panels and the button to close them.
    public void hideButton(){
      leagueTable.style.display = DisplayStyle.None;
      closeButton.style.display = DisplayStyle.None;
      freeAgentView.style.display = DisplayStyle.None;
      skillView.style.display = DisplayStyle.None;
      stadiumView.style.display = DisplayStyle.None;
      teamInfoView.style.display = DisplayStyle.None;
      clubInfoView.style.display = DisplayStyle.None;
      weekRecapView.style.display = DisplayStyle.None;
    }


    public void setMoneyLabel(int money){
      moneyLabel.text = "£" + money.ToString();
    }

    public void setGameWeekLabel(string label){
      gameWeekLabel.text = label;
    }

    public void setStadiumLabel(string label){
      stadiumLabel.text = label;
    }
}
