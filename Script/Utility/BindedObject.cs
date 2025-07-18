using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindedObject : MonoBehaviour
{
    //gameobject to bind towards so that their scripts can be accessed through the object this script is attached to
    public GameObject bind;

    public bool stick = false;
    void Update(){
        if(bind!=null){
            if(stick){
                transform.position = bind.transform.position;
            }
        }
        //if the binded object does not exist, hide the object from the scene
        else{
            if(gameObject.activeSelf){
                gameObject.SetActive(false);
            }
        }
    }
}
