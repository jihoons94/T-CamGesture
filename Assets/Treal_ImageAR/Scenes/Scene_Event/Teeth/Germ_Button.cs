using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Germ_Button : Amount_Click
{
    
    Vector3 MyScale;
    Vector3 DScale;
    Vector3 OScale;
    public float time =1f;

    // Use this for initialization
    void Start () {
        MyScale = transform.localScale;
        Amount = 0;
        OScale = MyScale / 10;
        MAxAmount = 8;
    }
	
	// Update is called once per frame
	void Update () {
        DScale = MyScale - OScale * Amount;
        if(Amount<10)
        transform.localScale = Vector3.Lerp(DScale, transform.localScale, time);



    }
}
