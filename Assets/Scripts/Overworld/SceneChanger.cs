using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

/// <summary>
/// Singleton responsible for changing scenes
/// </summary>
/// 
using System;
using UnityStandardAssets.Characters.ThirdPerson;

public class SceneChanger: MonoBehaviour
{
    // Use this for initialization
    bool isOverWorld = true;

    public static SceneChanger instance;

    public event EventHandler OnSceneChange;

    void Start ()
    {
        //Enforce singleton
        if (!instance) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("BattleChairs");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene("Overworld");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("BattleFlies");
        }
    }

    [YarnCommand("Change")]
    public void Change(string scene)
    {
        //Trigger Events
        if (OnSceneChange != null) OnSceneChange(this,null);

        //Make dialouge events run + clean up
        GetComponentInChildren<DialogueRunner>().Stop();

        //Change scene
        var sceneIndex = Int32.Parse(scene);
        SceneManager.LoadScene(sceneIndex);

        Invoke("SetPlayer", 0.5f); //AAAAAAAAAAAAAAAAAAAAAAAA
    }

    void SetPlayer()
    {
        //Get the player
        var player = NPCman.instance.GetCharacter("Charlie");
        if (!player) return; //DA FUCK
        GetComponentInChildren<DialogueUI>().playerControl = player.GetComponent<ThirdPersonUserControl>();  //yikes
    }

    public void Change(int scene)
    {
        Change(scene.ToString()); //yum yum
    }
}
