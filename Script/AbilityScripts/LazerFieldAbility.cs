using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerFieldAbility : BaseAbility
{

    public GameObject lightPrefab;

    public override void Activate(int abilityCount){
        GameObject light = Instantiate(lightPrefab,this.gameObject.transform.position, Quaternion.Euler(Vector3.zero));
        light.GetComponent<lazerField>().player = this.gameObject;
    }
}
