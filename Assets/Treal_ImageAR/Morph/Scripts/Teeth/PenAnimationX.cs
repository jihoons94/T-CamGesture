using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenAnimationX : MonoBehaviour {
    public float AnimationTime;
    private Animator MyAnimator;
    public Animator Fillbar01;
    public Animator Fillbar02;
    public Q_Button CbuttonQ;
    public GameObject Tartget;

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
        MyAnimator.ForceStateNormalizedTime(CbuttonQ.Amount / 8.0f);
        Fillbar01.ForceStateNormalizedTime(CbuttonQ.Amount / 8.0f);
        Fillbar02.ForceStateNormalizedTime(CbuttonQ.Amount / 8.0f);

        if (CbuttonQ.Amount == 1.0f || CbuttonQ.Amount == 0.0f)
        {
            Tartget.SetActive(false);
        }
    }
}
