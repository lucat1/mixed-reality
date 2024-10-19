using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDoor : MonoBehaviour
{
    public GameObject Door;
    public GameObject anchorPointLeftPrefab;
    public GameObject anchorPointRightPrefab;
    public GameObject areaPlanePrefab;

    // Define the positions where the spheres will appear
    private Vector3 anchorPointLeftPrefabInitialOffset = new Vector3(-0.043f, 0.03f, 0.223f);
    private Vector3 anchorPointRightPrefabInitialOffset = new Vector3(0.043f, -0.03f, 0.223f);

    private GameObject leftSphere;
    private GameObject rightSphere;
    private GameObject plane;

    // Method to be called on button press
    public void RenderAnchorPoints()
    {
        // Get camera position
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 anchorPointLeftPrefabInitialPosition = cameraPosition + anchorPointLeftPrefabInitialOffset;
        Vector3 anchorPointRightPrefabInitialPosition = cameraPosition + anchorPointRightPrefabInitialOffset;


        if (leftSphere != null && rightSphere != null && plane != null){
            cancelAnchorPoints();
        }
        // Instantiate the spheres at the specified positions
        leftSphere = Instantiate(anchorPointLeftPrefab, anchorPointLeftPrefabInitialPosition, Quaternion.identity);
        rightSphere = Instantiate(anchorPointRightPrefab, anchorPointRightPrefabInitialPosition, Quaternion.identity);
        plane = Instantiate(areaPlanePrefab);

        AdjustPlane();
    }

    void AdjustPlane()
    {
        if (leftSphere != null && rightSphere != null && plane != null)
        {
            // Get the positions of the spheres
            Vector3 leftPosition = leftSphere.transform.position;
            Vector3 rightPosition = rightSphere.transform.position;

            Vector3 midpoint = (leftPosition + rightPosition) / 2;

            // Set the plane's position to the midpoint
            plane.transform.position = midpoint;
            
            // Calculate the distance between the spheres
            float planeWidth = Mathf.Abs(rightPosition.x - leftPosition.x);
            float planeHeight = Mathf.Abs(leftPosition.y - rightPosition.y);

            // float planeHeight = leftPosition.y - rightPosition.y;
            // Adjust the plane's scale based on the distance
            plane.transform.localScale = new Vector3(planeWidth/10, plane.transform.localScale.y, planeHeight/10);
        }
    }

    public void createDoor()
    {
        if (leftSphere != null && rightSphere != null && plane != null)
        {
            Vector3 doorOffset = new Vector3(-4.22f,-1.63f,1.4f);
            Vector3 doorPosition = plane.transform.position;

            float planeWidth = plane.transform.localScale.x;

            Debug.Log(doorPosition);
            
            // Door = Instantiate(doorPrefab, doorPosition, Quaternion.Euler(0, 0, 0));
            Door.transform.position = doorPosition;
            Door.SetActive(true);

            Door.transform.position = plane.transform.position;
            
            Transform DoorScaleTrasform = Door.transform.Find("DoorContainer/DoorScale");
            DoorScaleTrasform.localScale = new Vector3(planeWidth/0.16583f, planeWidth/0.16583f,planeWidth/0.16583f);

            Transform DoorResetPosition = Door.transform.Find("DoorContainer");
            DoorResetPosition.localPosition = Vector3.zero;



        }else
        {
            Debug.Log("no anchor points created");
        }
    }

    // destroy anchor points for door placement
    public void cancelAnchorPoints()
    {
        if(leftSphere != null && rightSphere != null && plane != null)
        {
            Destroy(leftSphere);
            Destroy(rightSphere);
            Destroy(plane);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AdjustPlane();
        
        // Contrains
        if(leftSphere != null && rightSphere != null && plane != null)
        {
            // Get the positions of the spheres
            Vector3 leftPosition = leftSphere.transform.position;
            Vector3 rightPosition = rightSphere.transform.position;

            // X position
            if(leftPosition.x > rightPosition.x)
            {
                leftSphere.transform.position = new Vector3(rightPosition.x, leftPosition.y, leftPosition.z);
            }

            if(leftPosition.y < rightPosition.y)
            {
                leftSphere.transform.position = new Vector3(leftPosition.x, rightPosition.y, leftPosition.z);
            }

            if(leftPosition.z != rightPosition.z)
            {
                leftSphere.transform.position = new Vector3(leftPosition.x, leftPosition.y, rightPosition.z);
            }
        }
        
    }
}
