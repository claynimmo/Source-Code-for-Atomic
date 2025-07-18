using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class BaryonAbility : BaseAbility
{

    public GameObject baryonPrefab;
    //variable modified through external script to activate
    public bool creatingBaryon;

    //variables to determine the attributes of crafted baryon
    private float baryonCharge = 0;
    private float gravitymodifyer = 1;
    private float sizeModifyer = 1;
    private float lifetime = 9;

    public float gravityMultiplyer = 1;
    public float sizeMultiplyer = 2;
    public float lifetimeMulitplyer = 1;

    
    public UIFade BaryonUI;
    private float baryonCount = 0;

    public GameObject[] QuarkButtons;
    public GameObject QuarkButtonAnimation;

    public Image[] ChargeIconsPos;
    public Image[] ChargeIconsNeg;

    public Color selectColourPos;
    public Color selectColourNeg;
    public Color overCharmed;
    public Color selectColourNone;

    //post processing effects for the slow motion
    //volume that is modified
    public PostProcessVolume volume;
    //the profile that the volume switches between
    public PostProcessProfile lowSatVolume;
    public PostProcessProfile defaultVolume;
    
    private Dictionary<KeyCode,System.Action> baryonDict;

    //properties of the quarks, they are here for better maintainability
    private const float quarkGravMod = 0.15f;
    private const float quarkSizeMod = 0.2f;
    private const float quarkLifeMod = 3;

    private const float quarkGravModAnti = 0.2f;
    private const float quarkSizeModAnti = 0.2f;
    private const float quarkLifeModAnti = 2;

    private const float chargeModifyer = 2.375f;


    public AudioSource source;
    public AudioClip clip;
    private float sfxVolume;

    //awake is used over start incase the object is instantiated
    void Awake(){
        sfxVolume = Variables.sfxVolume;

        baryonDict = new Dictionary<KeyCode, System.Action>{
            //up
            { KeyCode.Alpha1, () => { 
                baryonCharge+=2;
                gravitymodifyer += quarkGravMod*chargeModifyer;
                baryonCount+=1;
                Instantiate(QuarkButtonAnimation,QuarkButtons[0].transform);
            }},
            //down
            { KeyCode.Alpha2, () => { 
                baryonCharge-=1;
                gravitymodifyer += quarkGravMod;
                baryonCount+=1;
                Instantiate(QuarkButtonAnimation,QuarkButtons[1].transform);
            }},
            //charm
            { KeyCode.Alpha3, () => { 
                baryonCharge+=2;
                sizeModifyer += quarkSizeMod*chargeModifyer;
                baryonCount+=1;
                Instantiate(QuarkButtonAnimation,QuarkButtons[2].transform);
            }},
            //strange
            { KeyCode.Alpha4, () => { 
                baryonCharge-=1;
                sizeModifyer -= quarkSizeMod;
                baryonCount +=1;
                Instantiate(QuarkButtonAnimation,QuarkButtons[3].transform);
            }},
            //top
            { KeyCode.Alpha5, () => { 
                baryonCharge+=2;
                lifetime += quarkLifeMod*chargeModifyer;
                baryonCount += 1;
                Instantiate(QuarkButtonAnimation,QuarkButtons[4].transform);
            }},
            //bottom
            { KeyCode.Alpha6, () => { 
                baryonCharge-=1;
                lifetime += quarkLifeMod;
                baryonCount += 1;
                Instantiate(QuarkButtonAnimation,QuarkButtons[5].transform);
            }},
            //antiup
            { KeyCode.Q, () => { 
                baryonCharge-=2;
                gravitymodifyer += quarkGravModAnti*chargeModifyer;
                baryonCount+=1;
                Instantiate(QuarkButtonAnimation,QuarkButtons[6].transform);
            }},
            //antidown
            { KeyCode.W, () => { 
                baryonCharge+=1;
                gravitymodifyer += quarkGravModAnti;
                baryonCount+=1;
                Instantiate(QuarkButtonAnimation,QuarkButtons[7].transform);
            }},
            //anticharm
            { KeyCode.E, () => { 
                baryonCharge-=2;
                sizeModifyer += quarkSizeModAnti*chargeModifyer;
                baryonCount+=1;
                Instantiate(QuarkButtonAnimation,QuarkButtons[8].transform);
            }},
            //antistrange
            { KeyCode.R, () => { 
                baryonCharge+=1;
                sizeModifyer += quarkSizeModAnti;
                baryonCount +=1;
                Instantiate(QuarkButtonAnimation,QuarkButtons[9].transform);
            }},
            //antitop
            { KeyCode.T, () => { 
                baryonCharge-=2;
                lifetime += quarkLifeModAnti*chargeModifyer;
                baryonCount += 1;
                Instantiate(QuarkButtonAnimation,QuarkButtons[10].transform);
            }},
            //antibottom
            { KeyCode.Y, () => { 
                baryonCharge+=1;
                lifetime += quarkLifeModAnti;
                baryonCount += 1;
                Instantiate(QuarkButtonAnimation,QuarkButtons[11].transform);
            }}
        };
    }
    public void Update(){
        if(BaryonUI==null){return;}

        if(!creatingBaryon){
            BaryonUI.hideUI();
            return;
        }

        volume.profile = lowSatVolume;
        Time.timeScale = 0.2f;
        BaryonUI.ShowUI();

        foreach(var keyActionPair in baryonDict){
            if(Input.GetKeyDown(keyActionPair.Key)){
                keyActionPair.Value.Invoke();
            }
        }

        if(baryonCount >= 3){
            if(baryonCharge==3||baryonCharge==-3||baryonCharge == 0){
                FinishBaryon(false);
            }
            else{
                FinishBaryon(true);
            }
        }
        
        //apply colours to the UI
        if(baryonCharge < 0){
            for(int i = 0; i < ChargeIconsNeg.Length; i++){
                if(i<-baryonCharge){
                    if(baryonCharge<-3){
                        ChargeIconsNeg[i].color = overCharmed;}
                    else{
                        ChargeIconsNeg[i].color = selectColourNeg;}
                }
                else{
                    ChargeIconsNeg[i].color = selectColourNone;
                }
            }
            for(int x = 0; x < ChargeIconsPos.Length; x++){
                ChargeIconsPos[x].color = selectColourNone;
            }
        }
        else{
            for(int i = 0; i < ChargeIconsPos.Length; i++){
                if(i<baryonCharge){
                    if(baryonCharge>3){
                        ChargeIconsPos[i].color = overCharmed;}
                    else{
                        ChargeIconsPos[i].color = selectColourPos;}
                }
                else{
                    ChargeIconsPos[i].color = selectColourNone;
                }
            }
            for(int x = 0; x < ChargeIconsPos.Length; x++){
                ChargeIconsNeg[x].color = selectColourNone;
            }
        }
    }

    private void FinishBaryon(bool failed){
        Time.timeScale = 1;
        creatingBaryon = false;
        Variables.inputAble=true;
        volume.profile = defaultVolume;
        baryonCount = 0;
        float barycharge = baryonCharge;
        baryonCharge = 0;
        float barySize = sizeModifyer;
        sizeModifyer = 1;
        float baryGravity = gravitymodifyer;
        gravitymodifyer = 1;
        float barylife = lifetime;
        lifetime = 9;

        barySize=barySize*sizeMultiplyer;
        baryGravity=baryGravity*gravitymodifyer;
        barylife=barylife*lifetimeMulitplyer;
        if(!failed){
            if(barycharge==3){barycharge = 1;}
            if(barycharge==-3){barycharge = -1;}
            if(barycharge==0){baryonCharge=0;}
            source.PlayOneShot(clip,sfxVolume);
            GameObject baryon = Instantiate(baryonPrefab,transform.position,Quaternion.Euler(Vector3.zero));
            BaryonObject barymod = baryon.GetComponent<BaryonObject>();
            barymod.BaryonCharge = barycharge;
            barymod.BaryonPullForce = baryGravity;
            barymod.BaryonSize = barySize;
            barymod.lifetime = barylife;
        }
    }

    public override void Activate(int abilityCount){
        Variables.inputAble = false;
        //include a delay so the initial key press that activates the ability is not counted towards the baryon
        Invoke("StartBaryon",0.01f);
    }
    private void StartBaryon(){
        creatingBaryon = true;
    }
    
}
