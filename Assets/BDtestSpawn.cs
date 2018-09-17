using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDtestSpawn : MonoBehaviour
{
	public BattleData BD;
	
	// Use this for initialization
	void Start () 
	{
		SceneChanger.instance.StartCombat(BD);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
