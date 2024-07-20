using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stadiumScript : MonoBehaviour
{

    int stadiumCapacity;
    int stadiumLevel;
    string[] stadiumLevelDescriptors = {"Sunday League", "Semi-Pro Team", "Professional Team", "Top Division Team", "World Class Team"};

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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


    public void setStadiumCapacity(int capacity){
      stadiumCapacity = capacity;
    }

    public void setStadiumLevel(int stadiumInput){
      stadiumLevel = stadiumInput;
    }
}
