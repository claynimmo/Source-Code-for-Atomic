using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed = 5.0f;

    public Transform cameraTransform; // Assign the camera transform in the Inspector
    private Rigidbody rbody;

    public float maxdragvelocity = 10;

    public float orbitForce = 20;
    public TrailRenderer invertedTrail;

    public FireParticle weapon;

    public GameObject lightMode;

    //inputs
    float horizontal;
    float vertical;
    float space;
    bool orbit;
    float orbitInput;
    [HideInInspector] public Vector3 forward;

    public bool inverted;
    //keycodes are used as variables so they can be changed if needed
    KeyCode switchWeapon = KeyCode.R;
    KeyCode UseAbil = KeyCode.Q;


    [Header("AI variables")]
    public bool AiControlled;

    

    //speed changes with direction;
    [HideInInspector] public float randomSpeed;
    void Awake(){
        rbody = GetComponent<Rigidbody>();
        weapon = GetComponent<FireParticle>();
    }
    void Update(){
        if(!Variables.hardStop){
            GetInputs();
            if(inverted){
                if(!lightMode.activeSelf){
                    lightMode.SetActive(true);
                }
            }
            else{
                if(lightMode.activeSelf){
                    lightMode.SetActive(false);
                }
            }
        }
    }
    
    void FixedUpdate(){
        if(!AiControlled){
            forward = cameraTransform.forward;
        }

        forward = Vector3.Normalize(forward);

        if(inverted)forward.y = forward.y/2;
        Vector3 right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        Vector3 moveDirection = (horizontal * right + vertical * forward) * speed;
        if(inverted){
            if(orbit){
                rbody.drag = 0.2f;
                Vector3 direction = Vector3.zero;
                //error management for when the enemy is potentially killed
                if(weapon.publicenemynumber1!=null){
                    direction = weapon.publicenemynumber1.transform.position- transform.position;}
                rbody.AddForce(direction*rbody.mass*orbitForce);
                
            }
            else{
                rbody.AddForce(moveDirection*rbody.mass*3+transform.up*space*speed);
                rbody.drag = rbody.velocity.magnitude/maxdragvelocity;
                invertedTrail.emitting = true;
            }
        }
        else{
            rbody.AddForce(moveDirection*rbody.mass+transform.up*space*speed);
            rbody.drag = rbody.velocity.magnitude/maxdragvelocity;
            invertedTrail.emitting = false;
        }
    }
    

    void GetInputs(){
        if(AiControlled){
            horizontal = randomSpeed;
            return;
        }
        
        if(Variables.inputAble){
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            space = -Input.GetAxis("Jump")*2;
            orbit = Input.GetKey(KeyCode.Space);
            orbitInput = Input.GetAxis("pressingSpace");

            
            if(Input.GetKeyDown(KeyCode.CapsLock)){
                inverted = !inverted;
                ChangeAttack();
            }
            if(inverted){
                space = -space;
            }
            else{
                if(orbit){
                    space = orbitInput;
                }
            }

            if(Input.GetKeyDown(switchWeapon)){
                ChangeAttack();
            }
            if(Input.GetKeyDown(UseAbil)){
                UseAbility();
            }
        }
    }

    public void ChangeAttack(){
        weapon.ChangeAttack(inverted);
    }
    public void UseAbility(){
        weapon.UseAbility();
    }
}