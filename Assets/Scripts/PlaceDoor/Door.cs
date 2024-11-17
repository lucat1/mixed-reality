using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MixedReality.Toolkit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteAlways]
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
        Debug.Log("highlighting " + toHighlight.Count() + " components");

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

            // Check if we should render the circle
            var mesh = go.transform.GetComponent<MeshRenderer>();
            if(mesh.bounds.Volume() <= showRingVolumeThreshold && showRing)
                DisplayRing(go);
        }

        currentlyHighlighted = Enumerable.ToHashSet(toHighlight);
    }

    // Mappings from small object to its relative ring being currently displayed
    Dictionary<GameObject, GameObject> gameObjectsWithRing = new ();

    // Displays a ring on 
    void DisplayRing(GameObject go) {
        var ring = Instantiate(ringPrefab);
        ring.transform.SetParent(go.transform);
        ring.transform.position = go.transform.position;
        ring.transform.localScale = new Vector3(1f, 1f, 1f);
        gameObjectsWithRing[go] = ring;
    }

    void RemoveRing(GameObject go) {
        var ring = gameObjectsWithRing[go];
        gameObjectsWithRing.Remove(go);
        Destroy(ring);
    }

    public void StartMaintenance() {
        gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        RemoveSmallComponents();
        foreach(var t in GetDoorComponents(go => !currentlyHighlighted.Contains(go)))
            SetMaterial(t, transparentMaterial);

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Make the rings face the player
        foreach(var go in gameObjectsWithRing.Values)
            go.transform.LookAt(Camera.main.transform);
    }
}
