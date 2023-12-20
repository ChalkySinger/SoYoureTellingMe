using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanScript : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 1f, zSpeed = 0.5f;

    float mouseX, mouseY, rotateZ;
 

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        //--------------Pan Test Rotation with Mouse------------
        mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
        mouseY = Input.GetAxis("Mouse Y") * rotateSpeed;
        if(Input.GetMouseButton(0))
        {
            rotateZ = -1 * zSpeed;
        }
        else if(Input.GetMouseButton(1))
        {
            rotateZ = 1 * zSpeed;
        }
        else
        {
            rotateZ = 0;
        }
        transform.Rotate(mouseY, rotateZ, mouseX);
        //-------------------------------------------------------
    }
}
