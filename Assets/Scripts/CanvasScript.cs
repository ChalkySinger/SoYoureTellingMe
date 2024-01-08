using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    [Header("Arduino")]
    [SerializeField] Arduino arduino;
    Vector3 joystickVal;
    bool joystickButton = false;
    bool buttonWait;

    float joyX, joyY;
    Vector3 joystickVector;

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


    [Header("Radial Menu")]
    [SerializeField] Transform middle;
    [SerializeField] Transform select;

    [SerializeField] GameObject radialMenu;

    [SerializeField] TextMeshProUGUI selectedText;
    [SerializeField] GameObject[] items;

    [SerializeField] GameObject TextPopUp;
    [SerializeField] BoxCollider[] cols;

    bool radialMenuActive; 
    void Start() 
    {
        fireLevelSlider.value = 1;

        radialMenu.SetActive(false);
        radialMenuActive = false;
    }

    
    void Update()
    {
        //hob fire level
        MoveFireLevel();
        SetFireSlider();
        CheckHandlePos();

        //progress bar
        ProgressSliderSection();

        //radial menu
        ShowRadialMenu();
        SelectRadialMenu();


        Joystick();

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


    //----------show and hide ingredient menu when press F--------
    void ShowRadialMenu()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            radialMenuActive = !radialMenuActive;
            if (radialMenuActive)
            {
                radialMenu.SetActive(true);
            }
            else
            {
                radialMenu.SetActive(false);
            }
        }

        if (joystickVal.z == 0)
        //if(joystickButton)
        {
            radialMenuActive = !radialMenuActive;
            if (radialMenuActive)
            {
                radialMenu.SetActive(true);
            }
            else
            {
                radialMenu.SetActive(false);
            }
        }
    }
    //-----------------------------------------------------------


    //select item based on mouse around the center (will change for joystick)
    //when chosen, close menu and say which item was chosen
    void SelectRadialMenu()
    {
        if (radialMenuActive)
        {
            Vector2 mousePos = middle.position - /*Input.mousePosition*/ joystickVector;           //get mouse pos from the middle and use ATan2 for circular movement
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            angle += 180;                                                       //add 180 to use 0 - 360 rather than -180 - 180

            int currentItem = 0;

            int itemsAngle = 360 / 5;   //divide by 5 for the 5 items
            for(int i = 0; i < 360; i += itemsAngle)
            {
                if(angle >= i &&  angle < i + itemsAngle)
                {
                    select.eulerAngles = new Vector3(0, 0, i);      //rotate selector around z axis
                    selectedText.text = items[currentItem].name;    //change text to selected item


                    if (Input.GetMouseButtonDown(0))                            //when item chosen
                    {   
                        Debug.Log(items[currentItem].name + " is selected");    //debug

                        //text pop up here
                        SpawnTextPopUp(items[currentItem].name);

                        radialMenuActive = false;                               //close menu
                        radialMenu.SetActive(false);
                    }
                }

                currentItem++;
            }

        }
    }

    void SpawnTextPopUp(string ItemName)
    {
        int rand = Random.Range(0, cols.Length);
        GameObject popUp = Instantiate(TextPopUp, RandomPointInBounds(cols[rand].bounds), TextPopUp.transform.rotation);
        popUp.GetComponent<TextMeshPro>().text = "+ " + ItemName;

    }

    Vector3 RandomPointInBounds(Bounds bounds)
    {
        Vector3 point = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), 4.3f);

        return point;
    }

    void Joystick()
    {
        joystickVal = arduino.GetJoyVal();

        Debug.Log("X: " + joystickVal.x + " Y: " + joystickVal.y + " Button: " + joystickVal.z);

        //button
        if (joystickVal.z == 0)
        {
            if (!buttonWait)
            {
                buttonWait = true;
                StartCoroutine(ButtonWait());
            }

        }

        /*float timer = 0.5f;

        if(joystickVal.z == 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                joystickButton = !joystickButton;

                timer = 0.5f;
            }


        }
        Debug.Log("pressed: " + joystickButton);*/

        //x and y
        joyX = MapValue(joystickVal.x, 0f, 1023f, 0f, 1920f);
        joyY = MapValue(joystickVal.y, 0f, 1023f, 1080f, 0f);
        joystickVector = new Vector2 (joyX, joyY);
    }

    IEnumerator ButtonWait()
    {
        yield return new WaitForSeconds(.2f);

        buttonWait = false;
        joystickButton = !joystickButton;

    }


    float MapValue(float InputVal, float min1, float max1, float min2, float max2)
    {
        InputVal = Mathf.Clamp(InputVal, min1, max1);
        float mappedVal = min2 + (InputVal - min1) * (max2 - min2) / (max1 - min1);

        return mappedVal;
    }
}
