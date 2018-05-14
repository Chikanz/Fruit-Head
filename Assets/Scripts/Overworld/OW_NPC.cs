using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_NPC : OW_Character
{
    private Quaternion targetRot;
    private Quaternion normalRot;
    private Transform lookAt;
    static GameObject thumbCam;

    public float lookSpeed = 0.1f;
    public bool LookAtPlayer = true;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        normalRot = transform.rotation;

        if (!thumbCam)
        {
            thumbCam = GameObject.Find("ThumbCam");
            thumbCam.SetActive(false);
        }

        //((DialougeUI)_DR.dialogueUI).OnDialougeStart += OW_NPC_OnDialougeStart;
        ((DialougeUI)_DR.dialogueUI).OnDialougeEnd += OW_NPC_OnDialougeEnd;
    }

    private void OW_NPC_OnDialougeEnd(string name)
    {
        if (!name.Contains(this.Name)) return; //only call for this NPC

        thumbCam.SetActive(false);
        targetRot = normalRot;
        lookAt = null;
        thumbCam.transform.SetParent(null);
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

        //Run dialouge
        _DR.StartDialogue(Name);

        //Setup thumb cam
        thumbCam.SetActive(true);
        thumbCam.transform.position = transform.position + (transform.forward * 1) + (transform.right * 1) + transform.up * 1.5f;
        thumbCam.transform.LookAt(transform.GetComponent<Collider>().bounds.center);
        thumbCam.transform.SetParent(transform, true);
    }
}
