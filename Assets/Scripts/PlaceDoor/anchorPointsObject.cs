using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anchorPointsObject : MonoBehaviour
{

    public GameObject componentsParent;
    public GameObject leftSphere;
    public GameObject rightSphere;
    public GameObject plane;
    public GameObject rotationController;

    void AdjustPlane()
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Contrains
        if(leftSphere != null && rightSphere != null && plane != null)
        {
            AdjustPlane();
            
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
