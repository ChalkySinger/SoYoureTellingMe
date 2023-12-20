using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HobDial : MonoBehaviour
{

    [SerializeField] float turnSpeed = 1f;
    [SerializeField] float fireLevel = 1f;
    [SerializeField] float fireEmmisionVal = 3f;


    [Range(-120f,120f)] float DialVal = -120f;

    private Vector3 PosIn = new Vector3(0.7f,2.1f,2.2f);
    private Quaternion RotIn = new Quaternion(0, -120, 0, 90);

    [SerializeField] ParticleSystem FireSystem;

    // Start is called before the first frame update
    void Start()
    {
        //transform.Rotate(0, -120f, 0);

        transform.SetPositionAndRotation(PosIn, RotIn);
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKey(KeyCode.Q))
        {
            DialVal -= Time.deltaTime * turnSpeed;
            
        }
        else if (Input.GetKey(KeyCode.E))
        {
            DialVal += Time.deltaTime * turnSpeed;
        }
  

        DialVal = Mathf.Clamp(DialVal, -120f, 120f);

        Vector3 rot = transform.localEulerAngles;
        rot.y = DialVal;
        transform.localEulerAngles = rot;

        //transform.Rotate(0, DialVal, 0);

        fireLevel = MapValue(DialVal, -120f, 120f, 1f, 100f);
        //Debug.Log("Fire Level: " + fireLevel);


        FireParticleUpdate();
     
    }


    //map float value from one range to another
    float MapValue(float InputVal, float min1, float max1, float min2, float max2)
    {
        InputVal = Mathf.Clamp(InputVal, min1, max1);
        float mappedVal = min2 + (InputVal - min1) * (max2 - min2) / (max1 - min1);

        return mappedVal;
    }


    //change the emission value of the fire particles using mapped value
    void FireParticleUpdate()
    {
        ParticleSystem.EmissionModule fireEmmision = FireSystem.emission;


        fireEmmisionVal = MapValue(fireLevel, 1f, 100f, 3f, 35f);
        fireEmmision.rateOverTime = fireEmmisionVal;
    }

    public float GetFireLevel()
    {
        return fireLevel;
    }
}
