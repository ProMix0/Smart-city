using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewSW : MonoBehaviour
{
    private Camera GameCam;
    private Camera MenuCam;
    void Start()
    {
        GameCam = GetComponent<Camera>();
        GameCam = Camera.main;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            GameCam.enabled = !GameCam.enabled;
            MenuCam.enabled = !MenuCam.enabled;
        }
    }
}
