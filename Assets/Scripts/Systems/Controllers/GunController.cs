using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunController : MonoBehaviour, IController
{
    
    [SerializeField] private bool canFire;
    [SerializeField] private float tickRate = .05f;

    private float counter = 0;


    void Update()
    {
        if (!canFire)
        {
            counter = tickRate;
            return;
        }

        counter += Time.deltaTime;

        if (counter >= tickRate)
        {
            counter -= tickRate;

        }
    }
}