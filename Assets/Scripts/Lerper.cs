using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just lerp this transform to a position
/// </summary>
public class Lerper : MonoBehaviour
{
	public Transform follow;
	public float speed;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = Vector3.Lerp(transform.position, follow.position, speed);
	}
}
