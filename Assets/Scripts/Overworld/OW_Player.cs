using System.Collections;
using System.Collections.Generic;
using Luminosity.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

/// <summary>
/// Singleton player object
/// </summary>
public class OW_Player : OW_Character
{
    private bool canTalk = true;
    
    public override void Start()
    {
        base.Start();
        Name = "Charlie";

        SceneChanger.instance.OnSceneChange += scene => canTalk = false; //Can't talk when scene is loading
        SceneChanger.instance.OnSceneLoaded += scene => canTalk = true;  //ok we can taco now
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Get everything in range
        if(canTalk && InputManager.GetButtonDown("UI_Submit") && !_DR.isDialogueRunning)
        {
            float highestDot = -1;
            OW_NPC highestDotNpc = null;
            var colliders = Physics.OverlapSphere(transform.position, 2);
            foreach(Collider c in colliders)
            {
                var npc = c.GetComponent<OW_NPC>();
                if (!npc) continue; //Must have OWC script, must not be player
                
                //Work out if we're facing
                var vec = (c.transform.position - transform.position).normalized;
                var dot = Vector3.Dot(transform.forward, vec);
                
                if(dot < 0.5f) continue; //We're not facing
                
                if (dot > highestDot) //Check if we're closer to facing this NPC
                {
                    highestDot = dot;
                    highestDotNpc = npc;
                }
            }

            //Only talk if we're facing an NPC
            if (highestDotNpc != null) highestDotNpc.TalkTo(transform);
        }
    }
}
