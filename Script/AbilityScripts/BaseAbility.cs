using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    public abstract void Activate(int abilityCount);
    public virtual void SetVariables(GameObject target, float cooldown, float softCooldown){return;}
}
