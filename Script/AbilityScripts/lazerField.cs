using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazerField : MonoBehaviour
{
    LineRenderer line;
    //set via script in fireparticle
    [HideInInspector] public GameObject player;


    public int maxLines = 5;
    public float timeBetweenLines = 0.5f;
    public float deconstructionTime = 0.1f;

    public float posAmplitude = 3;


    public GameObject effect;

    // Start is called before the first frame update
    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = maxLines;
        StartCoroutine(CreateLazer());
    }

    IEnumerator CreateLazer(){
        int i = 0;
        while(i < maxLines){
            Vector3 pos = randomPosition();
            line.SetPosition(i, pos);
            GameObject eff = Instantiate(effect,pos,Quaternion.Euler(Vector3.zero));
            eff.GetComponentInChildren<BindedObject>().bind = player;
            yield return new WaitForSeconds(timeBetweenLines);
            i++;
        }
        while(i > 0){
            int index = maxLines - i;
    
            line.SetPosition(index,Vector3.zero);
            yield return new WaitForSeconds(deconstructionTime);
            i--;
        }
        yield return null;
    }

    Vector3 randomPosition(){
        float randomX = Random.Range(-posAmplitude, posAmplitude);
        float randomY = Random.Range(-posAmplitude, posAmplitude);
        float randomZ = Random.Range(-posAmplitude, posAmplitude);
        //set position to this object as default, in case the variable is unassigned for whatever reason
        Vector3 playerPos = this.transform.position;
        if(player!=null){
            playerPos = player.transform.position;
        }

        Vector3 randomPosition = new Vector3(playerPos.x+randomX,playerPos.y+randomY,playerPos.z+randomZ);
        return randomPosition;
    }
}
