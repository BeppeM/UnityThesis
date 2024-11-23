using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCameraScript : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject SupermarketView;
    public GameObject BarView;    


    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            ActiveMainCamera();
        }

        if (Input.GetKeyDown("1"))
        {
            CameraOne();
        }

        if (Input.GetKeyDown("2"))
        {
            CameraTwo();
        }
    }

    private void ActiveMainCamera()
    {
        MainCamera.SetActive(true);
        SupermarketView.SetActive(false);
        BarView.SetActive(false);
    }

    void CameraOne()
    {
        MainCamera.SetActive(false);
        SupermarketView.SetActive(true);
        BarView.SetActive(false);        
    }

    void CameraTwo()
    {
        MainCamera.SetActive(false);
        BarView.SetActive(true);
        SupermarketView.SetActive(false);
    }

}