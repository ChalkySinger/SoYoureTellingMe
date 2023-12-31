using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    float timeToDestroy = 1f;


    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }


}
