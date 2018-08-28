using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class startDialogue : MonoBehaviour {

    static GameObject dialogue;
    public string scene;


    // Use this for initialization
    void Start () {

        if (!dialogue) dialogue = GameObject.Find("Yarn");
        if (gameObject.name != "MeetingTrigger")
        {
            runStartDialogue();
        }
        

    }
	
	void runStartDialogue()
    {
        Yarn.Value biStage = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$biStage");
        float stage = biStage.AsNumber;
        if (scene == "BerryIsland")
        {
            if (stage == 1)
            {
                dialogue.GetComponent<DialogueRunner>().StartDialogue("BerryIslandStart");
            }
            else if (stage == 2)
            {
                dialogue.GetComponent<DialogueRunner>().StartDialogue("KellsHouseExit");
            }
            else if (stage == 3)
            {
                dialogue.GetComponent<DialogueRunner>().StartDialogue("TownHallExit");
            }
            else if (stage == 5)
            {
                dialogue.GetComponent<DialogueRunner>().StartDialogue("StationExit");
            }
        }
        else if (scene == "KellsHouse")
        {
            dialogue.GetComponent<DialogueRunner>().StartDialogue("KellsHouseInt");
        }
        else if (scene == "MaisonsHouse")
        {
            dialogue.GetComponent<DialogueRunner>().StartDialogue("MaisonsHouseInt");
        }
        else if (scene == "Station")
        {
            dialogue.GetComponent<DialogueRunner>().StartDialogue("StationInt");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Yarn.Value biStage = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$biStage");
        float stage = biStage.AsNumber;
        print("debate");
        if (stage == 2)
        {
            dialogue.GetComponent<DialogueRunner>().StartDialogue("Debate");
        }
    }

}
