using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class MoneyManager : MonoBehaviour 
{
	Text displayText;
	GameObject dialogue;
	Yarn.Value money;

	public int ChairHittingScene = 2;

	private ExampleVariableStorage EVS; //Cache class reference since get component calls are not free
	
	// Use this for initialization
	void Start () {
		displayText = gameObject.GetComponent<Text>();

		dialogue = SceneChanger.Yarn;
		EVS = dialogue.GetComponent<ExampleVariableStorage>();

		//Only update money when changing scenes, instead of every frame
		SceneChanger.instance.OnSceneChange += UpdateMoney;
	}

	//unsub from event since it'll be called even if this object is destroyed
	private void OnDestroy()
	{
		SceneChanger.instance.OnSceneChange -= UpdateMoney;
		Debug.Log("money manager destroyed");
	}

	// Update is called once per frame
	void UpdateMoney(int scene) 
	{
		//Disable on chair hitting scene
		GetComponent<Text>().enabled = scene != ChairHittingScene;
		
		//Update val
		money = EVS.GetValue("$money");		 
		displayText.text = "$" + money.AsNumber; //Combine with string to avoid .toString()
	}
}
