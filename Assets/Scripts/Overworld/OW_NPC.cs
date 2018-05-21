using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An NPC you can talk to in the overworld
/// Please note, the name (inherited var) of this NPC should match how it appears in dialouge (ie Nancy)
/// </summary>
public class OW_NPC : OW_Character
{
    private Quaternion targetRot;
    private Quaternion normalRot;
    private Transform lookAt;    

    public float lookSpeed = 0.1f;
    public bool LookAtPlayer = true;

    [Tooltip("The yarn node to call when the player talks to this NPC")]
    public string StartNode;  

    // Use this for initialization
    public override void Start()
    {        
        base.Start();
        normalRot = transform.rotation;

        //((DialogueUI)_DR.dialogueUI).OnDialogueStart += OW_NPC_OnDialogueStart;
        ((DialogueUI)_DR.dialogueUI).OnDialogueEnd += OW_NPC_OnDialogueEnd;
    }

    private void OW_NPC_OnDialogueEnd(string name)
    {
        if (!name.Contains(this.Name)) return; //only call for this NPC
        
        targetRot = normalRot;
        lookAt = null;
    }

    // Update is called once per frame
    void Update ()
    {
        if (lookAt && LookAtPlayer)
        {
            Vector3 v = lookAt.transform.position - transform.position;
            targetRot = Quaternion.LookRotation(v);
            targetRot.eulerAngles = new Vector3(0, targetRot.eulerAngles.y, 0);
        }

        if (Quaternion.Angle(transform.rotation, targetRot) > 2)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lookSpeed);
        }
    }

    /// <summary>
    /// Make this NPC talk to someone
    /// </summary>
    /// <param name="other"></param>
    public void TalkTo(Transform other)
    {
        //Look at
        lookAt = other;        

        //Run dialogue
        _DR.StartDialogue(StartNode);    
    }
}
