using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make object always face the main camera
/// </summary>
public class Billboard : MonoBehaviour {

	
	// Use this for initialization

	private Transform cam;
	void Start ()
	{
		cam = GameObject.FindWithTag("MainCamera").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{

		transform.rotation = Quaternion.LookRotation((cam.position - transform.position).normalized);
	}
}
