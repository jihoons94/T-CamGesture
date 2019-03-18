using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Treal.BrowserCore;
using DG.Tweening;
using UnityEngine.UI;

public class MotionEvent_Q : Motion_Event
{

    int State;
    public Animator Teeth;
    public Animator Dinosaur;
    public GameObject[] Button;
    public GameObject[] ButtonBox;
    public GameObject GameIn;
    public GameObject GameOut;
    public GameObject[] Pens = new GameObject[2];
    public GameObject DimTip;
    public GameObject ClearEffect;
    bool ButtonPushCheck;
    SoundManager SoundMgr;
    public DOTweenAnimation[] button = new DOTweenAnimation[2];
    public GameObject buttonBack1;
    public GameObject buttonBack2;

    [Header("[사용된 Effect]")]
    [Tooltip("정답 시 효과입니다.")]
    public GameObject DoneEffect;
    [Tooltip("비정답 시 효과입니다.")]
    public GameObject NotEffect;

    
    int cState;
    int QState;
    int MaxState = 5;
    [Header("----------")]
    public Transform Background;
    public Transform Background2;
    public GameObject Window_Canvas;

    private float UISubmitSpeed = 10;
    private float OtherSubmitSpeed = 5;
    private float RecognitionMin = 0.1f;

    public GameObject[] Button_ = new GameObject[2];
    int UI_ButtonCount = 2;
    bool isPlay;

    public float[] PenTime = { 0f, 0f };
    private byte? Button_Number = null;

    public Text Dq01;
    public Text Dq02;
    int[] answer = { 1, 0, 1, 1, 0 };

    public GameObject Hand;

    public string[] topS2 = {
        "열심히 공부했으니 우리 같이 퀴즈를 풀어볼까?",
        "내가 문제를 낼 테니 정답인 버튼을 누르는 거야.",
        "Q1. 양치질은 1분이면 충분하다?",
        "Q2. 양치질은 치아를 보호해준다?",
        "Q3. 하루에 한번만 양치를 해야한다?",
        "Q4. 양치질은 무조건 식후에 바로 해야한다?",
        "Q5. 양치질은 입냄새를 없애준다?"
    };


    public string[] bottomS = {
        "퀴즈?  재미있겠다!",
        "문제 없지! 어서 시작하자.",
        "음 뭐였더라?...",
        "이건 알거같아!",
        "양치질은 중요하지!",
        "바로 했야 하는건가?",
        "입냄새는 끔찍해!"
    };


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
        num = 2;
    }

    public void ButtonOn02()
    {
        Click = true;
        num = 3;
    }

    public void DimeNext()
    {
        Click = true;
        num = 0;
    }

    public void ButtonOff()
    {
        Click = false;
    }

    private void Update()
    {
        if (cState == 1)
        {
            if (Click_Amount(2))
                cState = 3;
        }
        else
        {

        }

        bool Check = false;
        for (int i = 0; i < PenTime.Length; i++)
        {
            
            if (PenTime[i] > 0f)
            {
                Debug.Log("확인");
                if (i == 0)
                    Pens[0].GetComponent<SpriteRenderer>().enabled = true;
                else if (i == 1)
                    Pens[1].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;

                PenTime[i] -= Time.deltaTime;
                Check = true;
            }
            else
            {
                if (i == 0)
                    Pens[0].GetComponent<SpriteRenderer>().enabled = false;
                else if (i == 1)
                    Pens[1].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                PenTime[i] = 0f;
            }

        }

        if (!Check)
            Button_Number = null;

        if (Click)
        {
            FixedEvent_On(num);
        }
    }
    /*#########################################################################################################################*/
    private void Awake()
    {
        GameIn.SetActive(true);
        SceneChange();
    }
    private void Start()
    {
        cState = 0;
        CanvsOn();
        State = 0;
        isPlay = false;
        StartCoroutine(StartEvent());
        SoundMgr = GetComponent<SoundManager>();
        ButtonPushCheck = false;
    }

    void DimTipEvent(bool state)
    {
        DimTip.SetActive(state);
        MotionTrackingMgr.fixed_Buttons[0].gameObject.SetActive(state);
    }






    IEnumerator StartEvent()
    {
        yield return new WaitForSeconds(3f);
        Dq01.text = topS2[State];
        Dq02.text = "";
        yield return new WaitForSeconds(1.5f);
        Dinosaur.SetInteger("State", 9);
        Dq02.text = bottomS[State];
        State++;

        yield return new WaitForSeconds(1.5f);
        Dq01.text = topS2[State];
        yield return new WaitForSeconds(1.5f);
        Dq02.text = bottomS[State];
        yield return new WaitForSeconds(1.5f);
        Dinosaur.SetInteger("State", 0);
        DimTip.SetActive(true);
        yield return new WaitForSeconds(1f);
        Hand.SetActive(true);
        Dq01.text = "문제가 맞을 경우에는 O";
        yield return new WaitForSeconds(1.5f);
        cState = 1;
        Dq01.text = "아닐 경우에는 X 북을 치면 돼";
        yield return new WaitForSeconds(3f);
        Hand.SetActive(false);
        Dq01.text = "정말 간단하지?";
        yield return new WaitForSeconds(3f);
        Dq01.text = "그럼 시작한다?";
        Dq02.text = "";
        yield return new WaitForSeconds(1f);
        Destroy(DimTip);
        //yield return new WaitForSeconds(0.5f);
        //for (int i=0; i<Button.Length; i++)
        //{
        //    ButtonBox[i].SetActive(true);
        //}
        yield return new WaitForSeconds(1.5f);
        Button[0].SetActive(true);
        Button[1].SetActive(true);
        Destroy(ButtonBox[0].transform.GetChild(0).gameObject);
        Destroy(ButtonBox[1].transform.GetChild(0).gameObject);
        //StartCoroutine(DimTipEvent());
        NextQuizSet();

    }

    IEnumerator Stage_N()
    {
        if (QState < MaxState)
        {
            Teeth.SetInteger("State", 1);
            Dinosaur.SetInteger("State", 2);
            button[0].DORewind();
            button[1].DORewind();
            SoundMgr.AudioPlay(SoundManager.SoundName.Answer);
            Dq01.text = "잘했어 정답이야!!";
            Dq02.text = "";
            yield return new WaitForSeconds(1f);
            Dq02.text = "신난다! 다음 문제도 맞춰보자!";
            yield return new WaitForSeconds(2f);
            Dq01.text = "그럼 다음 문제야.";
            Dq02.text = "";
            yield return new WaitForSeconds(2f);
            Dinosaur.SetInteger("State", 0);
            Teeth.SetInteger("State", 0);
            NextQuizSet();

        }
        else
        {
            isPlay = false;
            Dinosaur.SetInteger("State", 0);

            SoundMgr.AudioPlay(SoundManager.SoundName.GameClear);
            //AllButtonOff();
            //ClearEffect.SetActive(true);
            Destroy(ClearEffect);
            Teeth.SetInteger("State", 3);
            Dq01.text = "굉장한데? 모두 정답이야!";
            Dq02.text = "";
            yield return new WaitForSeconds(1f);
            Dinosaur.SetInteger("State", 10);
            Dq02.text = "대단해!!";
            isPlay = false;
            yield return new WaitForSeconds(2f);
            Dinosaur.SetInteger("State", 0);
            Teeth.SetInteger("State", 0);
            Dq01.text = "이렇게나 빨리 배우다니";
            yield return new WaitForSeconds(2f);
            Dq01.text = "정말 대단하거 같아!";
            yield return new WaitForSeconds(2f);
            Dq01.text = "이제 부터 올바른 양치질로";
            Dq02.text = "";
            yield return new WaitForSeconds(2f);
            Dq01.text = "치아를 지켜주자!";
            yield return new WaitForSeconds(2f);
            Dinosaur.SetInteger("State", 4);
            Dq02.text = "도와줘서 정말 고마워!";
            yield return new WaitForSeconds(2f);
            Dinosaur.SetInteger("State", 0);
            Dq01.text = "도움이 되었다니 다행이야";

            yield return new WaitForSeconds(2f);
            Dq01.text = "양치질에 대해 더 알려주고 싶지만";
            yield return new WaitForSeconds(2f);
            Dq01.text = "아쉽지만 이제 헤어져야할 시간이야";
            Dq02.text = "";
            yield return new WaitForSeconds(2f);
            SoundMgr.OutSound();
            Dq01.text = "";
            Dq02.text = "정말 즐거운 시간이었어!";
            yield return new WaitForSeconds(2f);
            Dq01.text = "올바른 양치질을 잊지말아야해 알겠지?";
            yield return new WaitForSeconds(2f);
            Dq02.text = "물론이지 친구야!";
            yield return new WaitForSeconds(2f);
            Dq01.text = "";
            Dq02.text = "우리 다음에 또 재밌게 놀자!";
            yield return new WaitForSeconds(2f);
            Dinosaur.SetInteger("State", 7);
            Teeth.SetInteger("State", 5);

            Dq01.text = "즐거웠어!";
            Dq02.text = "안녕! 모두 다음에봐!";
            yield return new WaitForSeconds(1f);


            GameOut.SetActive(true);
            if (CMotionTrackingManager.isNomal)
            {
                yield return new WaitForSeconds(3f);
                isPlay = false;
                Loading.LoadScene("GameTeeth_MAIN");
            }
            else
            {
                yield return new WaitForSeconds(3f);
                isPlay = false;
                Loading.LoadScene("GameTeeth_MAIN");
            }
        }

    }

    void AllButtonOff()
    {
        buttonBack1.SetActive(false);
        buttonBack2.SetActive(false);
        MotionTrackingMgr.fixed_Buttons[2].gameObject.SetActive(false);
        MotionTrackingMgr.fixed_Buttons[3].gameObject.SetActive(false);
    }

    void NextQuizSet()
    {
        SoundMgr.AudioPlay(SoundManager.SoundName.NextQuiz);
        State++;
        if (QState < MaxState)
        {
            Dq01.text = topS2[State];
            Dq02.text = bottomS[State];
        }
        isPlay = true;
    }
    void OneMoreTry()
    {
        Dinosaur.SetInteger("State", 3);
        SoundMgr.AudioPlay(SoundManager.SoundName.NoAnswer);
        Dq02.text = "정답이 아닌가봐 다시한번 생각해보자!";
        isPlay = true;
    }

    bool AnswerCheck(int nAnswer)
    {
        return (answer[QState] == nAnswer ? true : false);
    }

    bool NoClick_Amount(int num)
    {

        Amount_Click temp = MotionTrackingMgr.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount -= Time.deltaTime * 0.3f;


        //temp.Activate = false;
        if (temp.Amount <= 0)
        {
            temp.Amount = 0;
            return true;
        }
        else
            return false;
    }

    IEnumerator ButtonPushDelay()
    {
        ButtonPushCheck = true;
        SoundMgr.AudioPlay(SoundManager.SoundName.ButtonPush);
        yield return new WaitForSeconds(0.2f);
        ButtonPushCheck = false;
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

    bool Click_Amount(int num)
    {
        Dinosaur.SetInteger("State", 0);

        if (!ButtonPushCheck)
            StartCoroutine(ButtonPushDelay());
        Amount_Click temp = MotionTrackingMgr.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount += Time.deltaTime * OtherSubmitSpeed;

        //if (!Pens[num - 2].activeSelf)
        //{
        //    Pens[num - 2].SetActive(true);

        //}

        PenTime[num - 2] = 0.5f;

        if (num == 2)
            MotionTrackingMgr.fixed_Buttons[3].GetComponent<Amount_Click>().Amount = 0;
        else if (num == 3)
            MotionTrackingMgr.fixed_Buttons[2].GetComponent<Amount_Click>().Amount = 0;



        if (!button[num - 2].tween.IsPlaying())
        {
            button[num - 2].DORewind();
            button[num - 2].DORestart();
            if (num == 2)
                button[1].DORewind();
            else
                button[0].DORewind();
        }

        if (temp.Amount >= temp.MaxAmount)
        {
            Amount_Click temp1 = MotionTrackingMgr.fixed_Buttons[2].GetComponent<Amount_Click>();
            Amount_Click temp2 = MotionTrackingMgr.fixed_Buttons[3].GetComponent<Amount_Click>();
            temp1.Amount = 0;
            temp2.Amount = 0;
            if (!isPlay)
                return true;
            isPlay = false;
            bool result = AnswerCheck(num - 2);
            if (result)
            {
                QState++;
                Instantiate(DoneEffect).transform.position = MotionTrackingMgr.fixed_Buttons[num].position;
                StartCoroutine(Stage_N());

            }
            else
            {
                Instantiate(NotEffect).transform.position = MotionTrackingMgr.fixed_Buttons[num].position;
                OneMoreTry();
            }
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

            if (Button_Number == null || _num  == Button_Number)
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
                    DimTipEvent(false);
                    NextQuizSet();
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
