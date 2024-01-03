using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public Button switchButton;

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
        // Switch the enabled state of the cameras
        camera1.enabled = !camera1.enabled;
        camera2.enabled = !camera2.enabled;
    }
}
