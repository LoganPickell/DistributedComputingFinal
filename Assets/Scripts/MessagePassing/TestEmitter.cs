using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEmitter : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            EventManager.TriggerEvent("Test");
            Debug.Log("Is that how a warped brain like yours gets its kicks? By planning the death of innocent people?");
        }
    }
}