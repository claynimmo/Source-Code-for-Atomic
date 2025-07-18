using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float enemyCount;
    public GameObject enemyPrefab;

    private float currentEnemyDeaths;

    EndingManager endMan;

    bool active;

    public bool player;

    void Awake(){
        if(player){
            enemyCount = Variables.playerCount;
        }
        else{
            enemyCount = Variables.enemyCount;
        }
        endMan = GetComponent<EndingManager>();
        for(int i=0;i<enemyCount;i++){
            GameObject enemy = Instantiate(enemyPrefab,transform.position,Quaternion.Euler(Vector3.zero));
            enemy.GetComponent<HealthScript>().man = this;
            enemy.GetComponent<HealthScript>().endman = endMan;
        }
        
    }
    public void enemyDied(){
        currentEnemyDeaths+=1;
        if(currentEnemyDeaths>=enemyCount &&!player){
            endMan.EndGame(true);
        }
    }
}
