﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using System;
using Luminosity.IO;
using UnityEngine.Experimental.UIElements;
using UnityStandardAssets.Characters.ThirdPerson;
using Image = UnityEngine.UI.Image;

/// <summary>
/// Singleton responsible for changing scenes
/// </summary>
/// 
public class SceneChanger: MonoBehaviour
{        
    public static SceneChanger instance;    

    public delegate void SceneChange(int scene); //Fired when the scene is changed

    public delegate void SceneLoaded();

    public event SceneChange OnSceneChange;
    public event SceneChange OnSceneLoaded;

    public event EventHandler OnGameReset;
    //use SceneManager.sceneLoaded for scene loaded event

    [HideInInspector]
    public Vector3 RememberPlayerPos;

    private BattleData BD;
    private Image[] LoadingUIList;

    private int precombatSceneCache;
    
    public static GameObject Yarn { get; private set; }

    private float ResetTimer;
   
    void Awake ()
    {
        //Enforce singleton
        if (!instance) instance = this;
        else Destroy(gameObject);

        if(!Yarn) Yarn = transform.GetChild(0).gameObject;

        DontDestroyOnLoad(gameObject);

        //Get all kiddy images
        transform.GetChild(1).gameObject.SetActive(true); //Make sure loading screen is active
        LoadingUIList = GetComponentInChildren<Canvas>().GetComponentsInChildren<Image>();
        LoadingFade(0); //Turn make loading screen transparent
    }

    private void Start()
    {
        //For debug really, if scene change code is written well this should do nothing
        if (OnSceneChange != null) OnSceneChange(SceneManager.GetActiveScene().buildIndex);
        if (OnSceneLoaded != null) OnSceneLoaded(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
	void Update ()
	{
	    //Add timer when pressed
	    ResetTimer = InputManager.GetButton("Start") ? ResetTimer + Time.deltaTime : 0;
	    if (ResetTimer > 5) ResetGame();
	}

    [YarnCommand("Change")]
    public void Change(string scene)
    {           
        //Trigger Events        
        var sceneIndex = int.Parse(scene);
        if (OnSceneChange != null) OnSceneChange(sceneIndex);
        
        //Make dialouge events run + clean up
        GetComponentInChildren<DialogueRunner>().Stop();

        //Change scene        
        StartCoroutine(LoadSceneAsync(sceneIndex, 0.25f));

        var player = GetComponentInChildren<DialogueUI>().playerControl;

        //todo: replace for a solution that works for all scenes
        //save position in OW scene
        //pass previous scene to enemy manager
        //pop stack/cache of player pos
        if (sceneIndex == 4 && player)
        {
            RememberPlayerPos = GameObject.FindWithTag("Player").transform.position;
            Debug.Log("set player pos");
        }

        if (sceneIndex == 5 && RememberPlayerPos != Vector3.zero && player)
        {
            Invoke("pp", 0.1f);
        }

        //since we're using a different player now just null the other ref and let it set itself
        GetComponentInChildren<DialogueUI>().playerControl = null;  
    }

    IEnumerator LoadSceneAsync(int index, float fadeTime)
    {
        //fade in loading screen first so it doesn't eat shit when the scene loads
        var elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            LoadingFade(elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        LoadingFade(1); //Avoid screen still being around on low frame rates  
        elapsedTime = 0;

        // Wait here until the asynchronous scene fully loads
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //Send out event
        if (OnSceneLoaded != null) OnSceneLoaded(index);

        //Fade out loading screen
        while (elapsedTime < fadeTime)
        {
            LoadingFade(1 - (elapsedTime / fadeTime)); //Reversi
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        LoadingFade(0);
    }

    void LoadingFade(float alpha)
    {
        foreach (Image img in LoadingUIList)
        {
            img.color = new Color(img.color.r,img.color.g,img.color.b, alpha);
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
        Debug.Assert(BD != null, "Couldn't find a battle with that name, try again? owo");
        StartCombat(BD);
    }

    //Load in a combat scene
    public void StartCombat(BattleData data)
    {
        BD = data;
        //Subby to scene loaded event
        SceneManager.sceneLoaded += CombatSceneLoaded;
        
        precombatSceneCache = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("scene " + precombatSceneCache);
        
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
        
        //sanity check
        foreach (GameObject p in cm.Party)
        {
            Debug.Assert(p != null);
        }
        
        SceneManager.sceneLoaded -= CombatSceneLoaded; //unsubby event
    }

    //Load the last scene we were in 
    public void EndCombat()
    {
        Debug.Assert(precombatSceneCache != -1);
        Change(precombatSceneCache);
        precombatSceneCache = -1;
    }

    public GameObject GetYarn()
    {
        return transform.GetChild(0).gameObject;
    }

    private void ResetGame()
    {
        //Fire off reset event
        if (OnGameReset != null) OnGameReset(this,null);
        
        ResetTimer = 0;
        Yarn.GetComponent<ExampleVariableStorage>().ResetToDefaults();
        Change(0);
    }
}


















