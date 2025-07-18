using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTargeting : MonoBehaviour
{
    //instead of raycasts, a trigger is used

    public List<GameObject> targetObjects;

    private GameObject currentTarget;

    private FireParticle fire;

    //binded to the player since this script is on a camera separated from the player
    private BindedObject playerBind;

    private KeyCode bindKey = KeyCode.E;
    private KeyCode secondaryBindKey = KeyCode.Mouse1;

    //billboarding target to visually indicate which enemy has been targeted. must have a bindedobject script attached
    public GameObject targetBillboard;

    public AudioSource source;
    public AudioClip clip;
    private float volume;

    void Awake(){
        playerBind = GetComponent<BindedObject>();
        fire = playerBind.bind.GetComponent<FireParticle>();
        volume = Variables.sfxVolume;
    }
    void OnTriggerEnter(Collider other){
        GameObject target = other.gameObject;
        if(other.CompareTag("enemy")){
            if(other.transform.root!=null){
                target = other.transform.root.gameObject;
            }
            targetObjects.Add(target);
        }
    }
    void OnTriggerExit(Collider other){
        GameObject target = other.gameObject;
        if(other.CompareTag("enemy")){
            if(other.transform.root!=null){
                target = other.transform.root.gameObject;
            }
            targetObjects.Remove(target);
        }
    }

    private GameObject getClosestTarget(){
        List<GameObject> objs = targetObjects;
        if(objs.Count==0){return null;}
    
        float lowestDist = 0;
        int lowestIndex = 0;
        for(int i=0; i<objs.Count;i++){
            GameObject tar = objs[i];
            if(tar==null){targetObjects.RemoveAt(i);continue;}
            float dist = Vector3.Distance(tar.transform.position, playerBind.bind.transform.position);

            if(dist<=lowestDist){
                lowestDist = dist;
                lowestIndex = i;
            }
        }
        return objs[lowestIndex];
        
    }

    private void SetTarget(GameObject target){
        fire.publicenemynumber1 = target;
        targetBillboard.GetComponent<BindedObject>().bind = target;

        if(!targetBillboard.activeSelf){
            targetBillboard.SetActive(true);
            if(source != null) source.PlayOneShot(clip,volume);
        }
    }

    void Update(){
        if(Input.GetKeyDown(bindKey)||Input.GetKeyDown(secondaryBindKey)){
            SetTarget(getClosestTarget());
        }
    }
}
