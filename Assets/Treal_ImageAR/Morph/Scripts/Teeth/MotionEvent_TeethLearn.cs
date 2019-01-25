using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MotionEvent_TeethLearn : Motion_Event
{
    public GameObject Eye01;
    public GameObject Eye02;
    public GameObject Eye03;
    public Animator Teeth;
    public Animator Dinosaur;
    public GameObject GameIn;
    public GameObject GameOut;
    public GameObject Effect;
    public GameObject[] Content;
    SoundManager SoundMgr;

    byte State = 0;

    public Text Dq01;
    public Text Dq02;


    private void Awake()
    {
        SoundMgr = GetComponent<SoundManager>();
        GameIn.SetActive(true);
        SceneChange();
        GetTextAnimation();
        //GameIn.SetActive(true);
    }

    void TextAnimPlay(DOTweenAnimation target)
    {
        target.CreateTween();
        target.DORestart();
    }

    void GetTextAnimation()
    {

    }

    IEnumerator TempEvent(bool isTop, string _Temp)
    {
        if(isTop)
        {
            
            yield return new WaitForSeconds(0.5f);
            Dq01.text = _Temp;

        }
        else
        {
            
            yield return new WaitForSeconds(0.5f);
            Dq02.text = _Temp;

        }
    }

    IEnumerator GameStory()
    {
        float time = 2.5f;
        yield return new WaitForSeconds(time);
        Dinosaur.SetInteger("State", 10);
        StartCoroutine(TempEvent(false, "우와 대단해! 너희들 덕분이야!"));
        yield return new WaitForSeconds(time);
        StartCoroutine(TempEvent(false, "양치질을 했더니 세균들이 모두 사라졌어!"));
        yield return new WaitForSeconds(time);
        Dinosaur.SetInteger("State", 0);
        StartCoroutine(TempEvent(false, "정말 고마워 덕분에 입안이 아프지가 않아"));
        yield return new WaitForSeconds(time);
        StartCoroutine(TempEvent(true, "양치질을 하지 않으면 세균들은 다시 나타날거야."));
        yield return new WaitForSeconds(time);
        Dinosaur.SetInteger("State", 9);
        StartCoroutine(TempEvent(false, "이제부터는 정말 열심히 양치질을 할거야"));
        yield return new WaitForSeconds(time);
        StartCoroutine(TempEvent(true, "양치질을 하는 것도 중요하지만 올바르게 해야해"));
        yield return new WaitForSeconds(time);
        Dinosaur.SetInteger("State", 0);
        StartCoroutine(TempEvent(true, "같이 올바른 양치질에 대해 알아볼까?"));
        yield return new WaitForSeconds(time);
        StartCoroutine(TempEvent(false, "어서 시작하자!"));
        yield return new WaitForSeconds(time);

        Content[0].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SoundMgr.AudioPlay(SoundManager.SoundName.NextQuiz);

        StartCoroutine(TempEvent(true, "그럼 첫번째, 양치질은 왜 해야 될까?"));
        yield return new WaitForSeconds(time);
        Dinosaur.SetInteger("State", 11);
        StartCoroutine(TempEvent(false, "세균을 물리쳐 주기 때문에??"));
        yield return new WaitForSeconds(time);
        Teeth.SetInteger("State", 5);
        StartCoroutine(TempEvent(true, "맞았어 하지만 이유는 더 있다는 사실!"));
        yield return new WaitForSeconds(time);
        Teeth.SetInteger("State", 0);
        Dinosaur.SetInteger("State", 0);
        StartCoroutine(TempEvent(true, "양치질을 하지 않으면 심한 입 냄새가 날 수 있어!"));
        yield return new WaitForSeconds(time);
        Eye01.SetActive(false);
        Eye02.SetActive(true);
        Dinosaur.SetInteger("State", 3);
        yield return new WaitForSeconds(0.5f);
        Dinosaur.SetInteger("State", 0);
        StartCoroutine(TempEvent(false, "으... 그건 너무 끔찍해"));
        yield return new WaitForSeconds(time);
        StartCoroutine(TempEvent(false, ""));
        StartCoroutine(TempEvent(true, "다음으로 넘어가자"));
        yield return new WaitForSeconds(time);

        Content[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        SoundMgr.AudioPlay(SoundManager.SoundName.NextQuiz);
        yield return new WaitForSeconds(1.5f);

        StartCoroutine(TempEvent(true, "그러면 양치질은 언제해야 될까?"));
        yield return new WaitForSeconds(time);
        Dinosaur.SetInteger("State", 11);
        StartCoroutine(TempEvent(false, "밥을 먹고 놀다가??"));
        yield return new WaitForSeconds(time);
        StartCoroutine(TempEvent(true, "틀렸어!, 바로 3분 이.내.에."));
        yield return new WaitForSeconds(time);
        Dinosaur.SetInteger("State", 0);
        StartCoroutine(TempEvent(false, "양치질은 얼마나 해야 하는 거야?"));
        yield return new WaitForSeconds(time);
        Teeth.SetInteger("State", 5);
        StartCoroutine(TempEvent(true, "너무 길지도 짧지도 않게 바로 3분 동안이야"));
        yield return new WaitForSeconds(time);
        Dinosaur.SetInteger("State", 10);
        StartCoroutine(TempEvent(false, "양치질은 3분동안! 깨끗하게!"));
        yield return new WaitForSeconds(time);


        Content[2].SetActive(true);
        yield return new WaitForSeconds(1f);
        SoundMgr.AudioPlay(SoundManager.SoundName.NextQuiz);
        yield return new WaitForSeconds(1.5f);
        Teeth.SetInteger("State", 0);
        Dinosaur.SetInteger("State",0);
        StartCoroutine(TempEvent(false, ""));
        StartCoroutine(TempEvent(true, "양치질은 하루에 몇번이나 해야 될까?"));
        yield return new WaitForSeconds(time);
        StartCoroutine(TempEvent(false, "많이 할수록 좋은 거 아니야?"));
        yield return new WaitForSeconds(time);
        StartCoroutine(TempEvent(true, "틀렸어! 바로 아침, 점식, 저녁 하루 3번이야"));
        yield return new WaitForSeconds(time);
        Dinosaur.SetInteger("State", 10);
        StartCoroutine(TempEvent(false, "식후 아침, 점심, 저녁 꼭 명심할게!"));
        yield return new WaitForSeconds(time);
        StartCoroutine(TempEvent(true, "이제부터 올바른 양치질로 치아를 지켜주자"));
        yield return new WaitForSeconds(time);
        Dinosaur.SetInteger("State", 0);
        StartCoroutine(TempEvent(false, "이제 문제없어!"));
        yield return new WaitForSeconds(time);

        Effect.SetActive(true);
        yield return new WaitForSeconds(2f);
        GameOut.SetActive(true);
        SoundMgr.OutSound();
        yield return new WaitForSeconds(3f);
        Loading.LoadScene("GameTeeth_Q");
    }


    private void Start()
    {
        StartCoroutine(GameStory());
    }

    override public void FixedEvent_On(int _num)
    {


    }

    override public void FixedEvent_Off(int _num)
    {
       
    }
}
