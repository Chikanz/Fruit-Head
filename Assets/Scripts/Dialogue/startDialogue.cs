using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class startDialogue : MonoBehaviour {

    GameObject dialogue;
    public string scene;
    public bool sceneStart;


    // Use this for initialization
    void Start () {

        if (!dialogue) dialogue = GameObject.Find("Yarn");

        if (sceneStart)
        {
            positioning();
            runStartDialogue();
        }

    }
	
	void runStartDialogue()
    {
        print("run");
        if (scene == "BerryIsland")
        {
            Yarn.Value biStage = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$biStage");
            int stage = (int)biStage.AsNumber;

            if (stage == 1)
            {
                dialogue.GetComponent<DialogueRunner>().StartDialogue("BerryIslandStart");
            }
            else if (stage == 2)
            {
                print("stage2");
                dialogue.GetComponent<DialogueRunner>().StartDialogue("KellsHouseExit");
            }
            else if (stage == 3)
            {
                dialogue.GetComponent<DialogueRunner>().StartDialogue("TownHallExit");
            }
            else if (stage == 5)
            {
                dialogue.GetComponent<DialogueRunner>().StartDialogue("Stage5");
            }
            else if (stage == 6)
            {
                dialogue.GetComponent<DialogueRunner>().StartDialogue("Stage6");
            }
            else if (stage == 9)
            {
                dialogue.GetComponent<DialogueRunner>().StartDialogue("HideoutExit");
            }
            else if (stage == 10)
            {
                dialogue.GetComponent<DialogueRunner>().StartDialogue("Stage10");
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
        else if (scene == "LucasOffice")
        {
            dialogue.GetComponent<DialogueRunner>().StartDialogue("Luca");
        }
        else if (scene == "TamsOffice")
        {
            dialogue.GetComponent<DialogueRunner>().StartDialogue("Tam");
        }
        else if (scene == "Cabin")
        {
            dialogue.GetComponent<DialogueRunner>().StartDialogue("CabinInt");
        }
        else if (scene == "OrchardIsland")
        {
            dialogue.GetComponent<DialogueRunner>().StartDialogue("Arrival");
        }
        else if (scene == "CultHQ")
        {
            dialogue.GetComponent<DialogueRunner>().StartDialogue("HQInt");
        }

    }

    void positioning()
    {

        if (scene == "TamsOffice")
        {
            Yarn.Value biStage = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$biStage");
            int stage = (int)biStage.AsNumber;
            if (stage != 5)
            {
                GameObject temp = GameObject.Find("Avery");
                temp.GetComponent<ActivateCharacter>().setMesh(false, true);
                GameObject temp2 = GameObject.Find("Maison");
                temp2.GetComponent<ActivateCharacter>().setMesh(false, true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        Yarn.Value biStage = dialogue.GetComponent<ExampleVariableStorage>().GetValue("$biStage");
        int stage = (int)biStage.AsNumber;

        if (gameObject.name == "MeetingTrigger")
        {
            if (enabled && stage == 2)
            {
                dialogue.GetComponent<DialogueRunner>().StartDialogue("Debate");
                enabled = false;
            }
        }
        else if (gameObject.name == "BoatTrigger")
        {
            if (stage == 10 && enabled)
            {
                dialogue.GetComponent<DialogueRunner>().StartDialogue("Docks");
                enabled = false;
            }
        }
        else if (gameObject.name == "HideoutTrigger")
        {
            dialogue.GetComponent<DialogueRunner>().StartDialogue("Hideout");
            enabled = false;
        }
        else if (gameObject.name == "KellTrigger")
        {
            if (enabled)
            {
                dialogue.GetComponent<DialogueRunner>().StartDialogue("Kell");
                enabled = false;
            }
        }
        else
        {
            if (enabled)
            {
                dialogue.GetComponent<DialogueRunner>().StartDialogue("FinalRoom");
                enabled = false;
            }
            
        }


    }

}
