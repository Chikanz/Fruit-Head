using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// sanity check for forest cam bug
/// </summary>
public class forceLook : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		var cm = GetComponent<Cinemachine.CinemachineVirtualCamera>();
		if (cm && cm.LookAt == null)
		{
			var ow = GameObject.Find("Overworld Character").transform;
			cm.LookAt = ow;
			cm.Follow = ow;
		}	
	}
}
