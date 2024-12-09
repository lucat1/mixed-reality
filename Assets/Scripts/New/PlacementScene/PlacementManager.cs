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
    public bool doorPlaced { get; private set; } = false;
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

        // Set DoorPlaced to true to ativate confirm button
        doorPlaced = true;
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
        // Check if door has been placed
        if (doorPlaced==true){
            // Check if tutorial
        if (NewSceneManager.Instance.TutorialActive)
        {
            BuildFinishPopUp2();
        }
        else if (NewSceneManager.Instance.ChallengeActive)
        {
            BuidSecondTaskChallengePopUp();
        }
        else
        {
            MoveToSteps();
        }
                }
        else { 
             NewPopUpManager.Instance.ShowSinglePopup(
            "Watch Out!",
            "You tried to confirm without having placed the door. \n You must place the door first by clicking on 'Place Door' and then confirm. \n Click 'Try Again' and proceed with the correct order.",
             "Try Again", 
            () =>
            {
             Debug.Log("User tried to confirm before placing the door!");

            }
            );
        }        
    }

    private void MoveToSteps(){
        Debug.Log("[PlacementManager] Moving to steps");
        MoveDoor();

        // Toggle anchor points anchor points
        anchorPoints.SetActive(false);

        // Move to the door scene and show the door
        NewSceneManager.Instance.GoTo(new List<string> { "DoorSceneCanvas", "BigDoor", "Steps", "PalmMiniature" });

        // GET TIME end place door timer and start actial manteinance timer
        if(TimeTracker.Instance){
            TimeTracker.Instance.EndAction();
            TimeTracker.Instance.StartAction("challenge|navigate steps");
        }
    }

    // Action for when the "Cancel" button is pressed
    public void CancelAction()
    {
        NewSceneManager.Instance.GoTo(new List<string> { "MenuSceneCanvas", "MenuPanel" });
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
        NewSceneManager.Instance.HideObject("PlaceDoor");
        NewSceneManager.Instance.HideObject("AnchorPoints(Clone)");
        NewPopUpManager.Instance.ShowBigPopUp(
            "Step 2: Place Door",
            "In this step, you will need to align the holographic door with the real train door. \n First move the blue ball to match the bottom-right corner of the door. \nThen move the Red Ball to match the top left-corner of the door. \n When the door is correctly placed click \"Confirm\".",
             "Continue", 
            () =>
            {
             NewSceneManager.Instance.ShowObject("PlaceDoor");

            }
            );
    }

    private void BuidSecondTaskChallengePopUp(){
        NewSceneManager.Instance.HideObject("PlaceDoor");
        NewPopUpManager.Instance.ShowBigPopUp(
            "Second Task Completed!",
            "Second task successfully completed! The next task is about showing the miniature view of step 1.",
            "Continue", 
            () =>
            {
             MoveToSteps();

            }
            );
    }

    private void BuildFinishPopUp2(){
        NewSceneManager.Instance.HideObject("PlaceDoor");
        NewPopUpManager.Instance.ShowSinglePopup(
            "Step 2 Successfully Completed!",
            "Click \"Continue\" to proceed with the tutorial.",
             "Continue", 
            () =>
            {
             MoveToSteps();

            }
            );
    }
}