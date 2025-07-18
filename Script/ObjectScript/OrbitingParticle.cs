using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingParticle : MonoBehaviour
{
    //Assign a GameObject in the Inspector to rotate around
    public GameObject target;
    public Vector3 direction;
    public float angle;

    private bool turnedOff;
    private bool turnedOff_fire_once;
    public float RechargeSpeed = 10;

    private Renderer renderer;

    void Awake(){
        RechargeSpeed *= Variables.electronRechardModifyer;
        renderer = GetComponent<Renderer>();
    }

    void Update(){

        transform.RotateAround(target.transform.position, direction, angle * Time.deltaTime);

        if(turnedOff){
            renderer.enabled = false;
            if(!turnedOff_fire_once){
                turnedOff_fire_once = true;
                Invoke("turn_back_on",RechargeSpeed);
            }
        }
        else{
            renderer.enabled = true;
            if(turnedOff_fire_once){
                turnedOff_fire_once = false;
            }
        }
    }

    public void turn_back_on(){
        turnedOff = false;
        GetComponent<TrailRenderer>().emitting = true;
    }
    public void turn_off(){
        turnedOff = true;
        GetComponent<TrailRenderer>().emitting = false;
    }
    public bool CheckOffStatus(){
        return turnedOff;
    }
}