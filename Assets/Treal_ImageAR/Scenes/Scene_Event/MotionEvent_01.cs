using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MotionEvent_01 : Motion_Event
{
    public Text Score_text;
    int Score = 0;
    bool GetHoney = false;


    private void Update()
    {
        if (Score < 0)
            Score = 0;
        Score_text.text = System.Convert.ToString(Score);
    }


    override public void FixedEvent_On(int _num)
    {
        switch(_num)
        {
            case 0:
                {
                    GetHoney = true;
                }
                break;
            case 1:
                {
                    if(GetHoney)
                    {
                        GetHoney = false;
                        Score += 20;
                    }
                    
                }
                break;
        }
    }

    override public void MoveEvent_On(int _num)
    {
        switch (_num)
        {
            case 0:
                {
                    if (Score > 0)
                    {
                        Score -= 10;
                    }

                }
                break;
            case 1:
                {
                    if (Score > 0)
                    {
                        Score -= 10;
                    }

                }
                break;
        }
    }

    override public void RandomEvent_On(int _num)
    {

    }

    override public void FixedEvent_Off(int _num)
    {

    }

    override public void MoveEvent_Off(int _num)
    {

    }

    override public void RandomEvent_Off(int _num)
    {

    }

}
