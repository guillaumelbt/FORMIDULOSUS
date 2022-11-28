using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Light light;
    [SerializeField] private PlayerInput input;

    private float lifeTime = 100;
    
    private void Awake()
    {
        input.actions["Flash"].started += ctx =>
        {
            if (lifeTime <= 0) return;
            light.enabled = !light.enabled;
        };
    }

    private void Update()
    {
        if (light.enabled) lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) light.enabled = false;
    }
}
