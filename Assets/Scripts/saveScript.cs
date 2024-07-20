using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Create a class to convert into JSON
[System.Serializable]
public class savedTeams
{
  public List<string> teamNames;
  public List<int> attackValues;
  public List<int> defenceValues;
  public List<int> goalKeeperValues;
  public List<int> seasonWins;
  public List<int> seasonLosses;
  public List<int> seasonDraws;
  public List<int> seasonsGoalsScored;
  public List<int> seasonsGoalsConceded;
  public List<int> attackBonus;
  public List<int> defenceBonus;
  public List<int> keeperBonus;
}


public class saveScript : MonoBehaviour
{

    public teamScript team;
    public gameManager gm;
    public playerScript playerData;
    // Start is called before the first frame update
    void Start()
    {
      // Assign references to other objects
      team = GameObject.Find("teamInfo").GetComponent<teamScript>();
      gm = GameObject.Find("gameManager").GetComponent<gameManager>();


    }

    void Awake(){
      Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Save(){
      // Update data
      savedTeams savedTeams = new savedTeams();
      savedTeams.teamNames = gm.getTeamNames();
      savedTeams.attackValues = gm.getAttackValues();
      savedTeams.defenceValues = gm.getDefenceValues();
      savedTeams.goalKeeperValues = gm.getGoalKeeperValues();
      savedTeams.seasonWins = gm.getSeasonWins();
      savedTeams.seasonDraws = gm.getSeasonDraws();
      savedTeams.seasonLosses = gm.getSeasonLosses();
      savedTeams.seasonsGoalsScored = gm.getSeasonGoalsScored();
      savedTeams.seasonsGoalsConceded = gm.getSeasonGoalsConceded();
      savedTeams.attackBonus = gm.getAttackBonus();
      savedTeams.defenceBonus = gm.getDefenceBonus();
      savedTeams.keeperBonus = gm.getKeeperBonus();

      // Turn into JSON and save the data in PlayerPrefs
      string savedTeamsJSON = JsonUtility.ToJson(savedTeams);
      PlayerPrefs.SetString("SavedTeams", savedTeamsJSON);
      PlayerPrefs.SetInt("GameWeek", gm.getGameWeek());
      PlayerPrefs.SetInt("Money", gm.player.getMoney());
      PlayerPrefs.SetInt("TicketPrices", gm.player.getTicketPrices());
      PlayerPrefs.SetInt("StadiumLevel", gm.player.getStadiumLevel());
      PlayerPrefs.SetInt("League", gm.player.getLeagueLevel());
      PlayerPrefs.SetInt("StadiumCapacity", gm.player.getStadiumCapacity());
      PlayerPrefs.SetInt("Skill0", gm.player.getSkillLevel(0));
      PlayerPrefs.SetInt("Skill1", gm.player.getSkillLevel(1));
      PlayerPrefs.SetInt("Skill2", gm.player.getSkillLevel(2));
      PlayerPrefs.SetInt("Skill3", gm.player.getSkillLevel(3));
      PlayerPrefs.Save();
    }

    // Load data
    public void Load(){
      // Get saved data
      string savedTeamData = PlayerPrefs.GetString("SavedTeams");
      savedTeams teamData = JsonUtility.FromJson<savedTeams>(savedTeamData);
      gm.player = GameObject.Find("Player").GetComponent<playerScript>();

      // Set player values
      gm.player.setMoney(PlayerPrefs.GetInt("Money"));
      gm.player.setTicketPrices(PlayerPrefs.GetInt("TicketPrices"));
      gm.player.setStadiumLevel(PlayerPrefs.GetInt("StadiumLevel"));
      gm.player.setLeagueLevel(PlayerPrefs.GetInt("League"));
      gm.player.setStadiumCapacity(PlayerPrefs.GetInt("StadiumCapacity"));
      gm.player.setSkillLevel(0, PlayerPrefs.GetInt("Skill0"));
      gm.player.setSkillLevel(1, PlayerPrefs.GetInt("Skill1"));
      gm.player.setSkillLevel(2, PlayerPrefs.GetInt("Skill2"));
      gm.player.setSkillLevel(3, PlayerPrefs.GetInt("Skill3"));
      gm.setGameWeek(PlayerPrefs.GetInt("GameWeek"));

      List<teamScript> firstDivison = new List<teamScript>();
      List<teamScript> secondDivision = new List<teamScript>();

      //Load in team values
      for(int i = 0; i < 10; i++){
        teamScript firstDivTeam = Instantiate(team);
        teamScript secondDivTeam = Instantiate(team);

        firstDivTeam.initialiseTeam(
        teamData.teamNames[i],
        teamData.attackValues[i],
        teamData.defenceValues[i],
        teamData.goalKeeperValues[i],
        teamData.seasonWins[i],
        teamData.seasonDraws[i],
        teamData.seasonLosses[i],
        teamData.seasonsGoalsScored[i],
        teamData.seasonsGoalsConceded[i],
        teamData.attackBonus[i],
        teamData.defenceBonus[i],
        teamData.keeperBonus[i]
        );

        secondDivTeam.initialiseTeam(
        teamData.teamNames[i+10],
        teamData.attackValues[i+10],
        teamData.defenceValues[i+10],
        teamData.goalKeeperValues[i+10],
        teamData.seasonWins[i+10],
        teamData.seasonDraws[i+10],
        teamData.seasonLosses[i+10],
        teamData.seasonsGoalsScored[i+10],
        teamData.seasonsGoalsConceded[i+10],
        teamData.attackBonus[i+10],
        teamData.defenceBonus[i+10],
        teamData.keeperBonus[i+10]
        );

        firstDivison.Add(firstDivTeam);
        secondDivision.Add(secondDivTeam);
      }

      gm.firstDivisionTeams = firstDivison;
      gm.secondDivisionTeams = secondDivision;
      if(gm.player.getLeagueLevel() == 0){
        gm.player.setTeam(secondDivision[0]);
      } else {
        gm.player.setTeam(firstDivison[0]);
      }

    //  Debug.Log(gm.player.getPlayerTeam().getBonusDefence());
      //Debug.Log(gm.player.getPlayerTeam().getBonusKeeper());
    }
}
