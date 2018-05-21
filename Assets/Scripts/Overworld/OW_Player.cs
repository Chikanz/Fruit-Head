using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Singleton player object
/// </summary>
public class OW_Player : OW_Character
{
    public static OW_Player instance;

    public override void Start()
    {
        //Enforce singleton
        if (!instance) instance = this;
        else Destroy(gameObject);

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
