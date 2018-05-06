using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object that is sent on attack to deliver info
/// </summary>
public class AttackMessage
{
	public int DamageStrength { get; private set; }

    public GameObject Attacker { get; private set; }

    public int KnockBack { get; private set; } //Knockback from move

    public float CritChance { get; private set; }

    /// <summary>
    /// Creates a new attack message
    /// </summary>
    /// <param name="Damage">Amount of damage</param>
    /// <param name="HurtBy">Who is sending the attack message? (e.g. bullet, Player)</param>
    /// <param name="KnockBack">How far to send this bad boy back</param>
    public AttackMessage(int Damage, GameObject HurtBy, int KnockBack)
    {
        DamageStrength = Damage;
        Attacker = HurtBy;
        this.KnockBack = KnockBack;
    }

}
