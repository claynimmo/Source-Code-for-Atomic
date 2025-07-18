using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticField : MonoBehaviour
{
    public float magnetForce = 100;
    public bool sucking=true;

    List<Rigidbody> caughtRigidbodies = new List<Rigidbody>();

    void FixedUpdate(){
        if(!sucking || caughtRigidbodies.Count == 0){return;}
        for(int i = 0; i < caughtRigidbodies.Count; i++){
            if(caughtRigidbodies[i]==null){caughtRigidbodies.Remove(caughtRigidbodies[i]);break;}
            Vector3 forceDirection = transform.position - (caughtRigidbodies[i].transform.position + caughtRigidbodies[i].centerOfMass);
            caughtRigidbodies[i].AddForce(forceDirection.normalized*magnetForce*caughtRigidbodies[i].mass);
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.GetComponent<Rigidbody>()){
            Rigidbody r = other.GetComponent<Rigidbody>();

            if(!caughtRigidbodies.Contains(r)){
                //Add Rigidbody
                caughtRigidbodies.Add(r);
            }
        }
    }

    void OnTriggerExit(Collider other){
        if(other.GetComponent<Rigidbody>()){
            Rigidbody r = other.GetComponent<Rigidbody>();

            if(caughtRigidbodies.Contains(r)){
                caughtRigidbodies.Remove(r);
            }
        }
    }
}