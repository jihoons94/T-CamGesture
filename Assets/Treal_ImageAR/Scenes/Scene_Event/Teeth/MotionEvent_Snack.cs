using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Treal.BrowserCore;
using DG.Tweening;
using UnityEngine.UI;


public class MotionEvent_Snack : Motion_Event
{
    int SnackCount = 4;

    int Snack01Count = 0;
    int Snack02Count = 0;
    int Snack03Count = 0;
    int Snack04Count = 0;

    int Count = 0;


    public List<int> Wait;
    public GameObject Window_Canvas;

    public GameObject Help_Text;

    public GameObject Effect;
    public Transform EffectPoint;
    public GameObject EatEffect;
    public GameObject SnackCreateEffect;
    public GameObject Remove_Effect;

    public GameObject[] Button_Snack = new GameObject[4];
    public GameObject[] Snacks = new GameObject[4];

    public Transform Background;
    public Transform Background2;

    private float UISubmitSpeed = 10;
    private float OtherSubmitSpeed = 8;
    private float RecognitionMin = 0.1f;

    private string[] scenario_Before = {
        "즐거운 간식 시간이야! ",
        " 어떤 간식들이 있는지 볼까?",
        "우와 어떤 것 부터 먹어볼까?"
    };

    private string[] scenario_After = {
        "사탕을 선택했구나 잘했어!",
        "빵은 너무 많이 먹으면 배불러",
        "초콜릿이자나 내가 정말 좋아하는 거야",
        "이 과자는 우유를 먹으면 더 맛있어!" ,
        "사탕만 먹지말구 다른 것도 먹는 거는 어때?",
        "빵을 너무 먹어서 배부른거 같아",
        "역시 너도 초콜릿이 좋은거지?",
        "이런 우유를 다 마셔서 찍어먹을 수가 없어"
    };


    public Text OutBottom;

    int UI_ButtonCount = 2;
    bool isPlay;
    public CMotionTrackingManager MotionTrackingMgr;

    /*#########################################################################################################################*/
    //윈도우 테스트용


    void CanvsOn()
    {
        if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Window_Canvas.SetActive(true);
        }
        else
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Window_Canvas.SetActive(false);
            }
        }
    }

    private bool Click = false;
    int num;
    public void ButtonOn01()
    {
        Click = true;
        num = 0;
    }

    public void ButtonOn02()
    {
        Click = true;
        num = 1;
    }
    public void ButtonOn03()
    {
        Click = true;
        num = 2;
    }

    public void ButtonOff()
    {
        Click = false;
    }

    private void Update()
    {

        if (Click)
        {
            switch (num)
            {
                case 0:
                    {
                        NoClick_Amount(3);
                        NoClick_Amount(4);
                        FixedEvent_On(2);
                    }
                    break;
                case 1:
                    {
                        NoClick_Amount(2);
                        NoClick_Amount(4);
                        FixedEvent_On(3);
                    }
                    break;
                case 2:
                    {
                        NoClick_Amount(2);
                        NoClick_Amount(3);
                        FixedEvent_On(4);
                    }
                    break;
            }
        }

        else
        {
            NoClick_Amount(2);
            NoClick_Amount(3);
            NoClick_Amount(4);
        }
    }
    /*#########################################################################################################################*/


    private void Start()
    {
        InitObject();
        isPlay = false;
        StartCoroutine(StartEvent());
        CanvsOn();
    }

    IEnumerator BubbleEffect(int num)
    {
        Wait.Add(num);
        GameObject buble_Effect = Instantiate(Effect, Background);
        buble_Effect.transform.position = MotionTrackingMgr.fixed_Buttons[num].localPosition;
        yield return new WaitForSeconds(1f);
        Wait.Remove(num);
    }

    void NewSnack(int num)
    {
        GameObject CreateEffects = Instantiate(SnackCreateEffect);

        CreateEffects.transform.position = Button_Snack[num].transform.position;
        GameObject CreateEffect = Instantiate(EatEffect);
        CreateEffect.transform.position = EffectPoint.position;
        int NewNum = Random.Range(0, SnackCount);
        GameObject temp = Button_Snack[num].transform.GetChild(0).gameObject;
        Snackyamyam(temp.tag);
        Destroy(temp);
        Debug.Log(NewNum);
        GameObject Newtemp = Instantiate(Snacks[NewNum], Button_Snack[num].transform);

        Newtemp.transform.parent.GetComponent<Snack_Button>().MyScale = Newtemp.transform.localScale;
    }

    void Snackyamyam(string SnackName)
    {
        Debug.Log(SnackName);
        switch (SnackName)
        {
            case "Snack01":
                {
                    Snack01Count++;
                    if (Snack01Count > 4)
                        OutBottom.text = scenario_After[5];
                    else
                        OutBottom.text = scenario_After[1];
                    
                }
                break;

            case "Snack02":
                {
                    Snack02Count++;
                    if (Snack02Count > 4)
                        OutBottom.text = scenario_After[4];
                    else
                        OutBottom.text = scenario_After[0];
                }
                break;

            case "Snack03":
                {
                    Snack03Count++;
                    if (Snack03Count > 4)
                        OutBottom.text = scenario_After[6];
                    else
                        OutBottom.text = scenario_After[2];
                }
                break;

            case "Snack04":
                {
                    Snack04Count++;
                    if (Snack04Count > 4)
                        OutBottom.text = scenario_After[7];
                    else
                        OutBottom.text = scenario_After[3];
                }
                break;
        }
    }

    void InitObject()
    {
        Help_Text.SetActive(false);
        for (int i = 0; i < Button_Snack.Length; i++)
        {
            Button_Snack[i].SetActive(false);
        }
    }

    IEnumerator StartEvent()
    {
        OutBottom.text = scenario_Before[0];
        yield return new WaitForSeconds(2f);
        OutBottom.text = scenario_Before[1];
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < Button_Snack.Length; i++)
        {
            yield return new WaitForSeconds(1f);
            Button_Snack[i].SetActive(true);
            GameObject CreateEffect = Instantiate(SnackCreateEffect, Button_Snack[i].transform);
            //CreateEffect.transform.localPosition = Button_Snack[i].transform.localPosition;
        }
        yield return new WaitForSeconds(1f);
        OutBottom.text = scenario_Before[2];
        yield return new WaitForSeconds(1f);
        Help_Text.SetActive(true);
        isPlay = true;
    }



    private void RemoveEffectCreate(int num)
    {
        GameObject RemoveEffect = Instantiate(Remove_Effect, Background2);

        RemoveEffect.transform.position =
            new Vector3(
                MotionTrackingMgr.fixed_Buttons[num].localPosition.x,
                MotionTrackingMgr.fixed_Buttons[num].localPosition.y,
                MotionTrackingMgr.fixed_Buttons[num].localPosition.z
                );
    }

    bool NoClick_Amount(int num)
    {
        Amount_Click temp = MotionTrackingMgr.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount -= Time.deltaTime * 0.3f;
        temp.Activate = false;
        if (temp.Amount <= 0)
        {
            temp.Amount = 0;
            return true;
        }
        else
            return false;
    }

    bool Click_Amount(int num)
    {
        Amount_Click temp = MotionTrackingMgr.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount += Time.deltaTime * OtherSubmitSpeed;
        
            //temp.Activate = true;
            if (!Wait.Contains(num))
                StartCoroutine(BubbleEffect(num));

        if (temp.Amount >= temp.MaxAmount-2)
        {
            temp.Amount = 0;
            RemoveEffectCreate(num);
            MotionTrackingMgr.Random_position(num);
            NewSnack(num - UI_ButtonCount);
            
            Count++;
            return true;
        }
        else
            return false;
    }

    override public void FixedEvent_On(int _num)
    {
        if (!isPlay)
            return;
        // 버튼 오브젝트가 활성화 되어 있을 경우만 이벤트가 발생한다.
        if (MotionTrackingMgr.fixed_Buttons[_num].gameObject.activeSelf)
        {
            if (_num >= UI_ButtonCount)
            {
                Click_Amount(_num);
            }
            else
            {

            }
        }
    }

    override public void FixedEvent_Off(int _num)
    {


        if (!isPlay)
            return;
        if (_num >= UI_ButtonCount)
        {
            NoClick_Amount(_num);
        }
    }

    override public void MoveEvent_On(int _num)
    {

    }
    override public void MoveEvent_Off(int _num)
    {

    }
    override public void RandomEvent_On(int _num)
    {

    }
    override public void RandomEvent_Off(int _num)
    {

    }
}
