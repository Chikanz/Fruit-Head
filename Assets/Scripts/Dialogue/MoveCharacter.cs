﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yarn.Unity {
	public class MoveCharacter : MonoBehaviour {

		GameObject target;
		float speed;
		static GameObject dialogue;


		// Use this for initialization
		void Start () {
			target = null;
			speed = 2.0f;

            if (!dialogue) dialogue = GameObject.Find("Yarn");

        }
		
		// Update is called once per frame
		void Update () {
			if (target != null ) {

				float step = speed * Time.deltaTime;
				Transform destination = target.gameObject.transform;
				transform.position = Vector3.MoveTowards(transform.position, destination.position, step);

				if (Vector3.Distance (transform.position, destination.position) < 2.0f) {
					if (target.GetComponent<OW_Character> ().Name == "Charlie") {
						string startNode = gameObject.GetComponent<OW_NPC> ().StartNode;
						dialogue.GetComponent<DialogueRunner>().StartDialogue (startNode);
					}
					target = null;
				}
			}

		}


		[YarnCommand("move")]
		public void movetopoint(string destination) {

			print (destination);
			target = GameObject.Find (destination);

			print (target.name);

		}

	}
}