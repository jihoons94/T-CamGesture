using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Motion_Test : MonoBehaviour {
    DOTweenAnimation dotAnim;
    public Transform Dest;
    bool Line;

    private void Start()
    {
        dotAnim = GetComponent<DOTweenAnimation>();
        dotAnim.CreateTween();
        dotAnim.DORestart();
        
    }

    public void WLine()
    {

    }

    public void WCricle()
    {
        
    }
    /// <summary>
    /// 1. 이동 경로 라인을 획마다 찍어서 저장한다.
    /// 2. Rigidbody로 이동시키고 해당 포인트에 접촉할 때마다 다음 이동 포인트를 반환한다.
    /// </summary>


    //private void Update()
    //{
    //    LookDest();
    //}

    //public void setup()
    //{
    //    dotAnim.endValueTransform = temp;
    //}

    public void LookDest()
    {
        var dir = Dest.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
