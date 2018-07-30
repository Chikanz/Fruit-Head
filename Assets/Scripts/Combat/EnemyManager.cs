using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int SceneToExitTo = 1;

    public List<CombatCharacter> Enemies { get; private set; }

    private bool _changingScene;

	// Use this for initialization
	void Start ()
    {
        Enemies = new List<CombatCharacter>();
        foreach(CombatCharacter c in GetComponentsInChildren<CombatCharacter>())
        {
            Enemies.Add(c);
            c.OnDefeat += RemoveDefeatedEnemy;
        }
	}

    private void RemoveDefeatedEnemy(object sender, EventArgs e)
    {
        Debug.Assert(sender is CombatCharacter, "Invalid defeated message");
        Enemies.Remove((CombatCharacter)sender);
    }

    // Update is called once per frame
	void Update ()
    {        
        //We ran out of enemies to kill!
        if (!_changingScene && Enemies.Count == 0)
        {
            _changingScene = true; 
            SceneChanger.instance.Change(SceneToExitTo);
        }
    }    
}
