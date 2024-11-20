using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MixedReality.Toolkit.UX;


public class PlaceDoor : MonoBehaviour
{

    // Door object to activate in the scene
    public GameObject door;
    private DoorManager dm;
    // Prefab for anchor points
    public GameObject anchorPointsPrefab;
    GameObject anchorPoints;

    private const string confirmPath = "UIContainer/ButtonGroup_32x32mm_H3/ButtonCollection/ConfirmButton";
    // Instantiate the anchor points to perform door placement
    public void CreateAnchorPoints()
    {
        // Get camera position
        Vector3 cameraPosition = Camera.main.transform.position;
        
        if(anchorPoints != null)
            Destroy(anchorPoints);
        // Instantiate anchor points
        anchorPoints = Instantiate(anchorPointsPrefab, cameraPosition + new Vector3(-0.057f, -0.03f, 0.323f), Quaternion.identity);
        
        // Activate confirm button 
        var pb = transform.Find(confirmPath).gameObject.GetComponent<PressableButton>();
        pb.enabled = true;

    }

    public void placeDoor()
    {
        if (anchorPoints != null)
        {
            Vector3 doorOffset = new Vector3(-4.22f,-1.63f,1.4f);
            Vector3 doorPosition = anchorPoints.transform.Find("Plane").transform.position;

            float planeWidth = anchorPoints.transform.Find("Plane").transform.localScale.x;
            
            // Postition door in the scene and activate it
            door.transform.position = doorPosition;
            door.SetActive(true);

            // Adjust door size
            Transform DoorScaleTrasform = door.transform.Find("DoorContainer/DoorScale");
            DoorScaleTrasform.localScale = new Vector3(planeWidth/0.16583f, planeWidth/0.16583f,planeWidth/0.16583f);

            // Adjust door position after scale change
            Transform DoorResetPosition = door.transform.Find("DoorContainer");
            DoorResetPosition.localPosition = Vector3.zero;

            TogglePlaceDoorMenu();
        }else
        {
            Debug.LogError("no anchor points created");
        }
    }

    public void TogglePlaceDoorMenu()
    {
        // Toggle anchor points anchor points
        anchorPoints.SetActive(!anchorPoints.activeSelf);

        // Toggle the visibility of the door
        if (dm.isVisible())
            dm.Hide();
        else
            dm.Show();
    }

    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }

    void Start()
    {
        dm = door.GetComponent<DoorManager>();
    }
}
