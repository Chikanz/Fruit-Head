using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Takeable : MonoBehaviour
{
    private bool taken = false;
    DialogueUI DUI;

    // Use this for initialization
    void Start ()
    {
        DUI = GameObject.Find("Yarn").GetComponent<DialogueUI>();
        DUI.OnDialogueEnd += Takeable_OnDialogueEnd;
	}

    // Update is called once per frame
    void Update () {
		
	}

    private void Takeable_OnDialogueEnd(string name)
    {
        if (taken) //If you are looking for ransom, I can tell you I don't have money. But what I do have are a very particular set of skills; skills I have acquired over a very long career in game development. Skills that make me a nightmare for gameobjects like you. If you let my memory go now, that'll be the end of it. I will not look for you, I will not pursue you. But if you don't, the unity garbage collector look for you, It will find you and It will kill you.
        {
            Destroy(gameObject); //delay enough to give thumb cam enough time to change parents

            //Make sure to unsub so this doesn't get called after we ded
            DUI.OnDialogueEnd -= Takeable_OnDialogueEnd;
        }
    }

    [YarnCommand("Take")]
    public void Take()
    {
        //Make sure we clean up after being talked to 
        taken = true;

        //Disable mesh renderers for now
        foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
            r.enabled = false;
    }
}
