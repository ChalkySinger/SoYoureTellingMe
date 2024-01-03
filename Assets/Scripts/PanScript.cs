using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanScript : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float rotateSpeed = 1f, zSpeed = 0.5f;

    float mouseX, mouseY, rotateZ;
 

    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        //rb = GetComponentInChildren<Rigidbody>();
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        //--------------Set Pan Rotation with Mouse-------------
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
        //transform.Rotate(mouseY, rotateZ, mouseX);
        //-------------------------------------------------------


        //-------------Pan rotate using rb------------------------
        Vector3 rotVector = new Vector3(-mouseY, rotateZ, mouseX);
        Quaternion rotQuat = Quaternion.Euler(rotVector * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * rotQuat);
        //--------------------------------------------------------
    }
}
