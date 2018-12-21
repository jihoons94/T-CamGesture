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
    public GameObject UI_ReadyImage;
    public GameObject UI_StartImage;

    //public DOTweenAnimation RSAnim;

    public Text Doq;

    bool[] Attack = new bool[3];
    public CMotionTrackingManager MotionTrackingMgr;
    public GameObject FiexdImage_Event;
    public GameObject[] WaitImage_Event = new GameObject[3];
    public GameObject[] Image_Event = new GameObject[3];
   
    public List<Transform> CreatePoint;
    public List<int> Wait;
    public List<int> Wait_Image;
    public List<int> Random_save;
    public GameObject Effect;
    public GameObject Remove_Effect;

    public Transform Background;
    public Transform Background2;

    private float UISubmitSpeed = 5;
    private float OtherSubmitSpeed = 8;

    private float RecognitionMin = 2;


    int UI_ButtonCount=2;
    bool isPlay;

    private delegate void TypeVoid();

    /*#########################################################################################################################*/
    //윈도우 테스트용
    private bool Click = false;

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

    private void Update()
    {
        if (Click)
        {
            FixedEvent_On(2);
            FixedEvent_On(3);
            FixedEvent_On(4);
            FixedEvent_On(5);
            FixedEvent_On(6);
        }
            
        else
        {
            NoClick_Amount(2);
            NoClick_Amount(3);
            NoClick_Amount(4);
            NoClick_Amount(5);
            NoClick_Amount(6);
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

    private void Start()
    {
        CreatePointFindSet();
        isPlay = false;
        UI_Score.SetActive(false);
    }

    public void GameStart()
    {
        TypeVoid DelTemp = new TypeVoid(GameStartEvent);
        UI_Loding.GetComponent<DOTweenAnimation>().DORewind();
        StartCoroutine(Loading_Animation(DelTemp));
    }

    public void GameStop()
    {
        Doq.text = "치아들을 지켜줘서 고마워!";
        isPlay = false;
        UI_Score.SetActive(false);
        for (int i = UI_ButtonCount; i < MotionTrackingMgr.fixed_Buttons.Count; i++)
        {
            MotionTrackingMgr.fixed_Buttons[i].gameObject.SetActive(false);
        }
    }

    public void GameInit()
    {
        TypeVoid DelTemp = new TypeVoid(GameInitEvent);
        UI_Loding.GetComponent<DOTweenAnimation>().DORewind();
        StartCoroutine(Loading_Animation(DelTemp));
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
        for (int i = UI_ButtonCount; i < MotionTrackingMgr.numOfixed; i++)
        {
            if (!MotionTrackingMgr.fixed_Buttons[i].gameObject.activeSelf)
            {
                Vector3 NewPoint = RandomNewPoint(MotionTrackingMgr.fixed_Buttons[i].gameObject);
                MotionTrackingMgr.SetTrack_position_One(i, NewPoint);
                MotionTrackingMgr.fixed_Buttons[i].gameObject.SetActive(true);
            }
        }
    }

    private void GameStartEvent()
    {
        isPlay = true;
        StartCoroutine(WaitCreate());

        for (int i = 0; i < UI_ButtonCount; i++)
        {
            MotionTrackingMgr.fixed_Buttons[i].gameObject.SetActive(false);
        }

        for (int i = UI_ButtonCount; i < MotionTrackingMgr.fixed_Buttons.Count; i++)
        {
            MotionTrackingMgr.fixed_Buttons[i].gameObject.SetActive(true);
        }

        UI_Start.SetActive(false);
        UI_Exit.SetActive(false);
        UI_Main_Icon.SetActive(false);
        UI_Score.SetActive(true);
        Doq.text = "입안에 세균들로 부터 치아를 지켜줘!";
    }

    private void GameInitEvent()
    {
        for (int i = 0; i < UI_ButtonCount; i++)
        {
            MotionTrackingMgr.fixed_Buttons[i].gameObject.SetActive(true);
        }
        Doq.text = "같이 양치질을 배워볼까?";
        UI_Start.SetActive(true);
        UI_Exit.SetActive(true);
        UI_Main_Icon.SetActive(true);
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

    /// <summary>
    /// 로딩 화면 이후 해당 함수 실행
    /// </summary>
    /// <param name="_fun"></param>
    /// <returns></returns>
    IEnumerator Loading_Animation(TypeVoid _fun)
    {
        UI_Loding.GetComponent<DOTweenAnimation>().DORewind();
        UI_Loding.GetComponent<DOTweenAnimation>().DOPlay();
        yield return new WaitForSeconds(4f);

        _fun();
    }

    /// <summary>
    /// 파티클 거품 생성
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    IEnumerator BubbleEffect (int num)
    {
        Wait.Add(num);
        GameObject buble_Effect = Instantiate(Effect, Background);
        buble_Effect.transform.position = MotionTrackingMgr.fixed_Buttons[num].localPosition;
        yield return new WaitForSeconds(1f);
        Wait.Remove(num);
    }

    //IEnumerator ReadyStart()
    //{
    //    RSAnim.DOPlay();
    //    yield return new WaitForSeconds(3f);
    //}

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
        temp.Amount -= Time.deltaTime * 0.6f;
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
            RemoveEffectCreate(num);
            temp.Amount = 0;
            return true;
        }
        else
            return false;
    }

    void ChangeDoqInGame(int _num)
    {
        if(_num>=30 &&_num<100)
        {
            Doq.text = "치아들이 위험해 서둘러야겠어";
        }
        else if(_num >= 100 && _num <150)
        {
            Doq.text = "그래 그렇게 하는거야!";
        }
        else if(_num >=150 && _num <200)
        {
            Doq.text = "잘하는데? 세균들이 힘을 못쓰고 있어";
        }
        else if(_num >= 200)
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
                if (Click_Amount(_num))
                {
                    Score.ScoreCount += 10;
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
                        GameStart();
                    }
                    else
                    {
                        GameExit();
                    }
                }
            }
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
    }

    override public void MoveEvent_On(int _num)
    {
        //if (!Attack[_num])
        //{
        //    //StartCoroutine(MoveTarget_Attacks(_num));
        //    //ImageEvent_On(_num);
        //}
    }
    override public void MoveEvent_Off(int _num)
    {
        //ImageEvent_Off(_num);
    }
    override public void RandomEvent_On(int _num)
    {

    }
    override public void RandomEvent_Off(int _num)
    {

    }
}
