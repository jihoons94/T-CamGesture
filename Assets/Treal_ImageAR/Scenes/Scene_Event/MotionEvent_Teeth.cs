using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Treal.BrowserCore;
using DG.Tweening;

public class MotionEvent_Teeth : Motion_Event
{
    public GameObject UI_Start;
    public GameObject UI_Exit;
    public GameObject UI_Score;
    public GameObject UI_Main_Icon;

    public GameObject UI_Loding;

    public Text Doq;

    bool[] Attack = new bool[3];
    public CMotionTrackingManager CMTM;
    public GameObject[] WaitImage_Event = new GameObject[3];
    public GameObject[] Image_Event = new GameObject[3];
    public GameObject FiexdImage_Event;
    public List<Transform> CreatePoint;
    public List<int> Wait;
    public List<int> Wait_Image;
    public List<int> Random_save;
    public GameObject Effect;
    public GameObject Remove_Effect;

    public Transform Background;
    public Transform Background2;


    int UI_ButtonCount=2;

    bool isPlay;

    //윈도우 테스트용
    bool Click = false;

    void DotWeenAnimation_Play(int _num, DOTweenAnimation _tar)
    {
        //CMTM.fixed_Buttons[_num].GetComponent<DOTweenAnimation>().DOPlay();
        _tar.DOPlay();

    }

    void DotWeenAnimation_DORewind(int _num, DOTweenAnimation _tar)
    {
        //CMTM.fixed_Buttons[_num].GetComponent<DOTweenAnimation>().DORewind();
        _tar.DORewind();
    }


    /// <summary>
    /// [jihoon]-해당 인덱스에 해당하는 이미지를 해당 위치로 두고 활성화 한다.
    /// </summary>
    /// <param name="num">검사하는 버튼의 인덱스 값</param>
    void ImageEvent_On(int _num)
    {
        Image_Event[_num].transform.position = new Vector3(CMTM.moving_Target[_num].localPosition.x - 10, CMTM.moving_Target[_num].localPosition.y, CMTM.moving_Target[_num].localPosition.z);
        Image_Event[_num].SetActive(true);
    }


    void WaitImageEvnet_On(int _num)
    {
        WaitImage_Event[_num].transform.position = new Vector3(CMTM.moving_Target[_num].localPosition.x + 10, CMTM.moving_Target[_num].localPosition.y, CMTM.moving_Target[_num].localPosition.z);
        WaitImage_Event[_num].SetActive(true);
    }

    /// <summary>
    /// [jihoon]-해당 인덱스에 해당하는 이미지를 해당 위치로 두고 비활성화 한다.
    /// </summary>
    /// <param name="num">검사하는 버튼의 인덱스 값</param>
    void ImageEvent_Off(int num)
    {
        Image_Event[num].SetActive(false);
    }

    void WaitImageEvnet_Off(int num)
    {
        WaitImage_Event[num].SetActive(false);
    }

    void find_set()
    {
        Transform CP = GameObject.FindGameObjectWithTag("CreatePoint").transform;
        for (int i = 0; i < CP.GetChildCount(); i++)
        {
            CreatePoint.Add(CP.GetChild(i));
        }
    }

    void UI_ButtonOnOff(bool _state)
    {
        for (int i = 0; i < UI_ButtonCount; i++)
        {
            CMTM.fixed_Buttons[i].gameObject.SetActive(_state);
        }

        for (int i = UI_ButtonCount; i < CMTM.fixed_Buttons.Count; i++)
        {
            CMTM.fixed_Buttons[i].gameObject.SetActive(!_state);
        }
    }

    private void Start()
    {
        find_set();
        isPlay = false;
        UI_Score.SetActive(false);

    }

    void GameStart()
    {
        UI_Loding.GetComponent<DOTweenAnimation>().DORewind();

        StartCoroutine(Loading_Animation());



    }

    public void GameStop()
    {
        Doq.text = "치아들을 지켜줘서 고마워!";
        isPlay = false;
        UI_Score.SetActive(false);
        for (int i = UI_ButtonCount; i < CMTM.fixed_Buttons.Count; i++)
        {
            CMTM.fixed_Buttons[i].gameObject.SetActive(false);
        }
    }

    public void GameInit()
    {
        UI_Loding.GetComponent<DOTweenAnimation>().DORewind();

        StartCoroutine(MainLoading_Animation());
    }

    void GameExit()
    {
        Application.Quit();
    }



    public Vector3 Rand_Point(GameObject _temp)
    {
        int temp = 0;
        while (true)
        {
            temp = Random.Range(0, CreatePoint.Count);
            if (!Random_save.Contains(temp))
            {
                _temp.name = System.Convert.ToString(temp);
                Random_save.Add(temp);
                break;
            }


        }
        return CreatePoint[temp].localPosition / 2;
    }

    void Set_Point()
    {
        for (int i = UI_ButtonCount; i < CMTM.numOfixed; i++)
        {
            if (!CMTM.fixed_Buttons[i].gameObject.activeSelf)
            {
                Vector3 NewPoint = Rand_Point(CMTM.fixed_Buttons[i].gameObject);
                CMTM.SetTrack_position_One(i, NewPoint);
                CMTM.fixed_Buttons[i].gameObject.SetActive(true);
            }
        }
    }

    IEnumerator WaitCreate()
    {
        while (isPlay)
        {
            Set_Point();
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator Target_Wait()
    {
        while (isPlay)
        {
            int temp = Random.Range(0, CMTM.numOfTarget);
            Debug.Log(CMTM.moving_Target[temp].gameObject.name);
            CMTM.moving_Target[temp].GetComponent<DOTweenPath>().DOPause();
            yield return new WaitForSeconds(3f);
            CMTM.moving_Target[temp].GetComponent<DOTweenPath>().DORewind();
            yield return new WaitForSeconds(6f);
        }
    }

    IEnumerator Loading_Animation()
    {
        UI_Loding.GetComponent<DOTweenAnimation>().DORewind();
        UI_Loding.GetComponent<DOTweenAnimation>().DOPlay();
        yield return new WaitForSeconds(2f);

        isPlay = true;
        StartCoroutine(WaitCreate());
        UI_ButtonOnOff(false);
        UI_Start.SetActive(false);
        UI_Exit.SetActive(false);
        UI_Main_Icon.SetActive(false);
        UI_Score.SetActive(true);
        Doq.text = "입안에 세균들로 부터 치아를 지켜줘!";
    }

    IEnumerator MainLoading_Animation()
    {
        UI_Loding.GetComponent<DOTweenAnimation>().DORewind();
        UI_Loding.GetComponent<DOTweenAnimation>().DOPlay();
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < UI_ButtonCount; i++)
        {
            CMTM.fixed_Buttons[i].gameObject.SetActive(true);
        }
        Doq.text = "같이 양치질을 배워볼까?";
        UI_Start.SetActive(true);
        UI_Exit.SetActive(true);
        UI_Main_Icon.SetActive(true);

    }



    IEnumerator MoveTarget_Attacks(int _num)
    {
        Attack[_num] = true;
        WaitImageEvnet_On(_num);
        CMTM.moving_Target[_num].GetComponent<DOTweenPath>().DOPause();
        yield return new WaitForSeconds(4f);
        CMTM.moving_Target[_num].GetComponent<DOTweenPath>().DOPlay();
        WaitImageEvnet_Off(_num);
        Attack[_num] = false;
    }

    IEnumerator Effect_(int num)
    {
        Wait.Add(num);
        GameObject buble_Effect = Instantiate(Effect, Background);
        buble_Effect.transform.position = CMTM.fixed_Buttons[num].localPosition;
        yield return new WaitForSeconds(1f);
        Wait.Remove(num);
    }

    void Wait_Event(int _num)
    {
        
        Wait_Image.Add(_num);

        GameObject buble_ImageEffect = Instantiate(FiexdImage_Event, Background);

        Bubble_ImageEvent temp = buble_ImageEffect.GetComponent<Bubble_ImageEvent>();
        buble_ImageEffect.transform.position =
            new Vector3(CMTM.fixed_Buttons[_num].localPosition.x, CMTM.fixed_Buttons[_num].localPosition.y, CMTM.fixed_Buttons[_num].localPosition.z);

        temp.Target = CMTM.fixed_Buttons[_num].gameObject;
        temp.Target_Button = CMTM.fixed_Buttons[_num].GetComponent<Amount_Click>();
        temp.Teeth = GetComponent<MotionEvent_Teeth>();
        temp.num = _num;
        return;
    }

    bool NoClick_Amount(int num)
    {
        Amount_Click temp = CMTM.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount -= Time.deltaTime * 0.6f;
        if (temp.Amount <= 0)
        {
            temp.Amount = 0;
            return true;
        }
        else
            return false;
    }

    bool UI_NoClick_Amount(int num)
    {
        Amount_Click temp = CMTM.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount -= Time.deltaTime;
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
        Amount_Click temp = CMTM.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount += Time.deltaTime * 8;
        if (temp.Amount >= 2)
        {
            if (!Wait.Contains(num))
            {
                StartCoroutine(Effect_(num));
            }
            if (!Wait_Image.Contains(num))
            {
                Wait_Event(num);
            }
        }
        if (temp.Amount >= temp.MAxAmount)
        {
            GameObject RemoveEffect = Instantiate(Remove_Effect, Background2);

            RemoveEffect.transform.position =
                new Vector3(CMTM.fixed_Buttons[num].localPosition.x, CMTM.fixed_Buttons[num].localPosition.y, CMTM.fixed_Buttons[num].localPosition.z);

            temp.Amount = 0;
            return true;
        }
        else
            return false;
    }

    bool UI_Click_Amount(int num)
    {
        Amount_Click temp = CMTM.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount += Time.deltaTime * 5;

        if (temp.Amount >= temp.MAxAmount)
        {
            GameObject RemoveEffect = Instantiate(Remove_Effect, Background2);

            RemoveEffect.transform.position =
                new Vector3(CMTM.fixed_Buttons[num].localPosition.x, CMTM.fixed_Buttons[num].localPosition.y, CMTM.fixed_Buttons[num].localPosition.z);

            temp.Amount = 0;
            return true;
        }
        else
            return false;
    }

    public void ButtonOn()
    {
        Click = true;
    }

    public void ButtonOff()
    {
        Click = false;
    }

    public void ButtonStart()
    {
        GameStart();
    }

    //private void Update()
    //{
    //    if (Click)
    //        FixedEvent_On(2);
    //    else
    //        NoClick_Amount(2);
    //}


    override public void FixedEvent_On(int _num)
    {

        if (CMTM.fixed_Buttons[_num].gameObject.activeSelf)
        {
            if (_num >= UI_ButtonCount)
            {
                if (Click_Amount(_num))
                {
                    Score.ScoreCount += 10;
                    CMTM.fixed_Buttons[_num].gameObject.SetActive(false);
                    int temp = System.Convert.ToInt32(CMTM.fixed_Buttons[_num].name);
                    Random_save.Remove(temp);
                }

            }
            else //  UI 버튼일 경우
            {
                if (UI_Click_Amount(_num))
                {
                    if (_num == 0)
                    {
                        GameStart();
                    }
                    else
                    {
                        GameExit();
                    }
                }
            }
        }


        switch (_num)
        {
            case 0:
                {

                }
                break;
        }
    }

    override public void FixedEvent_Off(int _num)
    {
        if (_num >= UI_ButtonCount)
        {
            NoClick_Amount(_num);
        }
        else
        {
            UI_NoClick_Amount(_num);
        }
           
        switch (_num)
        {
            case 0:
                {

                }
                break;
        }
    }



    override public void MoveEvent_On(int _num)
    {
        if (!Attack[_num])
        {
            StartCoroutine(MoveTarget_Attacks(_num));
            ImageEvent_On(_num);
        }
    }
    override public void MoveEvent_Off(int _num)
    {
        ImageEvent_Off(_num);
    }
    override public void RandomEvent_On(int _num)
    {

    }
    override public void RandomEvent_Off(int _num)
    {

    }
}
