using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conduit : BaseAbility
{

    public GameObject conduitPrefab;

    private GameObject[] conduits = new GameObject[6];

    public override void SetVariables(GameObject target, float cooldown, float softCooldown){
        return;
    }

    public override void Activate(int abilityCount){

        if(conduits[abilityCount]!=null){Destroy(conduits[abilityCount]);}

        conduits[abilityCount] = Instantiate(conduitPrefab,transform.position,Quaternion.Euler(Vector3.zero));
        
        //aim initial conduit at self
        if(abilityCount==1){
            conduits[abilityCount].GetComponent<FireParticle>().publicenemynumber1 = this.gameObject;
            return;
        }

        //aim subsequent conduits at the previous, forming a firing net
        conduits[abilityCount].GetComponent<FireParticle>().publicenemynumber1 = conduits[abilityCount-1];
    }


}
