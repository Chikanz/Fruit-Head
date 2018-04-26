using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An entity in the combat scene that has health and stats
/// </summary>
public class CombatCharacter : MonoBehaviour
{
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
            if (value < 0) //Can't be extra dead
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
    public GameObject[] Moves;
    private List<Move> _movelist = new List<Move>();
    private Transform _moveParent;

    int _isPerformingMove = -1; //Stores the move index currently being performed. -1 for no move

    // Unity Methods
    public virtual void Awake()
    {
        //Set max health
        MaxHealth = hp;

        //Spawn moves
        _moveParent = transform.GetChild(0);
        foreach (GameObject m in Moves)
        {
            var g = Instantiate(m, Vector3.zero, Quaternion.identity, _moveParent);
            var moveObj = g.GetComponent<Move>();
            Debug.Assert(moveObj, "No move component was found on " + m.name);
            _movelist.Add(moveObj);
        }
    }

    public virtual void Update()
    {

    }

    public void OnDestroy()
    {
        // Were we destroyed without telling anyone?
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
        AttackMessage attackMess = args as AttackMessage;

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

        //Perform knockback
        //todo

        //Filter damage through stats
        //todo        

        // Call the take damage method             
        TakeDamage(damage);
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
    /// Called when this unit has been defeated. Overwritable.
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
        Move m = _movelist[moveIndex];
        m.Init();
        _isPerformingMove = moveIndex;
    }

    /// <summary>
    /// Called by animation event, tells the current move to Execute;
    /// </summary>
    public void Hit()
    {
        _movelist[_isPerformingMove].Execute();
    }

    public bool IsPerformingMove()
    {
        return _isPerformingMove != -1;
    }

    //For quick debuggerinos
    [ContextMenu("Kill")]
    public void Kill()
    {        
        SendMessage("Attack", new AttackMessage(9999, this.gameObject, 0), SendMessageOptions.DontRequireReceiver);
    }
}
