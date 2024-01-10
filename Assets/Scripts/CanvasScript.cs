using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
//using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour
{
    [Header("Arduino")]
    [SerializeField] Arduino arduino;
    //----------joystick----------
    //Vector3 joystickVal;
    bool joystickButtonDown = false, buttonHeld = false, buttonWasPressed = false, selectIngredient = false, executeSelection = false;

    float joyX, joyY;
    Vector3 mappedJoystickVector;
    //----------------------------

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


    [Header("Ingredient Spawn")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject[] ingredients;

    bool spawnIngredient;
    string chosenItem;

    void Start() 
    {
        fireLevelSlider.value = 1;

        radialMenu.SetActive(false);
        radialMenuActive = false;

        joystickButtonDown = false;
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


        JoystickInputs();
        UseJoyStick();

        ToMainMenu();


        //to instantiate ingredients
        if(spawnIngredient)
        {
            if (chosenItem == "Sauce")  //particles
            {
                SpawnIngredients(1, 1, ingredients[0], Random.rotation);
            }
            if (chosenItem == "Green Onion")    //particles
            {
                SpawnIngredients(1, 1, ingredients[1], Quaternion.identity);
            }
            if (chosenItem == "Shrimp") //object
            {
                SpawnIngredients(3, 10, ingredients[2], Quaternion.identity);
            }
            if (chosenItem == "Egg")    //object
            {
                SpawnIngredients(1, 3, ingredients[3], Quaternion.identity);
            }
            if (chosenItem == "Rice")   //particles
            {
                SpawnIngredients(1, 1, ingredients[4], Quaternion.identity);
            }

            spawnIngredient = false;
        }
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


        if(radialMenuActive)
        {
            radialMenu.SetActive(true);
        }
        else
        {
            radialMenu.SetActive(false);
        }

    }
    //-----------------------------------------------------------


    //select item based on mouse around the center (will change for joystick)
    //when chosen, close menu and say which item was chosen
    void SelectRadialMenu()
    {
        if (radialMenuActive)
        {
            Vector2 mousePos = middle.position - /*Input.mousePosition*/ mappedJoystickVector;           //get pos from the middle and use ATan2 for circular movement
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

                    if (selectIngredient)           //when item chosen
                    {
                        selectIngredient = false;   //stop loop

                        SpawnTextPopUp(items[currentItem].name);    //show which item was chosen

                        //-------INSTANTIATE INGREDIENT CODE HERE------
                        Debug.Log("spawn ingredient");


                        chosenItem = items[currentItem].name;
                        spawnIngredient = true;
                        

                        //---------------------------------------------
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

    void JoystickInputs()
    {
        //joystickVal = arduino.GetJoyVal();

        Debug.Log("X: " + arduino.GetJoyVal().x + " Y: " + arduino.GetJoyVal().y + " Button: " + arduino.GetJoyVal().z);

        if(arduino.GetJoyVal().z == 0)
        {
            joystickButtonDown = true;

        }
        else
        {
            joystickButtonDown = false;

        }


        //x and y mapped to window size
        joyX = MapValue(arduino.GetJoyVal().x, 0f, 1023f, 0f, 1920f);
        joyY = MapValue(arduino.GetJoyVal().y, 0f, 1023f, 1080f, 0f);

        mappedJoystickVector = new Vector2 (joyX, joyY);
    }

    //this code will check when the button is held to show the menu, then when button is released, the item is selected and then the menu is closed
    void UseJoyStick()
    {
        if (joystickButtonDown && !buttonWasPressed)  //button is down and has not been released
        {
            radialMenuActive = true;    //show ingredient menu

            buttonWasPressed = true;
            buttonHeld = true;
            executeSelection = false;   //ingredient has not been selected so do not execute selection
        }
        else if(buttonWasPressed && joystickButtonDown)   //button was pressed down and still is
        {
            buttonHeld = true;  //button is held

        }
        else if(buttonWasPressed && !joystickButtonDown && buttonHeld && !executeSelection)     //button was pressed down, and now isnt pressed
        {                                                                                       //also the selection has not happened
            buttonWasPressed = false;
            buttonHeld = false;

            executeSelection = true;    //allow selection to happen
            selectIngredient = true;    //this goes to the selection on the radial menu to say the button has been released 

        }
        else if (!joystickButtonDown)   //button is not held down
        {
            buttonWasPressed = false;
            buttonHeld = false;
            if(executeSelection)            //selection has now happened so...
            {
                radialMenuActive = false;   //turn the ingredient menu off
            }
        }

    }


    float MapValue(float InputVal, float min1, float max1, float min2, float max2)
    {
        InputVal = Mathf.Clamp(InputVal, min1, max1);
        float mappedVal = min2 + (InputVal - min1) * (max2 - min2) / (max1 - min1);

        return mappedVal;
    }

    void ToMainMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    void SpawnIngredients(int minAmount, int maxAmount, GameObject ingredient, Quaternion rot)
    {
        int randAmount = Random.Range(minAmount, maxAmount);

        for(int i = 0; i < randAmount; i++)
        {

            Instantiate(ingredient, spawnPoint.position, rot);




        }
    }

}
