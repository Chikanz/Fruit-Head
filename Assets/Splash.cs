using System.Collections;
using System.Collections.Generic;
using Luminosity.IO;
using UnityEngine;

public class Splash : MonoBehaviour
{
	private bool changed = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (InputManager.AnyInput() && !changed)
		{
			changed = true;
			SceneChanger.instance.Change(2);
		}
	}
}
