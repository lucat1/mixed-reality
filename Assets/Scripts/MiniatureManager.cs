using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniatureManager : MonoBehaviour
{

    // this parameter controll the max distance within a piece is considered to be near another one
    public GameObject door;
    public DoorManager doorManager;
    public float nearObjectRadius;
    private List<string> compNames= new List<string> {"_25_310_0565_602","_25_802_1132_364","_25_375_0205_301"};

    List<GameObject> GetNearComponents(GameObject center){
        List<GameObject> pr =  doorManager.GetDoorComponents(ob => (ComputeComponentsDist(center, ob) <= nearObjectRadius && ob.transform.childCount == 0));
        return pr;
    }

    float ComputeComponentsDist(GameObject center, GameObject other){
        double distanceSquared = Math.Pow(center.transform.position.x - other.transform.position.x, 2) + Math.Pow(center.transform.position.y - other.transform.position.y,2) + Math.Pow(center.transform.position.z - other.transform.position.z,2);
        return (float)Mathf.Sqrt((float)distanceSquared);
    }

    private HashSet<GameObject> currentlyActive = new();

    public void DisplayComponent(GameObject component){
        // display a the target component hilighted
        component.SetActive(true);

        ActivateParents(component.transform);
        doorManager.HighlightComponents(new HashSet<string> {component.name}, false);
        // display the near components as transparent
        foreach(GameObject ob in GetNearComponents(component))
        {
            ob.SetActive(true);
            ob.transform.SetParent(component.transform);
            ActivateParents(ob.transform);
        }

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
        countIndex++;
        DeactivateAll();
        DisplayComponent(doorManager.GetDoorComponents(go => go.name == compNames[countIndex])[0]);
    }
    public void Previous(){
        countIndex--;
        DeactivateAll();
        DisplayComponent(doorManager.GetDoorComponents(go => go.name == compNames[countIndex])[0]);
    }

    void DeactivateAll(){
        foreach(GameObject go in doorManager.GetDoorComponents(_ => true))
            go.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

        DeactivateAll();
        DisplayComponent(doorManager.GetDoorComponents(go => go.name == compNames[countIndex])[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
