using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Checks to see if anyone else in the party has dropped below a certain health thresh
/// </summary>
public class ComradeHealth : Decision
{
    //Percentage of health as float    
    public float healthThresh = 0.5f;
    public bool setTarget = false;

    public ComradeHealth()
    {
        Debug.Assert(healthThresh < 1.0f && healthThresh > 0f);
    }
    
    public override bool Decide(StateController c)
    {
        List<CombatCharacter> teamToCheck;
        
        //Check which team we're on (fuck yeah global variables)
        teamToCheck = c.MyCC.Friendly ? CombatManager.instance.GetParty() : EnemyManager.instance.Enemies;

        foreach (var cc in teamToCheck)
        {
            if(cc == c.MyCC) continue; //Skip health check on self
            if ((float) cc.Health / cc.MaxHealth < healthThresh) //check if their health is below X percent
            {
                if (setTarget) c.Target = cc.transform; //Set target if we want
                return true;
            }
        }
        
        return false;
    }
}
