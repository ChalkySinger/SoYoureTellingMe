using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public Button switchButton;
    //public Animator transition;
    
    //public int Manager;

    private void Start()
    {
        // Ensure only the first camera is initially enabled
        camera1.enabled = true;
        camera2.enabled = false;

        // Add a listener to the button to call the SwitchCameras method
        switchButton.onClick.AddListener(SwitchCameras);
    }

    private void SwitchCameras()
    {
        //transition.SetTrigger("Start");

        // Switch the enabled state of the cameras
        camera1.enabled = !camera1.enabled;
        camera2.enabled = !camera2.enabled;
        
    }


    //pulic void ManageCamera()
    //{
    //    if(Manager == 0)
    //    {
    //        Cam_2();
    //        Manager = 1;
    //    }
    //    else
    //    {
    //        Cam_1();
    //        Manager = 0;
    //    }
    //}

    //void Cam_1()
    //{
    //    camera1.SetActive(true);
    //    camera2.SetActive(false);
    //}

    //void Cam_()
    //{
    //    camera2.SetActive(true);
    //    camera1.SetActive(false);
    //}
   
}
