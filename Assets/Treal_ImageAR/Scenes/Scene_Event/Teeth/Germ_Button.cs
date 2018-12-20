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
        Activate = false;
        MyScale = transform.localScale;
        Amount = 0;
        OScale = MyScale / 10;
        MaxAmount = 8;
    }
	
	// Update is called once per frame
	void Update () {
        DScale = MyScale - OScale * Amount;
        if(Amount<10)
        transform.localScale = Vector3.Lerp(DScale, transform.localScale, time);

        if(Activate)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
