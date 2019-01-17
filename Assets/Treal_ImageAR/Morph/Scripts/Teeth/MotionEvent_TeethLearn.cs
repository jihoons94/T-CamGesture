using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotionEvent_TeethLearn : Motion_Event
{
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
        //GameIn.SetActive(true);
    }

    IEnumerator StartEvent()
    {
        yield return new WaitForSeconds(2f);
        Dq02.text = "양치질을 했더니 세균들이 모두 사라졌어!";
        yield return new WaitForSeconds(2f);
        Dq02.text = "정말 고마워 덕분에 입안이 아프지가 않아 ";
        yield return new WaitForSeconds(2f);
        Dq01.text = "양치질을 하지 않으면 세균들은 다시 나타날거야.";
        Dq02.text = "";
        yield return new WaitForSeconds(2f);
        Dq02.text = "이제부터는 정말 열심히 양치질을 할거야";
        yield return new WaitForSeconds(2f);
        Dq01.text = "양치질을 하는 것도 중요하지만 올바르게 해야해";
        Dq02.text = "";
        yield return new WaitForSeconds(2f);
        Dq01.text = "같이 올바른 양치질에 대해 알아볼까?";
        yield return new WaitForSeconds(2f);
        Dq02.text = "어서 시작하자!";
        yield return new WaitForSeconds(2f);
        Dq02.text = " ";
        Dq01.text = "";
        yield return new WaitForSeconds(1f);

        Content[0].SetActive(true);
        yield return new WaitForSeconds(1f);
        SoundMgr.AudioPlay(SoundManager.SoundName.NextQuiz);

        yield return new WaitForSeconds(2f);
        Dq01.text = "그럼 첫번째, 양치질은 왜 해야될까?";
        yield return new WaitForSeconds(2f);
        Dq02.text = " 세균을 물리쳐 주기 때문에?? ";
        yield return new WaitForSeconds(2f);
        Dq01.text = "맞았어 하지만 이유는 더 있다는 사실!";
        yield return new WaitForSeconds(1f);
        Dq02.text = " ";
        yield return new WaitForSeconds(1f);
        Dq01.text = "양치질을 하지 않으면 심한 입냄새가 날 수 있어!";
        yield return new WaitForSeconds(2f);
        Dq02.text = " 으... 그건 너무 끔찍해";
        yield return new WaitForSeconds(2f);
        Dq02.text = " ";
        Dq01.text = "그리고 양치질은 치아를 깨끗하게 만들어줘서";
        yield return new WaitForSeconds(2f);
        Dq01.text = "충치가 생기는 것을 방지해 주지!";
        yield return new WaitForSeconds(2f);
        Dq02.text = "양치질은 정말 중요하구나!";
        yield return new WaitForSeconds(2f);
        Dq01.text = "다음으로 넘어가자";
        yield return new WaitForSeconds(2f);
        Dq02.text = "";
        Dq01.text = "";
        yield return new WaitForSeconds(1f);
        Content[1].SetActive(true);

        yield return new WaitForSeconds(1f);
        SoundMgr.AudioPlay(SoundManager.SoundName.NextQuiz);
        yield return new WaitForSeconds(2f);
        Dq01.text = "그러면 양치질은 언제해야 될까?";
        yield return new WaitForSeconds(2f);
        Dq02.text = "밥을 먹고 놀다가??";
        yield return new WaitForSeconds(2f);
        Dq02.text = "";
        Dq01.text = "틀렸어!, 바로 3분 이.내.에.";
        yield return new WaitForSeconds(2f);
        Dq01.text = "하지만 사탕이나 초콜렛을 먹었다면 10분뒤에!";
        yield return new WaitForSeconds(2f);
        Dq02.text = "어째서??";
        yield return new WaitForSeconds(2f);
        Dq01.text = "입안에 설탕이 남아있기 때문에 바로 해서는 안돼.";
        yield return new WaitForSeconds(2f);
        Dq01.text = "설탕이 들어간 단음식을 먹었다면 10분 뒤에를 명심해";
        yield return new WaitForSeconds(2f);
        Dq02.text = "단음식은 10분뒤에! 밥을 먹었을 때는 3분이내!";
        yield return new WaitForSeconds(2f);
        Dq02.text = "";
        Dq01.text = "자, 또 넘어가 볼까?";
        yield return new WaitForSeconds(1f);
        Dq01.text = "";
        yield return new WaitForSeconds(1f);

        Content[2].SetActive(true);
        yield return new WaitForSeconds(1f);
        SoundMgr.AudioPlay(SoundManager.SoundName.NextQuiz);
        yield return new WaitForSeconds(2f);
        Dq01.text = "양치질은 하루에 몇번이나 해야될까?";
        yield return new WaitForSeconds(2f);
        Dq02.text = "많이 할수록 좋은거 아니야?";
        yield return new WaitForSeconds(2f);
        Dq01.text = "틀렸어! 바로 아침, 점식, 저녁 하루 3번이야";
        yield return new WaitForSeconds(2f);
        Dq01.text = "";
        Dq02.text = "식후 아침, 점심, 저녁 꼭 명심할게!";
        yield return new WaitForSeconds(2f);
        Dq01.text = "그러면 다음으로 넘어가자";
        Dq02.text = "";
        yield return new WaitForSeconds(2f);
        Dq02.text = "";
        Dq01.text = "";
        yield return new WaitForSeconds(1f);

        Content[3].SetActive(true);
        yield return new WaitForSeconds(1f);
        SoundMgr.AudioPlay(SoundManager.SoundName.NextQuiz);
        yield return new WaitForSeconds(2f);
        Dq01.text = "양치질은 몇분 동안 해야될까?";
        yield return new WaitForSeconds(2f);
        Dq02.text = "구석구석 깨끗하게 오래오래?";
        yield return new WaitForSeconds(2f);
        Dq01.text = "구석구석 깨끗하게 해야하지만";
        yield return new WaitForSeconds(2f);
        Dq02.text = "";
        Dq01.text = "너무 길지도 짧지도 않게 바로 3분 동안이야";
        yield return new WaitForSeconds(2f);
        Dq02.text = "양치질은 3분동안! 깨끗하게!";
        yield return new WaitForSeconds(2f);
        Dq02.text = "";
        Dq01.text = "";
        yield return new WaitForSeconds(1f);

        Content[4].SetActive(true);
        yield return new WaitForSeconds(1f);
        SoundMgr.AudioPlay(SoundManager.SoundName.NextQuiz);
        yield return new WaitForSeconds(2f);
        Dq01.text = "마지막으로 사탕과 초콜릿은 치아에 좋을까?";
        yield return new WaitForSeconds(2f);
        Dq02.text = "맛있기는 하지만 치아 건강에는 좋지 않을거 같아";
        yield return new WaitForSeconds(2f);
        Dq01.text = "맞았어, 설탕이 들어간것은 치아 건강에 좋지않아.";
        yield return new WaitForSeconds(2f);
        Dq02.text = "";
        Dq01.text = "아무리 맛있더라도 너무 많이 먹으면 충치가 생길 수 있어!";
        yield return new WaitForSeconds(2f);
        Dq02.text = "으... 충치는 너무나도 싫어";
        yield return new WaitForSeconds(2f);
        Dq02.text = "";
        Dq01.text = "이제부터 올바른 양치질로 치아를 지켜주자";
        yield return new WaitForSeconds(2f);
        Dq02.text = "이제 문제없어!";
        yield return new WaitForSeconds(2f);
        Effect.SetActive(true);
        yield return new WaitForSeconds(2f);
        GameOut.SetActive(true);
        SoundMgr.OutSound();
        yield return new WaitForSeconds(3f);
        Loading.LoadScene("GameTeeth_Q");
    }


    private void Start()
    {
        StartCoroutine(StartEvent());
    }

    override public void FixedEvent_On(int _num)
    {


    }

    override public void FixedEvent_Off(int _num)
    {
       
    }
}
