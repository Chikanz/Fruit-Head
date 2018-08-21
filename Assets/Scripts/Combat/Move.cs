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
    private int Damage;

    [SerializeField]
    private float CoolDown;
    private float _coolDownTimer;

    public string AnimationTriggerName;

    [SerializeField]
    private bool _canMoveWhileUsing;

    [SerializeField]
    [Tooltip("projectile to spawn")]
    private GameObject Projectile;

    [SerializeField]
    private bool isProjectile;

    [SerializeField]
    [Tooltip("How long the attack object survives for")]
    private float lingerTime;

    [SerializeField]
    [Tooltip("Should we destroy this hitbox after it hits an enemy?")]
    private bool DestroyOnHit;

    [Tooltip("if no, this class will w8 for an animation event to trigger is. Ticking this will fire the move immediately")]
    public bool fireImmediate = false;

    private GameObject ActiveObject; //The hitbox/projectile spawned

    private Animator _myAnim;

    [HideInInspector]
    public Transform daddy;

    private bool _isSubscribed;

    [Range(0,500)]
    public int Knockback;

    private void Start()
    {
        daddy = transform.parent.parent;
        _myAnim = daddy.GetComponent<Animator>();

        ToggleChild(false);

        //set hitbox if we've got one
        //if (transform.childCount == 1)
        //{
        //    hitbox = transform.GetChild(0).gameObject;
        //    hitbox.SetActive(false);
        //}
    }

    private void Update()
    {
        //Coundown timer
        if (_coolDownTimer >= 0)
            _coolDownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Called when the move is executed
    /// </summary>
    public virtual void Init()
    {
        if (_coolDownTimer > 0)
        {
            Debug.Log("Move is still on cooldown!");
            return;
        }

        //Set cooldown
        _coolDownTimer = CoolDown;

        //Set the trigger for the animation
        if(_myAnim)
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
        //Spawn new or enable child
        if (isProjectile)
        {
            ActiveObject = Instantiate(Projectile, Vector3.zero, Quaternion.identity, transform);
            Destroy(ActiveObject, lingerTime); //Destroy hitbox after linger timer
        }
        else
        {
            ToggleChild(true); //enable child
            StartCoroutine(DelayToggleChild(lingerTime, false)); //disable child on timer
            ActiveObject = transform.GetChild(0).gameObject; //set active object to child
        }

        //Subscribe to hitbox event if it's lonely
        if (!_isSubscribed)
        {
            ActiveObject.GetComponent<Hitbox>().OnHit += Trigger;
            _isSubscribed = true;
        }
    }

    /// <summary>
    /// Called by the hitbox when the attack lands
    /// </summary>
    public virtual void Trigger(GameObject enemy)
    {
        enemy.GetComponentInParent<CombatCharacter>().Attack(new AttackMessage(Damage, daddy.gameObject, Knockback));

        if (DestroyOnHit)
        {
            if (isProjectile)
            {
                Destroy(ActiveObject);
                _isSubscribed = false;
            }
            else
            {
                ToggleChild(false);
            }
        }    
    }

    /// <summary>
    /// Getter because unity doesn't allow serialization of properties
    /// </summary>
    public bool CanMoveAndUse()
    {
        return _canMoveWhileUsing;
    }

    private IEnumerator DelayToggleChild(float timer, bool enabled)
    {
        yield return new WaitForSeconds(timer);
        ToggleChild(enabled);
    }

    private void ToggleChild(bool enabled)
    {
        transform.GetChild(0).gameObject.SetActive(enabled);
    }
}
