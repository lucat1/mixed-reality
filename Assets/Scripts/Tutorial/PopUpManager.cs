using UnityEngine;

// this script manages the placement of popups
public class PopUpManager : MonoBehaviour
{
    public float distanceFromUser = 1.0f; // distance in front of user where to place popup

    // ShowPopup - position and setActive popup
    public void ShowPopup(GameObject popup)
    {
        // if popup is null -> return
        if (popup == null) return;

        // if popup is not null -> get user viewpoint
        Camera mainCamera = Camera.main;

        // if popup and user view point are both not null -> position and set popup active
        if (mainCamera != null)
        {
            // compute position
            Vector3 positionInFrontOfUser = mainCamera.transform.position + mainCamera.transform.forward * distanceFromUser;


            // place popup
            popup.transform.localPosition = positionInFrontOfUser;
            Debug.Log("posizione attuale" + popup.transform.localPosition);

            // make pop up face user
            popup.transform.LookAt(mainCamera.transform);
            popup.transform.eulerAngles += new Vector3(0, 180, 0);
            
            // activate popup
            popup.SetActive(true);
        }
    }
}
