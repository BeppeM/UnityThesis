using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCameraScript : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject Camera1;
    public GameObject Camera2;    


    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            ActiveMainCamera();
        }

        if (Input.GetKeyDown("1"))
        {
            ActiveCameraOne();
        }

        if (Input.GetKeyDown("2"))
        {
            ActiveCameraTwo();
        }
    }

    private void ActiveMainCamera()
    {
        MainCamera.SetActive(true);
        Camera1.SetActive(false);
        Camera2.SetActive(false);
    }

    void ActiveCameraOne()
    {
        MainCamera.SetActive(false);
        Camera1.SetActive(true);
        Camera2.SetActive(false);        
    }

    void ActiveCameraTwo()
    {
        MainCamera.SetActive(false);
        Camera2.SetActive(true);
        Camera1.SetActive(false);
    }

}