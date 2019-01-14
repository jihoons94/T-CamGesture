using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Treal.BrowserCore;

public class Motion_Event : MonoBehaviour {
    public CMotionTrackingManager MotionTrackingMgr;
    CTCamCameraPreviewCtrl TCamCameraCtrl;

    virtual public void FixedEvent_On(int _num)
    {

    }

    virtual public void FixedEvent_Off(int _num)
    {

    }


    protected void SceneChange()
    {
        TCamCameraCtrl = GameObject.Find("Treal_CameraPreviewManager").GetComponent<CTCamCameraPreviewCtrl>();
        MotionTrackingMgr = GameObject.FindWithTag("MotionTrackingMgr").GetComponent<CMotionTrackingManager>();
        MotionTrackingMgr.FindandSet("MotionEvent");
        MotionTrackingMgr.ReSetTrackRoIs();
        TCamCameraCtrl.engine_init();
        Debug.Log("SceneChange(): Done");
    }

    private void Start()
    {
        MotionTrackingMgr = GameObject.FindWithTag("MotionTrackingMgr").GetComponent<CMotionTrackingManager>();
    }
}
