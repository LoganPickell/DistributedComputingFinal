using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestListener : MonoBehaviour
{
    private UnityAction onTest;

    private void Awake()
    {
        onTest = new UnityAction(OnTest);
    }

    private void OnEnable()
    {
        EventManager.StartListening("Test", onTest);
        
    }

    private void OnDisable()
    {
        EventManager.StopListening("Test", onTest);
    }

    private void OnTest()
    {
        //Debug.Log($"{gameObject.name} has received the message 'test'!");
        Debug.Log(name == "Hex" ? "No, that's my dialog Lex." : "No, by causing the death of innocent people.");
    }
}