using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class MiniatureManager : MonoBehaviour
{

    // this parameter controll the max distance within a piece is considered to be near another one
    public GameObject door;
    public float nearObjectRadius;

    private List<string> compNames= new List<string> {"_25_310_0565_602","_25_802_1132_364","_25_375_0205_301"};
    private List<GameObject> displayedGroups = new ();
    private bool iniatialized = false;
    private DoorManager dm;

    public void InitializeDisplayBlocks(){
        if(! iniatialized){

            dm.HighlightComponents(new HashSet<string>(compNames), false);
            // Create the groups to display
            foreach(string c in compNames)
                displayedGroups.Add(CreateDisplayGroup(dm.GetDoorComponents(go => go.name == c)[0]));
            
            // Highlits object in the door
            iniatialized = true;
    
            countIndex = 0;
            displayedGroups[0].SetActive(true);


        }
        countIndex = 0;
        // display the first element
        displayedGroups[0].SetActive(true);
    }

    public void Show() {
        // activate miniature
        gameObject.SetActive(true);
        dm.Show();

        // place the object in front of the player
        Camera playerCamera = Camera.main;
        Vector3 newPosition = playerCamera.transform.position + playerCamera.transform.forward * 0.30f;
        transform.position = newPosition;
        transform.LookAt(Camera.main.transform);
        transform.rotation = playerCamera.transform.rotation;

    }

    public void Hide() {
        gameObject.SetActive(false);
        dm.Hide();
    }

    public bool IsVisible() {
        return gameObject.activeSelf;
    }

    List<GameObject> GetNearComponents(GameObject center){
        List<GameObject> pr =  dm.GetDoorComponents(ob => ComputeComponentsDist(center, ob) <= nearObjectRadius && ob.transform.childCount == 0);
        return pr;
    }

    float ComputeComponentsDist(GameObject center, GameObject other){
        double distanceSquared = Math.Pow(center.transform.position.x - other.transform.position.x, 2) + Math.Pow(center.transform.position.y - other.transform.position.y,2) + Math.Pow(center.transform.position.z - other.transform.position.z,2);
        return (float)Mathf.Sqrt((float)distanceSquared);
    }

    private GameObject CreateDisplayGroup(GameObject component){
        // doorManager.HighlightComponents(new HashSet<string>(compNames), false);
        // display the near components as transparent
        foreach(GameObject ob in GetNearComponents(component))
        {
            ob.SetActive(true);
            ob.transform.SetParent(component.transform);
        }

        component.transform.SetParent(door.transform);
        component.transform.localPosition  = new Vector3(0,0,0);
        component.SetActive(false);

        SetObjectVolume(component, 0.0000005f);

        return component;
    }

    //DEBUG
    int countIndex = 0;
    public void Next(){
        if(countIndex < compNames.Count-1)
        {
            displayedGroups[countIndex].SetActive(false);
            countIndex++;
            displayedGroups[countIndex].SetActive(true);
        }
    }
    public void Previous(){
        if(countIndex > 0)
        {
            displayedGroups[countIndex].SetActive(false);
            countIndex--;
            displayedGroups[countIndex].SetActive(true);
        }
    }

    void SetObjectVolume(GameObject go, float targetVolume)
    {
        // Get the current volume using the MeshFilter
        MeshFilter meshFilter = go.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("MeshFilter or Mesh not found!");
            return;
        }

        // Calculate the local volume of the object
        Vector3 localSize = meshFilter.sharedMesh.bounds.size;
        float currentVolume = localSize.x * localSize.y * localSize.z;

        // Adjust for scaling to world space
        float worldVolume = currentVolume * 
                            (go.transform.lossyScale.x * 
                            go.transform.lossyScale.y * 
                            go.transform.lossyScale.z);

        // Calculate the scale factor needed to reach the target volume
        float scaleFactor = Mathf.Pow(targetVolume / worldVolume, 1f / 3f);

        // Apply the new scale to the object
        go.transform.localScale *= scaleFactor;

        Debug.Log("New scale applied to achieve target volume.");
    }
    void DeactivateAll() {
        foreach(GameObject go in dm.GetDoorComponents(_ => true))
            go.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        dm = door.GetComponent<DoorManager>();
        DeactivateAll();
        Hide();
    }
}
