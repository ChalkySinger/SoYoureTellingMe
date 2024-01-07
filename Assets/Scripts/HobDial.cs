using Palmmedia.ReportGenerator.Core.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HobDial : MonoBehaviour
{
    [SerializeField] Arduino arduino;


    [SerializeField] float fireLevel = 1f;
    [SerializeField] float fireEmmisionVal = 3f;
    int potVal;


    //[Range(-120f,120f)] float DialVal = -30;


    [SerializeField] ParticleSystem FireSystem;


    void Start()
    {

    }


    void Update()
    {
        //--------Potentiometer dial---------------------   potentiometer: (1-4) - (886-887)
        string stringVal = arduino.InputText;
        potVal = int.Parse(stringVal);
        Debug.Log("value: " + potVal);

        float mappedPotVal = MapValue(potVal, 2f, 886f, -30f, 210f);

        //-----------------------------------------------


        //---------Keyboard dial--------------------
        /*if (Input.GetKey(KeyCode.Q))
        {
            DialVal -= Time.deltaTime * turnSpeed;
            
        }
        else if (Input.GetKey(KeyCode.E))
        {
            DialVal += Time.deltaTime * turnSpeed;
        }*/
        //-------------------------------------------


        Vector3 rot = transform.localEulerAngles;
        rot.z = MapValue(potVal, 1f, 886f, -30f, 210f); ;
        transform.localEulerAngles = rot;
        
        fireLevel = MapValue(mappedPotVal, -30f, 210f, 1f, 100f);

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
