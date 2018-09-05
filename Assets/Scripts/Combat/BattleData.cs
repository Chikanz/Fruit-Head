using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Data that tells the battle spawner what to spawn
[CreateAssetMenu (menuName = "BattleData")]
public class BattleData : ScriptableObject
{
	//Players in resource folder
	//If ticked here, scene changer loads in from resources and puts into combat man for spawning

	[Header("Party")] 
	public bool Avery;
	public bool Mason;
	public bool Eden;

	[Header("Enemy")] 
	public EnemyAmount[] Enemies;

	[Header("Scene")] 
	public int SceneToLoad;
}

[System.Serializable]
public struct EnemyAmount
{
	public GameObject Prefab;
	public int Amount;
}
