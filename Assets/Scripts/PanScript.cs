using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanScript : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float rotateSpeed = 1f, zSpeed = 0.5f;

    float mouseX, mouseY, rotateZ;

    [SerializeField] Arduino arduino;

    Vector3 gyro;
    [SerializeField] int gyroMult = 5;

    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        //rb = GetComponentInChildren<Rigidbody>();
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
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
        //-------------------------------------------------------


        //-------------Pan rotate using rb------------------------
        //Vector3 rotVector = new Vector3(-mouseY, rotateZ, mouseX);
        //Quaternion rotQuat = Quaternion.Euler(rotVector * Time.fixedDeltaTime);
        //rb.MoveRotation(rb.rotation * rotQuat);
        //--------------------------------------------------------



        //----------Controller Section-----------------------------
        gyro.x = arduino.GetGyroVal().x;
        gyro.y = arduino.GetGyroVal().z;
        gyro.z = arduino.GetGyroVal().y;

        Quaternion rotQuat = Quaternion.Euler(gyro * Time.fixedDeltaTime * gyroMult);
        rb.MoveRotation(rb.rotation * rotQuat);


    }
}
