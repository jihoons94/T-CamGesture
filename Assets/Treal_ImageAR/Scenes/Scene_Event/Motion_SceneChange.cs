using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Treal.BrowserCore;

public class Motion_SceneChange : Motion_Event
{

    IEnumerator StartTime()
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("Tcam_NextScene2");

    }

    private void Start()
    {
        StartCoroutine(StartTime());
    }
    public GameObject[] Button;
    override public void FixedEvent_On(int _num)
    {
        Button[_num].SetActive(false);
    }


    override public void FixedEvent_Off(int _num)
    {
        Button[_num].SetActive(true);
    }
}
