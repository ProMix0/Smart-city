using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    public Text text;

    public int r1 = 1337;
    
    public void move()
    {
        GameObject.FindWithTag("MainCamera").transform.position += new Vector3(1, 1, 1);
    }
    public void test()
    {
        text.text += "+";
    }

    void Update()
    {
    }
}

