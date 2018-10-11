using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will start combat when a player enters 
/// </summary>
public class CombatTrigger : MonoBehaviour
{
	private string _key;

	private static Dictionary<int, bool> activeMask = new Dictionary<int, bool>();
	
	public BattleData BattleData;

	private int Num;
	
	// Use this for initialization
	void Start ()
	{
		Num = gameObject.GetInstanceID(); //set number to a unique number
		
		if (!activeMask.ContainsKey(Num))
		{
			activeMask.Add(Num, true);
		}

		bool state;
		if(activeMask.TryGetValue(Num, out state))
		
		transform.parent.gameObject.SetActive(state);
		
		Debug.Assert(BattleData, "No battle data found you dum dum");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.transform.tag.Equals("Player")) return;

		activeMask[Num] = false; //updat mask desu
		
		SceneChanger.instance.StartCombat(BattleData);
	}
}
