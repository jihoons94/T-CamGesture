using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Transform StartP;
    public Transform EndP;
    public GameObject OverEffect;
    public MotionEvent_Teeth Teeth;
    public Image UI_Score;
    public Image Test_UI_Score;
    public static int ScoreCount;
    public static int Test_ScoreCount;
    int MaxScore = 300;
    float distance;

    IEnumerator GameOver()
    {
        OverEffect.SetActive(true);
        Teeth.GameStop();
        yield return new WaitForSeconds(5f);   
        Teeth.GameInit();
        //yield return null;
    }
	// Use this for initialization
	void Start () {
        distance = EndP.position.x - StartP.position.x;
        ScoreCount = 0;
	}
	// Update is called once per frame
	void Update () {
        float x = (distance / (float)MaxScore)* ScoreCount+(-189.3f);
        StartP.position = new Vector3(x, StartP.position.y,StartP.position.z);
        UI_Score.fillAmount = (float)((float)ScoreCount / (float)MaxScore);
        Test_UI_Score.fillAmount = (float)((float)Test_ScoreCount / (float)MaxScore);
        if (ScoreCount >= MaxScore)
        {
            ScoreCount = 0;
            StopAllCoroutines();
            StartCoroutine(GameOver());
        }
	}
}
