using System.Collections;
using System.Collections.Generic;
using Luminosity.IO;
using UnityEngine;
using Yarn.Unity;
using UnityStandardAssets.Characters.ThirdPerson;

/// <summary>
/// Not a good idea to put script in a class name 
/// </summary>
public class MapEnabler : MonoBehaviour 
{
    //private CanvasGroup canvas;
	private GameObject map;
    private DialogueRunner dialogueR;
    public GameObject Charlie;

	// Use this for initialization
	void Start ()
	{
		//if (!dialogueR) //This will always be true! 			
		dialogueR = SceneChanger.Yarn.GetComponent<DialogueRunner>(); //Use our stored static reference

        //I don't even know what a canvas group is lol
		//canvas = GetComponent<CanvasGroup>();

		map = transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{       
        if (InputManager.GetButtonDown("Map") && !dialogueR.isDialogueRunning)
        {
	        map.SetActive(!map.activeInHierarchy); //just set the gameobject to be what it wasn't before
            Charlie.GetComponent<ThirdPersonUserControl>().CanMove = !Charlie.GetComponent<ThirdPersonUserControl>().CanMove;
        }
	}

}
