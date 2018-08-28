using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// For now just delay the battle start
/// in future we should spawn in enemies + party members
/// </summary>
public class CombatManager : MonoBehaviour
{
	public float StartTimer = 3;

	public static CombatManager instance { get; private set; }

	public static event EventHandler OnCombatStart;

	private Text TimerText;
		
	// Use this for initialization
	void Start ()
	{
		if (!instance)
			instance = this;
		else
		{
			Destroy(gameObject);
		}
		
		TimerText = transform.GetChild(0).GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (StartTimer <= 0)
		{
			if (OnCombatStart != null) OnCombatStart(this, null);
			Invoke("end", 1);
			TimerText.text = "Go!";
		}
		else
		{
			StartTimer -= Time.deltaTime;
			TimerText.text = Mathf.CeilToInt(StartTimer).ToString();
		}
		
	}

	void end()
	{
		gameObject.SetActive(false);
	}
}
