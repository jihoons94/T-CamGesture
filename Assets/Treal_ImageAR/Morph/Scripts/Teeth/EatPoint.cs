using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatPoint : MonoBehaviour {
    public Animator Dino;
    public MotionEvent_Snack Mgr;
    public GameObject EatSnackEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="Snack")
        {
            Destroy(collision.gameObject);
            Mgr.SnackCounts--;
            Mgr.Count++;
            Instantiate(EatSnackEffect).transform.position = transform.position;
            if (Mgr.Count > 5)
            {
                StartCoroutine(Mgr.GameOver());
                Dino.SetInteger("State", 0);
            }

            if (Mgr.SnackCounts==0)
            Dino.SetInteger("State", 0);
        }
    }
}
