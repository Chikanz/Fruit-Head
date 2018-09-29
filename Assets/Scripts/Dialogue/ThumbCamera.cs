using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controls the thumb cam, must be child of dialouge UI object for now
/// </summary>
public class ThumbCamera : MonoBehaviour
{
    DialogueUI DUI;
    Camera cam;
    Quaternion targetRot;
    Vector3 LookAt;

    public OW_Player player;

    public float lerpTime = 0.1f;

    bool firstTarget = true;

    Transform originalParent;
    private Vector3 targetPos;

    // Use this for initialization
	void Start ()
    {
        DUI = GetComponentInParent<DialogueUI>();

        DUI.OnDialogueStart += DUI_OnDialogueStart;
        DUI.OnTargetChanged += DUI_OnTargetChanged;
        DUI.OnDialogueEnd += DUI_OnDialogueEnd;

        cam = GetComponent<Camera>();
        cam.enabled = false;

        originalParent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if(cam.enabled)
        {
            //Update look rot
            var v = LookAt - transform.position;
            targetRot = Quaternion.LookRotation(v.normalized);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpTime); //lerp rot
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpTime); 
        }
    }

    private void DUI_OnDialogueStart(string name)
    {
        //Get player if null
        if (!player)
        {
            var n = NPCman.instance;
            Debug.Assert(n, "Can't find NPC man! Did you forget to include one in the scene?");
            player = (OW_Player) n.GetCharacter("Charlie");
        }                       
        //transform.rotation = Quaternion.Euler(player.transform.right);

        cam.enabled = true;        
    }

    private void DUI_OnTargetChanged(string name)
    {
        Debug.Log(name);
        Debug.Assert(NPCman.instance, "Can't find NPC man instance! Did you forget to include one in the scene?");
        var npc = NPCman.instance.GetCharacter(name).transform;

        //Pan in front of whomst ever is speking firstb 
        if (firstTarget)
        {                  
            transform.position = npc.position + (npc.forward * 1) + (npc.right * 1) + npc.up * 1.5f;
            targetPos = transform.position;
            transform.SetParent(npc, true);
            firstTarget = false;
        }
        
        //only move if target is a while away
        if (Vector3.Distance(targetPos, transform.position) > 5 || firstTarget) 
        {
            targetPos = npc.position + (npc.forward * 1) + (npc.right * 1) + npc.up * 1.5f;
            transform.SetParent(npc, true);
        }

        //Look at center of npc
        if (npc.GetComponent<Collider>())
        {
            LookAt = npc.GetComponent<Collider>().bounds.center;
        }
    }    

    private void DUI_OnDialogueEnd(string name)
    {        
        transform.SetParent(originalParent); //Return to papa
        cam.enabled = false;
        firstTarget = true;        
    }

    private void OnDestroy()
    {
        Debug.Log("Thumb cam destroyed");
        DUI.OnDialogueEnd -= DUI_OnDialogueEnd;
    }
}
