using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticField : MonoBehaviour
{
    public static MagneticField magneticField;
    public bool isRedm = true;
    [HideInInspector] public Transform ts = null;

    void Start()
    {
        if (magneticField == null)
        {
            magneticField = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  ts = this.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("red"))
        {
            isRedm = true;

            ts = other.transform;
        }
        else if (other.CompareTag("blue"))
        {
            ts = other.transform;

            isRedm = false;
        }
    }
}