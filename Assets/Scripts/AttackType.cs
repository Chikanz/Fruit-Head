using System;
using UnityEngine;

/// <summary>
/// Defines what type of attack is being inflicted.
/// NOTE: New magic damage types MUST be added to the Magic flag. Problems will
/// occur if this isn't done.
/// </summary>
[Flags]
public enum AttackType
{
	/// <summary>
	/// No attack type. Used for defense specifications.
	/// </summary>
	[Tooltip("No attack type.")]
	None 		= 0,

	/// <summary>
	/// Damage from getting shot by an arrow.
	/// </summary>
	[Tooltip("The physical damage of being hit with an arrow")]
	Arrow 		= 1 << 0,

	/// <summary>
	/// Damage cause by a nearby explosion.
	/// </summary>
	[Tooltip("The physical damage of being too close to an explosion")]
	Explosion 	= 1 << 1,

	/// <summary>
	/// Damage caused by being pushed.
	/// </summary>
	[Tooltip("The physical damage of being pushed by something")]
	Pushback 	= 1 << 2,

	/// <summary>
	/// Damage cause by an axe
	/// </summary>
	[Tooltip("The physical damage of being hit with an axe")]
	Axe			= 1 << 3,

	/// <summary>
	/// Damage cause by a club
	/// </summary>
	[Tooltip("The physical damage of being hit with a club")]
	Club		= 1 << 4,

	/// <summary>
	/// Damage cause by poop? Who throws poop?
	/// </summary>
	[Tooltip("The physical damage- seriously, who throws poop?")]
	Poo			= 1 << 5,

	/// <summary>
	/// Damage caused by flames
	/// </summary>
	[Tooltip("The magical damage of being burnt")]
	Fire 		= 1 << 6,

	/// <summary>
	/// Damage caused by static discharge
	/// </summary>
	[Tooltip("The magical damage of being shocked")]
	Lightning 	= 1 << 7,

	/// <summary>
	/// Damage cause by chilled water
	/// </summary>
	[Tooltip("The magical damage of being blizzard'ed")]
	Ice			= 1 << 8,

	/// <summary>
	/// Damage caused by the very ground itself!
	/// </summary>
	[Tooltip("The magical damage of being earthquaked")]
	Earth		= 1 << 9,

	/// <summary>
	/// Damage caused by any form of magic
	/// </summary>
	[Tooltip("The magic damage of magic")]
	Magic		= Fire | Lightning | Ice | Earth,

    /// <summary>
    /// Sent when player scores a crit, usually via arrow headshot
    /// </summary>
    [Tooltip("Critical Hit!")]
    Critical = 1 << 10,
}