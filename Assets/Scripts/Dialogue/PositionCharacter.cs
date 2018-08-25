using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PositionCharacter : MonoBehaviour {

	static GameObject dialogue;

	// Use this for initialization
	void Start () {

		if (!dialogue) dialogue = GameObject.Find("Yarn");

        Yarn.Value biStage = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$biStage");
        float stage = biStage.AsNumber;
        if (stage == 0)
        {
            positionStage0();            
        }
        else if (stage == 2)
        {
            positionStage2();
        }
        else if (stage == 4)
        {
            positionStage4();
        }
        else if (stage == 5)
        {
            positionStage5();
        }
	}
	
	[YarnCommand("position")]
	public void setAtPoint(string point) {

		GameObject location = GameObject.Find (point);

		gameObject.transform.position = location.transform.position + new Vector3 (1, 0, 1);

	}


    void positionStage0()
    {
        Yarn.Value forestComplete = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$foundDog");
        bool temp = forestComplete.AsBool;
        Yarn.Value forest = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$forest");
        bool temp2 = forest.AsBool;

        if (gameObject.name == "Avery")
        {
            //set avery's position on the other side of the gate so players can't talk to them before they should
            if (!temp && !temp2)
            {
                gameObject.transform.position -= new Vector3(10, 0, 0);
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
            //place charlie near forest entrance after they return
            if (temp)
            {
                GameObject destination = GameObject.Find("CharlieForest");
                Vector3 pos = destination.gameObject.transform.position;
                gameObject.transform.position = pos;
            }

        }
    }

    void positionStage2()
    {
        //place party near kell's house
        GameObject destination = GameObject.Find("KellsHouseExt");
        Vector3 pos = destination.gameObject.transform.position;
        if (gameObject.name == "Charlie")
        {
            gameObject.transform.position = pos;
        }
        else if (gameObject.name == "Avery") {
            gameObject.transform.position = pos - new Vector3(1, 0, 1);
        }
    }

    void positionStage4()
    {
        if (gameObject.name == "Charlie")
        {
            //place charlie near police station
            GameObject destination = GameObject.Find("MaisonsHouseExt");
            Vector3 pos = destination.gameObject.transform.position;
            gameObject.transform.position = pos;
        }
        else if (gameObject.name == "Avery") { }
    }

    void positionStage5()
    {
        if (gameObject.name == "Charlie")
        {
            //place charlie near police station
            GameObject destination = GameObject.Find("PoliceStationExt");
            Vector3 pos = destination.gameObject.transform.position;
            gameObject.transform.position = pos;
        }
        else if (gameObject.name == "Avery") { }
    }

}
