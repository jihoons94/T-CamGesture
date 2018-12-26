using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DigitalRuby.AnimatedLineRenderer;

public class MoveObject : MonoBehaviour {
    DOTweenAnimation dotAnim;
    public AnimatedLineRenderer AnimatedLine;
    public Transform Dest;
    public bool PathStart;
    public MotionEvent_Learn CMotionLearn;
    float Movespeed = 0.8f;
    float temp;

    // Use this for initialization
    void Start () {
        PathStart = true;
        dotAnim = GetComponent<DOTweenAnimation>();
        dotAnim.CreateTween();
        dotAnim.DORestart();
        temp = Vector3.Distance(transform.position, Dest.position);
    }
    void LineRenderer(bool _state)
    {
        if (AnimatedLine == null)
        {
            return;
        }
        else if (_state)
        {
            Vector3 pos = transform.localPosition;
            //pos = temp.ScreenToWorldPoint(new Vector3(pos.x, pos.y, AnimatedLine.transform.position.z));

            AnimatedLine.Enqueue(pos);
        }
        else if (!_state)
        {
           
        }
    }

    private void Update()
    {
        if(CMotionLearn.IsPlaying)
        {
            LineRenderer(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!CMotionLearn.IsPlaying)
        {
            return;
        }
        if(collision.gameObject.tag =="CreatePoint")
        {
            Dest = collision.gameObject.GetComponent<GuideLinePath>().NextDest;
            float temp2 = Vector3.Distance( transform.position, Dest.position);
           
            float Speed = (temp - temp2);
            dotAnim.duration += Speed/1000;

            dotAnim.endValueTransform = Dest;
            dotAnim.CreateTween();
            LookDest();
            //dotAnim.DOPlay();
        }
    }


    public void LookDest()
    {
        var dir = Dest.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
