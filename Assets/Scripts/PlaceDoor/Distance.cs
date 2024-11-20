using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Assertions;

public class Distance : MonoBehaviour
{
    public GameObject door;
    public GameObject miniature;
    // Threshold to decide when to switch to the miniature
    public float activateMiniatureThreshold;

    private DoorManager dm;
    private MiniatureManager mm;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(door);
        Assert.IsNotNull(miniature);
        dm = door.GetComponent<DoorManager>();
        mm = miniature.GetComponent<MiniatureManager>();
        Hide();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public bool IsVisible() {
        return gameObject.activeSelf;
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Toggle() {
        if(IsVisible())
            Hide();
        else
            Show();
    }

    float DistanceFromPlayer() {
        return Vector3.Distance(Camera.main.transform.position, door.transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        Assert.IsNotNull(dm);
        Assert.IsNotNull(mm);
        if (dm.IsVisible() || mm.IsVisible())
        {
            if (DistanceFromPlayer() < activateMiniatureThreshold)
            {
                if (dm.IsVisible())
                    dm.Hide();
                if (!mm.IsVisible())
                    mm.Show();
            }
            else
            {
                if(!dm.IsVisible())
                    dm.Show();
                if(mm.IsVisible())
                    mm.Hide();
            }
        }
    }
}