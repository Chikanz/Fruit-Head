using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Simply Transition after timer is finished
/// </summary>
public class TimedDecision : Decision
{
    const string key = "RandomTimer";
    
    public float TimeInState;
    public bool randomize = false;
    public float VariationAmount;
    
    public override bool Decide(StateController c)
    {
        //Set variation amount if not set
        if(!c.VarStorage.ContainsKey(key))
            c.SetVar(key, Random.Range(0.0f, VariationAmount));
           
        //Get amnt from var storage
        var randomAmount = c.GetVar<float>(key); 
            
        var t = randomize ? TimeInState + randomAmount : TimeInState;
        return c.CheckIfCountDownElapsed(t);
    }
}
