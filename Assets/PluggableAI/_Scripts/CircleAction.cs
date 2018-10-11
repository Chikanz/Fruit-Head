using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Circle")]
public class CircleAction : AIAction
{
	public float radius = 3;
	public Vector3 outttt;
	
	public override void Act(StateController c)
	{
		//Clear force look
		c.MyAI.ForceLookAt(null);
		
		//Circle		
		var pointer = c.Target.position - c.transform.position;
		var cross = Vector3.Cross(c.transform.up, pointer.normalized); //Get vector perpendicular to target
		
		//Maintain Distance	
		var clamp = Mathf.Clamp((radius - pointer.magnitude), -1, 1);		
		Vector3 outwards = Vector3.Cross(c.transform.up, cross) * clamp;
						
		//Boid the fuck away from other doggos
		Vector3 seperation = Vector3.zero;
		foreach (var e in c.GetComponentInParent<EnemyManager>().Enemies)
		{
			var d = Vector3.Distance(c.transform.position, e.transform.position);
			if (d == 0) continue; //Outta range or is me
			var diff = c.transform.position - e.transform.position;
			diff.Normalize();
			diff /= (d); // Weight by distance			
			seperation += diff;
		}
				
		//Our outwards vector conflicts with movement so move closer to zero it when it gets big 
		var outw = Vector3.Lerp(outwards, Vector3.zero, seperation.sqrMagnitude * 4);	
		
		var move = cross.normalized + outw + seperation;
		Debug.DrawLine(c.transform.position, c.transform.position + move * 10, Color.cyan);
		Debug.DrawLine(c.transform.position, c.transform.position + outw * 10, Color.green); 
		Debug.DrawLine(c.transform.position, c.transform.position + seperation * 10, Color.yellow);
		Debug.DrawLine(c.transform.position, c.transform.position + cross.normalized * 10, Color.blue);
		
		c.MyAI.Move(move);
	}
}
