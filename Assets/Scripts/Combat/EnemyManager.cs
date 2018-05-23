using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<CombatCharacter> Enemies = new List<CombatCharacter>();

    bool changingScene = false;

	// Use this for initialization
	void Start ()
    {
        foreach(CombatCharacter c in GetComponentsInChildren<CombatCharacter>())
        {
            Enemies.Add(c);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (changingScene) return;

        foreach (CombatCharacter c in Enemies) //yum yum AAAAAAAAAAAAAAAAAAAAAAAAA
        {
            if (!c.IsDead()) return;
            changingScene = true; 
            SceneChanger.instance.Change(1);
        }
    }
}
