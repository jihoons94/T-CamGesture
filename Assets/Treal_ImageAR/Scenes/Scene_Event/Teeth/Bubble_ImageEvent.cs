using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble_ImageEvent : MonoBehaviour {
    public GameObject Target;
    public Amount_Click Target_Button;
    public MotionEvent_Teeth Teeth;
    public int num;
    Vector3 MyScale;
    Vector3 DScale;
    Vector3 OScale;
    float temp = 0;
    bool StartEvent = false;

    public float time = 1.0f;

    // Use this for initialization
    void Start()
    {
        temp = 0;
        StartEvent = false;
        MyScale = transform.localScale;
        OScale = MyScale / 10;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(StartEvent)
        {
            temp += Time.deltaTime*8;
            DScale = MyScale - OScale * temp;
            transform.localScale = Vector3.Lerp(transform.localScale, DScale, time);
            if (temp>=10)
            {
                Teeth.Wait_Image.Remove(num);
                Destroy(gameObject);
            }
        }
        else
        {
            
            if (!Target.activeSelf)
            {
                MyScale = transform.localScale;
                OScale = MyScale / 10;
                StartEvent = true;
                return;
            }
            else if (Target_Button.Amount < 10)
            {
                DScale = OScale * Target_Button.Amount;
                transform.localScale = Vector3.Lerp(transform.localScale, DScale, time);
            }
        }
            

            


    }
}
