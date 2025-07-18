using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectTimer : MonoBehaviour
{

    public float deathTimer = 1;
    void Awake(){
        Destroy(gameObject,deathTimer);
    }
}
