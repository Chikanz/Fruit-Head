using UnityEngine;

using System;

/// <summary>
/// BaseHealth is the base class for managing the health of a GameObject. Intercepts
/// "Attack" messages to recieve damage. Sends "Defeated" message on death. Child
/// classes can overwrite the Attack method to allow for changes in how damage is 
/// recieved.
/// </summary>
public class BaseHealth : MonoBehaviour
{
    // Static Constants
    /// <summary>
    /// The multiplier for being strong against a certain type of attack.
    /// </summary>        
    public const float STRONG_DEFENSE = 0.5f;

    /// <summary>
    /// The multiplier for being weak against a certain type of attack.
    /// </summary>        
    public const float WEAK_DEFENSE = 2.0f;

    /// <summary>
    /// The multiplier for a critical hit
    /// </summary>        
    public const float CRITICAL = 2.0f;

    // Properties

    /// <summary>
    /// How much health this unit has.
    /// </summary>
    public int Health
    {
        get { return hp; }
        private set
        {
            if (value < 0)
            {
                value = 0;
            }

            // Call our notifier
            HealthChanged(hp, value);
            hp = value;
        }
    }

    public int Reward
    {
        get { return reward; }
        set
        {
            if (value == reward)
            {
                return;
            }

            reward = value;
        }
    }

    // Fields
    /// <summary>
    /// How much damage this GameObject can tke before it is defeated
    /// </summary>
    [SerializeField]
    private int hp = 100;

    private int _maxHealth;

    /// <summary>
    /// How long, in seconds, will this game object remaind after it is defeated?
    /// </summary>
    [SerializeField]
    private float lingerTimer = 5.0f;

    /// <summary>
    /// Value in gold for killing this unit
    /// </summary>
    [SerializeField]
    private int reward = 100;

    /// <summary>
    /// What kind of damage is this unit immune to?
    /// </summary>
    [SerializeField]
    private AttackType immunity = AttackType.None;

    /// <summary>
    /// What kind of damage is this unit strong against?
    /// </summary>
    [SerializeField]
    private AttackType strongAgainst = AttackType.None;

    /// <summary>
    /// What kind of damage is this unit weak against?
    /// </summary>
    [SerializeField]
    private AttackType weakAgainst = AttackType.None;

    /// <summary>
    /// Have we sent the defeated event?
    /// </summary>
    protected bool hasSentDefeated = false;

    /// <summary>
    /// The attack type of the final blow
    /// </summary>
    protected AttackType finalBlow;


    /// <summary>
    /// The contact point the enemy was hit at (not always sent!)
    /// </summary>
    protected Vector3 FatalHitPoint;

    /// <summary>
    /// Check for necromancer to stop reviving skeletons
    /// </summary>
    [HideInInspector]
    public bool WasRevived = false;

    // Events
    public event EventHandler DefeatedEventHandler;
    public event EventHandler OnHurt;

    //Impact stuff
    public enum EHitParticles
    {
        NONE,
        BLOOD,
        SPARK,
    }

    private readonly string[] ResourceNames =
    {
            "Hit Blood",
            "Hit Sparks"
        };

    /// <summary>
    /// Flags for other classes to work out what particles to spawn on hit and death
    /// </summary>
    public EHitParticles ParticlesOnHit = EHitParticles.NONE;
    public EHitParticles ParticlesOnDefeat = EHitParticles.NONE;

    static GameObject[] ImpactList = new GameObject[3];


    // Unity Methods
    public virtual void Awake()
    {
        // hp must be positive!
        if (Health <= 0)
        {
            // Set it to the default of 100
            Health = 100;
        }

        _maxHealth = hp;

        // lingerTimer must be positive, too!
        if (lingerTimer < 0.0f)
        {
            // Set it to the default of 5.0f
            lingerTimer = 5.0f;
        }

        //load in impacts from resources if not already
        if (!ImpactList[0])
        {
            for (int i = 0; i < ResourceNames.Length; i++)
            {
                ImpactList[i + 1] = (GameObject)Resources.Load(ResourceNames[i]);
            }
        }
    }

    public virtual void Update()
    {
        // Really, we only do stuff here if the linger timer has activated
        if (Health <= 0)
        {
            // We're dead, are we still lingering?
            if (lingerTimer > 0.0f)
            {
                // Count down
                lingerTimer -= Time.deltaTime;
            }
            else
            {
                // We've waited long enough, destroy it
                Destroy(gameObject);
            }
        }
    }

    public void OnDestroy()
    {
        // Were we destroyed without telling anyone?
        if (!hasSentDefeated)
        {
            // Fire the defeated event
            EventHandler hand = DefeatedEventHandler;

            if (hand != null)
            {
                hand(this, EventArgs.Empty);
            }
        }
    }

    // Unity Message Methods
    /// <summary>
    /// Recieves the Attack message. Changes to the attack formula should be done
    /// in the DamageModifier methods.
    /// </summary>
    /// <param name="theAttack">The packet containing all the needed attack
    /// information.</param>
    public virtual void Attack(object attackMess)
    {
        // Sanity check: You can't hurt something if it's dead
        if (isDead()) return;

        // First, make sure that we got the right argument
        if (!(attackMess is AttackMessageArgument))
        {
            // Post it in the log, but do nothing
            Debug.Log(this + " received Attack message with unsupported type.");
        }

        // Let's make it easy to call
        AttackMessageArgument theAttack = attackMess as AttackMessageArgument;

        // Let's see how hard this actually hits us
        int damage;

        // Are we immune?
        if ((immunity & theAttack.DamageType) != 0)
        {
            // We're immune, no damage!
            damage = 0;
        }

        // Are we strong to this?
        else if ((strongAgainst & theAttack.DamageType) != 0)
        {
            // Reduced damage!
            damage = (int)(theAttack.DamageStrength * STRONG_DEFENSE);
        }
        else if ((weakAgainst & theAttack.DamageType) != 0)
        {
            // Oh no, increased damage!
            damage = (int)(theAttack.DamageStrength * WEAK_DEFENSE);
        }

        // None of those apply?
        else
        {
            // Normal damage
            damage = theAttack.DamageStrength;
        }

        //Boost damage if it's a crit!
        if ((AttackType.Critical & theAttack.DamageType) != 0)
        {
            damage = (int)(damage * CRITICAL);
        }

        // Finally, open up a hook for subclasses to change things
        damage = DamageModifier(damage);

        //Cache hit point
        FatalHitPoint = theAttack.hitPoint;

        //can't take damage
        if (damage == 0) return;

        // Call the take damage method             
        TakeDamage(damage, theAttack);
    }

    /// <summary>
    /// Fires the DefeateEvent
    /// </summary>
    private void OnDefeatEvent()
    {
        // Check that the handler is valid
        EventHandler hand = DefeatedEventHandler;

        if (hand != null)
        {
            hand(this, EventArgs.Empty);
        }

        // We've now sent the defeated message
        hasSentDefeated = true;

        // Add money to the PlayerMoney singleton.
        //PlayerMoney.AddMoney(reward);

        // Show reward
        //if (GetComponent<ShowNumberPopups>())
        //transform.GetComponent<ShowNumberPopups>().ShowNewPopup(true, false, reward);
        //else
        //  Debug.Log("No Show number popups attached to this object!");
    }

    // Helper Methods
    /// <summary>
    /// Handles the actual work of the Attack message.
    /// </summary>
    /// <param name="damage">How much damage this unit sustained</param>
    private void TakeDamage(int damage, AttackMessageArgument args)
    {
        bool wasCrit = ((AttackType.Critical & args.DamageType) != 0);

        Health -= damage;

        //EnemySoundManager soundMan;
        //soundMan = GetComponent<EnemySoundManager>();

        //Send out hurt message            
        EventHandler hand = OnHurt;

        if (hand != null)
        {
            hand(this, EventArgs.Empty);
        }

        // Check for defeat.
        if (Health <= 0)
        {
            finalBlow = args.DamageType;
            OnDefeat();
            //if (soundMan) soundMan.PlayAudioHurt(true);

            //Spawn hit impact
            if (args.hitPoint != null)
                SpawnImpact(args.hitPoint, ParticlesOnDefeat);
        }
        else  // If not defeated, then show damage popup.
        {
            //if(GetComponent<ShowNumberPopups>())
            // GetComponent<ShowNumberPopups>().ShowNewPopup(false, wasCrit ,damage);

            //Play the song of our people
            //if (soundMan) soundMan.PlayAudioHurt(false);

            //Spawn hit dead impact
            if (args.hitPoint != null)
                SpawnImpact(args.hitPoint, ParticlesOnHit);
        }
    }

    // Virtual Methods
    /// <summary>
    /// This method is called everytime the Health of this GameObject changes. Can be
    /// overwritten in child classes if some functionality needs to be roped into
    /// these change events. Currently does nothing.
    /// </summary>
    /// <param name="oldHP">Health before change</param>
    /// <param name="newHP">Health after change</param>
    protected virtual void HealthChanged(int oldHP, int newHP)
    {

    }

    /// <summary>
    /// Called when this unit has been defeated. Overwritable.
    /// </summary>
    protected virtual void OnDefeat()
    {
        // Send the message to the other components, we don't care if anyone hears
        SendMessage("Defeated", SendMessageOptions.DontRequireReceiver);

        // And fire the event
        OnDefeatEvent();
    }

    /// <summary>
    /// Modifies the damage that could be done to this unit. Called after all other 
    /// modifiers, this method handles class specific changes to the damage formula.
    /// </summary>
    /// <returns>The modifier.</returns>
    /// <param name="damage">Damage.</param>
    protected virtual int DamageModifier(int damage)
    {
        return damage;
    }

    /// <summary>
    /// Heal the object
    /// </summary>
    public virtual void IncreaseHealth(int increaseAmount)
    {
        var healthToAdd = increaseAmount;
        if (Health + increaseAmount > _maxHealth)
        {
            healthToAdd = _maxHealth - Health;
        }

        Health += healthToAdd;
    }

    public bool isDead()
    {
        return Health <= 0;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    private void SpawnImpact(Vector3 pos, EHitParticles eHit)
    {
        if (eHit == EHitParticles.NONE) return; //Can't spawn no particles!

        GameObject obj = null;

        obj = SpawnImpactObject(eHit, isDead());

        ////Position impact
        //obj.transform.position = pos;
        //obj.transform.rotation = Quaternion.LookRotation(transform.root.forward); //Hmmm, feel like this should be replaced. 

        ////Maintain scale + set parent
        //var scale = obj.transform.lossyScale;
        //obj.transform.parent = transform.FindDeepChild("Chesticle");
        //obj.transform.localScale = scale;
    }

    /// <summary>
    /// Returns the desired hit impact that isn't positioned
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="hit"></param>
    /// <returns></returns>
    protected virtual GameObject SpawnImpactObject(EHitParticles hit, bool isDead)
    {
        Debug.Assert(hit != EHitParticles.NONE);

        var obj = Instantiate(ImpactList[(int)hit]); //Spawn the desired impact

        ////Fine tuning
        //switch(hit)
        //{
        //    //Make spark a little more legible
        //    case EHitParticles.SPARK:
        //        obj.transform.localScale *= 1.5f;
        //        break;

        //    //Make blood slightly shorter on non-fatal hit
        //    case EHitParticles.BLOOD:
        //        if (!isDead)
        //        {
        //            var ps = obj.GetComponent<ParticleSystem>();
        //            ps.Stop(); // Cannot set duration whilst particle system is playing
        //            var main = ps.main;
        //            main.duration = 0.2f;
        //            ps.Play();
        //        }
        //        break;
        //}

        return obj;
    }

    [ContextMenu("Kill")]
    public void Kill()
    {
        SendMessage("Attack", new AttackMessageArgument(999, AttackType.Arrow, gameObject), SendMessageOptions.DontRequireReceiver);
    }
}

