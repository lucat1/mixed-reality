using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniatureManager : MonoBehaviour
{

    // this parameter controll the max distance within a piece is considered to be near another one
    public GameObject door;
    public GameObject steps;
    public GameObject end;
    public GameObject doorCenter;
    public DoorManager doorManager;
    public float nearObjectRadius;
    private List<string> compNames= new List<string> {"_25_310_0565_602","_25_802_1132_364","_25_375_0205_301"};
    private List<GameObject> displayedGroups = new ();

    public void ActivateMiniature(){
        gameObject.SetActive(true);
        steps.SetActive(false);
        end.SetActive(false);
        var cameraPosition = Camera.main.transform.position;
        transform.position = cameraPosition + new Vector3(0.2f,0,0.323f);
        transform.LookAt(Camera.main.transform);
    }

    public void DeactivateMiniature(){
        gameObject.SetActive(false);
    }

    public bool CheckActive(){
        return gameObject.activeSelf;
    }

    List<GameObject> GetNearComponents(GameObject center){
        List<GameObject> pr =  doorManager.GetDoorComponents(ob => (ComputeComponentsDist(center, ob) <= nearObjectRadius && ob.transform.childCount == 0));
        return pr;
    }

    float ComputeComponentsDist(GameObject center, GameObject other){
        double distanceSquared = Math.Pow(center.transform.position.x - other.transform.position.x, 2) + Math.Pow(center.transform.position.y - other.transform.position.y,2) + Math.Pow(center.transform.position.z - other.transform.position.z,2);
        return (float)Mathf.Sqrt((float)distanceSquared);
    }

    private HashSet<GameObject> currentlyActive = new();

    private GameObject CreateDisplayGroup(GameObject component){

        doorManager.HighlightComponents(new HashSet<string>(compNames), false);
        // display the near components as transparent
        foreach(GameObject ob in GetNearComponents(component))
        {
            ob.SetActive(true);
            ob.transform.SetParent(component.transform);
        }

        component.transform.SetParent(doorCenter.transform);
        component.transform.localPosition  = new Vector3(0,0,0);
        component.SetActive(false);

        SetObjectVolume(component, 0.0000005f);

        return component;
    }

    void ActivateParents(Transform child)
    {
        Transform currentParent = child.parent;
        while (currentParent != null)
        {
            currentParent.gameObject.SetActive(true);
            currentParent = currentParent.parent;
        }
    }

    //DEBUG
    int countIndex = 0;
    public void Next(){
        if(countIndex < compNames.Count-1)
        {
            displayedGroups[countIndex].SetActive(false);
            countIndex++;
            displayedGroups[countIndex].SetActive(true);

            Renderer renderer = displayedGroups[countIndex].GetComponent<Renderer>();
            Vector3 localSize = renderer.bounds.size;
            Debug.Log(localSize);

        }
    }
    public void Previous(){
        if(countIndex > 0)
        {
            displayedGroups[countIndex].SetActive(false);
            countIndex--;
            displayedGroups[countIndex].SetActive(true);
            
            Renderer renderer = displayedGroups[countIndex].GetComponent<Renderer>();
            Vector3 localSize = renderer.bounds.size;
            Debug.Log(localSize);
        }
    }

    void DeactivateAll(){
        foreach(GameObject go in doorManager.GetDoorComponents(_ => true))
            go.SetActive(false);
    }

    void SetObjectVolume(GameObject gameObject, float targetVolume)
    {
        // Get the current volume using the MeshFilter
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
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
                            (gameObject.transform.lossyScale.x * 
                            gameObject.transform.lossyScale.y * 
                            gameObject.transform.lossyScale.z);

        // Calculate the scale factor needed to reach the target volume
        float scaleFactor = Mathf.Pow(targetVolume / worldVolume, 1f / 3f);

        // Apply the new scale to the object
        gameObject.transform.localScale *= scaleFactor;

        Debug.Log("New scale applied to achieve target volume.");
    }
    // Start is called before the first frame update
    void Start()
    {

        DeactivateAll();
        // Create the groups to display
        foreach(string c in compNames)
            displayedGroups.Add(CreateDisplayGroup(doorManager.GetDoorComponents(go => go.name == c)[0]));
        
        // display the first element
        displayedGroups[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
