using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snack_Button : Amount_Click
{
    public Vector3 MyScale;
    public Vector3 DScale;
    Vector3 OScale;
    public float time = 1f;


    // Use this for initialization
    void Start()
    {
        Activate = false;
        MyScale = transform.GetChild(0).localScale;
        Amount = 0;
        OScale = MyScale / 10;
        MaxAmount = 8;
    }

    void Update()
    {
        DScale = MyScale - OScale * Amount;
        if (Amount == 0)
            {
                transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, DScale, time);
            }

            else if (Amount < 10)
            {
                transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, DScale, time);
            }

    }
}
