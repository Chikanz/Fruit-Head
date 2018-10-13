using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Timers;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// legal guardian of the party
/// Spawn in Party and enemies
/// 
/// </summary>
public class CombatManager : MonoBehaviour
{
	public float StartTimer = 3;

	public static CombatManager instance { get; private set; }

	public static event EventHandler OnCombatStart;

	private Text TimerText;
	private bool timerOn = true;

	public GameObject CinemachineFreelook;

	public GameObject HealthUIObj;
	private List<GameObject> spawnedUIList = new List<GameObject>();

	//Passed in through combat loader
	[HideInInspector] public List<GameObject> Party = new List<GameObject>();
	[HideInInspector] public List<GameObject> Enemies = new List<GameObject>();

	private List<GameObject> SpawnedParty = new List<GameObject>();
	
	private Transform EnemyOrigin;
	private Transform PartyOrigin;

	public Color HealthGood;
	public Color HealthBad;
	
	

	public void Awake()
	{
		if (!instance)
			instance = this;
		else
		{
			Destroy(gameObject);
		}
	}
	
	
	public void Start()
	{		
		TimerText = GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
		
		//Get trans
		EnemyOrigin = transform.GetChild(1);
		PartyOrigin = transform.GetChild(0);
		
		//Feed into enemy man
		int alternate = 0;
		for (int i = 0; i < Enemies.Count; i++)
		{
			//Add enemy
			var obj = Enemies[i];
			var spawnedEnemy = EnemyManager.instance.AddEnemy(obj);

			var p = GetAlternatingPosition(EnemyOrigin.position, i, ref alternate);
			spawnedEnemy.transform.position = p; //Set to alternating pos
			spawnedEnemy.transform.rotation = EnemyOrigin.rotation;
		}
		
		//Spawn our HEROS
		alternate = 0; //ugh
		var uiRoot = transform.GetComponentInChildren<Canvas>().transform.Find("Party Health");
		for (int i = 0; i < Party.Count; i++)
		{
			//Spawn in at the right pos
			var p = GetAlternatingPosition(PartyOrigin.position, i, ref alternate);
			var g = Instantiate(Party[i], p, PartyOrigin.rotation);
			SpawnedParty.Add(g);
			
			//Refresh UI on hit
			g.GetComponent<CombatCharacter>().OnHurt += (sender, args) => RefreshUI();

			//Add associated UI
			var ui = Instantiate(HealthUIObj, uiRoot);			
			ui.GetComponent<RectTransform>().localPosition = new Vector3(0, i * 20,0);
			ui.GetComponent<RectTransform>().localScale = Vector3.one;
			spawnedUIList.Add(ui);			
		}

		//Generat
		RefreshUI();
		
		//Create camera + set ctg on charlie
		var fl = Instantiate(CinemachineFreelook, transform.position + Vector3.up * 10, Quaternion.identity);
		SpawnedParty[0].GetComponent<Targeter>().CTG = fl.GetComponentInChildren<CinemachineTargetGroup>();
		fl.GetComponentInChildren<CinemachineFreeLook>().Follow = SpawnedParty[0].transform.Find("Cam Point");
		
		//Set up targets
		for (int i = 0; i < Enemies.Count; i++)
		{
			EnemyManager.instance.Enemies[i].GetComponent<StateController>().Target = SpawnedParty[Random.Range(0, Party.Count)].transform;
		}
		
	}

	private void RefreshUI()
	{
		for (int i = 0; i < SpawnedParty.Count; i++)
		{
			var party = SpawnedParty[i].GetComponent<CombatCharacter>();
			var ui = spawnedUIList[i];

			//fill in text
			var text = ui.GetComponentsInChildren<Text>();
			text[0].text = party.friendlyName;
			text[1].text = "lvl 1"; //Don't have stats yet (will we ever lol)
			
			//health value + color
			var slider = ui.GetComponentInChildren<Slider>();
			slider.maxValue = party.MaxHealth;
			slider.value = party.Health;
			slider.image.color = Color.Lerp(HealthBad, HealthGood, party.Health / party.MaxHealth);
		}
	}

	private Vector3 GetAlternatingPosition(Vector3 origin, int i, ref int alternate)
	{	
		var x = i % 2 == 0 ? alternate : -alternate;
		if (i % 2 == 0) alternate++;
		var pos = origin;
		pos.x += x;
		return pos;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (StartTimer <= 0 && timerOn)
		{
			if (OnCombatStart != null) OnCombatStart(this, null);
			Invoke("end", 1);
			TimerText.text = "Go!";
			timerOn = false;
		}
		else if(StartTimer > 0 )
		{
			StartTimer -= Time.deltaTime;
			TimerText.text = Mathf.CeilToInt(StartTimer).ToString();
		}
		
	}

	private void OnDestroy()
	{
		instance = null; //Not sure if we actually need this or the GC does it
	}

	//Turn off timer
	void end()
	{
		TimerText.enabled = false;
	}
}