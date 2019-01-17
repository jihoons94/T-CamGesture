using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenAnimation : MonoBehaviour {
    public float AnimationTime;
    private Animator MyAnimator;
    public Q_Button CbuttonQ;


    // Use this for initialization
    private void Start()
    {
        MyAnimator = GetComponent<Animator>();
        MyAnimator.SetLayerWeight(0, 1);
    }


    private void Update()
    {
        //MyAnimation["Pen"].time= time;
        AnimationTime = MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        MyAnimator.ForceStateNormalizedTime(CbuttonQ.Amount/9.5f);

        if(CbuttonQ.Amount ==1.0f|| CbuttonQ.Amount == 0.0f)
        {
            gameObject.SetActive(false);
        }
    }

}
