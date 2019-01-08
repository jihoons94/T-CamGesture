using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TCamera;
using UnityEngine.UI;

public class PreviewFps : MonoBehaviour {
    public Text temp;
    public Text temp2;
    public TCam Manager;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
        Manager.SetPreviewFPS(60);
        temp.text = System.Convert.ToString(Manager.GetPreviewFPS()) ;
        temps();
    }

    void temps()
    {
        if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            temp2.text = "PC";
        }
        else
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                temp2.text = "Android";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                temp2.text = "IPhone";
            }
        }
    }


}
