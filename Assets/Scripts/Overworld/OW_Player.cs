using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class OW_Player : OW_Character
{
    
    public override void Start()
    {
        base.Start();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Get everything in range
        if(Input.GetKeyDown(KeyCode.Space) && !_DR.isDialogueRunning)
        {
            var colliders = Physics.OverlapSphere(transform.position, 2);
            foreach(Collider c in colliders)
            {
                var uwu = c.GetComponent<OW_NPC>();
                if (!uwu) continue; //Must have OWC script, must not be player

                uwu.TalkTo(transform);
            }
        }
    }
}
