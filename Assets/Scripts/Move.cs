using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base move class. Moves should spawn a thing that sends the attack message
/// </summary>
public class Move : MonoBehaviour
{
    [SerializeField]
    private int Damage;

    [SerializeField]
    private float CoolDown;
    private float _coolDownTimer;

    [SerializeField]
    private string AnimationTriggerName;

    [SerializeField]
    private bool _canMoveWhileUsing;

    [SerializeField]
    [Tooltip("thing to spawn")]
    private GameObject hitbox;

    [SerializeField]
    [Tooltip("How long the attack object survives for")]
    private float lingerTime;

    [SerializeField]
    [Tooltip("Should we destroy this hitbox after it hits an enemy?")]
    private bool DestroyOnHit;

    private GameObject SpawnedObj;

    private Animator _myAnim;

    private Transform _daddy;

    private void Start()
    {
        _daddy = transform.parent.parent;
        _myAnim = _daddy.GetComponent<Animator>();

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
        if (_coolDownTimer > 0)
            _coolDownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Called when the move is first executed
    /// </summary>
    public virtual void Init()
    {
        if (_coolDownTimer > 0)
        {
            Debug.Log("Move is still on cooldown!");
            return;
        }

        //Set the trigger for the animation
        _myAnim.SetTrigger(AnimationTriggerName);
    }

    /// <summary>
    /// Called by the hitbox when the attack lands
    /// </summary>
    public void Trigger(GameObject enemy)
    {
        enemy.SendMessage("Attack", new AttackMessage(Damage, _daddy.gameObject, 0));

        if (DestroyOnHit)
            Destroy(SpawnedObj);
    }

    /// <summary>
    /// Spawns the hitbox/projectile whatever
    /// </summary>
    public virtual void Execute()
    {
        SpawnedObj = Instantiate(hitbox, Vector3.zero, Quaternion.identity, transform);
        Destroy(SpawnedObj, lingerTime);
    }

    /// <summary>
    /// Getter because unity doesn't allow serialization of properties
    /// </summary>
    public bool CanMoveAndUse()
    {
        return _canMoveWhileUsing;
    }
}
