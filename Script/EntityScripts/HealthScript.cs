using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{

    public float maxHealth;
    public float maxRadiation;

    private float currentHealth;
    private float currentRadiation;

    FireParticle weaponCode;

    //since the defence is a divisor, it cannot be zero. values less than one increase damage taken while negative values heal
    [HideInInspector] public float defaultDefence = 1;
    [HideInInspector] public float radiationDefence = 1;

    [HideInInspector] public bool regenerating;
    public float regenerationSpeed = 0.5f;

    public GameObject body;
    public GameObject dropPrefab;
    private bool killed=false;

    public Image playerHealthBar;

    //enemy variables
    public bool enemy;

    [HideInInspector] public EnemyManager man;
    public EndingManager endman;

    void Awake(){
        weaponCode = GetComponent<FireParticle>();
        maxHealth *= Variables.healthModifyer;
    }

    void Start(){
        Variables.hardStop = false;
    }

    void Update(){
        if(regenerating){
            currentHealth -= regenerationSpeed*Time.deltaTime;
            if(currentHealth<0){
                currentHealth = 0;
            }
        }
        if(playerHealthBar!=null){
            playerHealthBar.fillAmount = 1 - (currentHealth * (1 / maxHealth));
        }
    }
    public void Heal(float healAmount){
        currentHealth -= healAmount;
        //ensure the currentHealth is not less than 0
        currentHealth = currentHealth <= 0 ? 0: currentHealth;
    }

    void OnParticleCollision(GameObject other){
        if(other.name == "Lazer"){
            currentRadiation += (1/radiationDefence);
            if(currentRadiation>=maxRadiation){
                try{
                    currentRadiation = 0;
                    weaponCode.GainElectron(true);
                    other.transform.parent.GetComponent<BindedObject>().bind.GetComponent<FireParticle>().GainElectron(false);}
                catch{
                    return;
                }
            }
        }
        else if(other.name == "positron"){
            currentHealth+= (3/defaultDefence);
            
        }
        else if(other.name == "neutrino"){
            currentHealth+= (1.5f/defaultDefence);
        }
        else if(other.name == "lightexplosion"){
            currentRadiation += (0.5f/radiationDefence);
            //the values are low since there are a lot of particles from the explosion and so the enemy will not instantly die
            currentHealth += (0.05f/defaultDefence);
           
            if(currentRadiation>=maxRadiation){
                try{
                    currentRadiation = 0;
                    weaponCode.GainElectron(true);
                    other.transform.parent.GetComponent<BindedObject>().bind.GetComponent<FireParticle>().GainElectron(false);}
                catch{
                    return;
                }
            }
        }
        else{
            currentHealth += (1/defaultDefence);
        }

        if(currentHealth>=maxHealth){
            currentHealth = 0;
            KillObject();
        }
    }

    void KillObject(){
        if(killed){return;}
        killed = true;

        GameObject pref = Instantiate(dropPrefab,transform.position, Quaternion.Euler(new Vector3(Random.Range(-1,1),Random.Range(-1,1),Random.Range(-1,1))));
        body.SetActive(false);

        if(enemy){
            man.enemyDied();
            Invoke("Death",0.5f);
        }
        else{
            Variables.hardStop = true;
            endman.EndGame(false);
        }
    }

    void Death(){
        Destroy(this.gameObject);
    }
}
