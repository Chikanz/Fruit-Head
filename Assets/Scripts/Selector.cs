﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    private int selected = 0;
    private int childCount;
    Vector3 targetPos;
    Image arrow;
    Transform Selected;

    public GameObject triggerOnEnabled;

    bool active = false;

	// Use this for initialization
	void Start ()
    {
        targetPos = transform.position;
        arrow = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        active = triggerOnEnabled.activeInHierarchy;

        //I wish this was event driven but unity's UI EventSystem is hot garbage
        if (active)
        {
            //See how many buttons are active
            if (childCount == 0)
                childCount = transform.parent.GetComponentsInChildren<Button>().Length;

            arrow.enabled = true;
            if (Input.GetKeyDown(KeyCode.W)) selected -= 1;
            else if (Input.GetKeyDown(KeyCode.S)) selected += 1;            

            selected = Mathf.Clamp(selected, 0, childCount - 1);

            //Get Y of selected button
            Selected = transform.parent.GetChild(selected);
            targetPos = new Vector3(transform.position.x, Selected.position.y, transform.position.z);

            //Lerp there reeeeeeeeeeel smoooooooooth
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);

            //Select the button
            if (Input.GetKeyDown(KeyCode.Space))
                Selected.GetComponent<Button>().onClick.Invoke();
        }
        else
        {
            childCount = 0;
            arrow.enabled = false;            
        }
    }
}