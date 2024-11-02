using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class cahngeObjectsVisibility : MonoBehaviour
{
    
    public Material glowingMaterial;
    public Material transparentMaterial;
    public GameObject Door;
    private List<string> Highlited = new List<string>();
    
    public void highlightObjects(List<string> componentsToHighlight)
    {

        List<string> toHighlight = componentsToHighlight.Except(Highlited).ToList();
        List<string> toHide = Highlited.Except(componentsToHighlight).ToList();
        foreach(string componentName in toHighlight){
            Renderer childrenToHighlight = Door.transform.Find(componentName)?.GetComponent<Renderer>();
            childrenToHighlight.materials = new Material[0];
            childrenToHighlight.material = glowingMaterial;

            Highlited.Add(componentName);        
        }

        foreach(string componentName in toHide){
            Renderer childrenToHide = Door.transform.Find(componentName)?.GetComponent<Renderer>();
            childrenToHide.materials = new Material[0];
            childrenToHide.material = transparentMaterial;

            Highlited.Remove(componentName);           
        }
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
        
    }
}
