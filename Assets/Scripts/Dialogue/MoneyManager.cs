using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class MoneyManager : MonoBehaviour {

	Text displayText;
	static GameObject dialogue;
	Yarn.Value money;

	// Use this for initialization
	void Start () {
		displayText = gameObject.GetComponent<Text> ();

		if (!dialogue) dialogue = GameObject.Find("Yarn");

	}
	
	// Update is called once per frame
	void Update () {
		money = dialogue.GetComponent<ExampleVariableStorage> ().GetValue ("$money");
		float temp = money.AsNumber;
		string temp2 = temp.ToString();
		displayText.text = "$" + temp2;

	}
}
