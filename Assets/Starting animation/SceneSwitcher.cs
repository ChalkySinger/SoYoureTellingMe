using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    void Start()
    {
        // Call the SwitchScene function after 27 seconds
        Invoke("SwitchScene", 28f);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SwitchScene();
        }
    }

    void SwitchScene()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}