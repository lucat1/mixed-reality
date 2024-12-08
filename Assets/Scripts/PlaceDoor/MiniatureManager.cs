using System;
using System.Collections;
using System.Collections.Generic;
using MixedReality.Toolkit;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

public class MiniatureManager : MonoBehaviour
{

    // this parameter controll the max distance within a piece is considered to be near another one
    public GameObject door;
    public float nearObjectRadius;

    // materials
    public Material transparentMaterial;
    public Material glowingMaterial;

    private List<GameObject> displayedGroups = new();

    public void InitializeDisplayBlocks(List<string> components)
    {
        Debug.Log("[MiniatureManager] Initialized display blocks");
        displayedGroups.Clear();
        HighlightComponents(new HashSet<string>(components), false);
        
        // Create the groups to display
        foreach (string c in components){
            Debug.Log("[MiniatureManager] creating display group for component:"+c);
            displayedGroups.Add(CreateDisplayGroup(GetDoorComponents(go => go.name == c)[0]));
        }

        Debug.Log("[MiniatureManager] Display blocks created");
    }

    // This function activate the Display block at the specified index
    public void ActivateDisplayBlock(int index)
    {
        Debug.Log("[MiniatureManager] Activating display block: " + index);
        for (int i = 0; i < displayedGroups.Count; i++)
            if(i == index)
                displayedGroups[i].SetActive(true);
            else
                displayedGroups[i].SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public bool IsVisible()
    {
        return gameObject.activeSelf;
    }

    List<GameObject> GetNearComponents(GameObject center)
    {
        List<GameObject> pr = GetDoorComponents(ob => ComputeComponentsDist(center, ob) <= nearObjectRadius && ob.transform.childCount == 0);
        return pr;
    }

    float ComputeComponentsDist(GameObject center, GameObject other)
    {
        double distanceSquared = Math.Pow(center.transform.position.x - other.transform.position.x, 2) + Math.Pow(center.transform.position.y - other.transform.position.y, 2) + Math.Pow(center.transform.position.z - other.transform.position.z, 2);
        return (float)Mathf.Sqrt((float)distanceSquared);// * ScaleFactor();
    }

    private GameObject CreateDisplayGroup(GameObject component)
    {
        // display the near components as transparent
        foreach (GameObject ob in GetNearComponents(component))
        {
            ob.SetActive(true);
            ob.transform.SetParent(component.transform);
            // GameObject obCopy =  Instantiate(ob);
            // obCopy.transform.position = ob.transform.position;
            // obCopy.transform.rotation = ob.transform.rotation;
            // obCopy.transform.localScale = ob.transform.localScale;
            // obCopy.transform.SetParent(component.transform);
        }

        component.transform.SetParent(door.transform);
        component.transform.localPosition = new Vector3(0, 0, 0);
        component.SetActive(false);

        SetObjectVolume(component, 0.0000005f);

        return component;
    }

    void SetObjectVolume(GameObject go, float targetVolume)
    {
        // Get the current volume using the MeshFilter
        MeshFilter meshFilter = go.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("[MiniatureManager] MeshFilter or Mesh not found!");
            return;
        }

        // Calculate the local volume of the object
        float currentVolume = meshFilter.sharedMesh.bounds.Volume();

        // Adjust for scaling to world space
        float worldVolume = currentVolume *
                            (go.transform.lossyScale.x *
                            go.transform.lossyScale.y *
                            go.transform.lossyScale.z);

        // Calculate the scale factor needed to reach the target volume
        float scaleFactor = Mathf.Pow(targetVolume / worldVolume, 1f / 3f);

        // Apply the new scale to the object
        go.transform.localScale *= scaleFactor;

        Debug.Log("[MiniatureManager] New scale applied to achieve target volume.");
    }
    void DeactivateAll()
    {
        foreach (GameObject go in GetDoorComponents(_ => true))
            go.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        DeactivateAll();
        // Hide();
    }

    //DOOR FUNCTIONS ########################################################################################
    private HashSet<GameObject> currentlyHighlighted = new();
    Dictionary<GameObject, GameObject> gameObjectsWithRing = new ();
    private Vector3 scale;
    public float showRingVolumeThreshold;
    public GameObject ringPrefab;




    public void HighlightComponents(HashSet<string> toHighlightNames, bool showRing = true) {
        var toHighlight = GetDoorComponents(go => toHighlightNames.Contains(go.name));
        
        // activate the component if not active (for small components that are hide) 
        foreach(GameObject go in toHighlight){

            Debug.Log("[MiniatureManager] highliting component: " + go.name);
            if(!go.activeSelf)
                go.SetActive(true);
        }

        Debug.Log("[Door " + gameObject.name + "] Highlighting " + toHighlight.Count + " components");

        foreach (var go in currentlyHighlighted) {
            if (toHighlightNames.Contains(go.name))
                // The component is already highlighted and should stay like that
                continue;

            SetMaterial(go, transparentMaterial);

            // remove ring if object has one attached
            if (gameObjectsWithRing.ContainsKey(go))
                RemoveRing(go);
        }

        // Highlight new components
        foreach(var go in toHighlight) {
            SetMaterial(go, glowingMaterial);

            // // Check if we should render the circle
            // var mesh = go.transform.GetComponent<MeshRenderer>();
            // if((mesh.bounds.Volume() * VolumeFactor()) <= showRingVolumeThreshold && showRing)
            //     DisplayRing(go);
        }

        currentlyHighlighted = Enumerable.ToHashSet(toHighlight);
    }

    // Recursively get all door components, filtered by the given function.
    public List<GameObject> GetDoorComponents(Func<GameObject, bool> f) {
        List<GameObject> list = new ();
        Queue<Transform> q = new ();
        Transform t = door.transform.Find("DoorContainer/DoorScale/_/Door");
        foreach(Transform c in t)
            q.Enqueue(c);

        while(q.Count > 0) {
            var child = q.Dequeue().gameObject;
            if(f(child))
                list.Add(child);
            
            foreach(Transform cc in child.transform)
                q.Enqueue(cc);
        }
        //foreach(GameObject go in list)
            //Debug.Log(go.name);

        return list;
    }

    void SetMaterial(GameObject t, Material mat) {
        var renderer = t.GetComponent<Renderer>();
        if (renderer == null)
            return;
        renderer.materials = new Material[]{mat};
    }

    void RemoveRing(GameObject go) {
        Debug.Log("Hiding ring on " + go.name);

        var ring = gameObjectsWithRing[go];
        gameObjectsWithRing.Remove(go);
        Destroy(ring);
    }

    // public float VolumeFactor() {
    //     return Mathf.Pow(ScaleFactor(), 3);
    // }

    // public float ScaleFactor() {
    //     Assert.IsFalse(scale.x == 0);
    //     return scale.x;
    // }

    // void DisplayRing(GameObject go) {
    //     var mesh = go.transform.GetComponent<MeshRenderer>();
    //     Debug.Log("Showing ring on name=" + go.name + ", volume=" + (mesh.bounds.Volume() * VolumeFactor()));

    //     var ring = Instantiate(ringPrefab);
    //     ring.transform.SetParent(go.transform);
    //     ring.transform.position = go.transform.position;
    //     ring.transform.localScale = new Vector3(1f, 1f, 1f);
    //     gameObjectsWithRing[go] = ring;
    // }

}