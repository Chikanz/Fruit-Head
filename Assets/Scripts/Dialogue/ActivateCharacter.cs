﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Yarn.Unity {

public class ActivateCharacter : MonoBehaviour {

		public bool startActive;
		SkinnedMeshRenderer[] meshes;
		Transform originalPosition;
		static GameObject dialogue;

					

	// Use this for initialization
	void Start () {

			if (!dialogue) dialogue = GameObject.Find("Yarn");
			//if (gameObject.name == "Avery") {
				Yarn.Value forestComplete = dialogue.GetComponent<ExampleVariableStorage> ().GetValue ("$foundDog");
				bool temp = forestComplete.AsBool;
				if (!temp) {
					meshes = gameObject.GetComponentsInChildren<SkinnedMeshRenderer> ();
					foreach (var m in meshes) {
						m.enabled = startActive;
					}
				}
			//}


			originalPosition = gameObject.transform;


	}
	
	// Update is called once per frame
	void Update () {

	}


		[YarnCommand("activate")]
		public void activate() {
			print (gameObject.name);
			if (gameObject.name == "Kim") {
				Vector3 temp = Camera.main.ViewportToWorldPoint (new Vector3 (1.1f, 0.5f, 15.0f));
				gameObject.transform.position = temp;
				gameObject.transform.position = new Vector3 (gameObject.transform.position.x, originalPosition.position.y, gameObject.transform.position.z);
			} 
			else if (gameObject.name == "Avery") {
				gameObject.transform.position += new Vector3(10, 0, 0);
			}

			foreach (var m in meshes) {
				m.enabled = !startActive;
			}
		}

}
}