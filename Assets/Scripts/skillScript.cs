using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillScript : MonoBehaviour
{

    int cost;
    string upgradeType;
    bool purchased;
    string description;
    int bonus;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }





    public void setCost(int price){
      cost = price;
    }

    public void setUpgradeType(string type){
      upgradeType = type;
    }

    public void setPurchased(bool input){
      purchased = input;
    }

    public void setBonus(int bonusInput){
      bonus = bonusInput;
    }

    public void setDescription(string desInput){
      description = desInput;
    }

    public int returnCost(){
      return cost;
    }

    public string returnUpgradeType(){
      return upgradeType;
    }

    public bool returnPurchased(){
      return purchased;
    }

    public int returnBonus(){
      return bonus;
    }

    public string returnDescription(){
      return description;
    }

}
