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
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Treal.BrowserCore
{

	public class CTrackingManager : MonoBehaviour
	{
        private static CTrackingManager instance = null;

        private ImageTracker _imageTracker;
        private float[] _cameraPose;

        private ArTrackingEngine _trackerType;

		private List<string> _descriptorSetFilePaths;
		private List<ImageDescriptors> _descriptorSets;
		private Dictionary<string, CTargetImage> _targetImageMap;

		public List<string> _descriptorSet; // descriptorSet 파일 
		public List<string> _descriptorID;
		public List<CTargetImage> _targetImageList;
        private CTargetImage _targetImage;

        private string _prevUUID = "";
        private bool _working = false;      // Is ImageTrackerWorking?
        private bool _isTracked = false;    // Is Tracked Image?

		// Property
        // indicate whether TrackingManager request the camera pose from ImageTracker
		public bool Working
		{
			get { return _working; }
			set
			{
				_working = value;
				// 인식 엔진 off 후, 남아있는 3D 모델을 없애주기 위해, LostUUID() 호출
				if (_working == false)
					LostUUID();
			}
		}

		public static CTrackingManager Instance
		{
			get
			{
                Debug.Log ("UNITY: CTrackingManager: Instance ");

				// private instance 변수의 참조를 반환
				return instance;
			}
		}

		// Delegate
		public delegate void ImageTrackingHandler(float[] campose, int imgWidth, int imgHeight);
		public static event ImageTrackingHandler OnImageTracking;

		void Awake()
		{
            Debug.Log ("UNITY: CTrackingManager: Awake() ");
			// 씬에 이미 인스턴스가 존재하는지 검사
			// 존재하는 경우, 씬에서 새로 로딩될 인스턴스를 소멸시킨다.
			if (instance)
			{
				DestroyImmediate(gameObject);
				return;
			}

			// 이 인스턴스를 싱글턴 오브젝트로
			instance = this;

			// This object remains until application exist.
			DontDestroyOnLoad(gameObject);

			// 이미지 트래커 참조 
			_imageTracker = ImageTracker.Instance;
		}

		IEnumerator Start()
		{
            Debug.Log ("UNITY: CTrackingManager: Start() ");

			_trackerType = GameObject.Find("Main").GetComponent<CMain>().mTrealConfig.arTracking;

			// 기본 에러 체크 
			if (ErrorCheck()) yield break;

			// 디스크립터 경로 파악 
			string dirString = FindDescriptorPath();

			_descriptorSetFilePaths = new List<string>();
			_descriptorSets = new List<ImageDescriptors>();
			for (int i = 0; i < _descriptorSet.Count; i++)
			{
				ImageDescriptors imageDescriptorSet = new ImageDescriptors();
				_descriptorSetFilePaths.Add(dirString + "/" + _descriptorSet[i]);
				_descriptorSets.Add(imageDescriptorSet);
			}

			FileInfo fi;
			for (int i = 0; i < _descriptorSetFilePaths.Count; i++)
			{
				// 일단 파일이 존재하는지 확인 후, Image Tracker로 넘겨야함
				fi = new FileInfo(_descriptorSetFilePaths[i]);
				if (fi.Exists)
				{
					Debug.Log("Descriptor Load : " + _descriptorSetFilePaths[i]);
					_descriptorSets[i].LoadDescriptorSet(_descriptorSetFilePaths[i]);
				}
				else
				{
                    Debug.Log("UNITY ERROR: CTrackingManager: Start() Descriptor File not found : " + _descriptorSetFilePaths[i]);
				}
			}

			_targetImageMap = new Dictionary<string, CTargetImage>();
			for (int i = 0; i < _targetImageList.Count; i++)
			{
				Debug.Log(_descriptorID[i]);
				_targetImageMap.Add(_descriptorID[i], _targetImageList[i]);
			}

			// Descriptor 로드 
			StartCoroutine("CoLoadDescriptor");
		}

		void Update()
		{
			if (_working)
			{
				_cameraPose = _imageTracker.GetCameraPose();

				if (_imageTracker.IsTracking())
				{
                    Debug.Log("UNITY: CTrackingManager: Now it's Image Tracking.....");
					TrackUUID(_imageTracker.GetUuid(), _imageTracker.GetImageWidth(), _imageTracker.GetImageHeight(), _cameraPose);
				}
				else
				{
					if ( _trackerType == ArTrackingEngine.TCAM ) {
						LostUUID ();
					}
				}
			}
		}
		
		private string FindDescriptorPath()
		{
			string dirString = null;

			if (Application.platform == RuntimePlatform.Android)
			{
				string externalPath = Application.persistentDataPath + "/DescriptorSets";
				for (int i = 0; i < _descriptorSet.Count; i++)
				{
					// Android
					string descPath = Path.Combine(Application.streamingAssetsPath, "DescriptorSets/" + _descriptorSet[i]);
					Debug.Log("descPath => " + descPath);

					UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(descPath);
					www.Send();
					while (!www.isDone) { }
					byte[] result = www.downloadHandler.data;

					if (www.error != null)
					{
						throw new System.Exception("DescriptorSet WWW Download: " + www.error);
					}

					Directory.CreateDirectory(externalPath);
					File.WriteAllBytes(externalPath + "/" + _descriptorSet[i], result);
				}
				dirString = externalPath;
			}
			else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				dirString = Application.streamingAssetsPath + "/DescriptorSets";
			}
			else
			{
				dirString = Application.dataPath + "/StreamingAssets/DescriptorSets";
			}

			return dirString;
		}

		private void TrackUUID(string UUID, float imgWidth, float imgHeight, float[] cameraPose)
		{
			Matrix4x4 camPoseMatrixByTreal;

			if (_targetImageMap.ContainsKey(UUID))
			{
				_isTracked = true;

				camPoseMatrixByTreal = GetMatrix4x4(cameraPose);

				_targetImage = _targetImageMap[UUID];

				switch (_trackerType)
				{
                    case ArTrackingEngine.TCAM:
                        _targetImage.SetTransformByNativeCamera(camPoseMatrixByTreal);
                        break;
                    case ArTrackingEngine.ARKIT:
						Vector3 translation = new Vector3 (0, 0, 0);
						Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
						Vector3 scale = new Vector3 (1.0f, 1.0f, 1.0f);
						Matrix4x4 mat90 = Matrix4x4.TRS(translation, rotation, scale);

						_targetImage.SetTransformByARKit(mat90 * camPoseMatrixByTreal);
						break;

					case ArTrackingEngine.ARCORE:
						_targetImage.SetTransformByARCore(camPoseMatrixByTreal);
						break;
				}

				_targetImage.SetImageSize(imgWidth, imgHeight);
				_targetImage.SetModelVisible(true);

				if (_prevUUID != UUID && _prevUUID != "")
				{
					_targetImageMap[_prevUUID].SetModelVisible(false);
				}

				_prevUUID = UUID;
			}
			return;
		}

		private void LostUUID()
		{
			if (_isTracked)
			{
				//_targetImageMap[_prevUUID].IsTracked = false;
				_targetImageMap[_prevUUID].SetModelVisible(false);
				_isTracked = false;
			}
		}

		private bool ErrorCheck()
		{
			// DescriptorSet 로딩
			if (_descriptorSet == null || _descriptorSet.Count == 0)
			{
                Debug.Log("UNITY ERROR: CTrackingManager: ErrorCheck() Null Descriptor Set");
				return true;
			}
			if (_descriptorID == null || _descriptorID.Count == 0)
			{
                Debug.Log("UNITY ERROR: CTrackingManager: ErrorCheck() Null Descriptor ID");
				return true;
			}
			if (_targetImageList == null || _targetImageList.Count == 0)
			{
                Debug.Log("UNITY ERROR: CTrackingManager: ErrorCheck() Error : TargetImage");
				return true;
			}
			if (_descriptorID.Count != _targetImageList.Count)
			{
                Debug.Log("UNITY ERROR: CTrackingManager: ErrorCheck() Error : The number of Descriptor ID is not equal the number of TargetImage.");
				return true;
			}

			return false;
		}

        public void LoadDescriptor(string path)
        {
            _descriptorSetFilePaths = new List<string>();
            _descriptorSets = new List<ImageDescriptors>();
            _descriptorSet = new List<string>();
            _descriptorSet.Add("one");

            for (int i = 0; i < _descriptorSet.Count; i++)
            {
                ImageDescriptors imageDescriptorSet = new ImageDescriptors();

                //Debug.Log("desc path: " + path);

                _descriptorSetFilePaths.Add(path);
                _descriptorSets.Add(imageDescriptorSet);
            }

            FileInfo fi;
            for (int i = 0; i < _descriptorSetFilePaths.Count; i++)
            {
                // 일단 파일이 존재하는지 확인 후, Image Tracker로 넘겨야함
                fi = new FileInfo(_descriptorSetFilePaths[i]);

                if (fi.Exists)
                {
                    _descriptorSets[i].LoadDescriptorSet(_descriptorSetFilePaths[i]);
                }
                else
                {
                    Debug.Log("KPJT: CTrackingManager: Start() Descriptor File not found : " + _descriptorSetFilePaths[i]);
                }
            }

            // find all TargetImage
            var targetImages = GameObject.FindObjectsOfType<CTargetImage>();

            _targetImageMap = new Dictionary<string, CTargetImage>();

            for (int i = 0; i < targetImages.Length; i++)
            {
                _targetImageMap.Add(targetImages[i].TargeUUID, targetImages[i]);
            }

            // Descriptor 로드 
            StartCoroutine("CoLoadDescriptor");
        }

        private IEnumerator CoLoadDescriptor()
		{
            Debug.Log ("UNITY: CTrackingManager: CoLoadDescriptor() ");
			// 이미지 트래커가 생성되기까지를 기다린다.
			do
			{
				yield return null;
			} while (_imageTracker == null);


			for (int i = 0; i < _descriptorSetFilePaths.Count; i++)
			{
				_imageTracker.ActivateDescriptor(_descriptorSets[i].GetDescriptorSet());
			}

            Debug.Log("Descriptor Activated!");
        }

		private Matrix4x4 GetMatrix4x4(float[] cameraPose)
		{
			Matrix4x4 matrix = new Matrix4x4();
			matrix.m00 = cameraPose[0];
			matrix.m10 = cameraPose[1];
			matrix.m20 = cameraPose[2];
			matrix.m30 = cameraPose[3];
			matrix.m01 = cameraPose[4];
			matrix.m11 = cameraPose[5];
			matrix.m21 = cameraPose[6];
			matrix.m31 = cameraPose[7];
			matrix.m02 = cameraPose[8];
			matrix.m12 = cameraPose[9];
			matrix.m22 = cameraPose[10];
			matrix.m32 = cameraPose[11];
			matrix.m03 = cameraPose[12];
			matrix.m13 = cameraPose[13];
			matrix.m23 = cameraPose[14];
			matrix.m33 = cameraPose[15];

			return matrix;
		}

//		public void AttachAnchor()
//		{
//			if (_trackerType == ArTrackingEngine.ARCORE)
//				_targetImage.SetArcoreAnchor ();
//			else if (_trackerType == ArTrackingEngine.ARKIT)
//				_targetImage.SetArkitAnchor ();

//			//! Stop Image Trakcing
////			_working = false;
		//	// ImageTracker 중단  
		//	ImageTrackerPause();
		//}


        public void ImageTrackerPause()
        {
            Debug.Log ("UNITY: CTrackingManager: ImageTrackerPause() ");

			if (_imageTracker != null && _working == true)
            {
                // ImageTracker에 preview 데이터 전달
                _working = false;
                _imageTracker.Pause();
            }
            else
            {
                Debug.Log("UNITY ERROR: ImageTracker is null");
            }
        }

        public void ImageTrackerResume()
        {
            Debug.Log ("UNITY: CTrackingManager: ImageTrackerResume() ");

            if (_imageTracker != null)
            {
                // ImageTracker 재개 
                _imageTracker.Resume();
                // TrakingManager 재개 
                _working = true;
            }
            else
            {
                Debug.Log("UNITY ERROR: ImageTracker is null");
            }
        }

        private void ImageTrackerStop()
        {
            Debug.Log ("UNITY: CTrackingManager: ImageTrackerStop() ");

            if (_imageTracker != null)
            {
                // ImageTracker에 preview 데이터 전달
                _working = false;
                // ImageTracker 중단  
                _imageTracker.Stop();
            }
        }

        public void SetSLAMCameraPose( Matrix4x4 matrix )
        {
            if (_targetImage != null)
            {
                _targetImage.SetSLAMCameraPose( matrix );
            }
        }

	}

}
