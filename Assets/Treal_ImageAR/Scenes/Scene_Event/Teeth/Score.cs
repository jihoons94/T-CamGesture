using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public MotionEvent_Teeth Teeth;
    public Text ScoreText;
    public static int ScoreCount;


	// Use this for initialization
	void Start () {
        ScoreCount = 0;
	}
	// Update is called once per frame
	void Update () {
        ScoreText.text = System.Convert.ToString(ScoreCount);
        if(ScoreCount >= 200)
        {
            Teeth.GameStop();
        }
	}
}
