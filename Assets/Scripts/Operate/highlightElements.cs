using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cahngeObjectsVisibility : MonoBehaviour
{
    
    public Material glowingMaterial;
    public Material transparentMaterial;
    public GameObject Door;



    public void highlightObjects(List<string> componentsToHighlight)
    {
        Renderer[] components = Door.GetComponentsInChildren<Renderer>();
        
        foreach (Renderer component in components)
        {
            component.materials = new Material[0];
            Debug.Log(component.name);

            if (componentsToHighlight.Contains(component.name))
            {
                component.material = glowingMaterial;
            }else
            {
                component.material = transparentMaterial;
            }
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
