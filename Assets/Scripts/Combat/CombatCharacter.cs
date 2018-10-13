using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;
using UnityStandardAssets.Characters.ThirdPerson;

/// <summary>
/// An entity in the combat scene that has health and stats
/// Combat character:
//  Bounding box: physics bounds objects that should be at least as tall as the enemy, and a smol space that the player can't enter needs to be a capsule
//  Hit box: should match the enemy pretty closely and be on the "Hitbox" layer (9)
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CombatCharacter : MonoBehaviour
{
    public string friendlyName = "charleee";
    
    /// <summary>
    /// The multiplier for a critical hit
    /// </summary>        
    public const float CRITICAL = 2.0f;

    // Properties

    /// <summary>
    /// How much health this ABSOLUTE unit has.
    /// </summary>
    public int Health
    {
        get { return hp; }
        private set
        {
            if (value < 0) //Can't be extra dead (but I wish I could be ayyyyyyyyyooooo)
            {
                value = 0;
            }

            // Call our notifier
            HealthChanged(hp, value);
            hp = value;
        }
    }

    // Fields
    /// <summary>
    /// How much damage this GameObject can take before it is defeated
    /// </summary>
    [SerializeField]
    private int hp = 100;    

    public int MaxHealth { get; private set; }

    /// <summary>
    /// Have we sent the defeated event?
    /// </summary>
    protected bool hasSentDefeated = false;

    //Damage events
    public event EventHandler OnDefeat;
    public event EventHandler OnHurt;

    //This character's move set to spawn
    [FormerlySerializedAs("Moves")] public GameObject[] MovesToSpawn;
    public List<Move> Moves { get; private set; }
    private Transform _moveParent;

    private static GameObject HitCanvas;

    public bool Friendly;

    [Range(0.0f,1.0f)]
    public float weight;

    public enum HitParticles
    {
        NONE,
        SPARKS,
        COINS,
    }

    public HitParticles ParticlesOnHit;

    private readonly string[] resourceNames =
    {
        "",
        "Sparks",
        "Coins",
    };

    private static List<GameObject> Particles = new List<GameObject>();

    private Rigidbody _RB;

    int _isPerformingMove = -1; //Stores the move index currently being performed. -1 for no move

    // Unity Methods
    public virtual void Awake()
    {
        //Set max health
        MaxHealth = hp;

        //Get RB
        _RB = GetComponent<Rigidbody>();

        if(!HitCanvas)
        {
            HitCanvas = Resources.Load<GameObject>("HitCanvas");
        }

        //Load Particles if not loaded already
        if (Particles.Count == 0)
        {
            //https://stackoverflow.com/questions/856154/total-number-of-items-defined-in-an-enum
            var particles = Enum.GetNames(typeof(HitParticles)).Length; 
            for (int i = 0; i < particles; i++)
            {
                Particles.Add(Resources.Load<GameObject>(resourceNames[i]));
            }
        }

        //Spawn moves
        Moves = new List<Move>();
        if (MovesToSpawn.Length > 0)
        {
            _moveParent = transform.GetChild(0);
            foreach (GameObject m in MovesToSpawn)
            {
                var g = Instantiate(m, Vector3.zero, Quaternion.identity);
                g.transform.SetParent(_moveParent,false);
                g.transform.localRotation = Quaternion.identity;
                g.transform.localPosition = Vector3.zero;
                var moveObj = g.GetComponent<Move>();
                g.transform.localPosition = Vector3.zero;
                Debug.Assert(moveObj, "No move component was found on " + m.name);
                Moves.Add(moveObj);
            }
        }
        
        
        //Cheese player activation for now
        var uc = GetComponent<ThirdPersonUserControl>();
        if (uc) CombatManager.OnCombatStart += (sender, args) => uc.CanMove = true;
    }

    public virtual void Update()
    {

    }

    public void OnDestroy()
    {        
        if (!hasSentDefeated)
        {
            // Fire the defeated event
            EventHandler hand = OnDefeat;

            if (hand != null)
            {
                hand(this, EventArgs.Empty);
            }
        }
    }

    // Unity Message Methods
    /// <summary>
    /// Perform an attack on this character.
    /// Changes to the attack formula should be done in the DamageModifier methods.
    /// </summary>
    /// <param name="theAttack">The packet containing all the needed attack
    /// information.</param>
    public void Attack(object args)
    {
        //Sanity check
        if (IsDead()) return;

        // Check to make sure that we got the right argument
        Debug.Assert(args is AttackMessage, "Attack recieved message of wrong type! How the fuck did you manage that?");

        //Cast from object and read this bad boy
        AttackMessage attackMess = (AttackMessage) args;

        int damage = attackMess.DamageStrength;

        //Calc crit
        bool wasCrit = false;
        if (UnityEngine.Random.Range(0.0f, 1.0f) < attackMess.CritChance)
        {
            damage *= Mathf.RoundToInt(CRITICAL);
            wasCrit = true;
        }

        //Open up a hook for subclasses to change things
        damage = DamageModifier(damage);

        //Can't take 0 damage
        if (damage == 0) return;

        //Get rest of the message
        //todo

        //Perform knockback
        var dist = transform.position - attackMess.Attacker.transform.position;

        //Hit back and slightly up (factor in weight tho)
        var knockback = attackMess.KnockBack * (1 - weight) * _RB.mass;
        _RB.AddForce((dist.normalized * knockback) + (Vector3.up * knockback / 2));
        
        //Filter damage through stats
        //todo        

        Debug.Log(gameObject.name + " was hit for " + damage);

        //Play Particles at center of mesh
        if (ParticlesOnHit != HitParticles.NONE)
        {
            var p = Instantiate(Particles[(int)ParticlesOnHit], GetComponentInChildren  <Renderer>().bounds.center, Quaternion.identity);
            Destroy(p, 3);
        }

        //Spawn damage canvas
        CapsuleCollider cap = transform.GetChild(1).GetComponent<CapsuleCollider>();
        if(!cap) cap = GetComponent<CapsuleCollider>();
        if(!cap) cap = GetComponentInChildren<CapsuleCollider>();
        
        if (cap)
        {
            var h = Instantiate(HitCanvas);
            h.GetComponent<DamageCanvas>().NewHit(cap, attackMess.DamageStrength, wasCrit);
        }
        else
        {
            Debug.Log("Bounding box not found on " + gameObject.name + " Is it named \"Bounding Box\"? " +
                      "Also needs to be a capsule");
        }

        // Call the take damage method (should always be last since it sends out the onHurt Event)
        TakeDamage(damage);

        //StartCoroutine(Sleep(0.05f));
        //StartCoroutine(Wobble(0.98f));
    }

    // Helper Methods
    /// <summary>
    /// DIRECTLY attack this character
    /// </summary>
    /// <param name="damage">How much damage this unit sustained</param>
    private void TakeDamage(int damage)
    {
        //Actually subtract damage
        Health -= damage;

        //Send out hurt message            
        EventHandler hand = OnHurt;

        if (hand != null)
        {
            hand(this, EventArgs.Empty);
        }

        // Check for defeat.
        if (IsDead())
        {
            Defeated();
        }
    }

    // Virtual Methods
    /// <summary>
    /// This method is called everytime the Health of this GameObject changes. Can be
    /// overwritten in child classes if some functionality needs to be roped into
    /// these change events.
    /// </summary>
    /// <param name="oldHP">Health before change</param>
    /// <param name="newHP">Health after change</param>
    protected virtual void HealthChanged(int oldHP, int newHP)
    {

    }

    /// <summary>
    /// Called when this unit has been defeated.
    /// </summary>
    protected virtual void Defeated()
    {
        Debug.Assert(!hasSentDefeated, gameObject.name + "has already called OnDefeat!");

        // Check that the handler is valid
        EventHandler hand = OnDefeat;

        if (hand != null)
        {
            hand(this, EventArgs.Empty);
        }

        // We've now sent the defeated message
        hasSentDefeated = true;
        
        //DEBUG
        Destroy(gameObject);
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
    public virtual void Heal(int healAmount)
    {
        if (healAmount <= 0) return;

        var healthToAdd = healAmount;
        if (Health + healAmount > MaxHealth)
        {
            healthToAdd = MaxHealth - Health;
        }

        Health += healthToAdd;
    }

    public bool IsDead()
    {
        return Health <= 0;
    }

    public void UseMove(int moveIndex)
    {
        //Check if performing move
        if (IsPerformingMove()) return;

        Move m = Moves[moveIndex];
        m.Init(); //Init the move if cooldown is good
        _isPerformingMove = moveIndex;

        //Reset isPerforming after animation has finished
        //Assumes the animation name is the same as the trigger name
        if (m.AnimationTriggerName != "" || !m.fireImmediate)
        {
            var resetTime = 1f;
            if (GetComponent<Animator>())
                resetTime = DookTools.GetAnimationLength(GetComponent<Animator>(), m.AnimationTriggerName);
            Debug.Assert(resetTime > 0, "Couldn't find clip in animator, you probably have the wrong " +
                                        "AnimationTriggerName on the move prefab");
            Invoke("ResetMove", resetTime);
        }
        else
        {
            ResetMove();
        }
    }

    public void ResetMove()
    {
        _isPerformingMove = -1;
    }

    /// <summary>
    /// Called by animation event, tells the current move to Execute;
    /// </summary>
    public void Hit()
    {
        Moves[_isPerformingMove].Execute();
    }

    public bool IsPerformingMove()
    {
        return _isPerformingMove != -1;
    }

    public bool CanMove()
    {
        return (!IsPerformingMove() || Moves[_isPerformingMove].CanMoveAndUse());
    }

    private IEnumerator Sleep(float t)
    {
        Time.timeScale = 0.1f;
        var dt = Time.deltaTime;        
        yield return new WaitForSeconds(t * Time.timeScale);
        Time.timeScale = 1;        
    }

    private IEnumerator Wobble(float decay)
    {
        var elapsed = 0.0f;
        var wobbleForce = 0.5f;
        var originalScale = transform.localScale;
        float wobble = 1;
        while(wobble > 0.01f)
        {
            elapsed += Time.deltaTime;
            wobble = (float)Math.Sin(elapsed) * wobbleForce * decay;
            transform.localScale = originalScale * wobble;
            yield return null;
        }

        transform.localScale = originalScale;
    }

    //For quick debuggerinos
    [ContextMenu("Kill")]
    public void Kill()
    {        
        SendMessage("Attack", new AttackMessage(9999, this.gameObject, 0), SendMessageOptions.DontRequireReceiver);
    }
}
