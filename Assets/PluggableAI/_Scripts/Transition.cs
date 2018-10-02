using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Transition
{	
	public DecisionFactory Decision;
	public State NextState;	
	public string AnimationToPlay; 
}