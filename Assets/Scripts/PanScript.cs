using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PanScript : MonoBehaviour
{
    Rigidbody rb;
    SoundFXManager sfx;
    [SerializeField] Arduino arduino;

    Vector3 gyro;
    [SerializeField] int gyroMult = 5;

    //[Header("Ingredients in Pan Check")]
    List<GameObject> itemsInPan = new List<GameObject>();


    bool cooking;
    float timeInTrigger;
    [SerializeField] float boolTrueTime = .7f;
    
    

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
        gyro.x = arduino.GetGyroVal().x;
        gyro.y = arduino.GetGyroVal().z;
        gyro.z = arduino.GetGyroVal().y;

        Quaternion rotQuat = Quaternion.Euler(-gyro * Time.fixedDeltaTime * gyroMult);
        rb.MoveRotation(rb.rotation * rotQuat);


    }

    private void Update()
    {

        CheckIngrMovement();

        if(cooking)
        {
            Debug.Log("uhh cooking");
        }
        else
        {
            Debug.Log("uhh not cooking");
        }
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("ingredients"))
        {

            itemsInPan.Add(col.gameObject);

        }

    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("ingredients"))
        {

            itemsInPan.Remove(col.gameObject);
            
        }
    }

    void CheckIngrMovement()
    {
        foreach (GameObject item in itemsInPan)
        {
            Vector3 itemVel = item.GetComponent<Rigidbody>().velocity;

            if(itemVel.magnitude > 1)
            {
                Debug.Log("Vel: " +  itemVel.magnitude);

                timeInTrigger += Time.deltaTime;

                if(timeInTrigger < boolTrueTime)
                {
                    cooking = true;
                }
                else
                {
                    cooking = false;
                    timeInTrigger = 0;
                }

                sfx.AudioTrigger(SoundFXManager.SoundFXTypes.Sizzle, transform.position);
            }
            else
            {
                cooking = false;
                timeInTrigger = 0;
            }
        }
    }

    public bool IsCooking()
    {
        return cooking;
    }
}
