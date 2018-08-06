using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Circle")]
public class NewBehaviourScript : Action
{
	public float radius = 3;
	
	public override void Act(StateController c)
	{
		//Circle
		var pointer = c.Target.transform.position - c.transform.position;
		var cross = Vector3.Cross(c.transform.up, pointer.normalized); //Get vector perpendicular to target
		
		//Maintain Distance	
		Vector3 outwards = Vector3.Cross(c.transform.up, cross) * Mathf.Clamp((radius - pointer.magnitude), -1, 1);

		var move = cross + outwards;
		Debug.DrawLine(c.transform.position, c.transform.position + cross, Color.cyan);
		Debug.DrawLine(c.transform.position, c.transform.position + outwards, Color.green);
		c.MyAI.Move(move);
	}
}
