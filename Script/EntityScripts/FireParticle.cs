using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireParticle : MonoBehaviour
{
    public OrbitingParticle[] orbit_electron;
    public GameObject electron_prefab;

    public GameObject electron_neutrino_prefab;
    public GameObject positron_prefab;

    public bool boostedAttack; //bool to determine whether to use the neutrino and positron shots

    private bool firingLazer;
    public float LazerAimTime = 5;
    //secondary bool to add a delay between firing the lazers, similar to the recharging of electrons
    private bool firingLazerCooldown;
    public float LazerCooldown = 4;

    public GameObject lazerEffect;

    [System.Serializable]
    public class abilities{

        public bool IgnoreAbilityCooldowns = false;
        
        public float cooldown;
        //cooldown between individual firing of the ability, given it can be used multiple times
        public float softCooldown;

        public int maxSummons;

        //determine which mode the ability can be accessed within
        public bool lightMode;
        public BaseAbility ability;

        public GameObject abilityIcon;

        public string loggValue;
        public Color loggColour;
        public AudioClip clip;

    }
    public abilities[] bils;

    //optional, but must be used with the abilityIcons in the abilities class
    public GameObject backgroundUI;

    [HideInInspector]
    public int currentbil;

    public bool Player;

    bool abilityuseable=true;
    bool abilityuseable2=true;
    //integer to determine the amount of times the ability is used for abilities with maxSummons > 1
    int currentUsedAbilities;
    
    //gameobject of the targeted enemy. can be defined via the aiming script on the player, acting as aim assist.
    public GameObject publicenemynumber1;

    //determine if the object is destroyd after firing used for single use turrets
    public bool dieOnShoot=false;
    public float dieCooldown=1;

    //used to obtain the 'inverted' value to determine the lightmode for deciding which attack to use
    movement MoveCode;

    //UI components, leave null if not required
    public GameObject lockEffect; //gameobject to visually showcase when there is a cooldown


    KeyCode fireInput = KeyCode.F;
    KeyCode fireInputSecondary = KeyCode.Mouse0;

    AbilityLogging logger;
    public Color logElecCol;
    public Color logLazerCol;
    public Color logNeutCol;
    public Color logPositCol;
    public Color logCondCol;
    
    public AudioSource source;
    public AudioSource abilitySource;
    public AudioClip[] fireSounds;
    public AudioClip lazerSound;

    private float volume;

    //awake is used over start incase the object is instantiated
    void Awake(){
        MoveCode = GetComponent<movement>();
        logger = GetComponent<AbilityLogging>();
        volume = Variables.sfxVolume;
    }
    // Update is called once per frame
    void Update(){

        if(Variables.hardStop){return;}

        if(publicenemynumber1==null){
            publicenemynumber1 = this.gameObject;
        }

        if((Input.GetKeyDown(fireInput)||Input.GetKeyDown(fireInputSecondary)) && Player && Variables.inputAble && !firingLazer){
            if(MoveCode!=null){
                if(MoveCode.inverted)
                    FireLazer();
                else Fire(publicenemynumber1);
            }
            else Fire(publicenemynumber1);
        }

        if(firingLazer&&!firingLazerCooldown){
            if(MoveCode!=null){
                if(!MoveCode.inverted){
                    lazer_timer = 100;
                }
            }
            AimLazer(publicenemynumber1);
        }
    }

    public void Fire(GameObject enemy){

        for(int i=0; i<orbit_electron.Length; i++){

            if(orbit_electron[i].CheckOffStatus()){continue;}

            SummonParticle(orbit_electron[i].gameObject.transform.position, enemy.transform.position);
            FireParticleSound();
            
            orbit_electron[i].turn_off();
            if(dieOnShoot){
                Destroy(this.gameObject,dieCooldown);
            }
            currentUsedAbilities = 0;
            
            break;
        }
        
    }
    public void FireParticleSound(){
        if(source == null){return;}

        AudioClip clip = fireSounds[(int)Random.Range(0,fireSounds.Length)];
        source.PlayOneShot(clip,volume);
    }

    //called through the health script, particularly from lazer attacks
    public void GainElectron(bool steal){

        for(int i=0; i<orbit_electron.Length; i++){
            if(orbit_electron[i].CheckOffStatus() && !steal){
                orbit_electron[i].turn_back_on();
                break;
            }
            else if(steal){
                orbit_electron[i].turn_off();
                break;
            }
        }
    }
    //start firing the light ray
    public void FireLazer(){
        firingLazer = true;
        source.PlayOneShot(lazerSound,volume*0.8f);
        if(logger!=null)
                logger.AppendLog("Radiation Shot",logLazerCol);
    }

    //constantly aim the ray at the targeted object for a set time until resetting
    private float lazer_timer = 0;
    public void AimLazer(GameObject enemy){

        if(lazerEffect.transform.parent!=this.transform)
            lazerEffect.transform.parent = this.transform;

        lazerEffect.transform.LookAt(enemy.transform.position);
        lazerEffect.transform.localPosition = new Vector3(0,0.5f,0);
        ParticleSystem effect = lazerEffect.GetComponent<ParticleSystem>();

        if(!effect.isPlaying){
            effect.Play();}


        lazer_timer+=1*Time.deltaTime;

        if(lazer_timer>=LazerAimTime){
            firingLazerCooldown = true;
            lazer_timer = 0;
            firingLazer = false;
            effect.Stop();
            source.Stop();
            lazerEffect.transform.parent = null;
            Invoke("ResetLazerCooldown",LazerCooldown);
        }
    }
    
    public void ResetLazerCooldown(){
        firingLazerCooldown = false;
    }
    
    void SummonParticle(Vector3 particle, Vector3 enemy){
        GameObject electron;
        bool logable = false;
        if(logger!=null){
            logable = true;
        }
        if(boostedAttack){
            int randomNumber = Random.Range(-2,2);
            if(randomNumber==0){
                electron = Instantiate(positron_prefab,particle,Quaternion.Euler(Vector3.zero));
                if(logable){
                    logger.AppendLog("Positron Shot",logPositCol);}
            }
            else{
                electron = Instantiate(electron_neutrino_prefab,particle,Quaternion.Euler(Vector3.zero));
                if(logable){
                    logger.AppendLog("E_Neutrino Shot",logNeutCol);}
            }
        }
        else{
            if(logable){
                logger.AppendLog("Electron Shot",logElecCol);}
            electron = Instantiate(electron_prefab,particle,Quaternion.Euler(Vector3.zero));
        }
        electron.transform.LookAt(enemy);   
    }
    
    //accesed within the movement script
    public void ChangeAttack(bool light){
        currentbil+=1;
        currentUsedAbilities = 0;
        
        //ensure that the indexing resets to remove an error
        if(currentbil>=bils.Length){
            currentbil = 0;
        }
        //change the attack recursively until the lightmode matches
        if(bils[currentbil].lightMode!=light){
            ChangeAttack(light);
        }
        else{
            if(backgroundUI!=null && bils[currentbil].abilityIcon){
                backgroundUI.transform.position = bils[currentbil].abilityIcon.transform.position;
            }
        }
    }
    public void UseAbility(){

        bool abilityUsable = abilityuseable && abilityuseable2;
        if(abilityUsable || bils[currentbil].IgnoreAbilityCooldowns){
            
            abilityuseable = false;
            currentUsedAbilities+=1;

            bils[currentbil].ability.SetVariables(publicenemynumber1,bils[currentbil].cooldown,bils[currentbil].softCooldown);
            bils[currentbil].ability.Activate(currentUsedAbilities);

            if(logger!=null){
                logger.AppendLog(bils[currentbil].loggValue,bils[currentbil].loggColour);
            }

            if(bils[currentbil].clip!=null){
                abilitySource.PlayOneShot(bils[currentbil].clip,volume);
            }
           
            if(currentUsedAbilities>=bils[currentbil].maxSummons){
                abilityuseable2 = false;
                if(lockEffect!=null && !bils[currentbil].IgnoreAbilityCooldowns){
                    lockEffect.SetActive(true);
                }
                Invoke("ResetAbil2",bils[currentbil].cooldown);
            }
            else{
                Invoke("ResetAbil1",bils[currentbil].softCooldown);
            }
        }
    }
    void ResetAbil2(){
        abilityuseable2 = true;
        abilityuseable = true;
        currentUsedAbilities = 0;
        if(lockEffect!=null){lockEffect.SetActive(false);}
    }
    void ResetAbil1(){
        abilityuseable = true;
    }
}
    
