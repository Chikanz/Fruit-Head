using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DisapearOnFinishTalking : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		SceneChanger.Yarn.GetComponent<DialogueUI>().OnDialogueEnd += disapear;
	}

	void disapear(string s)
	{
		gameObject.SetActive(false);
		Destroy(this);
	}

	private void OnDestroy()
	{
		SceneChanger.Yarn.GetComponent<DialogueUI>().OnDialogueEnd -= disapear;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
