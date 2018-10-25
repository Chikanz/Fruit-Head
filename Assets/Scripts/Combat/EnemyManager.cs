using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public int SceneToExitTo = 1;

	//List of spawned enemies
    public List<CombatCharacter> Enemies { get; private set; }

    private bool _changingScene;

    public static EnemyManager instance;

	public GameObject healthUI;

	// Use this for initialization
	void Awake ()
	{
		//THERE CAN ONLY BE ONE
	    if (!instance)
	        instance = this;
	    else
	    {
	        Destroy(gameObject);
	    }
        
        Enemies = new List<CombatCharacter>();

		//(mr) Test mode
		if (transform.childCount > 0)
		{
			var kiddo = transform.GetChild(0);
			if (kiddo && kiddo.GetComponent<CombatCharacter>())
			{
				AddEnemy(kiddo.gameObject);
			}
		}
	}

	public GameObject AddEnemy(GameObject g)
	{
		var spawned = Instantiate(g, transform.position, Quaternion.identity);
		var cc = spawned.GetComponent<CombatCharacter>();
		if (!cc)
		{
			Debug.Log("Invalid enemy yo");
			return null;
		}
		
		Enemies.Add(cc);
		cc.OnDefeat += RemoveDefeatedEnemy;
		spawned.transform.SetParent(transform,true); //Now belongs to meeeee
		
		//Add health UI
		var c = spawned.GetComponentInChildren<Collider>();
		var ui = Instantiate(healthUI, spawned.transform.position, Quaternion.identity);				
		ui.transform.SetParent(spawned.transform,true);
		
		return spawned;
	}

    private void RemoveDefeatedEnemy(object sender, EventArgs e)
    {
        Debug.Assert(sender is CombatCharacter, "Invalid defeated message");
        Enemies.Remove((CombatCharacter)sender);
	    
	    //End combat
	    if(Enemies.Count == 0 && SceneChanger.instance) 
		    SceneChanger.instance.EndCombat();
    }

	private void OnDestroy()
	{
		instance = null;
	}

	public CombatCharacter GetRandomEnemy()
	{
		return Enemies[Random.Range(0, Enemies.Count)];
	}
	

	// Update is called once per frame
	void Update ()
    {        
        //We ran out of enemies to kill!
//        if (!_changingScene && Enemies.Count == 0)
//        {
//            _changingScene = true; 
//            SceneChanger.instance.Change(SceneToExitTo);
//        }
    }    
}
