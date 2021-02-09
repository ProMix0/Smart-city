using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private readonly Dictionary<string, Canvas> canvases = new Dictionary<string, Canvas>();
    private Camera gameCam;
    private Camera menuCam;

    void Start()
    {
        gameCam = GameObject.Find("GameCamera").GetComponent<Camera>();
        menuCam = GameObject.Find("MenuCamera").GetComponent<Camera>();
        ActivateGameCam();

        canvases.Add("Tasks", GameObject.Find("TasksCanvas").GetComponent<Canvas>());
        canvases.Add("Happiness", GameObject.Find("HappinessCanvas").GetComponent<Canvas>());
        canvases.Add("Mods", GameObject.Find("ModsCanvas").GetComponent<Canvas>());
        canvases.Add("Notifications", GameObject.Find("NotificationsCanvas").GetComponent<Canvas>());
        SwitchTo("Tasks");
    }

    private void ActivateGameCam()
    {
        gameCam.enabled = true;
        menuCam.enabled = false;
    }
    private void ActivateMenuCam()
    {
        menuCam.enabled = true;
        gameCam.enabled = false;
    }

    private void SwitchTo(string name)
    {
        foreach (var item in canvases)
            if (item.Key == name)
                item.Value.enabled = true;
            else
                item.Value.enabled = false;
    }

    public void OnNotificationsButtonClick()
    {
        SwitchTo("Notifications");
    }
    public void OnModsButtonClick()
    {
        SwitchTo("Mods");
    }
    public void OnHappinessButtonClick()
    {
        SwitchTo("Happiness");
    }
    public void OnTasksButtonClick()
    {
        SwitchTo("Tasks");
    }
    public void OnResumeButtonClick()
    {
        Time.timeScale = 1;
        ActivateGameCam();
    }
    public void OnMenuButtonClick()
    {
        Time.timeScale = 0;
        ActivateMenuCam();
    }
}
