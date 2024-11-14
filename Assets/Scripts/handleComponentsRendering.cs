using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class handleComponentsRendering : MonoBehaviour
{
    public GameObject Door;
    public DataLoader data;

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
        Debug.Log("running small components ereasing");
        int componentsEreased = 0;
        foreach(string component in data.smallComponentsList)
        {
            GameObject componentObj = recursiveFind(Door.transform, component);
            if(componentObj)
            {
                componentObj.SetActive(false);
                componentsEreased++;
            }
        }

        Debug.Log("Small component ereased: " + componentsEreased + "/" + data.smallComponentsList.Count);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
