using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.InputSystem.Controls;
using MixedReality.Toolkit;


public class handleComponentsRendering : MonoBehaviour
{
    public GameObject Door;
    public DataLoader data;

    public float volumeThreshold;

    private GameObject recursiveFind(Transform parent, string target) {
        Queue<Transform> q = new ();
        foreach(Transform c in parent)
            q.Enqueue(c);

        while(q.Count > 0) {
            var child = q.Dequeue();
            if(child.name == target)
                return child.gameObject;
            
            foreach(Transform cc in child)
                q.Enqueue(cc);
        }

        return null;
    }

    // Start is called before the first frame update
    // On start we delete the small components
    void Start()
    {
        // Debug.Log("running small components ereasing");
        // int componentsEreased = 0;
        // foreach(string component in data.smallComponentsList)
        // {
        //     GameObject componentObj = recursiveFind(Door.transform, component);
        //     if(componentObj)
        //     {
        //         componentObj.SetActive(false);
        //         componentsEreased++;
        //     }
        // }

        
        int hidden = 0;
        int cnt = 0;
        Queue<Transform> q = new ();
        foreach(Transform c in Door.transform)
            q.Enqueue(c);

        while(q.Count > 0) {
            ++cnt;
            var child = q.Dequeue();
            var mesh = child.GetComponent<MeshRenderer>();
            if (mesh != null && mesh.bounds.Volume() <= volumeThreshold) {
                Debug.Log("Comp: " + child.gameObject.name + ", volume: " + mesh.bounds.Volume());
                child.gameObject.SetActive(false);
                ++hidden;
            }
            
            foreach(Transform cc in child)
                q.Enqueue(cc);
        }

        Debug.Log("Small component hidden: " + hidden + "/" + cnt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
