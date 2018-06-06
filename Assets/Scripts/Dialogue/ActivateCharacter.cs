using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Yarn.Unity {

public class ActivateCharacter : MonoBehaviour {

		public bool startActive;
		SkinnedMeshRenderer[] meshes;
		Transform originalPosition;


	// Use this for initialization
	void Start () {
			originalPosition = gameObject.transform;
			if (gameObject.name == "Avery") {
				//set avery's position on the other side of the gate so players can't talk to them before they should
				gameObject.transform.position -= new Vector3(10, 0, 0);
			}

			meshes = gameObject.GetComponentsInChildren<SkinnedMeshRenderer> ();
			foreach (var m in meshes) {
				m.enabled = false;
			}
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