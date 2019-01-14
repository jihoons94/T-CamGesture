/** 
*
* Copyright 2016-2018 SK Telecom. All Rights Reserved.
*
* This file is part of T real Platform.
*
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
*/

using System;
using System.Collections;
using TCamera;
using Treal.BrowserCore;
using UnityEngine;
using System.Runtime.InteropServices;

public class CTCamCameraPreviewCtrl : CCameraPreviewCtrl
{
	// Capture _START
    private bool display_capture_screen = false;
    private bool isCaptureUpdated = false;

	private Texture2D captureTex;
	public Transform captureScreen;

	byte[] captureData = null;
	// Capture _END

    TCam tcam;
    CSpaceAREngine _engine = null;

    private OnCapture onCapture;

    public override void Start()
    {
        Debug.Log ("UNITY: CTCamCameraPreviewCtrl: Start()");

		// Capture _START
		if (captureScreen) {
			captureScreen.gameObject.SetActive (false);
		}
		// Capture _END

        tcam = TCam.Instance;

        tcam.OnPreviewStart += OnTCamPreviewStart;
        tcam.OnPreviewUpdate += OnTCamPreviewUpdate;

		// Capture _START
		tcam.OnCaptureUpdate += OnTCamCaptureUpdate;
		// Capture _END

    }

    public void engine_init()
    {
        
        if(_engine == null)
        {
            Debug.Log("엔진을 찾을 수 없음");
            //_engine = new CSpaceAREngine(640, 360, ProcessMode.MOTIONTRACKER);
        }
        else
        _engine.orientationChanged();
    }

    public void engine_resume()
    {
        if (_engine == null)
        {
            Debug.Log("엔진을 찾을 수 없음");
            //_engine = new CSpaceAREngine(640, 360, ProcessMode.MOTIONTRACKER);
        }
        else
            _engine.resume();
    }



    void OnTCamPreviewStart(int width, int height, int fps)
    {
        Debug.Log ("UNITY: CTCamCameraPreviewCtrl: OnTCamPreviewStart()");

		_engine = new CSpaceAREngine(width, height, ProcessMode.MOTIONTRACKER);
        //_engine = new CSpaceAREngine(width, height, ProcessMode.QRCODE | ProcessMode.IMAGETRACKER);

        // Capture _START
        if (captureScreen) {
			captureTex = new Texture2D (0, 0);
			Material m = captureScreen.GetComponent<Renderer> ().material;
			m.mainTexture = captureTex;
		}
		// Capture _END
		StartCoroutine("PostSetup");
    }

    IEnumerator PostSetup()
    {
        Debug.Log ("UNITY: CTCamCameraPreviewCtrl: PostSetup()");

        while (!tcam.IsPlaying())
        {
            yield return null;
        }
    }

	void OnTCamPreviewUpdate(int frameId, TCamera.TCamParameters.PreviewFormat previewFormat, IntPtr frameData, int width, int height)
	{
		if (_engine == null) {
			return;
		}

		 _engine.process ( frameData );

		// Capture _START
        if ( isCaptureUpdated ) {
			if (captureScreen) {
				captureTex.LoadImage (captureData);
                if (onCapture != null)
                {
                    onCapture.Invoke(captureTex);
                }

                isCaptureUpdated = false;
			}
		}
		// Capture _END
	}

	public void OnApplicationPause( bool pauseStatus )
	{
        Debug.Log ("UNITY: CTCamCameraPreviewCtrl: OnApplicationPause() flag = " + pauseStatus);

		try {
			if (_engine == null) {
				return;
			}
		}       
		catch (NullReferenceException ex) {
			Debug.Log("UNITY: CTCamCameraPreviewCtrl: OnApplicationPause(): Null _enigne.");
		}


		if (pauseStatus) {
			_engine.pause ();
		} else {
			_engine.resume ();
		}
	}

	public void OnDestroy()
	{
        Debug.Log ("UNITY: CTCamCameraPreviewCtrl: OnDestroy()");
		if (_engine == null) {
			return;
		}

		_engine.pause ();
	}


	public void ChangeCamera()
	{
        Debug.Log ("UNITY: CTCamCameraPreviewCtrl: ChangeCamera()");
		if (_engine == null) {
			return;
		}

		_engine.pause ();
		TCamScript.Instance.ChangeCamera ();
	}


	// Capture _START
	// TCam Controller
	public void setCaptureScreen( Transform screen )
	{
        Debug.Log ("UNITY: CTCamCameraPreviewCtrl: setCaptureScreen()");
		captureScreen = screen;
	}

	public void Capture(OnCapture onCapture)
	{
        Debug.Log ("UNITY: CTCamCameraPreviewCtrl: Capture()");

        this.onCapture = onCapture;

        if ( !display_capture_screen ) {
			tcam.Capture ();

			if (captureScreen) {
				captureScreen.gameObject.SetActive (true);
			}

            display_capture_screen = true;
		} else {
			if (captureScreen) {
				captureScreen.gameObject.SetActive (false);
			}

			Array.Clear(captureData, 0, captureData.Length);
			captureTex.LoadImage (captureData);


            display_capture_screen = false;
		}
	}

	void OnTCamCaptureUpdate(IntPtr data, int size, int width, int height, int rotation, bool hflip, bool vflip)
	{
		if (captureData == null || captureData.Length != size) {
            GC.Collect();
			captureData = new byte[size];
		}

        Marshal.Copy (data, captureData, 0, size);
        isCaptureUpdated = true;
	}
	// Capture _END

}
