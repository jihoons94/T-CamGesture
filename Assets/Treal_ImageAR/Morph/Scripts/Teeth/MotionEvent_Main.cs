using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Treal.BrowserCore;
using UnityEngine.SceneManagement;

public class MotionEvent_Main : Motion_Event
{
    public GameObject GameIn;
    public GameObject GameOut;
    public GameObject Window_Canvas;
    private float UISubmitSpeed = 10;
    private float OtherSubmitSpeed = 5;
    public SoundManager SoundMgr;

    public float[] PenTime = { 0f, 0f, 0f, 0f, 0f};
    private byte? Button_Number = null;

    byte WindowSetUp=0;

    bool isPlay = false;

    //-----------------------------------------------------------------------------------------------------------------
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
    public void StartGame()
    {
        WindowSetUp = 1;
    }

    public void TeethRun()
    {
        WindowSetUp = 4;
    }

    public void SnackRun()
    {
        WindowSetUp = 3;
    }

    public void QuizRun()
    {
        WindowSetUp=5;
    }

    public void AllNo()
    {
        WindowSetUp = 0;
    }

    private void Update()
    {
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

        switch (WindowSetUp)
        {
            case 0:
                {
                    FixedEvent_Off(0);
                    FixedEvent_Off(1);
                    FixedEvent_Off(2);
                    FixedEvent_Off(3);
                    FixedEvent_Off(4);
                }
                break;
            case 1:
                {
                    FixedEvent_On(0);
                    FixedEvent_Off(1);
                    FixedEvent_Off(2);
                    FixedEvent_Off(3);
                    FixedEvent_Off(4);
                }
                break;
            case 2:
                {
                    FixedEvent_On(1);
                    FixedEvent_Off(0);
                    FixedEvent_Off(2);
                    FixedEvent_Off(3);
                    FixedEvent_Off(4);
                }
                break;
            case 3:
                {
                    FixedEvent_On(2);
                    FixedEvent_Off(0);
                    FixedEvent_Off(1);
                    FixedEvent_Off(3);
                    FixedEvent_Off(4);
                }
                break;
            case 4:
                {
                    FixedEvent_On(3);
                    FixedEvent_Off(0);
                    FixedEvent_Off(1);
                    FixedEvent_Off(4);
                    FixedEvent_Off(2);
                }
                break;
            case 5:
                {
                    FixedEvent_On(4);
                    FixedEvent_Off(0);
                    FixedEvent_Off(1);
                    FixedEvent_Off(3);
                    FixedEvent_Off(2);
                }
                break;
        }
    }
    //-----------------------------------------------------------------------------------------------------------------
    private void Awake()
    {
        SceneChange();
        GameIn.SetActive(true);
    }

    private void Start()
    {
        
        MotionTrackingMgr = GameObject.FindWithTag("MotionTrackingMgr").GetComponent<CMotionTrackingManager>();
        CanvsOn();
        if(MotionTrackingMgr != null)
        {
            isPlay = true;
        }
    }

    bool Click_Amount(int num)
    {
        Amount_Click temp = MotionTrackingMgr.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount += Time.deltaTime * OtherSubmitSpeed;

        PenTime[num] = 0.5f;

        if (temp.Amount >= temp.MaxAmount)
        {
            
            temp.Amount = 0;
            return true;
        }
        else
            return false;
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
    IEnumerator GameOutEvent(string Sname)
    {
        SoundMgr.AudioPlay(SoundManager.SoundName.Answer);
        
        GameOut.SetActive(true);
        yield return new WaitForSeconds(2f);
        Loading.LoadScene(Sname);
    }


    override public void FixedEvent_On(int _num)
    {
        if (!isPlay)
            return;

        // 버튼 오브젝트가 활성화 되어 있을 경우만 이벤트가 발생한다.
        if (!MotionTrackingMgr.fixed_Buttons[_num].gameObject.activeSelf)
        {
            return;
        }

        if (Button_Number == null || _num == Button_Number)
        {
            Button_Number = (byte)_num;
            if (Click_Amount(_num))
            {
                isPlay = false;
                switch (_num)
                {
                    case 0:
                        {
                            CMotionTrackingManager.isNomal = true;
                            //SceneManager.LoadScene("GameTeeth_Snack");
                            StartCoroutine(GameOutEvent("IntroVer02"));
                        }
                        break;
                    case 1:
                        {
                            Application.Quit();
                        }
                        break;
                    case 2:
                        {
                            CMotionTrackingManager.isNomal = false;
                            StartCoroutine(GameOutEvent("IntroVer02"));
                        }
                        break;
                    case 3:
                        {
                            CMotionTrackingManager.isNomal = false;
                            StartCoroutine(GameOutEvent("GameTeeth_MainGame"));
                            //SceneManager.LoadScene("GameTeeth_MainGame");
                        }
                        break;
                    case 4:
                        {
                            CMotionTrackingManager.isNomal = false;
                            StartCoroutine(GameOutEvent("GameTeeth_Q"));
                        }
                        break;
                }
                isPlay = false;
            }
        }
    }

    void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
    }

    override public void FixedEvent_Off(int _num)
    {
        if (!isPlay)
            return;
       if( MotionTrackingMgr.fixed_Buttons[_num].tag == "NotUsing")
            return;


        NoClick_Amount(_num);

    }

}
