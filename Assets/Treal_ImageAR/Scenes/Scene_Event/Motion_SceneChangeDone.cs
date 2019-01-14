using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Treal.BrowserCore;
using UnityEngine.SceneManagement;

public class Motion_SceneChangeDone : Motion_Event
{
    public string SceneName;
    CMotionTrackingManager MotionMgr;
    CTCamCameraPreviewCtrl TCamCameraP;

    private void Awake()
    {
        TCamCameraP = GameObject.FindGameObjectWithTag("mTrealPreviewObj").GetComponent<CTCamCameraPreviewCtrl>();
        //TCamCameraP.
        MotionMgr = GameObject.FindGameObjectWithTag("Treal_MotionMgr").GetComponent<CMotionTrackingManager>();

        //
       // MotionMgr.SceneChange();

        TCamCameraP.engine_init();
    }

    IEnumerator StartTime()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneName);
    }

    private void Start()
    {
        //StartCoroutine(StartTime());
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
