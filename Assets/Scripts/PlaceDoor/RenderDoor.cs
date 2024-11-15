using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RenderDoor : MonoBehaviour
{
    public GameObject Door;
    public GameObject anchorPointsPrefab;
    public GameObject anchorPoints;


    // Method to be called on button press
    public void RenderAnchorPoints()
    {
        // Get camera position
        Vector3 cameraPosition = Camera.main.transform.position;

        if (anchorPoints != null){
            Destroy(anchorPoints);
        }
        // Instantiate anchor points
        anchorPoints = Instantiate(anchorPointsPrefab, cameraPosition + new Vector3(-0.057f, -0.03f, 0.323f), Quaternion.identity);

    }
    public void createDoor()
    {
        if (anchorPoints != null)
        {
            Vector3 doorOffset = new Vector3(-4.22f,-1.63f,1.4f);
            Vector3 doorPosition = anchorPoints.transform.Find("Plane").transform.position;

            float planeWidth = anchorPoints.transform.Find("Plane").transform.localScale.x;

            Debug.Log(doorPosition);
            
            Door.transform.position = doorPosition;
            Door.SetActive(true);

            Transform DoorScaleTrasform = Door.transform.Find("DoorContainer/DoorScale");
            DoorScaleTrasform.localScale = new Vector3(planeWidth/0.16583f, planeWidth/0.16583f,planeWidth/0.16583f);

            Transform DoorResetPosition = Door.transform.Find("DoorContainer");
            DoorResetPosition.localPosition = Vector3.zero;

        }else
        {
            Debug.LogError("no anchor points created");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Done()
    {
        SceneManager.LoadScene("Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
