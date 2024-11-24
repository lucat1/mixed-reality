using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class AnchorPlane : MonoBehaviour
{
    public GameObject rightSphere;
    public GameObject leftSphere;
    public GameObject plane;

    // set the distance from the player when the object spawns
    public static float offset = 0.5f;
    private Vector3 pos(GameObject go) {
        return go.transform.position;
    }

    void AdjustPlane()
    {
        // See: https://gamedev.stackexchange.com/a/202243
        Vector3 A = pos(leftSphere);
        Vector3 D = pos(rightSphere);
        Vector3 B = new (D.x, A.y, D.z);
        Vector3 C = new (A.x, D.y, A.z);

        // Compute plane center 
        Vector3 midpoint = (A + D) / 2;

        // Set the plane's position to the midpoint
        plane.transform.position = midpoint;
        
        // Calculate the distance between the spheres
        float planeWidth = Mathf.Sqrt(Mathf.Pow(C.z - D.z, 2) + Mathf.Pow(C.x - D.x, 2));
        float planeHeight = Mathf.Sqrt(Mathf.Pow(B.y - D.y, 2) + Mathf.Pow(B.z - D.z, 2));

        // float planeHeight = leftPosition.y - rightPosition.y;
        // Adjust the plane's scale based on the distance
        plane.transform.localScale = new Vector3(planeHeight/10, plane.transform.localScale.y, planeWidth/10);

        var x = C - A + D - B;
        var y = C - D + A - B;
        var z = Vector3.Cross(x, y);
        plane.transform.rotation = Quaternion.LookRotation(z, y) * Quaternion.Euler(90, 0, 0);
    }

    void Start() {
        Vector3 positionInFront = Camera.main.transform.position + Camera.main.transform.forward * offset;
        transform.position = positionInFront;
    }

    void Update()
    {
        Assert.IsNotNull(rightSphere);
        Assert.IsNotNull(leftSphere);
        Assert.IsNotNull(plane);
        
        Vector3 leftPosition = pos(leftSphere);
        Vector3 rightPosition = pos(rightSphere);

        leftSphere.transform.position = new Vector3(
            Mathf.Min(leftPosition.x, rightPosition.x),
            Mathf.Max(leftPosition.y, rightPosition.y),
            leftPosition.z
        );

        AdjustPlane();
    }
}
