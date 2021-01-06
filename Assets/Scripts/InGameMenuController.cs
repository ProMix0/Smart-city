using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuController : MonoBehaviour
{
    private Dictionary<string, Canvas> canvases = new Dictionary<string, Canvas>();

    void Start()
    {
        canvases.Add("Tasks", GameObject.Find("TasksCanvas").GetComponent<Canvas>());
        canvases.Add("Happiness", GameObject.Find("HappinessCanvas").GetComponent<Canvas>());
        canvases.Add("Mods", GameObject.Find("ModsCanvas").GetComponent<Canvas>());
        canvases.Add("Notifications", GameObject.Find("NotificationsCanvas").GetComponent<Canvas>());
        SwitchTo("Tasks");
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
    public void OnExitButtonClick()
    {
        
    }
}
