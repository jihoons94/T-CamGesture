using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MotionEvent_Intro : Motion_Event
{
    public Text Doq;
    public GameObject GameIn;
    public GameObject GameOut;

    DOTweenAnimation TextOn;
    DOTweenAnimation TextOff;

    public GameObject Maincharacter;
    Animator MyAnim;

    public void Awake()
    {
        SceneChange();
        MyAnim = Maincharacter.GetComponent<Animator>();
    }

    private void Start()
    {
        Maincharacter.transform.position = new Vector3(-495.1f, -120.5f,0f);
        Maincharacter.GetComponent<DOTweenAnimation>().CreateTween();
        Maincharacter.GetComponent<DOTweenAnimation>().DOPlay();
        GetTextAnimation();
        GameIn.SetActive(true);
        StartCoroutine(StartIntro());
    }

    void GetTextAnimation()
    {
        TextOff = Doq.GetComponents<DOTweenAnimation>()[0];
        TextOn = Doq.GetComponents<DOTweenAnimation>()[1];
    }

    void TextAnimPlay(DOTweenAnimation target)
    {
        target.CreateTween();
        target.DORestart();
    }


    void TextOut(Text target, string _temp)
    {
        TextAnimPlay(TextOff);
        target.text = _temp;
        TextAnimPlay(TextOn);
    }


    IEnumerator TempEvent(string _Temp)
    {
        TextAnimPlay(TextOff);
        yield return new WaitForSeconds(0.5f);
        Doq.text = _Temp;
        TextAnimPlay(TextOn);
        yield return new WaitForSeconds(2.5f);
    }


    IEnumerator StartIntro()
    {
        yield return new WaitForSeconds(2f);
        MyAnim.SetInteger("State", 5);
        yield return new WaitForSeconds(1f);
        MyAnim.SetInteger("State", 0);
        yield return new WaitForSeconds(1f);
        MyAnim.SetInteger("State", 6);
        StartCoroutine(TempEvent("안녕? 난 공룡이라고해! 만나서 반가워!"));
        yield return new WaitForSeconds(3f);
        MyAnim.SetInteger("State", 0);
        StartCoroutine(TempEvent("놀러와줘서 고마워, 우리 뭐하고 놀까?"));
        yield return new WaitForSeconds(3f);
        MyAnim.SetInteger("State", 3);
        StartCoroutine(TempEvent("그래! 우선 간식부터 먹자! "));
        yield return new WaitForSeconds(3f);
        MyAnim.SetInteger("State", 0);
        StartCoroutine(TempEvent("어떤 간식들이 있는지 볼까?"));
        yield return new WaitForSeconds(3f);
    }

    override public void FixedEvent_On(int _num)
    {


    }

    override public void FixedEvent_Off(int _num)
    {

    }
}
