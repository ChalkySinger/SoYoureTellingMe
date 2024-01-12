using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PanScript : MonoBehaviour
{
    Rigidbody rb;
    SoundFXManager sfx;
    //[SerializeField] Arduino arduino;

    Vector3 gyro;
    [SerializeField] int gyroMult = 5;


    bool cooking;
    float panVel;
    [SerializeField] float boolTrueTime = .3f;
    
    

    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();

        sfx = FindObjectOfType<SoundFXManager>();
    }


    void FixedUpdate()
    {

        //---------------- Pan Rotate With Controller -----------------------------
        gyro.x = Arduino.instance.GetGyroVal().x;
        gyro.y = Arduino.instance.GetGyroVal().z;
        gyro.z = Arduino.instance.GetGyroVal().y;

        Quaternion rotQuat = Quaternion.Euler(-gyro * Time.fixedDeltaTime * gyroMult);
        rb.MoveRotation(rb.rotation * rotQuat);


    }

    private void Update()
    {

        CheckkMovement();

        if(cooking)
        {
            Debug.Log("uhh " + panVel);

        }
        else
        {
            Debug.Log("uhh not cooking");
        }
    }


    void CheckkMovement()
    {
        /*foreach (GameObject item in itemsInPan)
        {
            Vector3 itemVel = item.GetComponent<Rigidbody>().velocity;

            if(itemVel.magnitude > 1)
            {
                Debug.Log("Vel: " +  itemVel.magnitude);

                timeInTrigger += Time.deltaTime;
                cooking = true;

                if (timeInTrigger > boolTrueTime)
                {
                    cooking = false;
                    timeInTrigger = 0;
                }


                sfx.AudioTrigger(SoundFXManager.SoundFXTypes.Sizzle, transform.position);
            }
            *//*else
            {
                cooking = false;
                timeInTrigger = 0;
            }*//*
        }*/


        panVel = rb.rotation.eulerAngles.magnitude * 0.1f;

        if(panVel > 48)
        {
            cooking = true;

            sfx.AudioTrigger(SoundFXManager.SoundFXTypes.Sizzle, transform.position);
        }
        else
        {
            cooking = false;
        }
    }

    public bool IsCooking()
    {
        return cooking;
    }
}
