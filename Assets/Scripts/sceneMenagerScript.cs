using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Add this line

public class sceneMenagerScript : MonoBehaviour
{

    public void LoadNewScene(string scene)
    {
        SceneManager.LoadScene(scene); // Replace "SceneName" with your actual scene name
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
