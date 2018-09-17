using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base move class. Moves should spawn a thing (or have one childed) that triggers after a condition, and that condition performs a callback here
/// Only projectiles are spawned, hitboxes should be already childed
/// </summary>
public class Move : MonoBehaviour
{
    [SerializeField]
    protected int Damage;

    [SerializeField]
    protected float CoolDown;
    protected float _coolDownTimer;

    public string AnimationTriggerName;
    
    protected Animator _myAnim;
    
    [Tooltip("if no, this class will w8 for an animation event to trigger is. Ticking this will fire the move immediately")]
    public bool fireImmediate = false;

    public Transform daddy { private set; get; } //our root character combat script

    [SerializeField]
    private bool _canMoveWhileUsing;

    protected virtual void Start()
    {
        daddy = transform.parent.parent;
        _myAnim = daddy.GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        //Coundown timer
        if (_coolDownTimer >= 0)
            _coolDownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Called when the move is first executed, can't be overriden since it performs checks for all moves
    /// </summary>
    public void Init()
    {
        if (_coolDownTimer > 0)
        {
            Debug.Log("Move is still on cooldown!");
            return;
        }

        //Set cooldown (on every move???)
        _coolDownTimer = CoolDown;

        //Set the trigger for the animation
        if(_myAnim && AnimationTriggerName != "")
            _myAnim.SetTrigger(AnimationTriggerName);
        
        //Fire now if that's a thing we're doing 
        if(fireImmediate)
            Execute();
    }

    /// <summary>
    /// Spawns the hitbox/projectile whatever
    /// </summary>
    public virtual void Execute()
    {

    }

    /// <summary>
    /// Called by the hitbox when the attack lands
    /// </summary>
    public virtual void Trigger(GameObject enemy)
    {
        
    }

    /// <summary>
    /// Getter because unity doesn't allow serialization of properties
    /// </summary>
    public bool CanMoveAndUse()
    {
        return _canMoveWhileUsing;
    }
}
