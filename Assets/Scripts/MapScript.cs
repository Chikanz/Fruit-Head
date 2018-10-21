using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MapScript : MonoBehaviour {

    private CanvasGroup canvas;
    private bool viewable;
    private DialogueRunner dialogueR;

	// Use this for initialization
	void Start () {

        if (!dialogueR) dialogueR = GameObject.Find("Yarn").GetComponent<DialogueRunner>();

        canvas = GetComponent<CanvasGroup>();

	}
	
	// Update is called once per frame
	void Update () {

        if (dialogueR.isDialogueRunning) return;


        if (Input.GetKeyDown("m"))
        {
            
            if (canvas.interactable) //turn off map
            {
                Time.timeScale = 1.0f;
                canvas.alpha = 0;
                canvas.interactable = false;
                canvas.blocksRaycasts = false;
            }
            else //turn on map
            {
                Time.timeScale = 0;
                canvas.alpha = 1;
                canvas.interactable = true;
                canvas.blocksRaycasts = true;
            }
            
        }


	}

}
