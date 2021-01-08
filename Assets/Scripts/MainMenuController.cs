using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public void OnNewGameClick()
    {
        GameObject.Find("ImageBSOD").GetComponent<Image>().enabled = true;
    }
    public void OnExitClick()
    {
        Application.Quit(0);
    }
}
