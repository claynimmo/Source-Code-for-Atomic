using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public GameObject collectEffectPrefab;
    public int healthRegen = 50;
    void OnTriggerEnter(Collider other){
        if(other.GetComponent<HealthScript>()){
            other.GetComponent<HealthScript>().Heal(healthRegen);
            Instantiate(collectEffectPrefab,other.transform.position,Quaternion.Euler(Vector3.zero),other.transform);
            Destroy(gameObject);
        }
    }
}
