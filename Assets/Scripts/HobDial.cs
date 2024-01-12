//using Palmmedia.ReportGenerator.Core.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HobDial : MonoBehaviour
{
    //[SerializeField] Arduino arduino;


    [SerializeField] float fireLevel = 1f;
    [SerializeField] float fireEmmisionVal = 3f;
    int potVal, oldPotVal;


    [SerializeField] ParticleSystem FireSystem;


    void Start()
    {

    }


    void Update()
    {
        //--------Potentiometer dial---------------------   potentiometer: (1-4) - (886-887)

        potVal = Arduino.instance.GetPotVal();

        float mappedPotVal = MapValue(potVal, 2f, 886f, -30f, 210f);

        //-----------------------------------------------


        Vector3 rot = transform.localEulerAngles;
        rot.z = mappedPotVal;
        transform.localEulerAngles = rot;
        
        fireLevel = MapValue(mappedPotVal, -30f, 210f, 1f, 100f);

        FireParticleUpdate();

        if(potVal > oldPotVal + 75 || potVal < oldPotVal - 75)
        {
            FindObjectOfType<SoundFXManager>().AudioTrigger(SoundFXManager.SoundFXTypes.HobDial, transform.position, true);
            oldPotVal = potVal;
        }
     
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
