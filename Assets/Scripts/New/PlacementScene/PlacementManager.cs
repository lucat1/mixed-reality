using UnityEngine;
using UnityEngine.SceneManagement;
using MixedReality.Toolkit.UX;
using UnityEngine.Assertions;
using Unity.VisualScripting;
using System.Collections.Generic;


public class PlacementManager : MonoBehaviour
{
    // Prefab for anchor points
    public GameObject anchorPointsPrefab;
    private GameObject anchorPoints;

    private const string confirmPath = "PlaceDoor/ButtonGroup_32x32mm_H3/ButtonCollection/ConfirmButton";

    private Vector3 scale = Vector3.one;

    public Vector3 Scale() {
        return scale;
    }

    // Action for when the "Place" button is pressed
    public void PlaceDoorAction()
    {
        // Get camera position
        Vector3 cameraPosition = Camera.main.transform.position;
        
        if(anchorPoints != null)
            Destroy(anchorPoints);
        // Instantiate anchor points
        anchorPoints = Instantiate(anchorPointsPrefab, cameraPosition + new Vector3(-0.057f, -0.03f, 0.623f), Quaternion.identity, transform.parent);
        
        // Activate confirm button 
        var pb = transform.Find(confirmPath).GetComponent<PressableButton>();
        pb.enabled = true;
    }

    public void MoveDoor()
    {
        Assert.IsNotNull(anchorPoints);
        Debug.Log("Moving the door to the appropriate position");

        var plane = anchorPoints.transform.Find("Plane");
        var planeScale = plane.GetComponent<Renderer>().bounds.size;
        
        // Postition door in the scene and activate it
        DoorManager.Instance.SetPosition(plane.position);
        DoorManager.Instance.SetRotation(plane.rotation * Quaternion.Euler(90, 0, 90));
        DoorManager.Instance.SetPlaneScale(planeScale);
    }

    // Action for when the "Confirm" button is pressed
    public void ConfirmAction()
    {
        Debug.Log("[PlacementManager] Moving to steps");
        MoveDoor();

        // Toggle anchor points anchor points
        anchorPoints.SetActive(false);

        // Move to the door scene and show the door
        NewSceneManager.Instance.GoTo(new List<string> { "DoorSceneCanvas", "BigDoor", "Steps" });

        // GET TIME end place door timer and start actial manteinance timer
        if(TimeTracker.Instance){
            TimeTracker.Instance.EndAction();
            TimeTracker.Instance.StartAction("challenge|navigate steps");
        }
    }

    // Action for when the "Cancel" button is pressed
    public void CancelAction()
    {
        SceneManager.LoadScene("Menu");
    }

    void Start()
    {
        if (NewSceneManager.Instance.TutorialActive)
        {
            BuildStep2PopUp();
        }

        if(TimeTracker.Instance){
            if(TimeTracker.Instance.challengeOn){
                Debug.Log("[PlacementManager] Started challenge: place door");
                TimeTracker.Instance.StartAction("challenge|place the door");
            }
        }
    }

    void OnEnable() {
        // Re-show anchor points when going back to to the placement
        if (anchorPoints != null)
            anchorPoints.SetActive(true);
    }

    private void BuildStep2PopUp(){
        NewPopUpManager.Instance.ShowBigPopUp(
            "Step 2: Place Door",
            "In this step, you will need to align the holographic door with the real train door. \n First move the blue ball to match the bottom-right corner of the door. \nThen move the Red Ball to match the top left-corner of the door. \n When the door is correctly placed click \"Confirm\".",
             "Continue", 
            () =>
            {
             Debug.Log("top girl");

            }
            );
    }
}