using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTarget : BaseAbility
{

    //these variables are hidden since they are updated when the ability is used
    [HideInInspector] public bool dragging;

    [HideInInspector] public GameObject target;

    private Vector3 dir;

    public float dragForce;

    LineRenderer line;

    public AudioSource source;

    public float duration = 1;


    void Awake(){
        line = GetComponent<LineRenderer>();
    }

    void FixedUpdate(){
        if(dragging){
            if(target==null&&line==null){return;}
            dir = Vector3.Normalize(transform.position - target.transform.position);
            Rigidbody rbody = target.GetComponent<Rigidbody>();
            line.positionCount = 2;
            Vector3[] linePoints = new Vector3[2];
            linePoints[0] = transform.position;
            linePoints[1] = target.transform.position;
            line.SetPositions(linePoints);
            rbody.AddForce(dir*rbody.mass*dragForce);
        }
        else{
            //remove the line when it is not used
            line.positionCount = 0;
        }
    }

    public override void Activate(int amount){
        if(target == null){return;}
        dragging = true;
        Invoke("Deactivate",duration);
    }

    public override void SetVariables(GameObject target, float cooldown, float softCooldown){
        if(target == null){return;}
        this.duration = cooldown;
        this.target = target;
    }

    void Deactivate(){
        dragging = false;
    }
}
