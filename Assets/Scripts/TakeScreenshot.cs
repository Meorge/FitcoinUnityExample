using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TakeScreenshot : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) {
            print("take screenshot");
            ScreenCapture.CaptureScreenshot($"{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.png");
        }
    }
}
