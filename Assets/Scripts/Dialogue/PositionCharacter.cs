using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PositionCharacter : MonoBehaviour {

	static GameObject dialogue;
    GameObject Charlie;

	// Use this for initialization
	void Start () {

		if (!dialogue) dialogue = GameObject.Find("Yarn");

        Charlie = GameObject.Find("Charlie");

        Yarn.Value biStage = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$biStage");
        int stage = (int)biStage.AsNumber;
        if (stage == 0)
        {
            positionStage0();            
        }
        else if (stage == 2)
        {
            //positionKellExt(false);
            positionParty("KellsHouseExt", false, false);
        }
        else if (stage == 3)
        {
            //positionTownHall(false);
            positionParty("TownHallExt", false, false);
        }
        else if (stage == 4 /*|| stage == 8*/)
        {
            //positionMaisonExt();
            positionParty("MaisonsHouseExt", true, false);
            if (gameObject.name == "Riley" || gameObject.name == "Devon")
            {
                setAtPoint("BankExt");
                GetComponent<ActivateCharacter>().show("false");
            }
            if (gameObject.name == "Devon")
            {
                transform.position += new Vector3(1.0f, 0, 1.0f);
            }
        }
        else if (stage == 5)
        {
            Yarn.Value temp = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$stage5");
            string stage5 = temp.AsString;

            if (stage5 == "station")
            {
                positionParty("PoliceStationExt", true, false);
                //positionStationExt(false);
            }
            else if (stage5 == "tam")
            {
                positionParty("TownHallExt", true, false);
                //positionTownHall(true);
            }
            else
            {
                positionParty("KellsHouseExt", true, false);
                //positionKellExt(true);
            }
        }
        else if (stage == 6 || stage == 8)
        {
            positionParty("PoliceStationExt", true, false);
            //positionStationExt(false);
        }
        else if (stage == 9)
        {
            positionParty("CabinExt", true, false);
            //positionHideoutExt();
        }
        else if (stage == 10)
        {
            positionParty("PoliceStationExt", true, true);
            //positionStationExt(true);
        }
	}
	
	[YarnCommand("position")]
	public void setAtPoint(string point) {

		GameObject location = GameObject.Find (point);

        if (gameObject.name != "Tam" && gameObject.name != "Luca")
        {
            gameObject.transform.position = location.transform.position + new Vector3(1, 0, 1);
        }
        else
        {
            gameObject.transform.position = location.transform.position;
        }

	}


    void positionStage0()
    {
        //positioning for blossom island
        Yarn.Value forestComplete = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$foundDog");
        bool temp = forestComplete.AsBool;
        Yarn.Value forest = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$forest");
        bool temp2 = forest.AsBool;
        Yarn.Value house = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$afterhouse");
        string spawn = house.AsString;

        if (gameObject.name == "Avery")
        {
            //set avery's position on the other side of the gate so players can't talk to them before they should
            if (!temp && !temp2)
            {
                transform.position -=
                       new Vector3(10, 0, 0);
            }
        }

        if (gameObject.name == "Jack")
        {
            Yarn.Value finnyes = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$finnyes");
            bool temp3 = finnyes.AsBool;

            //place jack with finn if matched
            if (temp3)
            {
                GameObject destination = GameObject.Find("JackDestination");
                Vector3 pos = destination.gameObject.transform.position;
                gameObject.transform.position = pos;
            }

        }

        if (gameObject.name == "Kim")
        {
            //place kim near forest entrance after charlie returns
            if (temp)
            {
                GameObject destination = GameObject.Find("KimDestination");
                Vector3 pos = destination.gameObject.transform.position;
                gameObject.transform.position = pos;
            }

        }

        if (gameObject.name == "Charlie")
        {
            //place charlie outside nancy's house when they finish killing the bats
            if (spawn == "nancy")
            {
                GameObject destination = GameObject.Find("CharlieBats");
                Vector3 pos = destination.gameObject.transform.position;
                gameObject.transform.position = pos;
            }
            //place charlie near forest entrance after they return
            if (temp)
            {
                GameObject destination = GameObject.Find("CharlieForest");
                Vector3 pos = destination.gameObject.transform.position;
                gameObject.transform.position = pos;
            }

        }
    }

    //void positionKellExt(bool maison)
    //{
    //    //place party near kell's house
    //    GameObject destination = GameObject.Find("KellsHouseExt");
    //    Vector3 pos = destination.gameObject.transform.position;
    //    if (gameObject.name == "Charlie")
    //    {
    //        gameObject.transform.position = pos;
    //    }
    //    else if (gameObject.name == "Avery") {
    //        gameObject.transform.position = pos - new Vector3(1, 0, 1);
    //    }
    //    else if (maison && gameObject.name == "Maison")
    //    {
    //        gameObject.transform.position = pos + new Vector3(1, 0, 1);
    //    }
    //}

    //void positionTownHall(bool party)
    //{

    //    GameObject destination = GameObject.Find("TownHallExt");
    //    Vector3 pos = destination.gameObject.transform.position;

    //    if (gameObject.name == "Charlie")
    //    {
    //        //place charlie outside town hall
    //        gameObject.transform.position = pos;
    //    }
        
    //    if (party) { 
    //        if (gameObject.name == "Avery")
    //        {
    //            gameObject.transform.position = pos - new Vector3(1, 0, 1);
    //        }
    //        else if (gameObject.name == "Maison")
    //        {
    //            gameObject.transform.position = pos + new Vector3(1, 0, 1);
    //        }
    //    }
    //    else
    //    {
    //        if (gameObject.name == "Avery")
    //        {
    //            destination = GameObject.Find("Debate");
    //            pos = destination.gameObject.transform.position;
    //            gameObject.transform.position = pos - new Vector3(1, 0, 1);
    //        }
    //    }
    //}

    //void positionMaisonExt()
    //{

    //    //GameObject destination = GameObject.Find("MaisonsHouseExt");
    //    //Vector3 pos = destination.gameObject.transform.position;
        

    //    //if (gameObject.name == "Charlie")
    //    //{
    //    //    //place charlie near maison's house
    //    //    gameObject.transform.position = pos;
    //    //    gameObject.transform.forward = destination.transform.forward;
            
    //    //}
    //    //else if (gameObject.name == "Avery")
    //    //{
    //    //    positionAvery(pos);
    //    //}
    //    //{
            
    //    //    gameObject.transform.position = pos + (temp.transform.forward - new Vector3(0, 0, 2));
    //    //}
    //    //else if (gameObject.name == "Maison")
    //    //{
    //    //    gameObject.transform.position = pos + (temp.transform.forward + new Vector3(0, 0, 2));
    //    //}
    //}

    //void positionStationExt(bool eden)
    //{

    //    GameObject destination = GameObject.Find("PoliceStationExt");
    //    Vector3 pos = destination.gameObject.transform.position;
    //    GameObject temp = GameObject.Find("Charlie");


    //    if (gameObject.name == "Charlie")
    //    {
    //        //place charlie near maison's house
    //        gameObject.transform.position = pos;
    //        gameObject.transform.forward = destination.transform.forward;

    //    }
    //    else if (gameObject.name == "Avery")
    //    {

    //        gameObject.transform.position = pos + (temp.transform.forward - new Vector3(0, 0, 2));
    //    }
    //    else if (gameObject.name == "Maison")
    //    {
    //        gameObject.transform.position = pos + (temp.transform.forward + new Vector3(0, 0, 2));
    //    }

    //    //if (gameObject.name == "Charlie")
    //    //{
    //    //    //place charlie near police station
    //    //    gameObject.transform.position = pos;
    //    //}
    //    //else if (gameObject.name == "Avery")
    //    //{
    //    //    gameObject.transform.position = pos - new Vector3(1, 0, 1);
    //    //}
    //    //else if (gameObject.name == "Maison")
    //    //{
    //    //    gameObject.transform.position = pos + new Vector3(1, 0, 1);
    //    //}

    //    //if (eden)
    //    //{
    //    //    if (gameObject.name == "Eden")
    //    //    {
    //    //        gameObject.transform.position = pos - new Vector3(1, 0, 1);
    //    //    }
    //    //}

    //    //else if (gameObject.name == "Tam")
    //    //{
    //    //    gameObject.transform.position += new Vector3(10, 0, 0);
    //    //}
    //}


    //void positionHideoutExt()
    //{

    //    GameObject destination = GameObject.Find("CabinExt");
    //    Vector3 pos = destination.gameObject.transform.position;

    //    if (gameObject.name == "Charlie")
    //    {
    //        //place charlie near cult hideout/cabin
    //        gameObject.transform.position = pos;
    //    }
    //    else if (gameObject.name == "Avery")
    //    {
    //        gameObject.transform.position = pos - new Vector3(1, 0, 1);
    //    }
    //    else if (gameObject.name == "Maison")
    //    {
    //        gameObject.transform.position = pos + new Vector3(1, 0, 1);
    //    }

    //}



    void positionParty(string location, bool maison, bool eden)
    {
        GameObject destination = GameObject.Find(location);
        Vector3 pos = destination.gameObject.transform.position;

        gameObject.transform.forward = destination.transform.forward;

        //Vector3 localForward = destination.transform.worldToLocalMatrix.MultiplyVector(transform.forward);

        
        Vector3 localForward = Charlie.transform.worldToLocalMatrix.MultiplyVector(transform.forward);


        if (gameObject.name == "Charlie")
        {
            gameObject.transform.position = pos;
        }
        else if (gameObject.name == "Avery")
        {
            gameObject.transform.position = pos;
            print("ab");
            //if (location != "TownHallExt")
            //{
            //    gameObject.transform.position = pos + (localForward - new Vector3(-1, 0, 3));
            //}
            //else
            //{
            //    if (!maison)
            //    {
            //        gameObject.transform.position = GameObject.Find("Debate").transform.position;
            //        gameObject.transform.LookAt(Charlie.transform);
            //    }
            //    else
            //    {
            //        gameObject.transform.position = pos + (localForward - new Vector3(-1, 0, 3));
            //    }
            //}
            
        }

        if (maison) { 
            if (gameObject.name == "Maison")
            {
                gameObject.transform.position = pos - (localForward - new Vector3(-1, 0, -0.5f));
                
            }
        }

        if (eden)
        {
            if (gameObject.name == "Eden")
            {
                gameObject.transform.position = pos - (localForward - new Vector3(1, 0, 1));
            }

        }
    }
}
