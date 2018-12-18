using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


// 그리는 애니메이션 작업 중 멈춤
public class MotionEvent_Learn : Motion_Event
{
    public GameObject MoveTarget;
    Transform MoveTar_transform;
    public DOTweenAnimation dotAnim;
    Transform Alphabet;

    public List<Image> Bars;
    public List<Transform> startpoint;
    public List<Transform> endpoint;

    int StartIndex = 0;
    int EndIndex = 0;
    int BarIndex = 0;

    int MaxIndex = 2;
    bool IsMoving;
    bool IsPlaying;
    bool BarAnimation;

    void Find_Set()
    {
        Alphabet = GameObject.FindWithTag("Alphabet").transform;
        Transform StartP = Alphabet.transform.GetChild(1);
        Transform EndP= Alphabet.transform.GetChild(2);
        Transform Bar = Alphabet.transform.GetChild(3);

        for(int i=0; i<StartP.GetChildCount(); i++)
        {
            startpoint.Add(StartP.GetChild(i));
        }
        for (int i = 0; i < EndP.GetChildCount(); i++)
        {
            endpoint.Add(EndP.GetChild(i));
        }
        for (int i = 0; i < Bar.GetChildCount(); i++)
        {
            Bars.Add(Bar.GetChild(i).GetComponent<Image>());
        }
    }

    private void Start()
    {
        IsMoving = false;
        IsPlaying = false;
        BarAnimation = false;
        MoveTar_transform = MoveTarget.transform;
        Find_Set();
    }

    void Fill_Bar()
    {
        BarAnimation = true;
        StartCoroutine(WaitFillEvent());
    }

    IEnumerator WaitFillEvent()
    {
        while(Bars[BarIndex].fillAmount>=100)
        {
            Bars[BarIndex].fillAmount += 1;
            yield return new WaitForSeconds(0.1f);
        }
        BarIndex++;
        BarAnimation = false;
    }


    private void Update()
    {

        if (IsPlaying)
        {
            if (IsMoving)
            {
                bool temp = Check_P(startpoint[EndIndex]);
                if (temp) // 목표에 도착
                {
                    EndIndex++;
                    IsPlaying = false;
                    Fill_Bar();
                    return;
                }
            }
            else //경로가 설정되지 않음
            {
                NewPath_setup(endpoint[EndIndex]);
                Path_Start();
                IsMoving = true;
            }
        }
        else
        {
            if (IsMoving)
            {
                //IsPlaying = false;
                bool temp = Check_P(startpoint[StartIndex]);
                if (temp) // 목표에 도착
                {
                    StartIndex++;
                    IsPlaying = true;
                    return;
                }
            }
            else
            {
                if (BarAnimation)
                {

                }
                else
                {
                    //도착하지 못함
                    NewPath_setup(startpoint[StartIndex]);
                    Path_Start();
                    IsMoving = true;
                }
            }
        }
    }

    bool Check_P(Transform _nTrans)
    {
        if (_nTrans.position == MoveTar_transform.position)
        {
            dotAnim.DOPause();
            IsMoving = false;
            return true;
        }
        else
            return false;
    }


    public void Move_Pause()
    {
        dotAnim.DOPause();
    }

    public void Path_Start()
    {
        dotAnim.DOPlay();
    }

    public void NewPath_setup(Transform _newpoint)
    {
        Vector2 dir = _newpoint.position - MoveTarget.transform.position;

        dotAnim.endValueTransform = _newpoint;
        dotAnim.CreateTween();
    }

    override public void FixedEvent_On(int _num)
    {
        if(IsPlaying)
        {
            switch (_num)
            {
                case 0:
                    {
                        Path_Start();
                    }
                    break;
            }
        }


    }

    override public void FixedEvent_Off(int _num)
    {
        if (IsPlaying)
        {
            switch (_num)
            {
                case 0:
                    {
                        //Move_Pause();
                    }
                    break;
            }
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
