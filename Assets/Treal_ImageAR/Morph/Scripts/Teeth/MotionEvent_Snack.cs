using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Treal.BrowserCore;
using DG.Tweening;
using UnityEngine.UI;



public class MotionEvent_Snack : Motion_Event
{
    int State;
    public GameObject Hand;
    public GameObject TeethCharacter;
    public GameObject TeethDoqObj;
    public GameObject DinosaurDoqObj;
    public GameObject DimTip;
    Animator MyAnim;

    bool ButtonPushCheck;
    SoundManager SoundMgr;

    public GameObject GameDoneEffect;
    int SnackCount = 4;

    int Snack01Count = 0;
    int Snack02Count = 0;
    int Snack03Count = 0;
    int Snack04Count = 0;

    public int Count = 0;
    public GameObject[] MySnacks = new GameObject[4];

    public SpriteRenderer Snack;
    public List<int> Wait;
    public GameObject Window_Canvas;
    public Transform EffectPoint;

    [Header("[사용된 Effect]")]
    [Tooltip("제스쳐시 발생되는 효과입니다.")]
    public GameObject Effect;
    [Tooltip("공룡이의 입에서 발생되는 효과입니다.")]
    public GameObject EatEffect;
    [Tooltip("과자가 생성될 때 발생되는 효과입니다.")]
    public GameObject SnackCreateEffect;
    [Tooltip("선택된 과자가 사라질때 나타나는 효과입니다.")]
    public GameObject Remove_Effect;

    public GameObject[] Button_Snack = new GameObject[4];
    public GameObject[] Snacks = new GameObject[4];

    public Transform Background;
    public Transform Background2;

    private float UISubmitSpeed = 10;
    private float OtherSubmitSpeed = 8;
    private float RecognitionMin = 0.1f;

    public GameObject MainCharacter;

    public GameObject GameIn;
    public GameObject GameOut;

    public int SnackCounts = 0;

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
    public float[] PenTime = { 0f, 0f };
    private byte? Button_Number = null;

    public Text Doq01;
    public Text Doq02;

    int UI_ButtonCount = 2;
    bool isPlay;

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
        if (State == 1)
        {
            if (Click_Amount(4))
                State = 3;
        }
        else
        {

        }


        bool Check = false;
        for (int i = 0; i < PenTime.Length; i++)
        {

            if (PenTime[i] > 0f)
            {
                PenTime[i] -= Time.deltaTime;
                Check = true;
            }
            else
            {
                PenTime[i] = 0f;
            }

        }

        if (!Check)
            Button_Number = null;


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
                        FixedEvent_On(0);
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

    private void Awake()
    {
        SceneChange();
        MyAnim = MainCharacter.GetComponent<Animator>();
    }


    private void Start()
    {
        State = 0;
        SnackCount = 4;

        Snack01Count = 0;
        Snack02Count = 0;
        Snack03Count = 0;
        Snack04Count = 0;

        Count = 0;
        SnackCounts = 0;
        // MainCharacter.transform.position = new Vector3(1000f, -120.5f, 0f);
        MainCharacter.GetComponent<DOTweenAnimation>().CreateTween();
        MainCharacter.GetComponent<DOTweenAnimation>().DOPlay();
        GameIn.SetActive(true);
        StartCoroutine(StartIntro());


        ButtonPushCheck = false;
        SoundMgr = GetComponent<SoundManager>();
        InitObject();
        isPlay = false;
        //StartCoroutine(StartEvent());
        CanvsOn();
    }

    IEnumerator BubbleEffect(int num)
    {
        Wait.Add(num);
        GameObject buble_Effect = Instantiate(Effect, Background2);
        buble_Effect.transform.position = MotionTrackingMgr.fixed_Buttons[num].position;
        yield return new WaitForSeconds(1f);
        Wait.Remove(num);
    }

    public IEnumerator GameOver()
    {
        Doq02.text = "";

        Destroy(Snack.gameObject);
        MyAnim.SetInteger("State", 0);

        GameDoneEffect.SetActive(true);
        for (int i = 0; i < Button_Snack.Length; i++)
        {
            Button_Snack[i].SetActive(false);
        }
        SoundMgr.AudioPlay(SoundManager.SoundName.GameClear);
        yield return new WaitForSeconds(3f);
        // SoundMgr.OutSound();
        yield return new WaitForSeconds(2f);

        GameDoneEffect.SetActive(false);
        MyAnim.SetInteger("State", 12);
        StartCoroutine(TempEvent(false, "너무 배불러! 더이상은 못먹겠어"));
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(TempEvent(false, "배도 부른데 나가서 놀지 않을래?"));
        
        yield return new WaitForSeconds(2.5f);

        StartCoroutine(TempEvent(false, "다른 친구들도 불러서 같이 놀자!"));
        yield return new WaitForSeconds(1.8f);
        MyAnim.SetInteger("State", 5);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(TempEvent(false, "아야!,  왜그러지 입안이 아파!"));
        yield return new WaitForSeconds(2f);
        StartCoroutine(TempEvent(false, "입안을 누가 바늘로 찌르는 것 같아!"));
        yield return new WaitForSeconds(2.5f);
        TeethDoqObj.SetActive(true);
        yield return new WaitForSeconds(2f);
        TeethCharacter.SetActive(true);
        yield return new WaitForSeconds(1f);
        StartCoroutine(TempEvent(true, "공룡이에 치아에 무슨일이 생긴거야!"));
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(TempEvent(false, "내 치아에!?"));
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(TempEvent(true, "응! 분명 충치균의 짓일거야!"));
        yield return new WaitForSeconds(2.5f);
        MyAnim.SetInteger("State", 6);
        StartCoroutine(TempEvent(false, "입안이 너무 아파, 친구들아 도와줘!"));
        yield return new WaitForSeconds(2.5f);
        SoundMgr.OutSound();
        StartCoroutine(TempEvent(true, "치아 요정인 내가 도와줄게 "));
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(TempEvent(true, "같이 공룡이에 입속으로 들어가 보자!"));
        yield return new WaitForSeconds(3f);
        TeethCharacter.GetComponent<Animator>().SetInteger("State", 4);
        yield return new WaitForSeconds(0.5f);
        TeethCharacter.GetComponent<Animator>().SetInteger("State", 0);
        yield return new WaitForSeconds(0.5f);
        GameOut.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        if (CMotionTrackingManager.isNomal)
        {
            isPlay = false;
            Loading.LoadScene("GameTeeth_MainGame");
        }
        else
        {

            yield return new WaitForSeconds(4f);
            isPlay = false;
            Loading.LoadScene("GameTeeth_MAIN");
        }
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
            GameObject Newtemp = Instantiate(Snacks[NewNum], Button_Snack[num].transform);

            Newtemp.transform.parent.GetComponent<Snack_Button>().MyScale = Newtemp.transform.localScale;
    }

    void Snackyamyam(string SnackName)
    {

        SnackCounts++;
        switch (SnackName)
        {
            case "Snack01":
                {
                    Instantiate(MySnacks[0], Snack.transform);

                    if (!isPlay)
                    {
                        return;
                    }
                    Debug.Log("?01");
                    Snack01Count++;
                    if (Snack01Count > 3)
                        Doq02.text = scenario_After[5];
                    else
                        Doq02.text = scenario_After[1];

                }
                break;

            case "Snack02":
                {
                    Instantiate(MySnacks[1], Snack.transform);
                    if (!isPlay)
                        return;
                    Debug.Log("?02");
                    Snack02Count++;
                    if (Snack02Count > 3)
                        Doq02.text = scenario_After[6];
                    else
                        Doq02.text = scenario_After[2];
                }
                break;

            case "Snack03":
                {
                    Instantiate(MySnacks[2], Snack.transform);
                    if (!isPlay)
                        return;
                    Debug.Log("?03");
                    Snack03Count++;
                    if (Snack03Count > 3)
                        Doq02.text = scenario_After[4];
                    else
                        Doq02.text = scenario_After[0];
                }
                break;

            case "Snack04":
                {
                    Instantiate(MySnacks[3], Snack.transform);
                    if (!isPlay)
                        return;
                    Debug.Log("?04");
                    Snack04Count++;
                    if (Snack04Count > 3)
                        Doq02.text = scenario_After[7];
                    else
                        Doq02.text = scenario_After[3];
                }
                break;
        }
    }

    void InitObject()
    {
        for (int i = 0; i < Button_Snack.Length; i++)
        {
            Button_Snack[i].SetActive(false);
        }
    }

    void GetTextAnimation(GameObject Target)
    {

    }

    void TextAnimPlay(DOTweenAnimation target)
    {
        target.CreateTween();
        target.DORestart();
    }


    void TextOut(Text target, string _temp)
    {

        target.text = _temp;

    }

    IEnumerator TempEvent(bool isTop, string _Temp)
    {
        if (isTop)
        {
            yield return new WaitForSeconds(0.5f);
            Doq01.text = _Temp;
        }
        else
        {

            yield return new WaitForSeconds(0.5f);
            Doq02.text = _Temp;
        }
    }


    IEnumerator StartIntro()
    {
        MyAnim.SetInteger("State", 1);
        yield return new WaitForSeconds(5f);
        MyAnim.SetInteger("State", 0);
        yield return new WaitForSeconds(1f);
        MyAnim.SetInteger("State", 4);
        StartCoroutine(TempEvent(false, "안녕? 난 공룡이라고해! 만나서 반가워!"));
        yield return new WaitForSeconds(2.5f);
        MyAnim.SetInteger("State", 0);
        StartCoroutine(TempEvent(false, "놀러 와줘서 고마워, 우리 뭐하고 놀까?"));
        yield return new WaitForSeconds(2.5f);
        MyAnim.SetInteger("State", 2);
        StartCoroutine(TempEvent(false, "그래! 우선 간식부터 먹자! "));
        yield return new WaitForSeconds(2.5f);
        MyAnim.SetInteger("State", 0);
        StartCoroutine(TempEvent(false, " 어떤 간식들이 있는지 볼까?"));
        yield return new WaitForSeconds(2.5f);


        for (int i = 0; i < Button_Snack.Length; i++)
        {
            yield return new WaitForSeconds(0.6f);
            SoundMgr.AudioPlay(SoundManager.SoundName.Boomb);
            Button_Snack[i].SetActive(true);
            GameObject CreateEffect = Instantiate(SnackCreateEffect, Button_Snack[i].transform);
            //CreateEffect.transform.localPosition = Button_Snack[i].transform.localPosition;
        }
        DimTip.SetActive(true);
        yield return new WaitForSeconds(1f);
        Hand.SetActive(true);
        StartCoroutine(TempEvent(false, " 내 위에 떠있는 간식들을 손으로 문질러서"));
        Hand.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(TempEvent(false, " 먹을 간식을 선택할 수 있어!"));
        
        State = 1;
        yield return new WaitForSeconds(1f);
        Hand.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        
        StartCoroutine(TempEvent(false, " 정말 간단하지?"));
        yield return new WaitForSeconds(2.5f);
        MyAnim.SetInteger("State", 2);
        StartCoroutine(TempEvent(false, " 잘보고 맛있는 간식을 골라줘!"));
        yield return new WaitForSeconds(2.5f);
        MyAnim.SetInteger("State", 0);
        Destroy(DimTip);
        State = 3;
        
        isPlay = true;
    }



    private void RemoveEffectCreate(int num)
    {
        SoundMgr.AudioPlay(SoundManager.SoundName.Done);
        GameObject RemoveEffect = Instantiate(Remove_Effect, Background2);

        RemoveEffect.transform.position =
            new Vector3(
                MotionTrackingMgr.fixed_Buttons[num].localPosition.x,
                MotionTrackingMgr.fixed_Buttons[num].localPosition.y,
                MotionTrackingMgr.fixed_Buttons[num].localPosition.z
                );
    }

    IEnumerator ButtonPushDelay()
    {
        ButtonPushCheck = true;
        SoundMgr.AudioPlay(SoundManager.SoundName.ButtonPush);
        yield return new WaitForSeconds(0.2f);
        ButtonPushCheck = false;
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
        if (!ButtonPushCheck)
            StartCoroutine(ButtonPushDelay());
        Amount_Click temp = MotionTrackingMgr.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount += Time.deltaTime * OtherSubmitSpeed;

        //temp.Activate = true;
        if (!Wait.Contains(num))
            StartCoroutine(BubbleEffect(num));

        if (temp.Amount >= temp.MaxAmount - 2)
        {
            temp.Amount = 0;
            RemoveEffectCreate(num);
            //MotionTrackingMgr.Random_position(num);
            NewSnack(num - UI_ButtonCount);
            return true;
        }
        else
            return false;
    }

    bool UI_NoClick_Amount(int num)
    {
        Amount_Click temp = MotionTrackingMgr.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount -= Time.deltaTime;
        if (temp.Amount <= 0)
        {
            temp.Amount = 0;
            return true;
        }
        else
            return false;
    }

    bool UI_Click_Amount(int num)
    {
        Amount_Click temp = MotionTrackingMgr.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount += Time.deltaTime * UISubmitSpeed;

        if (temp.Amount >= temp.MaxAmount)
        {
            temp.Amount = 0;
            return true;
        }
        else
            return false;
    }

    override public void FixedEvent_On(int _num)
    {

        // 버튼 오브젝트가 활성화 되어 있을 경우만 이벤트가 발생한다.
        if (!MotionTrackingMgr.fixed_Buttons[_num].gameObject.activeSelf)
        {
            return;
        }
        if (_num >= UI_ButtonCount)
        {
            if (!isPlay)
                return;
            if (Button_Number == null || _num == Button_Number)
            {
                Button_Number = (byte)_num;
                Click_Amount(_num);
            }
        }
        else
        {
            if (UI_Click_Amount(_num))
            {
                if (_num == 0)
                {
                    isPlay = true;
                    DimTip.SetActive(false);
                    MotionTrackingMgr.fixed_Buttons[0].gameObject.SetActive(false);
                }
                else
                {
                    isPlay = false;
                    Loading.LoadScene("GameTeeth_MAIN");
                }
            }

        }
    }

    override public void FixedEvent_Off(int _num)
    {
        if (!isPlay)
            return;

        if (MotionTrackingMgr.fixed_Buttons[_num].tag == "NotUsing")
            return;

        if (_num >= UI_ButtonCount)
        {
            NoClick_Amount(_num);
        }
    }

}
