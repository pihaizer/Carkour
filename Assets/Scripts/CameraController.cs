using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera backVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera topVirtualCamera;

    private void Start()
    {
        backVirtualCamera.enabled = true;
        topVirtualCamera.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            backVirtualCamera.enabled = !backVirtualCamera.enabled;
            topVirtualCamera.enabled = !topVirtualCamera.enabled;
        }
    }
}
