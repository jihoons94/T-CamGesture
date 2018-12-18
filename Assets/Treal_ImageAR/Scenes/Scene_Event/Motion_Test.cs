using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Motion_Test : MonoBehaviour {
    DOTweenAnimation dotAnim;
    public Transform temp;

    private void Start()
    {
        dotAnim = GetComponent<DOTweenAnimation>();
        dotAnim.DORewind();
    }

    public void setup()
    {
        dotAnim.endValueTransform = temp;
    }
}
