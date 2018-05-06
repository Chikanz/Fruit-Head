using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class OW_Player : OW_Character {

    private DialogueRunner _DR;

	// Use this for initialization
	void Start () {
        _DR = FindObjectOfType<DialogueRunner>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !_DR.isDialogueRunning)
        {
            var colliders = Physics.OverlapSphere(transform.position, 2);
            foreach(Collider c in colliders)
            {
                var uwu = c.GetComponent<OW_Character>();
                if (!uwu || c.GetComponent<OW_Player>()) return; //Must have OWC script, must not be player

                _DR.StartDialogue(uwu.Name);
            }
        }
    }
}
