using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Watch")]
public class WatchAction : AIAction 
{
    public override void OnEnter(StateController c)
    {
        c.MyAI.ForceLookAt(c.Target);
    }

    public override void OnExit(StateController c)
    {
        c.MyAI.ForceLookAt(null);
    }

    public override void Act(StateController c)
    {
        
    }
}
