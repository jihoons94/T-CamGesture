using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Animation : MonoBehaviour {
    Animator MyAnim;
    public int State;
    // Use this for initialization
    private void Awake()
    {
        MyAnim = GetComponent<Animator>();
        MyAnim.SetInteger("State", State);
    }
}
