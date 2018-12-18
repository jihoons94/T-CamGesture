using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectOrientation : MonoBehaviour
{
    
	void Update ()
    {
        if (Application.isMobilePlatform)
        {
            if (Screen.orientation == ScreenOrientation.LandscapeLeft)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (Screen.orientation == ScreenOrientation.LandscapeRight)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else if (Screen.orientation == ScreenOrientation.Portrait)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            {
                transform.rotation = Quaternion.Euler(0, 0, 270);
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

}
