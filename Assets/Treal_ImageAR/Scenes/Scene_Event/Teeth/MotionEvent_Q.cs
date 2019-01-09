using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Treal.BrowserCore;
using DG.Tweening;
using UnityEngine.UI;

public class MotionEvent_Q : Motion_Event
{
    public GameObject Popup;
    public DOTweenAnimation[] button = new DOTweenAnimation[2];
    public GameObject buttonBack1;
    public GameObject buttonBack2;

    public GameObject DoneEffect;
    public GameObject NotEffect;
    public GameObject Help_Text;
    int State;
    int QState;
    int MaxState = 4;
    public Transform Background;
    public Transform Background2;
    public GameObject Window_Canvas;

    private float UISubmitSpeed = 10;
    private float OtherSubmitSpeed = 5;
    private float RecognitionMin = 0.1f;

    public GameObject[] Button_ = new GameObject[2];
    int UI_ButtonCount = 2;
    bool isPlay;
    public CMotionTrackingManager MotionTrackingMgr;

    public Text topText;
    public Text bottomText;
    int[] answer = {1,0,1,1};

    public string[] topSa = {
        "열심히 공부했으니 우리 같이 퀴즈를 풀어볼까?",
        "내가 문제를 낼 테니 정답인 버튼을 누르는 거야.",
        "Q1. 양치질은 1분이면 충분하다?",
        "Q2. 양치질은 치아를 보호해준다?",
        "Q3. 하루에 한번만 양치를 해야한다?",
        "Q4. 양치질은 무조건 식후에 바로 해야한다.",
        ""
    };

    public string[] bottomSa = {
        "퀴즈?  재미있겠다!",
        "문제 없지! 어서 시작하자.",
        "음 뭐였더라?...",
        "이건 알거같아!",
        "양치질은 중요하지!",
        "바로 했야 하는건가?",
        ""
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
        num = 0;
    }

    public void ButtonOn02()
    {
        Click = true;
        num = 1;
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
                        FixedEvent_On(2);
                    }
                    break;
                case 1:
                    {
                        FixedEvent_On(3);
                    }
                    break;
            }
        }

        //else
        //{
        //    NoClick_Amount(2);
        //    NoClick_Amount(3);
        //}
    }
    /*#########################################################################################################################*/

    private void Start()
    {
        State = 0;
        isPlay = false;
        Help_Text.SetActive(false);
        StartCoroutine(StartEvent());
    }

    IEnumerator StartEvent()
    {
        topText.text = topSa[State];
        bottomText.text = "";
        yield return new WaitForSeconds(1f);
        bottomText.text = bottomSa[State];
        State++;
        yield return new WaitForSeconds(1f);
        topText.text = topSa[State];
        yield return new WaitForSeconds(1f);
        bottomText.text = bottomSa[State];
        yield return new WaitForSeconds(1f);

        
        NextQ();
        isPlay = true;
        yield return new WaitForSeconds(1f);
        Help_Text.SetActive(true);

    }

    IEnumerator Stage_N()
    {
        if (QState < MaxState)
        {
            topText.text = "잘했어 정답이야!!";
            bottomText.text = "";
            yield return new WaitForSeconds(1f);
            bottomText.text = "신난다! 다음 문제를 맞춰보자!";
            yield return new WaitForSeconds(1f);
            NextQ();
            isPlay = true;
        }
        else
        {
            buttonBack1.SetActive(false);
            buttonBack2.SetActive(false);
            MotionTrackingMgr.fixed_Buttons[2].gameObject.SetActive(false);
            MotionTrackingMgr.fixed_Buttons[3].gameObject.SetActive(false);
            Help_Text.SetActive(false);
            topText.text = "굉장한데? 모두 정답이야!";
            bottomText.text = "";
            yield return new WaitForSeconds(1f);
            bottomText.text = "대단해!!";
            isPlay = false;
            yield return new WaitForSeconds(2f);
            Popup.SetActive(true);
            yield return new WaitForSeconds(5f);
            Application.Quit();
        }

    }

    void NextQ()
    {
        State++;
        if(QState< MaxState)
        {
            Debug.Log(State);
            topText.text = topSa[State];
            bottomText.text = bottomSa[State];
        }
        else
        {
            
        }

    }
    void Try()
    {
        bottomText.text = "정답이 아닌가봐 다시한번 생각해보자!";
        isPlay = true;
    }

   bool AnswerF(int nAnswer)
    {
        return (answer[QState] == nAnswer? true : false);
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

    bool Click_Amount(int num)
    {

        Amount_Click temp = MotionTrackingMgr.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount += Time.deltaTime * OtherSubmitSpeed;
        Debug.Log("함수실행");
        //temp.Activate = true;
        if(!button[num-2].tween.IsPlaying())
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
            temp1.Amount = 0;
            Amount_Click temp2 = MotionTrackingMgr.fixed_Buttons[3].GetComponent<Amount_Click>();
            temp2.Amount = 0;
            bool result = AnswerF(num - 2);
            if(result)
            {
                QState++;
                Instantiate(DoneEffect).transform.position = MotionTrackingMgr.fixed_Buttons[num].position;
                StartCoroutine(Stage_N());
                
            }
            else
            {
                Instantiate(NotEffect).transform.position = MotionTrackingMgr.fixed_Buttons[num].position;
                Try();
            }
            temp.Amount = 0;
            
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
