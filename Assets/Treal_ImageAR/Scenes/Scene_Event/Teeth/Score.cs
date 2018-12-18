using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public GameObject OverEffect;
    public MotionEvent_Teeth Teeth;
    public Text ScoreText;
    public static int ScoreCount;

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
        ScoreText.text = System.Convert.ToString(ScoreCount);
        if(ScoreCount >= 200)
        {
            ScoreCount = 0; 
            StartCoroutine(GameOver());
        }
	}
}
