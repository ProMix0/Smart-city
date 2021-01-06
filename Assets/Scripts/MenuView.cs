using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuView : MonoBehaviour
{
    public GameObject obj;
    void Start()
    {
        obj = GameObject.Find("NotificationsC");
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (GUI.Button(new Rect(100f, 100f, 80f, 15f), "Нажми"))
        {
            obj.SetActive(false);
        }
        
        if (GUI.Button(new Rect(100f, 100f, 80f, 15f), "Нажми"))
        {
            obj.SetActive(true);
        }
    }
}
