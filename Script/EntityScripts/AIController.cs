using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private movement movementMan;


    //a random value is taken from these two values to activate an effect when the tick is
    //a larger difference causes more randomness
    [Range(0.00f,20.00f)]
    public float randomTickMax = 10;
    [Range(0.00f,20.00f)]
    public float randomTickMin = 5;

    //different ticks are used for the different functions so they run independently to eachother, but still randomly
    private float shootTick;
    private float swapModeTick;
    private float useAbilityTick;

    //random floats taken from the tick max and min to decide when an ability should be used
    private float randomShoot;
    private float randomSwap;
    private float randomUse;

    //a float to determine the movement bias towards the targeted object, so the enemy generally moves towards the player
    //values of 0 give no bias, values > 1 prefference the random direction over the player direction, values < 1 preffence the player direction. values of 1 use standard vector addition to give bias
    [Range(0.00f,2.00f)]
    public float moveBias=0.5f;

    //public modifyers to multiply with the times. values larger than 1 increases the time for when the function is used
    [Range(0,5)]
    public float shootTickModifyer = 1;
    [Range(0,5)]
    public float swapModeTickModifyer = 3;
    [Range(0,5)]
    public float useAbilityTickModifyer = 2;

    void Awake(){
        movementMan = GetComponent<movement>();

        randomTickMin = Variables.AI_Tick_Min_Variance;
        randomTickMax = Variables.AI_Tick_Max_Variance;
        if(Variables.randomiseTickModifyers){
            shootTickModifyer = Random.Range(0,1f);
            swapModeTickModifyer = Random.Range(3.5f,5);
            useAbilityTickModifyer = Random.Range(0.1f,1f);
        }
        else{
            shootTickModifyer = Variables.shootTickModifyer;
            swapModeTickModifyer = Variables.swapModeTickModifyer;
            useAbilityTickModifyer = Variables.useAbilityTickModifyer;
        }
        InvokeRepeating("GetForwardDirection",0,2f);
        randomShoot = Random.Range(randomTickMin,randomTickMax)*shootTickModifyer;
        randomSwap = Random.Range(randomTickMin,randomTickMax)*swapModeTickModifyer;
        randomUse = Random.Range(randomTickMin,randomTickMax)*useAbilityTickModifyer;
    }

    void Update(){
        if(Variables.hardStop){return;}
        GetTicks();
    }


    void RandomShoot(){
        randomShoot = Random.Range(randomTickMin,randomTickMax)*shootTickModifyer;

        //determine which effect to shoot based on the mode
        if(movementMan.inverted){
            movementMan.weapon.FireLazer();
        }
        else{
            movementMan.weapon.Fire(movementMan.weapon.publicenemynumber1);
        }
    }
    void RandomSwap(){
        randomSwap = Random.Range(randomTickMin,randomTickMax)*swapModeTickModifyer;
        movementMan.inverted = !movementMan.inverted;
        //run the change attack function to change the ability to the correct swap mode
        movementMan.ChangeAttack();
        previousAttack = -1;
    }

    private float previousAttack=-1;
    void RandomUseAbility(){
        randomUse = Random.Range(randomTickMin,randomTickMax)*useAbilityTickModifyer;
        int changeAmount = Random.Range(0,3); //randomly change attack by 1 or 2 so that there is more variety in abilities used
        //ensure there is variation
        if(changeAmount==previousAttack){
            changeAmount = Random.Range(0,3);
        }
        for(int i=0; i<changeAmount;i++){
            movementMan.ChangeAttack();
        }
        previousAttack = changeAmount;
        movementMan.UseAbility();
    }

    Vector3 RandomForward(){
        float x = 0;
        float y = 0;
        float z = 0;
        //make the AI more likely to move in the opposite direction so it does not tend to move in only one direction
        if(movementMan.forward.x>0){
            x = Random.Range(-10,5);
        }
        else{
            x = Random.Range(-5,10);
        }
        if(movementMan.forward.z>0){
            z = Random.Range(-10,5);
        }
        else{
            z = Random.Range(-5,10);
        }
        if(movementMan.forward.y>0){
            y = Random.Range(-10,5);
        }
        else{
            y = Random.Range(-5,10);
        }
        Vector3 dir = new Vector3(x,y,z);
        Vector3 bias = Vector3.zero;
        dir = Vector3.Normalize(dir);
        if(movementMan.weapon.publicenemynumber1!=null){
            //get direction of target relative to this object
            Vector3 targetDirRel = Vector3.Normalize(movementMan.weapon.publicenemynumber1.transform.position - this.transform.position);
            //get the direction between the current random forward and the target position through vector addition
            bias = dir + targetDirRel;
            //multiply the vector by the bias, then normalize once again
            bias = Vector3.Normalize(new Vector3(bias.x*moveBias,bias.y*moveBias,bias.z*moveBias));
        }
        dir = Vector3.Normalize(dir + bias);
        return dir;
    }
    void GetForwardDirection(){
        movementMan.forward = RandomForward();
        movementMan.randomSpeed = Random.Range(0.3f,1);
    }

    void GetTicks(){
        shootTick += Time.deltaTime;
        swapModeTick += Time.deltaTime;
        useAbilityTick += Time.deltaTime;

        //error prevention algorithm to cap the tick rate, reseting to 0 when it is above 10
        shootTick %= 10 * shootTickModifyer;
        swapModeTick %= 10 * swapModeTickModifyer;
        useAbilityTick %= 10 * useAbilityTickModifyer;

        if(shootTick>=randomShoot){
            RandomShoot();
            shootTick = 0;
        }
        if(swapModeTick>=randomSwap){
            RandomSwap();
            swapModeTick = 0;
        }
        if(useAbilityTick>=randomUse){
            RandomUseAbility();
            useAbilityTick = 0;
        }
    }
}
