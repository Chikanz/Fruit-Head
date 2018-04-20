using UnityEngine;
using System.Collections;


/// <summary>
/// Used as the argument for an Attack message. Contains information
/// regarding an attack performed on a GameObject by another.
/// </summary>
public class AttackMessageArgument : Object
{
	// Properties

	/// <summary>
	/// How strong of an attack this is.
	/// </summary>
	public int DamageStrength { get; private set; }

	/// <summary>
	/// What kind of damage is does this attack do?
	/// </summary>
	public AttackType DamageType { get; private set; }

	/// <summary>
	/// Who attacked us?
	/// </summary>
	public GameObject Attacker { get; private set; }


    /// <summary>
    /// Where we got hit (optional)
    /// </summary>
    public Vector3 hitPoint;

    /// <summary>
    /// Override hit particles
    /// </summary>
    public BaseHealth.EHitParticles forceHit;

    // Contructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AttackMessageArgument"/> class.
    /// </summary>
    /// <param name="attStrength">Strength of the attack</param>
    /// <param name="attType">Type of the attack</param>
    /// <param name="attFrom">Who did this?</param>
    public AttackMessageArgument(int attStrength, AttackType attType, GameObject attFrom)
	{
		DamageStrength = attStrength;
		DamageType = attType;
		Attacker = attFrom;
	}


    //Override for hit position
    public AttackMessageArgument(int attStrength, AttackType attType, GameObject attFrom, Vector3 hitpos)
    {
        DamageStrength = attStrength;
        DamageType = attType;
        Attacker = attFrom;
        hitPoint = new Vector3(hitpos.x,hitpos.y,hitpos.z);
    }


    //Override for force hit particles + position
    public AttackMessageArgument(int attStrength, AttackType attType, GameObject attFrom, Vector3 hitpos, BaseHealth.EHitParticles forceParticles)
    {
        DamageStrength = attStrength;
        DamageType = attType;
        Attacker = attFrom;
        hitPoint = new Vector3(hitpos.x, hitpos.y, hitpos.z);
        forceHit = forceParticles;
    }
}

