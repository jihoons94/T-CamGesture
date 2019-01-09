using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Q_Button : Amount_Click
{
    public Image fillbar;
	// Use this for initialization
	void Start () {
        Amount = 0;
        MaxAmount = 8;
    }
	
	// Update is called once per frame
	void Update () {
        fillbar.fillAmount = (float)(Amount / MaxAmount);
	}
}
