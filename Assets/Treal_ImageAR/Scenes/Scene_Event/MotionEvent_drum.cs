using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotionEvent_drum : Motion_Event
{
    int Score=0;
    bool done = false;
    bool Event_A = false;
    public Image Bar_Fill;

    private void Update()
    {
        Bar_Fill.fillAmount = Score;
    }

    private void Start()
    {
        Score = 0;
        done = true;
        Event_A = false;
        StartCoroutine(DrumOn());
    }

    IEnumerator DrumOn()
    {
        while(done)
        {
            if(Event_A)
            {
                
                Score += 10;
                if (Score >= 100)
                {
                    done = false;
                    StopCoroutine(DrumOn());
                }
                    
            }
            else
            {
                if (Score >= 10)
                    Score -= 10;
                else
                    Score = 0;
            }

            yield return new WaitForSeconds(1.5f);
        }
    }

    override public void FixedEvent_On(int _num)
    {
        switch (_num)
        {
            case 0:
                {
                    Event_A = true;
                }
                break;
        }
    }

    override public void FixedEvent_Off(int _num)
    {
        switch (_num)
        {
            case 0:
                {
                    Event_A = false;
                }
                break;
        }
    }

    override public void MoveEvent_On(int _num)
    {

    }

    override public void RandomEvent_On(int _num)
    {

    }

    
    override public void MoveEvent_Off(int _num)
    {

    }

    override public void RandomEvent_Off(int _num)
    {

    }
}
