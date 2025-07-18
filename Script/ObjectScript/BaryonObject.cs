using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaryonObject : MonoBehaviour
{
    public MagneticField mag;
    public ParticleSystemForceField field;
    public ParticleSystemForceField field2;
    public float growSpeed = 0.3f;
    private float totalGrowSpeed;


    public float BaryonPullForce =1;

    public float BaryonCharge = 1;

    public float BaryonSize=1;
    public float lifetime = 9;
    // Start is called before the first frame update
    void Awake()
    {
        mag.sucking = false;
        Invoke(nameof(StartSucking),2);
        
        transform.localScale = new Vector3(0.01f,0.01f,0.01f);
    }
    float x;
    bool imploding;
    // Update is called once per frame
    void Update()
    {
        
        if(transform.localScale.x<=BaryonSize&&!imploding){
            x+=totalGrowSpeed*Time.deltaTime;
            transform.localScale = new Vector3(x,x,x);
        }

        if(imploding){
            x -=totalGrowSpeed*2*Time.deltaTime;
            transform.localScale = new Vector3(x,x,x);
            if(x<=0.1f){
                Destroy(gameObject);
            }
        }
    }
    void StartSucking(){
        mag.sucking = true;
        mag.magnetForce *= BaryonCharge*BaryonPullForce;
        totalGrowSpeed = growSpeed*BaryonSize;
        if(BaryonCharge==1){
            field.gameObject.SetActive(true);
            field2.gameObject.SetActive(false);
        }
        else{
            field.gameObject.SetActive(false);
            field2.gameObject.SetActive(true);
        }
        Invoke(nameof(implode),lifetime);
    }
    void implode(){
        imploding = true;
        x = BaryonSize;
    }
   
}
