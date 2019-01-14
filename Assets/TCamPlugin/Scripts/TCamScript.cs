using System;
using TCamera;
using UnityEngine;
using UnityEngine.UI;

public class TCamScript : MonoBehaviour
{
    public static TCamScript Instance;

    public int rearWidth, rearHeight, rearFps;
    public int frontWidth, frontHeight, frontFps;
    public int captureWidth, captureHeight;

    TCam tcam;
    bool front = false;
    private int _framewidth, _frameheight, fps;

    // jiyoun_choi@sk.com 181014: added for exposure control
    public Slider mExposureSlider;
    // _END jiyoun_choi@sk.com 181014: added for exposure control

    void FindTCam()
    {
        Transform temp = GameObject.FindWithTag("TCam").transform;
        if(temp != null)
        mExposureSlider = temp.GetChild(2).GetChild(2).GetComponent<Slider>();
    }

    void Awake()
    {
        Debug.Log("UNITY: TCamScript: Awake()");
        TCamScript.Instance = this;
        FindTCam();
    }


    void Start()
    {
        Debug.Log("UNITY: TCamScript: Start()");
        mExposureSlider.gameObject.SetActive(false);

        tcam = TCam.Instance;
        tcam.Init(TCam.NativeCameraHardware.AUTO, TCam.RenderMethod.NATIVE_GL_SHADER_POST_RENDER);
        StartPreview();
    }

    public void ChangeCamera()
    {
        Debug.Log("UNITY: TCamScript: ChangeCamera()");

        front = !front;

        tcam.mExposureToggle.isOn = false;

        StopPreview();
        StartPreview();
    }


    // jiyoun_choi@sk.com 181014: added for exposure control
    // init Exposure Control Value
    public void setExposureValue(Boolean flag)
    {

        if (flag == true)
        {
            mExposureSlider.gameObject.SetActive(true);

            float max = tcam.GetMaxExposure();
            float min = tcam.GetMinExposure();

            tcam.SetExposure((max + min) / 2);
            tcam.SetExposureMode(TCamParameters.ExposureMode.ON);
            tcam.SetFlashMode(TCamera.TCamParameters.FlashMode.OFF);

            mExposureSlider.maxValue = max;
            mExposureSlider.minValue = min;

            mExposureSlider.value = (mExposureSlider.maxValue + mExposureSlider.minValue) / 2.0f;
        }
        else
        {
            mExposureSlider.value = 0.0f;
            onExposureValueChanged();

            mExposureSlider.gameObject.SetActive(false);
        }
    }

    // Setting Exposure Value
    public void onExposureValueChanged()
    {
        tcam.SetExposureMode(TCamParameters.ExposureMode.CUSTOM);
        TCamPlugin.SetExposure(mExposureSlider.value);
        Debug.Log("UNITY: TCamScript: Setted Exposure value = " + mExposureSlider.value);
    }
    // _END jiyoun_choi@sk.com 181014: added for exposure control

    void StartPreview()
    {
        Debug.Log("UNITY: TCamScript: StartPreview()");

        _framewidth = front ? frontWidth : rearWidth;
        _frameheight = front ? frontHeight : rearHeight;
        fps = front ? frontFps : rearFps;

        tcam.SetFocusMode(TCamera.TCamParameters.FocusMode.INFINITY);
        tcam.SetFlashMode(TCamera.TCamParameters.FlashMode.OFF);
        tcam.SetPreview(front, _framewidth, _frameheight, fps);

        tcam.SetCaptureResolution(captureWidth, captureHeight);
        tcam.GetCaptureResolution(out captureWidth, out captureHeight);

        Debug.Log("UNITY: TCamScript: StartPreview(): =================== Parameters =================");
        Debug.Log("UNITY: TCamScript: StartPreview(): Is Front Camera = " + front);
        Debug.Log("UNITY: TCamScript: StartPreview(): Preview Size " + _framewidth + " " + _frameheight);
        Debug.Log("UNITY: TCamScript: StartPreview(): Capture Size " + captureWidth + " " + captureHeight);
        Debug.Log("UNITY: TCamScript: StartPreview(): =================== Parameters =================");

        tcam.Play();
    }

    void StopPreview()
    {
        Debug.Log("UNITY: TCamScript: StopPreview()");
        tcam.Stop();
    }

    void DisposePreview()
    {
        Debug.Log("UNITY: TCamScript: DisposePreview()");
        StopPreview();
        tcam.Dispose();
    }

    // Getter
    public int GetFrameWidth()
    {
        return _framewidth;
    }

    public int GetFrameHeight()
    {
        return _frameheight;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("UNITY: CTCamCameraPreviewCtrl: OnApplicationPause() flag = " + pauseStatus);

        try
        {
            if (tcam == null)
            {
                return;
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("UNITY: CTCamCameraPreviewCtrl: OnApplicationPause(): Null _enigne.");
        }

        if (pauseStatus)
        {
            StopPreview();
        }
        else
        {
            tcam.Play();
            StartPreview();
        }
    }

}
