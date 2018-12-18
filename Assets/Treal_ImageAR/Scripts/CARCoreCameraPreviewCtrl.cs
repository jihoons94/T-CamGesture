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
using UnityEngine;
#if USE_ARCORE
using GoogleARCore;
using GoogleARCore.Examples.ComputerVision;
#endif

namespace Treal.BrowserCore
{

    /// <summary>
    /// ARCore camera preview ctrl.
    /// </summary>
    public class CARCoreCameraPreviewCtrl : CCameraPreviewCtrl
    {
#if USE_ARCORE

        // CARCoreCameraPreviewCtrl Instance
        public static CARCoreCameraPreviewCtrl Instance = null;

        // Flag: Using Hybrid Tracker
        public bool enableHybridTracker = false;

        // Flag: Quit ARCore Camera Preview Ctrl.
        private bool m_IsQuitting = false;

        // CSpaceAREngine
        CSpaceAREngine _engine = null;

        // Unity Lifecycle Start
        public override void Start()
        {
            Debug.Log("UNITY: CARCoreCameraPreviewCtrl: Start ");

            CARCoreCameraPreviewCtrl.Instance = this;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

			if (_engine != null ) {
				_engine.resume ();
			}

		}

        // Unity Lifecycle Update
        public void Update()
        {
            // 종료 처리. Android/IOS는 무시
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            _QuitOnConnectionErrors();

            if (!Session.Status.IsValid())
            {
                Debug.Log("CARCoreCameraPreviewCtrl: Session not vaild");
                return;
            }

            // ARCore의 Framedata를 가져 올 수 있다면, _OnImageAvailable를 호출한다.
            using (var image = Frame.CameraImage.AcquireCameraImageBytes())
            {
                if (!image.IsAvailable)
                {
                    Debug.Log("CARCoreCameraPreviewCtrl: Image is not vaild");
                    return;
                }

				// ARCore의 Frame Intrinsic data를 가져온다.
				var cameraIntrinsics = Frame.CameraImage.ImageIntrinsics;
				Debug.Log(String.Format("CARCoreCameraPreviewCtrl: {0} {1}", cameraIntrinsics.PrincipalPoint.ToString(), cameraIntrinsics.ImageDimensions.ToString()));

				// AREngine이 없다면 ARCore Frame의 width, height에 맞게 Engine을 생성한.
				// 이 때 사용 할 엔진은 ImageTracker, QRCode이다.
				// 만약 엔진이 있다면, resume 한다.
				if (_engine == null)
				{
					_engine = new CSpaceAREngine(cameraIntrinsics.ImageDimensions.x, cameraIntrinsics.ImageDimensions.y, ProcessMode.IMAGETRACKER | ProcessMode.QRCODE);
				}

				if (_engine != null ) {
					_OnImageAvailable (TextureReaderApi.ImageFormatType.ImageFormatGrayscale, image.Width, image.Height, image.Y, 0);

					// Hybrid tracker가 enable되었다면 ImageTracker의 camera pose에 ARCore의 camera pose를 반영하여 최종적인 ImageTracker camera pose를 계산한다.
					if( enableHybridTracker )
					{
						Pose pose = Frame.Pose;
						CTrackingManager.Instance.SetSLAMCameraPose( Matrix4x4.TRS(pose.position, pose.rotation, Vector3.one) );
					}

				}
            }
        }

        // ARCore에서 가져 온 frame을 AR Engine으로 전달한다.
		private void _OnImageAvailable(TextureReaderApi.ImageFormatType format, int width, int height, IntPtr pixelBuffer, int bufferSize)
		{
			if (format != TextureReaderApi.ImageFormatType.ImageFormatGrayscale)
			{
				Debug.Log("CARCoreCameraPreviewCtrl: No edge detected due to incorrect image format.");
				return;
			}
			// Debug.Log ("CARCoreCameraPreviewCtrl: _OnImageAvailable");
			if (_engine != null) {
				_engine.process (pixelBuffer);
			}
		}
			

        // 종료 처리
		private void _QuitOnConnectionErrors()
		{
			if (m_IsQuitting)
			{
				return;
			}

			if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
			{
				Debug.Log("CARCoreCameraPreviewCtrl: Camera permission is needed to run this application.");
				m_IsQuitting = true;
			}
			else if (Session.Status == SessionStatus.FatalError)
			{
				Debug.Log("CARCoreCameraPreviewCtrl: ARCore encountered a problem connecting.");
				m_IsQuitting = true;
			}

		}

        // Unity Lifecycle Pause
		void OnApplicationPause()
		{
            // AR Engine을 pause 시킨다.
            if (_engine != null)
            {
				CTrackingManager.Instance.SetSLAMCameraPose( Matrix4x4.identity );
                _engine.pause();
            }
		}

        // Unity Lifecycle Destroy
		void OnDestroy()
		{
            // AR Engine을 pause 시킨다. (이후 destroy된다 )
			if (_engine != null) {
				_engine.pause ();
			}
		}


        // 종료 처리
        private void _DoQuit()
		{
			Application.Quit();
		}
#else
		public override void Start()
		{

		}
#endif
	}
}

