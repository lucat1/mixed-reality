using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MixedReality.Toolkit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorManager : MonoBehaviour
{
    // Threshold to decide if a component should be hidden. If the component's
    // volume is lower than the threshold, it gets hidden.
    public float volumeThreshold;

    public Material transparentMaterial;

    public Material glowingMaterial;

    // Threshold to decide if a component should . If the component's
    // volume is lower than the threshold, it gets hidden.
    public float showRingVolumeThreshold;

    public GameObject ringPrefab;

    // Recursively get all door components, filtered by the given function.
    public List<GameObject> GetDoorComponents(Func<GameObject, bool> f) {
        List<GameObject> list = new ();
        Queue<Transform> q = new ();
        foreach(Transform c in transform)
            q.Enqueue(c);

        while(q.Count > 0) {
            var child = q.Dequeue().gameObject;
            if(f(child))
                list.Add(child);
            
            foreach(Transform cc in child.transform)
                q.Enqueue(cc);
        }

        return list;
    }

    // Returns true if a component should be removed. That is, if:
    // 1. It has a MeshRenderer.
    // 2. Its volume is below the threshold.
    bool ShouldRemoveComponent(GameObject t) {
        var mesh = t.transform.GetComponent<MeshRenderer>();
        Debug.Log("mesh " + mesh);
        if (mesh != null)
            Debug.Log(mesh.bounds.Volume());
        return mesh != null && mesh.bounds.Volume() <= volumeThreshold;
    }

    void SetMaterial(GameObject t, Material mat) {
        var renderer = t.GetComponent<Renderer>();
        if (renderer == null)
            return;
        if (!t.activeSelf)
            t.SetActive(true);
        renderer.materials = new Material[]{mat};
    }

    // Disable all components smaller than the provided threshold.
    void RemoveSmallComponents() {
        var toRemove = GetDoorComponents(ShouldRemoveComponent);
        Debug.Log("Hiding " + toRemove.Count() + " components");
        foreach(var t in toRemove)
            t.SetActive(false);
    }

    private HashSet<GameObject> currentlyHighlighted = new();

    public void HighlightComponents(HashSet<string> toHighlightNames, bool showRing = true) {
        var toHighlight = GetDoorComponents(go => toHighlightNames.Contains(go.name));

        // de-highlight currently highlighted components
        // foreach(var go in currentlyHighlighted) {
        //     if (toHighlightNames.Contains(go.name))
        //         // The component is already highlighted and should stay like that
        //         continue;

        //     SetMaterial(go, transparentMaterial);
        //     currentlyHighlighted.Remove(go);
        // }

        for (int i = 0; i < currentlyHighlighted.Count; i++) {
            var go = currentlyHighlighted.ElementAt(i);
            if (toHighlightNames.Contains(go.name))
                // The component is already highlighted and should stay like that
                continue;

            SetMaterial(go, transparentMaterial);
            currentlyHighlighted.Remove(go);
        }

        // highlight new components
        foreach(var go in toHighlight) {
            if (currentlyHighlighted.Contains(go))
                // The component is already highlighted
                continue;

            SetMaterial(go, glowingMaterial);
            currentlyHighlighted.Add(go);

            // check if render the circle
            var mesh = go.transform.GetComponent<MeshRenderer>();
            if(mesh.bounds.Volume() <= showRingVolumeThreshold && showRing)
                DisplayRing(go);
        }
    }

    // Mappings from small object to its relative ring being currently displayed
    Dictionary<GameObject, GameObject> gameObjectsWithRing;

    // Displays a ring on 
    void DisplayRing(GameObject go) {
        var ring = Instantiate(ringPrefab);
        ring.transform.SetParent(go.transform);
        ring.transform.position = go.transform.position;
        ring.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    void RemoveRing(GameObject go) {
        var ring = gameObjectsWithRing[go];
        Destroy(ring);
    }

    void RemoveRings() {
        foreach(var go in gameObjectsWithRing)
            RemoveRing(go.Key);
    }

    // Start is called before the first frame update
    void Start()
    {
        RemoveSmallComponents();
        foreach(var t in GetDoorComponents(_ => true))
            SetMaterial(t, transparentMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}