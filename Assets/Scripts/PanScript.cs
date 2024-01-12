using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PanScript : MonoBehaviour
{
    Rigidbody rb;
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

        InvokeRepeating("PlaySizzle", 0f, 60f);

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

        CheckMovement();

        if(cooking)
        {
            Debug.Log("uhh " + panVel);
            

        }
        else
        {
            Debug.Log("uhh not cooking");
        }
    }


    void CheckMovement()
    {
        panVel = rb.rotation.eulerAngles.magnitude * 0.1f;

        if(panVel > 48)
        {
            cooking = true;

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

    void PlaySizzle()
    {
        FindObjectOfType<SoundFXManager>().AudioTrigger(SoundFXManager.SoundFXTypes.Sizzle, transform.position, true);
    }
}
