/*
The PlacementManager script is responsible for managing the placement menu.

Key Features:
- Allows user to place virtual door on top of real door through anchor balls.
*/
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
    public bool doorPlaced { get; private set; } = false; // flag for door placement
    private const string confirmPath = "PlaceDoor/ButtonGroup_32x32mm_H3/ButtonCollection/ConfirmButton"; // confirm button path

    private Vector3 scale = Vector3.one; // scale of popup

    public Vector3 Scale() {
        return scale;
    }

    
    void Start()
    {
        if (NewSceneManager.Instance.TutorialActive)
        {
            BuildStep2TutorialPopUp();
        }

    }

    // Re-show anchor points when going back to to the placement
    void OnEnable() {
        if (NewSceneManager.Instance && NewSceneManager.Instance.PreviousScene != "Door") {
            // Destroy(anchorPoints);
            anchorPoints = null;
        }

        if (anchorPoints != null)
            anchorPoints.SetActive(true);
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
        DoorManager.Instance.UpdatePosition();
    }

    // Action for when the "Confirm" button is pressed
    public void ConfirmAction()
    {
        // Check if door has been placed
        if (doorPlaced==true){
        // Check if tutorial
        if (NewSceneManager.Instance.TutorialActive)
        {
            BuildFinishStep2TutorialPopUp();
        }
        // check if challenge
        else if (NewSceneManager.Instance.ChallengeActive)
        {
            BuildTask2ChallengePopUp();
        }
        else
        {
            MoveToSteps();
        }
                }
        else // if door has not been placed -> give error thorough popup 
        { 
            NewPopUpManager.Instance.ShowSinglePopup(
            "DoorNotPlaced",
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

    // move to steps menu
    private void MoveToSteps(){
        MoveDoor();

        // Toggle anchor points anchor points
        anchorPoints.SetActive(false);

        // Move to the door scene and show the door
        NewSceneManager.Instance.GoTo("Door", new List<string> { "BigDoor", "Steps", "PalmMiniature" });
    }

    // Action for when the "Cancel" button is pressed
    public void CancelAction()
    {
        NewSceneManager.Instance.GoTo("Menu", new List<string> { "MenuPanel" });
    }

    // step 2 tutorial popup
    private void BuildStep2TutorialPopUp(){
        NewSceneManager.Instance.HideObject("PlaceDoor");
        NewSceneManager.Instance.HideObject("AnchorPoints(Clone)");
        NewPopUpManager.Instance.ShowBigPopUp(
            "PlaceDoor",
            "Step 2: Place Door",
            "In this step, you will need to align the holographic door with the real train door. \n First move the blue ball to match the bottom-right corner of the door. \nThen move the Red Ball to match the top left-corner of the door. \n When the door is correctly placed click \"Confirm\".",
             "Continue", 
            () =>
            {
             NewSceneManager.Instance.ShowObjects(new (){"PlaceDoor"});
            }
            );
    }

    // task 2 challenge popup
    private void BuildTask2ChallengePopUp(){
        NewSceneManager.Instance.HideObject("PlaceDoor");
        NewPopUpManager.Instance.ShowSinglePopup(
            "PlaceDoorFinishChallenge",
            "Second Task Completed!",
            "Second task successfully completed! The next task is about showing the miniature view of step 1.",
            "Continue", 
            MoveToSteps
            );
    }

    // finish step 2 tutorial popup
    private void BuildFinishStep2TutorialPopUp(){
        NewSceneManager.Instance.HideObject("PlaceDoor");
        NewPopUpManager.Instance.ShowSinglePopup(
            "PlaceDoorFinishTutorial",
            "Step 2 Successfully Completed!",
            "Click \"Continue\" to proceed with the tutorial.",
             "Continue", 
            MoveToSteps
            );
    }
}