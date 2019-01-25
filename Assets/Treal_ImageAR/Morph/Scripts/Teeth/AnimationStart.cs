using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStart : MonoBehaviour {
    public Animator Dino;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Snack")
        {
            Dino.SetInteger("State", 8);
        }
    }
}
