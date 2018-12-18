using System;
using Treal.BrowserCore;
using QRCode;
using UnityEngine;
using Treal.Browser;

public class TCamManager : MonoBehaviour
{
    private static TCamManager instance;
    public static TCamManager Instance
    {
        get
        {
            return instance;
        }
    }

    public CMain CMain;
    public FakeAR FakeAR;

    private bool hasInit = false;
    public bool IsFront
    {
        get
        {
            return CustomTCamScript.Instance.Front;
        }
    }
    
    private bool isPreviewOn = false;
    public bool IsPreviewOn
    {
        get
        {
            return isPreviewOn;
        }
    }

    private bool imageInitFlag = false;


    private void Awake()
    {
        Debug.Log("T Cam Manager: Awake");

        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log("T Cam Manager OnApplicationPause: " + pause);

        if (pause)
        {
            Debug.Log("T Cam Manager: StopPreview");
            CustomTCamScript.Instance.StopPreview();

            if (CSpaceAREngine.isIMAGETRACKER_WORKING)
            {
                Debug.Log("T Cam Manager: StopImageTracking");
                CTrackingManager.Instance.ImageTrackerPause();
            }
        }
        else
        {
            if (isPreviewOn)
            {
                Debug.Log("T Cam Manager: StartPreview");
                CustomTCamScript.Instance.StartPreview();
            }

            if (CSpaceAREngine.isIMAGETRACKER_WORKING)
            {
                Debug.Log("T Cam Manager: StartImageTracking");
                CTrackingManager.Instance.ImageTrackerResume();
            }
        }
    }


    public void StartPreview()
    {
        if (!isPreviewOn)
        {
            Debug.Log("T Cam Manager: StartPreview");
            CustomTCamScript.Instance.StartPreview();
            isPreviewOn = true;
        }
    }

    public void StartPreview(bool front)
    {
        if (!isPreviewOn)
        {
            Debug.Log("T Cam Manager: StartPreview");
            CustomTCamScript.Instance.StartPreview(front);
            isPreviewOn = true;
        }
    }

    public void StopPreview()
    {
        if (isPreviewOn)
        {
            Debug.Log("T Cam Manager: StopPreview");
            CustomTCamScript.Instance.StopPreview();
            isPreviewOn = false;
        }
    }

    public void ChangeCamera()
    {
        Debug.Log("T Cam Manager: ChangeCamera");
        CMain.ChangeCamera();
    }


    public void StartImageTracking()
    {
        if (!CSpaceAREngine.isIMAGETRACKER_WORKING)
        {
            Debug.Log("T Cam Manager: StartImageTracking");

            if (!imageInitFlag)
            {
                ImageTracker.Instance.Start();
                CTrackingManager.Instance.Working = true;
                imageInitFlag = true;
            }
            else
            {
                CTrackingManager.Instance.ImageTrackerResume();
            }

            CSpaceAREngine.isIMAGETRACKER_WORKING = true;
        }
    }

    public void StopImageTracking()
    {
        if (CSpaceAREngine.isIMAGETRACKER_WORKING)
        {
            Debug.Log("T Cam Manager: StopImageTracking");
            CTrackingManager.Instance.ImageTrackerPause();
            CSpaceAREngine.isIMAGETRACKER_WORKING = false;
        }
    }



    public void StartFakeAR(GameObject obj, bool instantiate, Vector3 pos, Vector3 rot, Vector3 scale)
    {
        StartPreview();
        FakeAR.Play(obj, instantiate, pos, rot, scale);
    }

    public void StopFakeAR()
    {
        StopPreview();
        FakeAR.Stop();
    }
}
