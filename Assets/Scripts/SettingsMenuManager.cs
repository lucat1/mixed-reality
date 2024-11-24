using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.SpatialManipulation;

public class SettingsMenuManager : MonoBehaviour
{
    public void ToggleMenu(){
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void ToggleMenusFollowYou(bool selected){
        Debug.Log("[SettingsMenuManager] toggle Menus Follow You");
        Settings.Instance.MenusFollowYou = selected;
    }

    public void ToggleMenusFaceYou(bool selected){
        Debug.Log("[SettingsMenuManager] toggle Menus Face You");
        Settings.Instance.MenusFaceYou = selected;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

}
