using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitSceneSetUp : MonoBehaviour {
    private void Awake()
    {
        SceneManager.LoadScene("GameTeeth_MAIN");
    }

}
