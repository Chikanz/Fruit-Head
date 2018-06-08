using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int SceneToExitTo = 1;
    
    List<CombatCharacter> Enemies = new List<CombatCharacter>();

    private bool _changingScene;

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
        if (_changingScene) return;

        foreach (CombatCharacter c in Enemies) //yum yum AAAAAAAAAAAAAAAAAAAAAAAAA
        {
            if (!c.IsDead()) return;
        }
        _changingScene = true; 
        SceneChanger.instance.Change(SceneToExitTo);
    }
}
