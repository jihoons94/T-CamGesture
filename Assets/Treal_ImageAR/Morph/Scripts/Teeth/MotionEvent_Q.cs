using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Treal.BrowserCore;
using DG.Tweening;
using UnityEngine.UI;

public class MotionEvent_Q : Motion_Event
{
    public GameObject[] Pens = new GameObject[2];
    public GameObject DimTip;
    public GameObject ClearEffect;
    bool ButtonPushCheck;
    SoundManager SoundMgr;
    public GameObject Popup;
    public DOTweenAnimation[] button = new DOTweenAnimation[2];
    public GameObject buttonBack1;
    public GameObject buttonBack2;

    public GameObject DoneEffect;
    public GameObject NotEffect;
    int State;
    int QState;
    int MaxState = 5;
    public Transform Background;
    public Transform Background2;
    public GameObject Window_Canvas;

    private float UISubmitSpeed = 10;
    private float OtherSubmitSpeed = 5;
    private float RecognitionMin = 0.1f;

    public GameObject[] Button_ = new GameObject[2];
    int UI_ButtonCount = 2;
    bool isPlay;

    public Text topText;
    public Text bottomText;
    int[] answer = { 1, 0, 1, 1, 0 };

    public string[] topS = {
        "열심히 공부했으니 우리 같이 퀴즈를 풀어볼까?",
        "내가 문제를 낼 테니 정답인 버튼을 누르는 거야.",
        "Q1. 양치질은 1분이면 충분하다?",
        "Q2. 양치질은 치아를 보호해준다?",
        "Q3. 하루에 한번만 양치를 해야한다?",
        "Q4. 양치질은 무조건 식후에 바로 해야한다?",
        "Q5. 사탕은 치아의 건강에 좋지않다?"
    };

    public string[] bottomS = {
        "퀴즈?  재미있겠다!",
        "문제 없지! 어서 시작하자.",
        "음 뭐였더라?...",
        "이건 알거같아!",
        "양치질은 중요하지!",
        "바로 했야 하는건가?",
        "사탕은 맛있는데..."
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
        if (Click)
        {
            FixedEvent_On(num);
        }
    }
    /*#########################################################################################################################*/
    private void Awake()
    {
        SceneChange();
    }
    private void Start()
    {
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
        topText.text = topS[State];
        bottomText.text = "";
        yield return new WaitForSeconds(1.5f);
        bottomText.text = bottomS[State];
        State++;
        yield return new WaitForSeconds(1.5f);
        topText.text = topS[State];
        yield return new WaitForSeconds(1.5f);
        bottomText.text = bottomS[State];
        yield return new WaitForSeconds(1.5f);
        topText.text = "그럼 시작한다?";
        bottomText.text = "";
        yield return new WaitForSeconds(2f);
        DimTipEvent(true);

    }

    IEnumerator Stage_N()
    {
        if (QState < MaxState)
        {
            button[0].DORewind();
            button[1].DORewind();
            SoundMgr.AudioPlay(SoundManager.SoundName.Answer);
            topText.text = "잘했어 정답이야!!";
            bottomText.text = "";
            yield return new WaitForSeconds(1f);
            bottomText.text = "신난다! 다음 문제를 맞춰보자!";
            yield return new WaitForSeconds(2f);
            topText.text = "그럼 다음 문제야.";
            bottomText.text = "";
            yield return new WaitForSeconds(2f);
            NextQuizSet();

        }
        else
        {
            SoundMgr.OutSound();
            SoundMgr.AudioPlay(SoundManager.SoundName.GameClear);
            AllButtonOff();
            ClearEffect.SetActive(true);
            topText.text = "굉장한데? 모두 정답이야!";
            bottomText.text = "";
            yield return new WaitForSeconds(1f);
            bottomText.text = "대단해!!";
            isPlay = false;
            yield return new WaitForSeconds(4f);

            if (CMotionTrackingManager.isNomal)
            {
                Popup.SetActive(true);
                yield return new WaitForSeconds(4f);
                isPlay = false;
                Loading.LoadScene("GameTeeth_MAIN");
            }
            else
            {
                Popup.SetActive(true);
                yield return new WaitForSeconds(4f);
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
            topText.text = topS[State];
            bottomText.text = bottomS[State];
        }
        isPlay = true;
    }
    void OneMoreTry()
    {
        SoundMgr.AudioPlay(SoundManager.SoundName.NoAnswer);
        bottomText.text = "정답이 아닌가봐 다시한번 생각해보자!";
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
        if (!ButtonPushCheck)
            StartCoroutine(ButtonPushDelay());
        Amount_Click temp = MotionTrackingMgr.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount += Time.deltaTime * OtherSubmitSpeed;

        if(!Pens[num-2].activeSelf)
        Pens[num - 2].SetActive(true);

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
            

            isPlay = false;

            Amount_Click temp1 = MotionTrackingMgr.fixed_Buttons[2].GetComponent<Amount_Click>();
            Amount_Click temp2 = MotionTrackingMgr.fixed_Buttons[3].GetComponent<Amount_Click>();
            temp1.Amount = 0;
            temp2.Amount = 0;

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
        if (MotionTrackingMgr.fixed_Buttons[_num].gameObject.activeSelf)
        {
            if (_num >= UI_ButtonCount)
            {
                if (!isPlay)
                    return;

                Click_Amount(_num);
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

}
