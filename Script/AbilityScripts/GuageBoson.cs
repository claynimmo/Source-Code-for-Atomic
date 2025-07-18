using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GuageBoson : BaseAbility
{

    FireParticle attackCode;
    HealthScript health;

    [HideInInspector] public bool poweredUp;


    //defined through script, uses the same variable names as in the fireparticle script for easier use
    [HideInInspector] public float cooldown; //cooldown period after buff is depleted
    [HideInInspector] public float softCooldown; //duration of the buff

    private bool onCoolDown = false;
    private bool onCoolDown2 = false;

    //variables to manage the changing of buffs
    private float currentBuff = 0;

    private float currentAbilityTime = 0;


    public GameObject defenceBuffEffects;
    public float defenceModifyer = 1.75f;

    public GameObject rechargeBuffEffects;

    //materials for the orbitting leptons
    public Material nuetrinoTrailMaterial;
    public Material nuetrinoParticleMaterial;
    private Material standardParticleMaterial;
    private Material standardTrailMaterial;

    public Image bosonFill;
    //colours to visually show the current boson, indexs are: 0 = gluon, 1 = photon, 2 = W/Z 
    public Color[] barColours;


    public float newRechargeSpeed = 4;

    //recharge speed is an array since the individual electrons may have different recharge times, in which a single float reverts all the electrons to the same time
    private float[] defaultRechargeSpeed;

    private bool currentCooldown = false;
    private float fcurrentCooldown = 0;

    public AudioSource source;
    public AudioClip clip;
    private float volume;

    // Start is called before the first frame update
    void Awake(){
        volume = Variables.sfxVolume;
        attackCode = GetComponent<FireParticle>();
        health = GetComponent<HealthScript>();
        if(attackCode!=null){
            standardParticleMaterial = attackCode.orbit_electron[0].gameObject.GetComponent<Renderer>().material;
            standardTrailMaterial = attackCode.orbit_electron[0].gameObject.GetComponent<TrailRenderer>().material;
            defaultRechargeSpeed = new float[attackCode.orbit_electron.Length];
            for(int i = 0; i<attackCode.orbit_electron.Length; i++){
                defaultRechargeSpeed[i] = attackCode.orbit_electron[i].RechargeSpeed;
            }
        }
    }

    // Update is called once per frame
    void Update(){
        if(poweredUp){
            currentAbilityTime += Time.deltaTime;
            float fillAmount = (1/softCooldown) * (softCooldown - currentAbilityTime);
            if(bosonFill!=null){
                bosonFill.fillAmount = fillAmount;
            }
            if(currentAbilityTime>=softCooldown){
                StopBuffs(true);
            }
        }
        else if(bosonFill!=null){
            bosonFill.color = Color.white;
        }

        if(currentCooldown){
            fcurrentCooldown += 1*Time.deltaTime;
            if(bosonFill!=null){
                float fll = 1 - ( (1/cooldown) * (cooldown - fcurrentCooldown) );
                bosonFill.fillAmount = fll;
            }
            if(fcurrentCooldown >= cooldown){
                AbilityCooldown();
            }
        }
    }

    public void ActivateBuff(){
        if(!onCoolDown2&&!currentCooldown){
            onCoolDown2 = true;
            poweredUp = true;
            currentAbilityTime = 0;
            currentBuff = 0;
            ChangeBuff();
        }
    }

    public void ChangeBuff(){
        //oncooldown2 is not checked here so that the buff can be change while active, but not reseting the buff duration
        if(!onCoolDown){
            onCoolDown = true;
            //cooldown between changing attacks so that it does not change to quickly
            Invoke("ResetCooldown",0.3f);
            StopBuffs(false);
            currentBuff += 1;
            switch(currentBuff){
                case 1:
                    Buff1_Defence();
                    break;
                case 2:
                    Buff2_ElectronCooldown();
                    break;
                case 3:
                    Buff3_Attack();
                    break;
                default:
                    currentBuff = 1;
                    Buff1_Defence();
                    break;
            }
            source.PlayOneShot(clip,volume);
        }
    }

    //gluon: exchange of gluons cause strong nuclear force, binding the atoms together and hence increases defence with a slight regenerative property
    void Buff1_Defence(){
        defenceBuffEffects.SetActive(true);
        health.defaultDefence = defenceModifyer;
        health.regenerating = true;

        if(bosonFill!=null&&barColours[0]!=null){
            bosonFill.color = barColours[0];
        }
        
    }
    //photon: mediates the electromagnetic force.
    //therefore, converting to gameplay the photons would increase the recharge speed of the electrons and builds resistance to the ionising radiation beam
    void Buff2_ElectronCooldown(){
        rechargeBuffEffects.SetActive(true);
        for(int i=0;i<attackCode.orbit_electron.Length; i++){
            int randomNumber = Random.Range(0,100);
            //50 percent chance of recharging particle instantly
            if(randomNumber <= 50){
                attackCode.orbit_electron[i].turn_back_on();
            }
            else{
                attackCode.orbit_electron[i].Invoke("turn_back_on",newRechargeSpeed);
            }
            attackCode.orbit_electron[i].RechargeSpeed = newRechargeSpeed;
        }
        if(bosonFill!=null&&barColours[1]!=null){
            bosonFill.color = barColours[1];
        }
    }

    //W and Z bosons: act between quarks (particles in the nucleus) and leptons (the electrons orbiting the particle), mediating the weak nuclear force
    //these bosons mediate neutrino emissions, and thus nuclear decay.
    //converting this into gameplay, the electrons are emitted more violently and occasionally expells a positron (antielectron) causing higher bullet speed and more damage
    void Buff3_Attack(){
        //change colours of the orbitting leptons
        for(int i=0; i<attackCode.orbit_electron.Length; i++){
            attackCode.orbit_electron[i].gameObject.GetComponent<Renderer>().material = nuetrinoParticleMaterial;
            attackCode.orbit_electron[i].gameObject.GetComponent<TrailRenderer>().material = nuetrinoTrailMaterial;
        }
        attackCode.boostedAttack=true;
        if(bosonFill!=null&&barColours[2]!=null){
            bosonFill.color = barColours[2];
        }
    }

    void StopBuffs(bool end){
        //stop defence buffs
        defenceBuffEffects.SetActive(false);
        rechargeBuffEffects.SetActive(false);
        health.regenerating = false;
        health.defaultDefence = 1;

        //stop attack and cooldown buffs
        for(int i=0; i<attackCode.orbit_electron.Length; i++){
            attackCode.orbit_electron[i].gameObject.GetComponent<Renderer>().material = standardParticleMaterial;
            attackCode.orbit_electron[i].gameObject.GetComponent<TrailRenderer>().material = standardTrailMaterial;
            attackCode.orbit_electron[i].RechargeSpeed = defaultRechargeSpeed[i];
        }
        attackCode.boostedAttack=false;

        if(end){
            onCoolDown2 = false;
            currentAbilityTime = 0;
            poweredUp = false;
            //cooldown to activate another buff
            currentCooldown = true;
        }
        
    }


    void ResetCooldown(){
        onCoolDown = false;
    }

    void AbilityCooldown(){
        onCoolDown2 = false;
        onCoolDown = false;
        currentCooldown = false;
        fcurrentCooldown = 0;

        //reset the current buff for consistency when activating the ability again
        currentBuff = 0;
    }

    public override void Activate(int abilityCount){
        if(!poweredUp){
            ActivateBuff();
        }
        else{
            ChangeBuff();
        }
    }

    public override void SetVariables(GameObject target, float cooldown, float softCooldown){
        this.cooldown = cooldown;
        this.softCooldown = softCooldown;
    }

}
