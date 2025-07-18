using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITargeting : MonoBehaviour
{
    //instead of raycasts, a trigger is used

    public List<GameObject> targetObjects;

    private GameObject currentTarget;

    public FireParticle fire;

    public string tagName = "Player";

    void OnTriggerEnter(Collider other){
        GameObject target = other.gameObject;
        if(other.CompareTag(tagName)){
            if(other.transform.root!=null){
                target = other.transform.root.gameObject;
            }
            targetObjects.Add(target);
        }
    }
    void OnTriggerExit(Collider other){
        GameObject target = other.gameObject;
        if(other.CompareTag(tagName)){
            if(other.transform.root!=null){
                target = other.transform.root.gameObject;
            }
            targetObjects.Remove(target);
        }
    }

    private GameObject getClosestTarget(){
        List<GameObject> objs = targetObjects;
        if(objs.Count==0){
            return null;
        }
        else{
            float lowestDist = 0;
            int lowestIndex = 0;
            for(int i=0; i<objs.Count;i++){
                GameObject tar = objs[i];
                if(tar==null){targetObjects.RemoveAt(i);return getClosestTarget();}
                float dist = Vector3.Distance(tar.transform.position, transform.position);

                //if distances are the same, <= results in the target selected last, < gives the target that was detected first. flip sign to instead target the furthest enemy
                if(dist<=lowestDist){
                    lowestDist = dist;
                    lowestIndex = i;
                }
            }
            return objs[lowestIndex];
        }
    }

    private void SetTarget(GameObject tar){
        fire.publicenemynumber1 = tar;
    }

    void Update(){
        GameObject tar = getClosestTarget();
        if(tar!=null){
            SetTarget(tar);
        }
    }
}
