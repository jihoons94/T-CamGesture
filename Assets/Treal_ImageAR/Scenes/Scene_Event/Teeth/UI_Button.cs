using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : Amount_Click
{
    public Image StateBar;
    // Use this for initialization
    void Start () {
        Amount = 0;
        StateBar.fillAmount = 0;
        MAxAmount = 10;
	}
	
	// Update is called once per frame
	void Update () {
        StateBar.fillAmount = Amount / MAxAmount;
	}
}
