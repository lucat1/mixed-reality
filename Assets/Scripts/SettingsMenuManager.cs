using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.SpatialManipulation;

public class SettingsMenuManager : MonoBehaviour
{
    public Follow follow;
    public void ToggleMenu(){
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void ToggleMenusFollowYou(bool selected){
        Debug.Log("[SettingsMenuManager] toggle Menus Follow You");
        Settings.Instance.MenusFollowYou = selected;
        follow.enabled = Settings.Instance.MenusFollowYou;
    }

    public void ToggleMenusFaceYou(bool selected){
        Debug.Log("[SettingsMenuManager] toggle Menus Face You");
        Settings.Instance.MenusFaceYou = selected;
    }
    // Start is called before the first frame update
    void Start()
    {
        follow = gameObject.GetComponent<Follow>();
        Debug.Log("follow messo");
        Debug.Log(follow);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Settings.Instance.MenusFaceYou){
            transform.LookAt(Camera.main.transform);
            transform.eulerAngles += new Vector3(0, 180, 0);
        }
        
    }

    void OnEnable()
    {
        follow = gameObject.GetComponent<Follow>();
        if(follow == null){
            Debug.Log("follow null");
        }
        follow.enabled = Settings.Instance.MenusFollowYou;
        // make the object spawn in front of the player
        float distanceInFront = 0.15f;
        Vector3 positionInFront = Camera.main.transform.position + Camera.main.transform.forward * distanceInFront;
        transform.position = positionInFront;
    }
}
