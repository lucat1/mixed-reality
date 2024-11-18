using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject door;
    public MiniatureManager miniature;
    // Threshold decide when to switch to the miniature
    public float activateMiniatureThreshold;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    float DistanceFromPlayer(){
        
        return Vector3.Distance(Camera.main.transform.position, door.transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        if(door.activeSelf || miniature.CheckActive()){
            if(DistanceFromPlayer() < activateMiniatureThreshold){
                door.SetActive(false);
                if(! miniature.CheckActive())
                    miniature.ActivateMiniature();
            }        
            else{
                door.SetActive(true);
                miniature.DeactivateMiniature();
            }
        }
        
    }
}
