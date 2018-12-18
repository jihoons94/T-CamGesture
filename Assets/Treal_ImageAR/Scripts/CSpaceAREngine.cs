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


using UnityEngine;
using System;
using Treal.BrowserCore;
using QRCode;
public enum ProcessMode
{
    None = 0,
    IMAGETRACKER = 1 << 0,
    QRCODE = 1 << 1,
    FACEDETECTOR = 1 << 2,
    MOTIONTRACKER = 1 << 3
};

public class CSpaceAREngine
{
	/// <summary>
	/// Frame Data width and height.
	/// </summary>
	int _frameWidth = 0;
	int _frameHeight = 0;

	/// <summary>
	/// ProcessMode
	/// </summary>
	private ProcessMode _processMode = 0;
	private IMAGE_FLIP _flip = IMAGE_FLIP.NO_FLIP;

	public TrealConfig mTrealConfig;

    public static bool isQRCODE_WORKING = false;
    public static bool isIMAGETRACKER_WORKING = false;
    public static bool isDETECTOR_WORKING = false;
    public static bool isMOTIONTRACKER_WORKING = false;

    /**
    * @brief TCam Event를 Android(JAVA)단으로 전달한다.
    *
    * @param width: VisionEngine에 제공될 frame data의 width
    * @param height: VisionEngine에 제공될 frame data의 height
    * @param mode: VisionEngine에서 사용 할 Engine 종류. ProcessMode 참고
    */
    public CSpaceAREngine(int width, int height, ProcessMode mode ) 
	{
        Debug.Log ("UNITY: CSpaceAREngine() width : height = " + width + ":" + height );
		_frameWidth = width;
		_frameHeight = height;
		_processMode = mode;
		SetSpaceAREngine ( );
	}

	/// <summary>
	/// ProcessMode에서 설정한 엔진이 있다면, 해당 엔진을 사용할 준비를 한다.
	/// </summary>
	private void SetSpaceAREngine( )
	{
        Debug.Log ("UNITY: CSpaceAREngine: SetSpaceAREngineMode() : mode = " + _processMode );


		if ( ( _processMode & ProcessMode.QRCODE ) != 0 && ( QRCodeManager.Instance != null ) ) {
			if (!isQRCODE_WORKING) {
				SetQRCode ();
			}
		} 

		if ( ( _processMode & ProcessMode.IMAGETRACKER ) != 0 && ( CTrackingManager.Instance != null ) ) {
			if (!isIMAGETRACKER_WORKING) {
				SetImageTracker ();
			}
		}

		if ( ( _processMode & ProcessMode.FACEDETECTOR ) != 0 && ( CFaceDetectingManager.Instance != null ) )
		{
			if (!isDETECTOR_WORKING) {
				SetDetector ();
			}
		}

        if ((_processMode & ProcessMode.MOTIONTRACKER) != 0 && (CMotionTrackingManager.Instance != null))
        {
            if (!isMOTIONTRACKER_WORKING)
            {
                SetMotionDetector();
            }
        }
    }

	/// <summary>
	/// QRCode 엔진을 시작한다.
    /// Windows/OSX/Android/iOS에서 동작
	/// </summary>
	private void SetQRCode() 
	{
        Debug.Log ("UNITY: CSpaceAREngine: SetQRCode() ");
		isQRCODE_WORKING = true;
		QRCodeManager.Instance.StartQRCode( _frameWidth, _frameHeight);
	}

	/// <summary>
	/// 2D Image Tracker 엔진을 설정하고 시작한다. 
	/// Main config에 설정 된 TrackingEngine(TCam, NatCam, ARKit, ARCore )의 종류에 따라 동작한다.
	/// </summary>
	private void SetImageTracker() 
	{
        Debug.Log ("UNITY: CSpaceAREngine: SetImageTracker() ");

		isIMAGETRACKER_WORKING = true;

		ArTrackingEngine trackerType = GameObject.Find ("Main").GetComponent<CMain> ().mTrealConfig.arTracking;

		ImageTracker.Instance.StartImageTracker(trackerType);

        if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            ImageTracker.Instance.SetCameraParam(_frameWidth, _frameHeight, IMAGE_FORMAT.RGBA, IMAGE_FLIP.HORIZONTAL_FLIP, IMAGE_ORIENTATION.PORTRAIT);
        }
        else
        {
            ImageTracker.Instance.SetCameraParam(_frameWidth, _frameHeight, IMAGE_FORMAT.GRAYSCALE, IMAGE_FLIP.NO_FLIP, IMAGE_ORIENTATION.LANDSCAPE);
        }

        if (ImageTracker.Instance.isImageTrackerWorking == false)
        {
            ImageTracker.Instance.Start();
        }
        else
        {
            ImageTracker.Instance.Resume();
        }

        Debug.Log("UNITY: CSpaceAREngine: SetImageTracker() End");
    }

	/// <summary>
	/// Face Detector 엔진을 설정하고 시작한다.
	/// BoxParam의 영역에서 얼굴 검출을 수행한다.
    /// Android/iOS에서 동작
	/// </summary>
	private void SetDetector()
	{
        Debug.Log ("UNITY: CSpaceAREngine: SetDetector() ");

		isDETECTOR_WORKING = true;

		if (FaceDetector.Instance.isFaceDetectorWorking == false) {
			CFaceDetectingManager.Instance.SetCameraParam (_frameWidth, _frameHeight, IMAGE_FORMAT.GRAYSCALE, _flip, IMAGE_ORIENTATION.LANDSCAPE);
			CFaceDetectingManager.Instance.StartFaceDetector ();
		} else {
			CFaceDetectingManager.Instance.SetCameraParam (_frameWidth, _frameHeight, IMAGE_FORMAT.GRAYSCALE, _flip, IMAGE_ORIENTATION.LANDSCAPE);
			CFaceDetectingManager.Instance.DetectorResume();
		}
	}

    private void SetMotionDetector()
    {
        Debug.Log("UNITY: CSpaceAREngine: SetMotionTracker() ");

        isMOTIONTRACKER_WORKING = true;
        Debug.Log("MotionTracker.Instance.isMotionDetectorWorking: " + MotionTracker.Instance.isMotionDetectorWorking);
        if (MotionTracker.Instance.isMotionDetectorWorking == false)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            CMotionTrackingManager.Instance.SetCameraParam(_frameWidth, _frameHeight, IMAGE_FORMAT.RGBA, IMAGE_FLIP.NO_FLIP);
#else
            CMotionTrackingManager.Instance.SetCameraParam(_frameWidth, _frameHeight, IMAGE_FORMAT.GRAYSCALE, IMAGE_FLIP.BOTH_FLIP);
#endif
            CMotionTrackingManager.Instance.StartMotionDetector();
        }
        else
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            CMotionTrackingManager.Instance.SetCameraParam(_frameWidth, _frameHeight, IMAGE_FORMAT.RGBA, IMAGE_FLIP.NO_FLIP);
#else
            CMotionTrackingManager.Instance.SetCameraParam(_frameWidth, _frameHeight, IMAGE_FORMAT.GRAYSCALE, IMAGE_FLIP.BOTH_FLIP);
#endif
            CMotionTrackingManager.Instance.StartMotionDetector();
        }
    }

    /**
    * @brief 엔진에 FrameData(Y or YUV)를 넘긴다.
    * @param frameData: IntPtr 타입
    */
    public void process( IntPtr frameData )
	{
		if ( isQRCODE_WORKING )
		{
			QRCodeManager.Instance.QRCodeProcess (frameData);
		} 

		if ( isIMAGETRACKER_WORKING )
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                    ImageTracker.Instance.SetCameraFrameNative(frameData, _frameWidth * _frameHeight * 4);
            }
            else
            {
                    ImageTracker.Instance.SetCameraFrameNative(frameData, _frameWidth * _frameHeight);
            }
		}

        if (isDETECTOR_WORKING)
        {
			CFaceDetectingManager.Instance.SetCameraFrameNative(frameData, _frameWidth * _frameHeight);
        }

        if (isMOTIONTRACKER_WORKING)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            CMotionTrackingManager.Instance.SetCameraFrameNative(frameData, _frameWidth * _frameHeight * 4);
#else
            CMotionTrackingManager.Instance.SetCameraFrameNative(frameData, _frameWidth * _frameHeight);
#endif
        }

    }

    /**
    * @brief 엔진에 FrameData(Y or YUV)를 넘긴다.
    * @param frameData: Unsigned IntPtr 타입
    */
    public void process(UIntPtr frameData)
    {
        if (isIMAGETRACKER_WORKING)
        {
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                ImageTracker.Instance.SetCameraFrameNative(frameData, _frameWidth * _frameHeight * 4);
            }
            else
            {
                ImageTracker.Instance.SetCameraFrameNative(frameData, _frameWidth * _frameHeight);
            }
        }
    }


	private void ResumeSpaceAREngine( )
	{
        Debug.Log ("UNITY: CSpaceAREngine: ResumeSpaceAREngine() ");

		if ( ( _processMode & ProcessMode.QRCODE ) != 0 && ( QRCodeManager.Instance != null ) ) {
			if (!isQRCODE_WORKING) {
				SetQRCode ();
			}
		} 

		if ( ( _processMode & ProcessMode.IMAGETRACKER ) != 0 && ( CTrackingManager.Instance != null ) ) {
			if (!isIMAGETRACKER_WORKING) {
				SetImageTracker ();
			}
		}

		if ( ( _processMode & ProcessMode.FACEDETECTOR ) != 0 && ( FaceDetector.Instance != null ) )
		{
			if (!isDETECTOR_WORKING) {
				SetDetector ();
			}
		}

        if ((_processMode & ProcessMode.MOTIONTRACKER) != 0 && (CMotionTrackingManager.Instance != null))
        {
            if (!isMOTIONTRACKER_WORKING)
            {
                SetMotionDetector();
            }
        }
    }


    /**
    * @brief 화면 전환 시 엔진을 초기화 한.
    */
	public void orientationChanged()
	{
        Debug.Log ("UNITY: CSpaceAREngine: orientationChanged() ");

		pause ();
        if (_flip == IMAGE_FLIP.NO_FLIP)
            _flip = IMAGE_FLIP.VERTICAL_FLIP;
        else
            _flip = IMAGE_FLIP.NO_FLIP;

        Debug.Log(_flip);
		
		resume ();
	}

    /**
    * @brief 엔진을 resume 한다.
    */
	public void resume()
	{
        Debug.Log ("UNITY: CSpaceAREngine: resume() ");

		ResumeSpaceAREngine();
	}

    /**
    * @brief 엔진을 pause 한다.
    */
	public void pause()
	{
        Debug.Log ("UNITY: CSpaceAREngine: pause() ");

		if (isQRCODE_WORKING) {
			QRCodeManager.Instance.pause ();
		}

		if (isIMAGETRACKER_WORKING) {
			CTrackingManager.Instance.ImageTrackerPause ();
		}

		if (isDETECTOR_WORKING) {
			CFaceDetectingManager.Instance.DetectorPause ();
		}
        if (isMOTIONTRACKER_WORKING)
        {
            CMotionTrackingManager.Instance.DetectorPause();
        }
        isQRCODE_WORKING = false;
		isIMAGETRACKER_WORKING = false;
        isDETECTOR_WORKING = false;
        isMOTIONTRACKER_WORKING = false;
    }
}
