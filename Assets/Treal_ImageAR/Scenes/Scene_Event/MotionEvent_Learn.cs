using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Treal.BrowserCore;

// 그리는 애니메이션 작업 중 멈춤
public class MotionEvent_Learn : Motion_Event
{
    //public GameObject GuideLinePoint;
    public Transform GuideLineP;
    public GameObject CompEffect;

    DOTweenAnimation TarAnim;
    public GameObject MoveTarget;
    Transform MoveTar_transform;
    MoveObject CMoveObject;

    Transform Alphabet;

    public List<Image> Bars;
    public List<Transform> startpoint;
    public List<Transform> endpoint;

    public Text DOq;

    int StartIndex = 0;
    int EndIndex = 0;
    int BarIndex = 0;
    int GuideLineCount;

    int MaxIndex = 2;
    bool IsMoving;
    public bool IsPlaying;
    bool BarAnimation;

    bool IsFirst;

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
        IsFirst = true;
        IsMoving = false;
        IsPlaying = false;
        BarAnimation = false;
        MoveTar_transform = MoveTarget.transform;
        CMoveObject = MoveTarget.GetComponent<MoveObject>();
        TarAnim = MoveTarget.GetComponent<DOTweenAnimation>();
        Find_Set();
        
    }

    void Fill_Bar()
    {
        BarAnimation = true;
        StartCoroutine(WaitFillEvent());
    }

    IEnumerator WaitFillEvent()
    {
            Debug.Log("애니메이션 시작");
        while(Bars[BarIndex].fillAmount<1)
        {
            Bars[BarIndex].fillAmount += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        GuideLineP.transform.GetChild(BarIndex).gameObject.SetActive(false);
        BarIndex++;
        if(BarIndex <= MaxIndex)
            GuideLineP.transform.GetChild(BarIndex).gameObject.SetActive(true);
        BarAnimation = false;
    }

    void MyCallback(int waypointIndex)
    {
        Debug.Log("Waypoint index changed to " + waypointIndex);
    }

    void AddNewPath()
    {
        GuideLineP.GetChild(BarIndex).gameObject.SetActive(true);
        Debug.Log("애니메이션 시작");
    }

    //void CreateGuideLine(Vector3 _StartPoint, Vector3 _EndPoint)
    //{
    //    Debug.Log("Start: "+_StartPoint+", End: "+_EndPoint);
    //    Vector3 Line = _EndPoint - _StartPoint;
    //    float Length = Line.magnitude;
    //    GuideLineCount = (int)(Length * 0.027f)+1;
   
    //    Vector3 standard = Line / GuideLineCount;
    //    for(int i=0; i <= GuideLineCount; i++)
    //    {
    //        Vector3 temp = standard * i + _StartPoint;
    //        GameObject NewPoint = Instantiate(GuideLinePoint, GuideLineP);
    //        NewPoint.transform.localPosition = temp;
    //    }
    //}

    void ChangeDoq(int _num)
    {
        if(_num == 1)
        {
            DOq.text = "그렇지! 그렇게 하는거야";
        }
        if(_num == 2)
        {
            DOq.text = "잘 했어! 정말 잘하는데?";
        }
    }

    void CreateDoneEffect()
    {
      GameObject temp =  MoveTarget.transform.GetChild(2).gameObject;
        temp.GetComponent<ParticleSystem>().Play();
    }

    void MoveEffect(bool _state)
    {
        GameObject temp = MoveTarget.transform.GetChild(1).gameObject;
        if (_state)
            temp.GetComponent<ParticleSystem>().Play();
        else
            temp.GetComponent<ParticleSystem>().Stop();
    }

    private void Update()
    {
        if (IsPlaying)
        {
            if (IsMoving) // 출발 시작
            {
                if (IsFirst)
                {
                    CreateDoneEffect();
                    IsFirst = false;
                }
                if (Check_P(endpoint[EndIndex])) // 목표에 도착
                {
                    EndIndex++;
                    IsPlaying = false;
                    IsMoving = false;
                    Fill_Bar();
                    IsFirst = true;
                    CreateDoneEffect();
                    return;
                }
            }
            else //출발할 준비를 마친 상태
            {
                // NewPath_setup(endpoint[EndIndex]);
                // 빌드시 주석처리
                  // Path_Start();
                   IsMoving = true;
            }
        }
        else
        {
            if (IsMoving)
            {
                //IsPlaying = false;
                if (Check_P(startpoint[StartIndex])) // 목표에 도착
                {
                    CMoveObject.PathStart = false;
                    IsMoving = false;

                    if (EndIndex == 0)
                        DOq.text = "토끼가 길을 잃었나봐 토끼를 도와서 알파벳을 완성해 보자!";
                    //CreateGuideLine(startpoint[StartIndex].transform.localPosition, endpoint[EndIndex].transform.localPosition);
                    AddNewPath();
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
                { // 새로운 지점 설정

                    if (StartIndex <= MaxIndex)
                    {
                        Debug.Log("새로운 지점 설정");
                        ChangeDoq(EndIndex);
                        NewPath_setup(startpoint[StartIndex]);
                        Path_Start();
                        IsMoving = true;
                    }
                    else
                    {
                        Move_Pause();
                        CompEffect.SetActive(true);
                        DOq.text = "덕분에 길을 찾은 것 같아, 고마워!";
                    }
                }
            }
        }
    }

    bool Check_P(Transform _nTrans)
    {
        float Length = Vector2.Distance(_nTrans.position, MoveTar_transform.position);
        //Debug.Log(Length);
        if (Length <=1)
        {
            MoveTar_transform.position = _nTrans.position;
            Debug.Log(" Check_P() 목표지점 도착");
            return true;
        }
        else
            return false;
    }


    public void Move_Pause()
    {
        Debug.Log("정지");
        TarAnim.DOPause();
        MoveEffect(false);
    }

    public void Path_Start()
    {
        Debug.Log("시작");
        TarAnim.DOPlay();
        MoveEffect(true);  
    }

    public void NewPath_setup(Transform _newpoint)
    {
        CMoveObject.Dest = _newpoint;
        CMoveObject.LookDest();
        TarAnim.endValueTransform = CMoveObject.Dest;
        TarAnim.CreateTween();
        TarAnim.DOPlay();

        Debug.Log("새로운 경로 설정 완료");
    }

    void FixedButtonSet()
    {

    }

    override public void FixedEvent_On(int _num)
    {



    }

    override public void FixedEvent_Off(int _num)
    {

    }

    override public void MoveEvent_On(int _num)
    {
        if (IsPlaying)
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

    override public void RandomEvent_On(int _num)
    {

    }


    override public void MoveEvent_Off(int _num)
    {
        if (IsPlaying)
        {
            switch (_num)
            {
                case 0:
                    {
                        Move_Pause();
                    }
                    break;
            }
        }
    }

    override public void RandomEvent_Off(int _num)
    {

    }
}
