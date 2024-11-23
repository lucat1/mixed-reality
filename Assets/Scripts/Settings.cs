using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    // Default Constructor
    private Settings() {}

    private static Settings instance;

    public static Settings Instance
    {
        get
        {
            if (instance == null)
                instance = new Settings();
            return instance;
        }
    }

    // Settings
    public bool MenusFaceYou = true;
    public bool MenusFollowYou = false;
}
