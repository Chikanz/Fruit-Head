using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base move class. Moves should spawn a thing that sends the attack message
/// </summary>
public abstract class Move : MonoBehaviour
{
    [SerializeField]
    private int Damage;

    [SerializeField]
    private float CoolDown;

    [SerializeField]
    private string AnimationTriggerName;

    public abstract void Exec();
}
