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

    [HideInInspector]
    public Vector3 RememberPlayerPos;

    private BattleData BD;

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

        //hmm this could be done better
        if (sceneIndex == 4)
        {
            RememberPlayerPos = GameObject.FindWithTag("Player").transform.position;
            Debug.Log("set player pos");
        }

        if (sceneIndex == 5 && RememberPlayerPos != Vector3.zero)
        {
            Invoke("pp", 0.1f);
        }
    }

    //player position
    void pp()
    {
        GameObject.FindWithTag("Player").transform.position = RememberPlayerPos;
        //var cam = GameObject.Find("CM vcam1").transform;
        //cam.position = new Vector3(RememberPlayerPos.x, cam.position.y ,RememberPlayerPos.z);
        Debug.Log("read player pos");
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

    //Load in a combat scene from yarn
    [YarnCommand("Combat")]
    public void StartCombat(string resourceName)
    {
        //Load in battle data
        BD = Resources.Load<BattleData>(resourceName);
        Debug.Assert(BD != null);
        StartCombat(BD);
    }

    //Load in a combat scene
    public void StartCombat(BattleData data)
    {
        BD = data;
        //Subby to scene loaded event
        SceneManager.sceneLoaded += CombatSceneLoaded;
        
        //Then load combat scene
        Change(BD.SceneToLoad);
    }

    //Called when a combat scene is finished loading
    private void CombatSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Should only be a combat scene we just loaded
        Debug.Assert(scene.buildIndex == BD.SceneToLoad);
        
        //Load in enemy data into combat man
        CombatManager cm = GameObject.Find("Combat Manager").GetComponent<CombatManager>();
        foreach (var e in BD.Enemies)
        {
            for (int i = 0; i < e.Amount; i++)
            {
                cm.Enemies.Add(e.Prefab);
            }
        }
        
        //Party
        cm.Party.Add(Resources.Load<GameObject>("CharlieCombat"));
        if(BD.Avery)
            cm.Party.Add(Resources.Load<GameObject>("AveryCombat"));
        if(BD.Eden)
            cm.Party.Add(Resources.Load<GameObject>("EdenCombat"));
        if(BD.Mason)
            cm.Party.Add(Resources.Load<GameObject>("MasonCombat"));
        
        SceneManager.sceneLoaded -= CombatSceneLoaded; //unsubby event
    }
}


















