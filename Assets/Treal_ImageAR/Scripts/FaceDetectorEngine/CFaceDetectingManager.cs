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
using System.IO;
using Debug = UnityEngine.Debug;
using System;

namespace Treal.BrowserCore
{
	public class CFaceDetectingManager : MonoBehaviour
    {
		public static CFaceDetectingManager Instance = null;
        private FaceDetector _detector;

        public string _classifierFilePath;
		private float minThr = 0.43f;
        private float maxThr = 0.63f;

        private float wRatio, hRatio;
		private bool _working = false;
		private bool _isDetected = false;
        public RectTransform FaceGuide;

        private int previewWidth, previewHeight; // preview resolution
		private int boxWidth, boxHeight;
        private int px = 260;
        private int py = 480; // bottom index of chin in the guide region
        private string dirPath;

        public bool Detecting
        {
            get { return _isDetected; }
        }

        public bool Working
        {
            get { return _working; }
            set
            {
                _working = value;
            }
        }
			
        private void Awake()
        {
            Debug.Log ("UNITY: CFaceDetectingManager: Awake() ");
            CFaceDetectingManager.Instance = this;

            // 이미지 트래커 참조 
            _detector = FaceDetector.Instance;
        }
        void Start()
        {
            Debug.Log ("UNITY: CFaceDetectingManager: Start() ");

            dirPath = FindClassifierPath();
            string dirString = dirPath + "/" + _classifierFilePath;
            FileInfo fi = new FileInfo(dirString);

            if (fi.Exists)
            {
                _detector.Load(dirString);
            }
            else
            {
                Debug.Log ("UNITY ERROR: CFaceDetectingManager: Start() File is not found : " + _classifierFilePath);
            }
        }

		/// <summary>
		/// FaceDetector 엔진을 실행시킨다.
		/// </summary>
		public void StartFaceDetector()
        {
            Debug.Log ("UNITY: CFaceDetectingManager: StartFaceDetector() ");
            _working = true;
            _detector.Start();
        }

		void Update()
        {
            if (_working)
            {
                _isDetected = _detector.IsDetected(minThr, maxThr);
				if (_isDetected) {
                    FaceGuide.gameObject.SetActive (false);
				} else
                    FaceGuide.gameObject.SetActive (true);
            }
        }


        public void DetectorPause()
        {
            Debug.Log ("UNITY: CFaceDetectingManager: DetectorPause()");
            if (_detector != null && _working == true) {
                _working = false;
                _detector.Pause();
            } else {
                Debug.Log ("UNITY ERROR: CFaceDetectingManager: DetectorPause() Detector is NULL");
            }
        }

        public void DetectorResume()
        {
            Debug.Log ("UNITY: CFaceDetectingManager: DetectorResume()");

			if (_detector != null) {
                _detector.Resume();
                _working = true;
            } else {
                Debug.Log ("UNITY ERROR: CFaceDetectingManager: DetectorResume() Detector is NULL");
            }
        }

        private void DetectorStop()
        {
            Debug.Log ("UNITY: CFaceDetectingManager: DetectorStop()");
			if (_detector != null) {
				_working = false;
				_detector.Stop ();
			} else {
                Debug.Log ("UNITY ERROR: CFaceDetectingManager: DetectorStop() Detector is NULL");
			}
        }

        /// <summary>
        /// FaceDetector에 Camera Preview Data(IntPtr)를 전달한다.
        /// </summary>
        /// <returns>The camera frame native.</returns>
        /// <param name="webCamPixels">Web cam pixels.</param>
        /// <param name="size">Size.</param>
        public int SetCameraFrameNative(IntPtr webCamPixels, int size)
        {
            int result;
            result = FaceDetector.Instance.SetCameraFrameNative(webCamPixels, size);
            return result;
        }        

        /// <summary>
        /// Finds the classifier path.
        /// </summary>
        /// <returns>The classifier path.</returns>
        private string FindClassifierPath()
        {
            Debug.Log ("UNITY: CFaceDetectingManager: FindClassifierPath()");
            string dirString = null;

            if (Application.platform == RuntimePlatform.Android)
            {
                string externalPath = Application.persistentDataPath + "/ClassifierFile";
                // Android
                string classifierPath = Path.Combine(Application.streamingAssetsPath, "ClassifierFile/" + _classifierFilePath);
                Debug.Log("classifierPath => " + classifierPath);

                UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(classifierPath);
                www.Send();
                while (!www.isDone) { }
                byte[] result = www.downloadHandler.data;

                if (www.error != null)
                {
                    throw new System.Exception("ClassifierFile WWW Download: " + www.error);
                }

                Directory.CreateDirectory(externalPath);
                File.WriteAllBytes(externalPath + "/" + _classifierFilePath, result);
                dirString = externalPath;
            }
            else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                dirString = Application.streamingAssetsPath + "/ClassifierFile";
            }
            else
            {
                dirString = Application.dataPath + "/StreamingAssets/ClassifierFile";
            }

            return dirString;
        }

        public void CapturePreviewFrame()
        {
            string name = "/capture_" + Time.time.ToString();
            FaceDetector.Instance.SavePreview(dirPath + name, minThr, maxThr);
        }

        public int SetCameraParam(int width, int height, IMAGE_FORMAT format, IMAGE_FLIP flip, IMAGE_ORIENTATION orientation)
        {
            Debug.Log ("UNITY: CFaceDetectingManager: SetCameraParam()");
            //Debug.Log ("width : " + width + ", height : " + height );	
            //Debug.Log ("chinPoint_X : " + px + ", chinPoint_Y : " + py );	

            if (Screen.width > Screen.height)
            {
                previewWidth = Screen.width;
                previewHeight = Screen.height;
            }
            else 
            {
                previewWidth = Screen.height;
                previewHeight = Screen.width;
            }

            // k-mobile 방식대로 localScale계산, k-mobile app 적용 시 주석처리 하기
            /*=========================================================*/
            var r = GetTextureRatio(previewWidth, previewHeight);
            Vector3 rr = new Vector3(r, r, 1);
            FaceGuide.localScale = rr;
            /*=========================================================*/

            Vector3 ratio = FaceGuide.localScale;
            boxWidth = (int)(FaceGuide.rect.width * ratio.x);
            boxHeight = (int)(FaceGuide.rect.height * ratio.y);

            float w_prevRatio = width / (float)previewWidth;
            float h_prevRatio = height / (float)previewHeight;

            px = (int)(boxWidth * (520 / 1024.0f));
            py = (int)(boxHeight * (960 / 1024.0f));

            int reBoxWidth = (int)(boxWidth * w_prevRatio);
            int reBoxHeight = (int)(boxHeight * h_prevRatio);

            boxWidth = reBoxWidth > width ? previewWidth : boxWidth;
            boxHeight = reBoxHeight > height ? previewHeight : boxHeight;

            int rePx = (int)(px * w_prevRatio);
            int rePy = (int)(py * h_prevRatio);

            px = rePx > boxWidth ? (int)(boxWidth * (520 / 1024.0f)) : px;
            py = rePy > boxHeight ? (int)(boxHeight * (960 / 1024.0f)) : py;

            if (FaceDetector.Instance == null)
            {
                while(true)
                {
                    if (FaceDetector.Instance != null)
                    {
                        break;
                    }
                }

                Debug.Log("FaceDetector.Instance Null");
            }

            FaceDetector.Instance.SetCameraParam(width, height, format, flip, orientation);
            FaceDetector.Instance.SetBoxParam(
                (int)((previewWidth / 2 - boxWidth / 2) * w_prevRatio),
                (int)((previewHeight / 2 - boxHeight / 2) * h_prevRatio),
                (int)(boxWidth * w_prevRatio),
                (int)(boxHeight * h_prevRatio));

			return FaceDetector.Instance.SetDetectParam((int)(px * w_prevRatio), (int)(py * h_prevRatio), 30);


        }

        public float GetTextureRatio(float previewWidth, float previewHeight)
        {
			Debug.Log ("UNITY: CFaceDetectingManager: GetTextureRatio()");
            var textureSize = 1024;
            var canvasWidth = 1920f;
            return (previewHeight / (textureSize * (previewWidth / canvasWidth)));            
        }



    }

}
