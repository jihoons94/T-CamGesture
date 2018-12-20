using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Motion_Test : MonoBehaviour {
    DOTweenAnimation dotAnim;
    public Transform temp;

    public Transform Dest;

    private void Start()
    {
        dotAnim = GetComponent<DOTweenAnimation>();
        dotAnim.DORewind();
        
    }

    private void Update()
    {
        LookDest();
    }

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
