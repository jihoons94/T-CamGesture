using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public GameObject OverEffect;
    public MotionEvent_Teeth Teeth;
    public Image UI_Score;
    public static int ScoreCount;
    int MaxScore = 300;

    IEnumerator GameOver()
    {
        OverEffect.SetActive(true);
        Teeth.GameStop();
        yield return new WaitForSeconds(5f);
        OverEffect.SetActive(false);
        Teeth.GameInit();
        

    }
	// Use this for initialization
	void Start () {
        ScoreCount = 0;
	}
	// Update is called once per frame
	void Update () {

        UI_Score.fillAmount = (float)((float)ScoreCount / (float)MaxScore);
        if (ScoreCount >= MaxScore)
        {
            ScoreCount = 0;
            StartCoroutine(GameOver());
        }
	}
}
