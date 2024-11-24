using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.SpatialManipulation;
using UnityEngine.Assertions;

public class MenuHandler : MonoBehaviour
{

    // set the distance from the player when the object spawns
    public static float offset = 0.5f;

    // if true the object will spawn infront of the player
    public bool spawnInfront = true;

    // if the option is toggoled the object can follow you
    public bool canFollowYou = true;
    // some components are flipped so we need to invert the rotation along x axe
    public bool invertedX = false;

    Follow follow;
    // Start is called before the first frame update
    void Start()
    {
        OnEnable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Settings.Instance.MenusFaceYou)
        {
            transform.LookAt(Camera.main.transform);
            transform.eulerAngles += new Vector3(0, -180, 0);
            if (invertedX)
            {
                // Invert the x-axis rotation
                transform.eulerAngles -= new Vector3(2 * transform.eulerAngles.x, 0, 0);
            }
        }

        if (canFollowYou)
            follow.enabled = Settings.Instance.MenusFollowYou;
    }

    void OnEnable()
    {
        follow = gameObject.GetComponent<Follow>();
        Assert.IsNotNull(follow);
        Debug.Log("[MenuHandler] Found follow component: " + follow);

        // activate the object solver if menus follow you is enabled
        follow.enabled = Settings.Instance.MenusFollowYou;

        // make the menu spawn in front of the player when enabled
        if (spawnInfront)
        {
            Vector3 positionInFront = Camera.main.transform.position + Camera.main.transform.forward * offset;
            transform.position = positionInFront;
        }
    }
}
