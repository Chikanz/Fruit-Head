﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCman : MonoBehaviour
{
    public static NPCman instance;

    Dictionary<string, OW_Character> Phonebook = new Dictionary<string, OW_Character>();

    void Awake ()
    {
        //Enforce singleton
        if (!instance) instance = this;
        else Destroy(gameObject);

        //Fill NPC list
        foreach (OW_Character n in GetComponentsInChildren<OW_Character>())
        {
            AddCharacter(n);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AddCharacter(OW_Character n)
    {
        if(!Phonebook.ContainsKey(n.Name))
        {
            Phonebook.Add(n.Name, n);
        }
    }

    public OW_Character GetCharacter(string name)
    {
        OW_Character n;
        Phonebook.TryGetValue(name, out n);
        if (n == null) Debug.Log("The NPC " + name + " was not found! Check to make sure they're a child of NPC man " +
                                 "and the spelling of their name is correct");
        return n;
    }
}
