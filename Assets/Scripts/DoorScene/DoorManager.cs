using System;
using System.Collections.Generic;
using System.Linq;
using MixedReality.Toolkit;
using UnityEngine;
using UnityEngine.Assertions;

public class DoorManager : MonoBehaviour
{
    // Threshold to decide if a component should be hidden. If the component's
    // volume is lower than the threshold, it gets hidden.
    public float volumeThreshold;
    public Material transparentMaterial;

    public Material glowingMaterial;
    // Distance between the door and the plyer below which the door gets hidden.
    public float hideDoorThreshold;

    // Threshold to decide if a component should . If the component's
    // volume is lower than the threshold, it gets hidden.
    public float showRingVolumeThreshold;

    public GameObject ringPrefab;

    public static DoorManager Instance;
    // sets up the singleton instance and ensures the SceneManager persists across scenes
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Log(object message) {
        Debug.Log("[Door " + gameObject.name + "] " + message);
    }

    private Vector3 planeScale = Vector3.zero;
    private Vector3 scale = Vector3.zero;
    private Vector3 position = Vector3.zero;
    private Quaternion rotation = Quaternion.identity;

    public void SetPlaneScale(Vector3 ps) {
        Log($"Setting scale to = {ps}");
        planeScale = ps;
    }
    public void SetPosition(Vector3 p) {
        Log($"Setting position to = {p}");
        position = p;
    }
    public void SetRotation(Quaternion r) {
        Log($"Setting rotation to = {r}");
        rotation = r;
    }
    public float ScaleFactor() {
        Log("Getting scale factor");
        Assert.IsFalse(scale == Vector3.zero);
        return scale.x;
    }

    public float VolumeFactor() {
        return Mathf.Pow(ScaleFactor(), 3);
    }

    // Recursively get all door components, filtered by the given function.
    public List<GameObject> GetDoorComponents(Func<GameObject, bool> f) {
        List<GameObject> list = new ();
        Queue<Transform> q = new ();
        Transform t = transform.Find("DoorContainer/DoorScale/_/Door 1");
        foreach(Transform c in t)
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
        return mesh != null && (mesh.bounds.Volume() * VolumeFactor()) <= volumeThreshold;
    }

    void SetMaterial(GameObject t, Material mat) {
        var renderer = t.GetComponent<Renderer>();
        if (renderer == null)
            return;
        renderer.materials = new Material[]{mat};
    }

    // Disable all components smaller than the provided threshold.
    void RemoveSmallComponents() {
        var toRemove = GetDoorComponents(ShouldRemoveComponent);
        Log("Hiding " + toRemove.Count() + " components");
        foreach(var t in toRemove)
            t.SetActive(false);
    }

    private HashSet<GameObject> currentlyHighlighted = new();

    public void HighlightComponents(HashSet<string> toHighlightNames, bool showRing = true) {
        var toHighlight = GetDoorComponents(go => toHighlightNames.Contains(go.name));
        
        // activate the component if not active (for small components that are hide) 
        foreach(GameObject go in toHighlight)
            if(!go.activeSelf)
                go.SetActive(true);

        Debug.Log("[Door " + gameObject.name + "] Highlighting " + toHighlight.Count() + " components");

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
            if((mesh.bounds.Volume() * VolumeFactor()) <= showRingVolumeThreshold && showRing)
                DisplayRing(go);
        }

        currentlyHighlighted = Enumerable.ToHashSet(toHighlight);
    }

    // Mappings from small object to its relative ring being currently displayed
    Dictionary<GameObject, GameObject> gameObjectsWithRing = new ();

    // Displays a ring on 
    void DisplayRing(GameObject go) {
        var mesh = go.transform.GetComponent<MeshRenderer>();
        Log("Showing ring on name=" + go.name + ", volume=" + (mesh.bounds.Volume() * VolumeFactor()));

        var ring = Instantiate(ringPrefab);
        ring.transform.SetParent(go.transform);
        ring.transform.position = go.transform.position;
        ring.transform.localScale = new Vector3(1f, 1f, 1f);
        gameObjectsWithRing[go] = ring;
    }

    void RemoveRing(GameObject go) {
        Log("Hiding ring on " + go.name);

        var ring = gameObjectsWithRing[go];
        gameObjectsWithRing.Remove(go);
        Destroy(ring);
    }
    private Bounds DoorBounds(GameObject g) {
        // From: https://gamedev.stackexchange.com/a/86999
        var b = new Bounds(g.transform.position, Vector3.zero);
        foreach (Renderer r in g.GetComponentsInChildren<Renderer>())
        {
            b.Encapsulate(r.bounds);
        }
        return b;
    }

    void ComputeScale() {
        Transform doorScaleContainer = transform.Find("DoorContainer/DoorScale");
        doorScaleContainer.localScale = Vector3.one;
        var doorScale = DoorBounds(doorScaleContainer.gameObject).size;
        var sc = Mathf.Max(planeScale.x / doorScale.x, planeScale.y / doorScale.y, planeScale.z / doorScale.z);
        scale = new Vector3(sc, sc, sc);
        doorScaleContainer.localScale = scale;
    }

    public void UpdatePosition() {
        Assert.IsFalse(planeScale == Vector3.zero);
        Assert.IsFalse(position == Vector3.zero);
        Assert.IsFalse(rotation == Quaternion.identity);

        // Set the door position
        transform.position = position;
        transform.rotation = rotation;

        ComputeScale();
        Assert.IsFalse(scale == Vector3.zero);
        Transform doorScaleContainer = transform.Find("DoorContainer/DoorScale");
        doorScaleContainer.gameObject.SetActive(true);

        transform.Find("DoorContainer").localPosition = Vector3.zero;
    }

    void Start()
    {
        RemoveSmallComponents();
        foreach(var t in GetDoorComponents(go => !currentlyHighlighted.Contains(go)))
            SetMaterial(t, transparentMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        // Make the rings face the player
        foreach(var go in gameObjectsWithRing.Values)
            go.transform.LookAt(Camera.main.transform);

        // Hide the door when it's too close to the player
        var distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        // TODO: hide instead of doing SetActive(false) so this logic doesn't stop working.
        if (distance < hideDoorThreshold)
        {
            if(gameObject.activeSelf)
                gameObject.SetActive(false);
        }
        else
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }
    }
}