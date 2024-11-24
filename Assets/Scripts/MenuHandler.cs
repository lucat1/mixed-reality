using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.SpatialManipulation;

public class MenuHandler : MonoBehaviour
{

    // set the distance from the player when the object spawns
    public float offset = 0.25f;

    // if true the object will spawn infront of the player
    public bool spawnInfront = true;
    
    // if the option is toggoled the object can follow you
    public bool canFollowYou = true;
    // some components are flipped so we need to invert the rotation along x axe
    public bool invertedX = false;

    Follow follow;
    // Start is called before the first frame update
    void Start()
    {
        follow = gameObject.GetComponent<Follow>();
        if(follow != null)
            follow.enabled = Settings.Instance.MenusFollowYou;

        // make the menu spawn in front of the player when enabled
        if(spawnInfront){
            Vector3 positionInFront = Camera.main.transform.position + Camera.main.transform.forward * offset;
            transform.position = positionInFront;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Settings.Instance.MenusFaceYou){
            transform.LookAt(Camera.main.transform);
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.y += 180;
            if(invertedX){
                // Invert the x-axis rotation
                eulerAngles.x = -eulerAngles.x;
            }
            transform.eulerAngles = eulerAngles;
            
        }

        if(canFollowYou)
            follow.enabled = Settings.Instance.MenusFollowYou;

        
    }

    void OnEnable(){
        // activate the object solver if menus follow you is enabled
        follow = gameObject.GetComponent<Follow>();
        if(follow != null)
            follow.enabled = Settings.Instance.MenusFollowYou;

        // make the menu spawn in front of the player when enabled
        if(spawnInfront){
            Vector3 positionInFront = Camera.main.transform.position + Camera.main.transform.forward * offset;
            transform.position = positionInFront;
        }
    }
}
