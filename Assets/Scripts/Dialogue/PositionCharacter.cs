using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PositionCharacter : MonoBehaviour {

	static GameObject dialogue;

	// Use this for initialization
	void Start () {

		if (!dialogue) dialogue = GameObject.Find("Yarn");

		Yarn.Value forestComplete = dialogue.GetComponent<ExampleVariableStorage> ().GetValue ("$foundDog");
		bool temp = forestComplete.AsBool;
		Yarn.Value forest = dialogue.GetComponent<ExampleVariableStorage> ().GetValue ("$forest");
		bool temp2 = forest.AsBool;

		if (gameObject.name == "Avery") {
			//set avery's position on the other side of the gate so players can't talk to them before they should
			if (!temp && !temp2) {
				gameObject.transform.position -= new Vector3 (10, 0, 0);
			} 
		}

		if (gameObject.name == "Jack") {
			Yarn.Value finnyes = dialogue.GetComponent<ExampleVariableStorage> ().GetValue ("$finnyes");
			bool temp3 = finnyes.AsBool;

			if (temp3) {
				GameObject destination = GameObject.Find ("JackDestination");
				Vector3 pos = destination.gameObject.transform.position;
				gameObject.transform.position = pos;
			}

		}

		if (gameObject.name == "Kim") {

			if (temp) {
				GameObject destination = GameObject.Find ("KimDestination");
				Vector3 pos = destination.gameObject.transform.position;
				gameObject.transform.position = pos;
			}

		}

		if (gameObject.name == "Charlie") {

			if (temp) {
				GameObject destination = GameObject.Find ("CharlieForest");
				Vector3 pos = destination.gameObject.transform.position;
				gameObject.transform.position = pos;
			}

		}

	}
	
	[YarnCommand("position")]
	public void setAtPoint(string point) {

		GameObject location = GameObject.Find (point);

		gameObject.transform.position = location.transform.position + new Vector3 (1, 0, 1);

	}
}
