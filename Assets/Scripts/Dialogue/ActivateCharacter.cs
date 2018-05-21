using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Yarn.Unity {

public class ActivateCharacter : MonoBehaviour {

		public bool startActive;
		SkinnedMeshRenderer[] meshes;


	// Use this for initialization
	void Start () {
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
			foreach (var m in meshes) {
				m.enabled = !startActive;
			}
		}

}
}