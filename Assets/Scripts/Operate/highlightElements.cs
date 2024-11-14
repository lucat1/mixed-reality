using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Collections;
using UnityEngine.Assertions;
using UnityEditor.Animations;

public class cahngeObjectsVisibility : MonoBehaviour
{
    
    public Material glowingMaterial;
    public Material transparentMaterial;
    public GameObject smallComponentsSphere;
    public GameObject Door;
    public DataLoader data;
    private HashSet<string> Highlited = new ();
    private GameObject smallComponentsOutline;


    // Recursively search for children of a Transform with a BFS
    public Transform recursiveFind(Transform parent, string target) {
        Queue<Transform> q = new ();
        foreach(Transform c in parent)
            q.Enqueue(c);

        while(q.Count > 0) {
            var child = q.Dequeue();
            if(child.name == target)
                return child;
            
            foreach(Transform cc in child)
                q.Enqueue(cc);
        }

        return null;
    }

    private Renderer getRendererObject(string componentName) {
        return recursiveFind(Door.transform, componentName)?.GetComponent<Renderer>();
    }
    
    public void highlightObjects(HashSet<string> highlightSet)
    {
        foreach(string componentName in highlightSet){
            if (Highlited.Contains(componentName))
                // This component is already highlighted and we want to keep it like that
                continue;

            Debug.Log("highlighting: " + componentName);
            Renderer toHighlight = getRendererObject(componentName);
            Assert.IsNotNull(toHighlight);
            toHighlight.materials = new Material[0];
            toHighlight.material = glowingMaterial;

            // check if it's a small components and add cricle
            if(data.smallComponentsList.Contains(componentName))
            {
                Debug.Log("small component: " + componentName);
                Transform smallCompTr = recursiveFind(Door.transform, componentName);
                smallComponentsOutline = Instantiate(smallComponentsSphere);
                smallComponentsOutline.transform.SetParent(smallCompTr);
                smallComponentsOutline.transform.position = smallCompTr.transform.position;
                smallComponentsOutline.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            }
        }

        foreach(string componentName in Highlited){
            if (highlightSet.Contains(componentName))
                // This component is already highlighted and we want to keep it like that
                continue;

            Debug.Log("making transparent: " + componentName);
            Renderer toDeHighlight = getRendererObject(componentName);
            Assert.IsNotNull(toDeHighlight);
            toDeHighlight.materials = new Material[0];
            toDeHighlight.material = transparentMaterial;

            // also remove the sphere if is a small component
            if(data.smallComponentsList.Contains(componentName))
            {
                Destroy(smallComponentsOutline);
            }
        }

        Highlited = highlightSet;
    }

    public void hideComponents(List<string> componentsToHide)
    {
        Renderer[] components = Door.GetComponentsInChildren<Renderer>();
        
        foreach (Renderer component in components)
        {
            if(componentsToHide.Contains(component.name))
            {
                component.enabled = false;
            }
        }
    }

    public void hideAllComponents()
    {
        Renderer[] components = Door.GetComponentsInChildren<Renderer>();
        
        foreach (Renderer component in components)
        {
            component.materials = new Material[0];
            component.material = transparentMaterial;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Renderer[] components = Door.GetComponentsInChildren<Renderer>();
        
        foreach (Renderer component in components)
        {
            component.materials = new Material[0];
            component.material = transparentMaterial;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("debug: " + smallComponentsOutline);
        // make small components outline always face the player 
        if(smallComponentsOutline != null)
        {
            smallComponentsOutline.transform.LookAt(Camera.main.transform);
        }
    }
}
