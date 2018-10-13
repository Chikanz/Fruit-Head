using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Checks to see if a change in health has happened
/// </summary>
public class HitDecision : Decision
{
    private const string key = "_health";
    
    public override bool Decide(StateController c)
    {
        if (c.HasKey(key) && c.GetVar<int>(key) != c.MyCC.Health)
        {
            c.RemoveVar(key);
            return true;
        }

        c.SetVar(key,c.MyCC.Health);
        return false;
    }
}
