using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDoor : MonoBehaviour
{
    // Prefabs for the spheres
    public GameObject spherePrefab;

    // Define the positions where the spheres will appear
    public Vector3 sphere1Position = new Vector3(0, 0.5f, 1);
    public Vector3 sphere2Position = new Vector3(2, 0.5f, 1);

    // Method to be called on button press
    public void RenderAnchorPoints()
    {
        // Instantiate the spheres at the specified positions
        Instantiate(spherePrefab, sphere1Position, Quaternion.identity);
        Instantiate(spherePrefab, sphere2Position, Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
