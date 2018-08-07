using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// These are the base stats that are multiplied by the enemy's level 
/// </summary>
[CreateAssetMenu (menuName = "PluggableAI/EnemyStats")]
public class EnemyStats : ScriptableObject {

	public float moveSpeed = 1;
	public float lookSphereCastRadius = 1f;

	public float attackRange = 1f;
	public float attackRate = 1f;
	public int attackDamage = 50;
}