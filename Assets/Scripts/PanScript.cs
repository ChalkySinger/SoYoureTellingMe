using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanScript : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] Arduino arduino;

    Vector3 gyro;
    [SerializeField] int gyroMult = 5;


    //[Header("Ingredients in Pan Check")]
    List<GameObject> itemsInPan = new List<GameObject>();

    List<ParticleSystem.Particle> particlesInPan = new List<ParticleSystem.Particle>();

    GameObject[] allItems;

    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
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
        Debug.Log("pan: " + itemsInPan.Count /*+ " , " + allItems.Length*/);
        
        //Debug.Log("particles: " +  particlesInPan.Count);

        /*allItems = GameObject.FindGameObjectsWithTag("ingredients");
        foreach (GameObject item in allItems)
        {
            itemsInPan.Append(item);
        }*/

    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("ingredients"))
        {
            Debug.Log("In pan");

            itemsInPan.Add(col.gameObject);
        }

    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("ingredients"))
        {
            Debug.Log("Not in pan");

            itemsInPan.Remove(col.gameObject);
        }
    }

    /*private void OnParticleTrigger()
    {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particlesInPan);
    }*/


}
