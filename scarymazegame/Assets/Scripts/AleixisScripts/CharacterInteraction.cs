﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    [Header ("Declaring Variables")]
    public bool InRange;

    // Merchant Interaction
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Merchant"))
        {
            InRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Merchant"))
        {
            InRange = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(InRange);
        if (InRange && Input.GetKey(KeyCode.E))
        {
            Debug.Log("The merchant is staring at you");
        }
    }
}
