using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Treal.BrowserCore;
using DG.Tweening;

public class MotionEvent_Teeth : Motion_Event
{
    public GameObject GameIn;
    public GameObject GameOut;
    public GameObject[] Dimobject;
    public GameObject TempTip;
    public GameObject Window_Canvas;
    public GameObject ScoreEffect;
    public GameObject DQ;
    public GameObject ICon;
    public GameObject IConViewEffect;
    public GameObject UI_Score;
    SoundManager SoundMgr;

    public Text Doq;
    public GameObject FiexdImage_Event;

    public List<Transform> CreatePoint;
    public List<int> Wait;
    public List<int> Wait_Image;
    public List<int> Random_save;
    public GameObject Effect;
    public GameObject Remove_Effect;

    public Transform Background;
    public Transform Background2;

    private float UISubmitSpeed = 10;
    private float OtherSubmitSpeed = 8;

    private float RecognitionMin = 1;


    int UI_ButtonCount = 2;
    bool UserActivate;
    bool isPlay;

    private delegate void TypeVoid();

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

    public void ButtonOn()
    {
        Click = true;
    }

    public void ButtonOff()
    {
        Click = false;
    }


    private void Update()
    {
        if (Click)
        {
            FixedEvent_On(0);
            FixedEvent_On(2);

        }

        else
        {
            NoClick_Amount(2);

        }
    }
    /*#########################################################################################################################*/


    //만들어 놓고 미사용 함수
    /*#########################################################################################################################*/
    ///// <summary>
    ///// [jihoon]-해당 인덱스에 해당하는 이미지를 해당 위치로 두고 활성화 한다.
    ///// </summary>
    ///// <param name="num">검사하는 버튼의 인덱스 값</param>
    //void ImageEvent_On(int _num)
    //{
    //    Image_Event[_num].transform.position = new Vector3(MotionTrackingMgr.moving_Target[_num].localPosition.x - 10, MotionTrackingMgr.moving_Target[_num].localPosition.y, MotionTrackingMgr.moving_Target[_num].localPosition.z);
    //    Image_Event[_num].SetActive(true);
    //}

    ///// <summary>
    ///// [jihoon]-해당 인덱스에 해당하는 이미지를 해당 위치로 두고 비활성화 한다.
    ///// </summary>
    ///// <param name="num">검사하는 버튼의 인덱스 값</param>
    //void ImageEvent_Off(int num)
    //{
    //    Image_Event[num].SetActive(false);
    //}

    //void WaitImageEvnet_On(int _num)
    //{
    //    WaitImage_Event[_num].transform.position = new Vector3(MotionTrackingMgr.moving_Target[_num].localPosition.x + 10, MotionTrackingMgr.moving_Target[_num].localPosition.y, MotionTrackingMgr.moving_Target[_num].localPosition.z);
    //    WaitImage_Event[_num].SetActive(true);
    //}

    //void WaitImageEvnet_Off(int num)
    //{
    //    WaitImage_Event[num].SetActive(false);
    //}

    ///// <summary>
    ///// 일정 시간마다 움직이는 버튼이 랜덤으로 이동 정지
    ///// </summary>
    ///// <returns></returns>
    //IEnumerator Target_Wait()
    //{
    //    while (isPlay)
    //    {
    //        int temp = Random.Range(0, MotionTrackingMgr.numOfTarget);
    //        Debug.Log(MotionTrackingMgr.moving_Target[temp].gameObject.name);
    //        MotionTrackingMgr.moving_Target[temp].GetComponent<DOTweenPath>().DOPause();
    //        yield return new WaitForSeconds(3f);
    //        MotionTrackingMgr.moving_Target[temp].GetComponent<DOTweenPath>().DORewind();
    //        yield return new WaitForSeconds(6f);
    //    }
    //}

    ///// <summary>
    ///// 움직이는 버튼이 인식 될 경우 이벤트 처리문
    ///// </summary>
    ///// <param name="_num"></param>
    ///// <returns></returns>
    //IEnumerator MoveTarget_Attacks(int _num)
    //{
    //    Attack[_num] = true;
    //    WaitImageEvnet_On(_num);
    //    MotionTrackingMgr.moving_Target[_num].GetComponent<DOTweenPath>().DOPause();
    //    yield return new WaitForSeconds(4f);
    //    MotionTrackingMgr.moving_Target[_num].GetComponent<DOTweenPath>().DOPlay();
    //    WaitImageEvnet_Off(_num);
    //    Attack[_num] = false;
    //}
    /*#########################################################################################################################*/

    void CreatePointFindSet()
    {
        Transform CP = GameObject.FindGameObjectWithTag("CreatePoint").transform;
        for (int i = 0; i < CP.GetChildCount(); i++)
        {
            CreatePoint.Add(CP.GetChild(i));
        }
    }
    private void Awake()
    {
        SceneChange();
        GameIn.SetActive(true);
    }

    private void Start()
    {
        CanvsOn();
        SoundMgr = GetComponent<SoundManager>();
        UserActivate = true;
        CreatePointFindSet();
        isPlay = false;
        UI_Score.SetActive(false);
        GameStart();
    }

    /// <summary>
    /// 게임 전 시나리오가 진행된다.
    /// </summary>
    /// <returns></returns>
    IEnumerator GameStartEventS()
    {
        Doq.text = "";
        SoundMgr.StartBGM();
        DQ.SetActive(true);
        yield return new WaitForSeconds(2f);
        ICon.SetActive(true);
        SoundMgr.AudioPlay(SoundManager.SoundName.Boomb);
        IConViewEffect.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        Doq.text = "세균들이 치아들을 괴롭히고 있어!";
        yield return new WaitForSeconds(2.5f);
        Doq.text = "우리들이 도와줘야 할것 같아";
        yield return new WaitForSeconds(2.5f);
        Doq.text = "양치질처럼 세균들을 모두 닦아내자!";
        yield return new WaitForSeconds(2.5f);
        Doq.text = "준비 됐지? 그럼 시작한다?";
        yield return new WaitForSeconds(2.5f);
        Doq.text = "치아들을 구해줘!";
        yield return new WaitForSeconds(1f);
        UI_Score.SetActive(true);
        ScoreEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        //TempTipEvent(true);
        StartCoroutine(DimTipEvent());
    }


    public void GameStart()
    {
        isPlay = true;
        //SetNewButtonPosition();

        UserActivate = false;
        SoundMgr.SetSoundVolume(0.7f);
        StartCoroutine(GameStartEventS());
    }

    public void GameStop()
    {
        Doq.text = "치아들을 지켜줘서 고마워!";
        isPlay = false;
        UserActivate = false;
        UI_Score.SetActive(false);
        for (int i = UI_ButtonCount; i < 7; i++)
        {
            MotionTrackingMgr.fixed_Buttons[i].gameObject.SetActive(false);
        }
        Debug.Log("GameStop()");
    }

    public void GameInit()
    {
        StartCoroutine(GameOutEvent());
    }
    IEnumerator GameOutEvent()
    {
        Debug.Log("Init실행");
        SoundMgr.OutSound();
        isPlay = false;
        UserActivate = false;
        GameOut.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        if (CMotionTrackingManager.isNomal)
        {
            Loading.LoadScene("GameTeeth_Q");
        }
        else
        {
            Loading.LoadScene("GameTeeth_MAIN");
        }

    }


    public void GameExit()
    {
        Application.Quit();
    }

    /// <summary>
    /// 미리 지정되어 있는 여러 좌표중 랜덤으로 하나를 정해 반환한다.
    /// 이때 해당 버튼의 이름 선택된 좌표의 번호를 이름으로 갖는다.
    /// </summary>
    /// <param name="_temp"></param>
    /// <returns></returns>
    public Vector3 RandomNewPoint(GameObject _temp)
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

    /// <summary>
    /// 비활성화 된 모든 인식 영역 버튼이 새로운 좌표로 변경되어 다시 활성화 된다.
    /// </summary>
    void SetNewButtonPosition()
    {
        for (int i = UI_ButtonCount; i < 7; i++)
        {
            if (!MotionTrackingMgr.fixed_Buttons[i].gameObject.activeSelf)
            {
                Vector3 NewPoint = RandomNewPoint(MotionTrackingMgr.fixed_Buttons[i].gameObject);
                MotionTrackingMgr.SetTrack_position_One(i, NewPoint);
                MotionTrackingMgr.fixed_Buttons[i].gameObject.SetActive(true);
            }
        }
    }

    private void GameInitEvent()
    {
        for (int i = 0; i < UI_ButtonCount; i++)
        {
            MotionTrackingMgr.fixed_Buttons[i].gameObject.SetActive(true);
        }
        DQ.SetActive(false);
    }

    /// <summary>
    /// 정해진 간격마다 비활성화 된 인식영역 버튼을 새로 설정하고 활성화 한다.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitCreate()
    {
        while (isPlay)
        {
            SetNewButtonPosition();
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator DimTipEvent()
    {
        TempTip.SetActive(true);
        yield return new WaitForSeconds(1f);
        for (int i=0; i<3; i++)
        {
            yield return new WaitForSeconds(1f);
            Dimobject[i].SetActive(true);
        }

        yield return new WaitForSeconds(1.5f);
        for (int z=3; z<5; z++)
        {
            Dimobject[z].SetActive(true);
        }
        yield return new WaitForSeconds(3f);
        Dimobject[5].SetActive(true);
        MotionTrackingMgr.fixed_Buttons[0].gameObject.SetActive(true);

    }

    void TempTipEvent(bool State)
    {
        MotionTrackingMgr.fixed_Buttons[0].gameObject.SetActive(State);
        Destroy(TempTip);
    }

    /// <summary>
    /// 파티클 거품 생성
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    IEnumerator BubbleEffect(int num)
    {
        Wait.Add(num);
        GameObject buble_Effect = Instantiate(Effect, Background);
        buble_Effect.transform.position = MotionTrackingMgr.fixed_Buttons[num].localPosition;
        yield return new WaitForSeconds(1f);
        Wait.Remove(num);
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

    void ImageBubbleEffect(int _num)
    {

        Wait_Image.Add(_num);

        GameObject buble_ImageEffect = Instantiate(FiexdImage_Event, Background);

        Bubble_ImageEvent temp = buble_ImageEffect.GetComponent<Bubble_ImageEvent>();

        buble_ImageEffect.transform.position =
            new Vector3(
            MotionTrackingMgr.fixed_Buttons[_num].localPosition.x,
            MotionTrackingMgr.fixed_Buttons[_num].localPosition.y,
            MotionTrackingMgr.fixed_Buttons[_num].localPosition.z
            );

        temp.Target = MotionTrackingMgr.fixed_Buttons[_num].gameObject;
        temp.Target_Button = MotionTrackingMgr.fixed_Buttons[_num].GetComponent<Amount_Click>();
        temp.Teeth = GetComponent<MotionEvent_Teeth>();
        temp.num = _num;
        return;
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

    bool Click_Amount(int num)
    {
        Amount_Click temp = MotionTrackingMgr.fixed_Buttons[num].GetComponent<Amount_Click>();
        temp.Amount += Time.deltaTime * OtherSubmitSpeed;

        if (temp.Amount >= RecognitionMin)
        {
            temp.Activate = true;
            if (!Wait.Contains(num))
                StartCoroutine(BubbleEffect(num));

            if (!Wait_Image.Contains(num))
                ImageBubbleEffect(num);
        }
        if (temp.Amount >= temp.MaxAmount)
        {
            RemoveEffectCreate(num);
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

    void ChangeDoqInGame(int _num)
    {
        if (_num >= 30 && _num < 100)
        {
            Doq.text = "치아들이 위험해 서둘러야겠어";
        }
        else if (_num >= 100 && _num < 150)
        {
            Doq.text = "그래 그렇게 하는거야!";
        }
        else if (_num >= 150 && _num < 200)
        {
            Doq.text = "잘하는데? 세균들이 힘을 못쓰고 있어";
        }
        else if (_num >= 200)
        {
            Doq.text = "얼마 안남았어 힘을 내!";
        }
    }

    override public void FixedEvent_On(int _num)
    {


        // 버튼 오브젝트가 활성화 되어 있을 경우만 이벤트가 발생한다.
        if (MotionTrackingMgr.fixed_Buttons[_num].gameObject.activeSelf)
        {
            // 씬의 게임에 사용되는 버튼일 경우
            if (_num >= UI_ButtonCount)
            {
                if (!UserActivate)
                    return;
                if (Click_Amount(_num))
                {
                    Score.ScoreCount += 20;
                    ChangeDoqInGame(Score.ScoreCount);
                    MotionTrackingMgr.fixed_Buttons[_num].gameObject.SetActive(false);
                    int temp = System.Convert.ToInt32(MotionTrackingMgr.fixed_Buttons[_num].name);
                    Random_save.Remove(temp);
                }

            }
            else //  UI 버튼일 경우
            {
                if (UI_Click_Amount(_num))
                {
                    if (_num == 0)
                    {
                        TempTipEvent(false);
                        StartCoroutine(WaitCreate());
                        UserActivate = true;
                    }
                    else
                    {
                        isPlay = false;
                        UserActivate = false;
                        Loading.LoadScene("GameTeeth_MAIN");
                    }
                }
            }
        }
    }

    override public void FixedEvent_Off(int _num)
    {
        if (!UserActivate ||_num >= 7)
            return;
        if (_num >= UI_ButtonCount)
        {
            NoClick_Amount(_num);
        }
        else
        {
            UI_NoClick_Amount(_num);
        }
    }

}
