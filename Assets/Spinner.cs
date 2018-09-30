using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generalized spinner class
/// </summary>
public class Spinner : MonoBehaviour 
{
	[Range(0.0f, 1.0f)]
	public float x, y, z;
    
	[Range(0, 1000)]
	public float Speed;
	
	//todo stop spinning when not active	

	void Update ()
	{
		transform.Rotate(x * Speed * Time.deltaTime,
						 y * Speed * Time.deltaTime,
						 z * Speed * Time.deltaTime);
	}
}
