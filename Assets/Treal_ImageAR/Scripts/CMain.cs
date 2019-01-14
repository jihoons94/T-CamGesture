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

namespace Treal.BrowserCore
{
    public delegate void OnCapture(Texture2D capture);

    public class CMain : MonoBehaviour
    {
        public TrealConfig mTrealConfig;
        public Transform captureScreen;

        private CTCamCameraPreviewCtrl tcamManager;

        public event OnCapture onCapture;

        int m_numberOfRecoveryAttempts = 0;

        private void Awake()
        {
            Debug.Log("UNITY: CMain: Awake()");

            GameObject mTrealPreviewObj = new GameObject();
            mTrealPreviewObj.name = "Treal_CameraPreviewManager";

            switch (mTrealConfig.arTracking)
            {
                case ArTrackingEngine.TCAM:
                    Debug.Log("UNITY: CMain: addComponent<CTCamCameraPreviewCtrl>");
                    tcamManager = mTrealPreviewObj.AddComponent<CTCamCameraPreviewCtrl>();
                    tcamManager.setCaptureScreen(captureScreen);
                    break;
                case ArTrackingEngine.ARKIT:
                    Debug.Log("UNITY: CMain: addComponent<CARKitCameraPreviewCtrl>");
                    break;

                case ArTrackingEngine.ARCORE:
                    Debug.Log("CMain: new CARCoreCameraPreviewCtrl()");
                    break;
            }

            if (mTrealConfig.debugEnable)
            {
                GameObject debugManagerObj = Instantiate(Resources.Load("Prefabs/DebugManager")) as GameObject;
                CDebugManager cDebugManager = debugManagerObj.GetComponent<CDebugManager>();
            }
        }

        public void Capture()
        {
            Debug.Log("UNITY: CMain: Capture()");
            tcamManager.Capture(onCapture);
        }

        public void ChangeCamera()
        {
            Debug.Log("UNITY: CMain: ChangeCamera()");

            tcamManager.ChangeCamera();
        }
    }
}
