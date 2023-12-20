using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{

    [Header ("Fire Slider Dial")]
    [SerializeField] private Slider fireLevelSlider;

    [SerializeField] private HobDial hobScript;


    [Header ("Fire Level")]
    [SerializeField] Transform startPivot;
    [SerializeField] Transform endPivot;
    [SerializeField] Transform movingFireLevel;
    [SerializeField] Transform handle;


    float firePos, fireDestination, moveSpeed, fireTimer;
    [SerializeField] float timerMult = 5f, smoothSpeed = 1f;

    bool insideFireSection;

    [Header("Progress Bar")]
    [SerializeField] Slider progressSlider;
    [SerializeField] float progressIncreaseVal = 1f; //how much to increase by
    [SerializeField] float progressBarTimerCountdown = .5f; //how often to increase

    float progressBarTimer;
   
        
    void Start()
    {
        fireLevelSlider.value = 1;
    }

    
    void Update()
    {
        SetFireSlider();
        MoveFireLevel();

        CheckHandlePos();

        ProgressSliderSection();
    }

    void SetFireSlider()
    {
        fireLevelSlider.value = hobScript.GetFireLevel();

    }

    void MoveFireLevel()
    {
        fireTimer -= Time.deltaTime;
        if(fireTimer < 0)
        {
            fireTimer = Random.value * timerMult;
            fireDestination = Random.value;

        }

        firePos = Mathf.SmoothDamp(firePos, fireDestination, ref moveSpeed, smoothSpeed);
        movingFireLevel.position = Vector3.Lerp(startPivot.position, endPivot.position, firePos);


    }

    void CheckHandlePos()
    {
        if((handle.position.x > movingFireLevel.position.x - 50) && (handle.position.x < movingFireLevel.position.x + 50))
        {
            insideFireSection = true;
        }
        else
        {
            insideFireSection = false;
        }
    }


    void ProgressSliderSection()
    {
        if (insideFireSection)
        {
            progressBarTimer -= Time.deltaTime;
            if (progressBarTimer < 0)
            {
                progressSlider.value += progressIncreaseVal;

                progressBarTimer = progressBarTimerCountdown;
            }
            
        }
        else
        {
            progressBarTimer = progressBarTimerCountdown;
        }
        
    }
}
