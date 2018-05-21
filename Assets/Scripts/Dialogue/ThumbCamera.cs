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

    bool firstLookat = true;

	// Use this for initialization
	void Start ()
    {
        DUI = GetComponentInParent<DialogueUI>();

        DUI.OnDialogueStart += DUI_OnDialogueStart;
        DUI.OnTargetChanged += DUI_OnTargetChanged;
        DUI.OnDialogueEnd += DUI_OnDialogueEnd;

        cam = GetComponent<Camera>();
        cam.enabled = false;       
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
        }
    }

    private void DUI_OnDialogueStart(string name)
    {
        //Pan in from player position
        //transform.position = player.transform.position + (Vector3.up * 2);
        transform.rotation = Quaternion.Euler(player.transform.right);

        cam.enabled = true;
        firstLookat = true;
    }

    private void DUI_OnTargetChanged(string name)
    {
        Debug.Log(name);
        var npc = NPCman.instance.GetCharacter(name).transform;

        //Stay in one position 
        if (firstLookat)
        {
            firstLookat = false;
            transform.position = npc.position + (npc.forward * 1) + (npc.right * 1) + npc.up * 1.5f;
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
        transform.SetParent(null);
        cam.enabled = false;
    }
}
