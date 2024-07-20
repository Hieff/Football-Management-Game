using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{

     int money, ticketPrices;
     int stadiumLevel, leagueLevel, stadiumCapacity;
     teamScript team;
     string[] stadiumLevelDescriptors = {"Sunday League", "Semi-Pro Team", "Professional Team", "Top Division Team", "World Class Team"};
     int[] skillLevels;


    // Start is called before the first frame update
    void Start()
    {
      money = 0;
      stadiumLevel = 0;
      leagueLevel = 0;
      skillLevels = new int[]{0,0,0,0};
      ticketPrices = 5;

    }

    // Update is called once per frame
    void Update()
    {

    }



    public int getMoney(){
      return money;
    }

    public int getLeagueLevel(){
      return leagueLevel;
    }

    public teamScript getPlayerTeam(){
      return team;
    }

    public int getStadiumCapacity(){
      return stadiumCapacity;
    }

    public int getStadiumLevel(){
      return stadiumLevel;
    }

    public string getStadiumDescription(){
      return stadiumLevelDescriptors[stadiumLevel];
    }

    public int getTicketPrices(){
      return ticketPrices;
    }

    public int[] getSkillLevels(){
      return skillLevels;
    }

    public int getSkillLevel(int skill){
      return skillLevels[skill];
    }

    public void setLeagueLevel(int level){
      leagueLevel = level;
    }

    public void setMoney(int updateAmount){
      money = updateAmount;
    }

    public void setTeam(teamScript playerTeam){
      team = playerTeam;
    }

    public void setStadiumCapacity(int capacity){
      stadiumCapacity = capacity;
    }

    public void setStadiumLevel(int level){
      stadiumLevel = level;
    }

    public void setTicketPrices(int price){
      ticketPrices = price;
    }

    public void setSkillLevel(int skill, int level){
      skillLevels[skill] = level;
    }




}
