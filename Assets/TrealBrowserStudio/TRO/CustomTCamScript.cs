using TCamera;
using UnityEngine;

public class CustomTCamScript : MonoBehaviour
{
    public static CustomTCamScript Instance;

    public int rearWidth = 1280, rearHeight = 720, rearFps = 30;
    public int frontWidth = 640, frontHeight = 480, frontFps = 30;
    public int captureWidth = 2560, captureHeight = 1440;

    TCam tcam;
    bool front = false;
    public bool Front
    {
        get { return front; }
    }

    private int _framewidth, _frameheight, fps;

    private bool hasInit = false;
    private bool hasStartedPreview = false;


    void Awake()
    {
        CustomTCamScript.Instance = this;
    }


    public void Init()
    {
        if (!hasInit)
        {
            tcam = TCam.Instance;
            tcam.Init(TCam.NativeCameraHardware.AUTO, TCam.RenderMethod.NATIVE_GL_SHADER_POST_RENDER);

            hasInit = true;
        }
    }

    public void ChangeCamera()
    {
        front = !front;

        StopPreview();
        StartPreview();
    }

    public void StartPreview()
    {
        Init();

        if (!hasStartedPreview)
        {
            _framewidth = front ? frontWidth : rearWidth;
            _frameheight = front ? frontHeight : rearHeight;
            fps = front ? frontFps : rearFps;

            tcam.SetFocusMode(TCamera.TCamParameters.FocusMode.INFINITY);
            //tcam.SetFlashMode(TCamera.TCamParameters.FlashMode.OFF);
            tcam.SetPreview(front, _framewidth, _frameheight, fps);
            tcam.SetCaptureResolution(captureWidth, captureHeight);

            tcam.Play();

            hasStartedPreview = true;
        }
    }

    public void StartPreview(bool front)
    {
        Init();

        if (!hasStartedPreview)
        {
            this.front = front;

            _framewidth = front ? frontWidth : rearWidth;
            _frameheight = front ? frontHeight : rearHeight;
            fps = front ? frontFps : rearFps;

            tcam.SetFocusMode(TCamera.TCamParameters.FocusMode.INFINITY);
            //tcam.SetFlashMode(TCamera.TCamParameters.FlashMode.OFF);
            tcam.SetPreview(front, _framewidth, _frameheight, fps);
            tcam.SetCaptureResolution(captureWidth, captureHeight);

            tcam.Play();

            hasStartedPreview = true;
        }
    }

    public void StopPreview()
    {
        if (hasStartedPreview)
        {
            tcam.Stop();
            hasStartedPreview = false;
        }
    }

    public void DisposePreview()
    {
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
}
