using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightExplosion : BaseAbility
{
    public float duration = 2f;
    public bool active;
    private float currentduration;

    public GameObject prefab;

    void Update(){
        if(!active){return;}

        currentduration += Time.deltaTime;
        if(currentduration>=duration){
            Deactivate();
        }
    }

    public override void Activate(int abilityCount){
        currentduration = 0;
        Instantiate(prefab,transform.position,Quaternion.Euler(Vector3.zero));
    }
    
    public void Deactivate(){
        currentduration = 0;
    }
}
