using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Quick hack to make scene work for passive + battle
/// </summary>
/// 
public class PassiveSwitcher : MonoBehaviour
{	
	private static int Stage = -1;

	public string[] StageNode;
	
	public GameObject[] Stage0Stuff;
	public GameObject[] Stage1Stuff;
	
	// Use this for initialization
	void Awake()
	{
		OnSceneChange();
	}

	private void OnSceneChange()
	{
		Stage++;

		GameObject[] TurnOn = Stage == 0 ? Stage0Stuff : Stage1Stuff;		

		foreach (GameObject o in TurnOn)
		{
			o.SetActive(true);
		}
		
		GameObject[] TurnOff = Stage == 1 ? Stage0Stuff : Stage1Stuff;
		foreach (GameObject o in TurnOff)
		{
			o.SetActive(false);
		}
		
		Invoke("yarn",0.1f);
	}


	void yarn()
	{
		//Run the proper node
		SceneChanger.instance.GetYarn().GetComponent<DialogueRunner>().StartDialogue(StageNode[Stage]);
	}


	// Update is called once per frame
	void Update () {
		
	}
}
