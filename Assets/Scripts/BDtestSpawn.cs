using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDtestSpawn : MonoBehaviour
{
	public BattleData BD;
	public BattleData BD2;

	private static bool yeeted = false;
	
	// Use this for initialization
	void Start () 
	{
		Invoke("changeScene",1);
	}

	void changeScene()
	{
		if (!yeeted)
		{
			SceneChanger.instance.StartCombat(BD);
			yeeted = true;
		}
		else
		{
			SceneChanger.instance.StartCombat(BD2);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
