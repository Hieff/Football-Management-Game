using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teamScript : MonoBehaviour
{

     string teamName;
     int attackValue;
     int defenceValue;
     int goalKeeperValue;
     int seasonWins, seasonLosses, seasonDraws, seasonsGoalsScored, seasonsGoalsConceded;
     int attackBonus;
     int defenceBonus;
     int keeperBonus;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Set all values for a team
    public void initialiseTeam(string team, int attack, int defence, int goalie, int wins, int draws, int loss, int gs, int gc, int aBonus, int dBonus, int kBonus){
      setTeamName(team);
      setAttackValue(attack);
      setDefenceValue(defence);
      setGoalKeeperValue(goalie);
      setSeasonWins(wins);
      setSeasonDraws(draws);
      setSeasonLosses(loss);
      setGoalsScored(gs);
      setGoalsConceded(gs);
      setBonusAttack(aBonus);
      setBonusDefence(dBonus);
      setBonusKeeper(kBonus);
    }

    // Setter functions
    public void setTeamName(string team){
      teamName = team;
    }

    public void setAttackValue(int attack){
      attackValue = attack;
    }

    public void setDefenceValue(int defence){
      defenceValue = defence;
    }

    public void setGoalKeeperValue(int goalie){
      goalKeeperValue = goalie;
    }

    public void setSeasonWins(int wins){
      seasonWins = wins;
    }

    public void setSeasonDraws(int draws){
      seasonDraws = draws;
    }

    public void setSeasonLosses(int loss){
      seasonLosses = loss;
    }

    public void setGoalsScored(int goals){
      seasonsGoalsScored = goals;
    }

    public void setGoalsConceded(int conceded){
      seasonsGoalsConceded = conceded;
    }

    public void setBonusAttack(int bonus){
      attackBonus = bonus;
    }

    public void setBonusDefence(int bonus){
      defenceBonus = bonus;
    }

    public void setBonusKeeper(int bonus){
      keeperBonus = bonus;
    }


    // Getter functions
    public string getTeamName(){
      return teamName;
    }

    public int getAttackValue(){
      return attackValue;
    }

    public int getDefenceValue(){
      return defenceValue;
    }

    public int getKeeperValue(){
      return goalKeeperValue;
    }

    public int getSeasonWins(){
      return seasonWins;
    }

    public int getSeasonDraws(){
      return seasonDraws;
    }

    public int getSeasonLosses(){
      return seasonLosses;
    }

    public int getGoalsScored(){
      return seasonsGoalsScored;
    }

    public int getGoalsConceded(){
      return seasonsGoalsConceded;
    }

    // Calculate season points
    public int getSeasonPoints(){
      int points;
      points = getSeasonWins() * 3;
      points+= getSeasonDraws();
      return points;
    }

    public int getSeasonGoalDifference(){
      return (getGoalsScored() - getGoalsConceded());
    }

    public int getBonusAttack(){
      return attackBonus;
    }

    public int getBonusDefence(){
      return defenceBonus;
    }

    public int getBonusKeeper(){
      return keeperBonus;
    }

    public int getFinalAttack(){
      return attackValue + attackBonus;
    }

    public int getFinalDefence(){
      return defenceValue + defenceBonus;
    }

    public int getFinalKeeper(){
      return keeperBonus + goalKeeperValue;
    }
}
