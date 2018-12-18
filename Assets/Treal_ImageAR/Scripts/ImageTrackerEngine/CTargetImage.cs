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
using CameraPoseFilter;
using Treal.Browser.Core;
using System.Collections.Generic;
using Treal.Browser.Log;

namespace Treal.BrowserCore
{
    public class CTargetImage : MonoBehaviour
    {
        public ITRO ARTro;
        public string TargetName;
        public string TargeUUID;

        public List<ITRO> contentsTROList;


        private float _width;
        private float _height;
        private float _initScaleX;
        private float _initScaleY;
        private float _initScaleZ;

        private Vector3 _position;
        private Vector3 _scale;
        private Quaternion _rotation;

        // For MWC ... 600 marker
        private float POSE_OFFSET = 2.0f / 3.0f;

        // SLAM Pose
        private Matrix4x4 _SLAMPose = Matrix4x4.identity;

        public enum TrackingStatus
        {
            None,
            Found,
            Lost
        }

        private TrackingStatus mLastStatus = TrackingStatus.None;
        public ITrakcingStatusChange mHandler;
        public FilterStyleConfig filterConfig;
        private FilterManager filterManager;

        private ArTrackingEngine _trackerType;
        private bool firstLostFlag = false;

        void Awake()
        {
            Debug.Log("_2KPJT: CTargetImage: Awake() ");
            filterManager = new FilterManager(filterConfig);
        }

        private void Start()
        {
            Debug.Log("_2KPJT: CTargetImage: Start() ");

            mHandler = GetComponent<ITrakcingStatusChange>();
            _trackerType = GameObject.Find("Main").GetComponent<CMain>().mTrealConfig.arTracking;

            _initScaleX = transform.localScale.x;
            _initScaleY = transform.localScale.y;
            _initScaleZ = transform.localScale.z;

            _position = new Vector3();
            _rotation = new Quaternion();
            _scale = new Vector3();

            SetModelVisible(false);

            if (ARTro != null)
            {
                TrealBrowser.FireARStart(ARTro, this, contentsTROList);
            }
        }

        public void SetTransformByNativeCamera(Matrix4x4 matrix)
        {
            _position.x = matrix.GetColumn(3).x * 100.0f;
            _position.y = matrix.GetColumn(3).y * 100.0f;
            _position.z = -matrix.GetColumn(3).z * 100.0f;

            Quaternion q = Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
            q.x = -q.x;
            q.y = -q.y;
            q *= Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            _rotation = q;

            transform.position = _position;
            transform.rotation = _rotation;
            transform.localScale = _scale;
        }

        public void SetTransformByARCore(Matrix4x4 matrixFromTreal)
        {
            // For MWC ... 600 marker.. offset ���� ����
            _position.x = matrixFromTreal.GetColumn(3).x * POSE_OFFSET;
            _position.y = matrixFromTreal.GetColumn(3).y * POSE_OFFSET;
            _position.z = -matrixFromTreal.GetColumn(3).z * POSE_OFFSET;

            Quaternion q = Quaternion.LookRotation(matrixFromTreal.GetColumn(2), matrixFromTreal.GetColumn(1));
            q.x = -q.x;
            q.y = -q.y;

            _rotation = q;

            float scaleFactorX = _width / _initScaleX;
            float scaleFactorZ = _height / _initScaleZ;

            _scale.x = _initScaleX * scaleFactorX;
            _scale.y = _initScaleY * scaleFactorZ;
            _scale.z = _initScaleZ * scaleFactorZ;

            Matrix4x4 treal_matrix = Matrix4x4.TRS(_position, _rotation, Vector3.one);

            Quaternion core_rotation = (Quaternion.LookRotation(_SLAMPose.GetColumn(2), _SLAMPose.GetColumn(1)));

            Vector3 core_position;
            core_position.x = _SLAMPose.GetColumn(3).x;
            core_position.y = _SLAMPose.GetColumn(3).y;
            core_position.z = _SLAMPose.GetColumn(3).z;

            core_rotation *= Quaternion.Euler(0.0f, 0.0f, -90.0f);

            Matrix4x4 arcore_matrix = Matrix4x4.TRS(core_position, core_rotation, Vector3.one);

            treal_matrix = arcore_matrix * treal_matrix;

            //Filtering Camera Pose 
            treal_matrix = filterManager.cameraPoseFilter(treal_matrix);

            _rotation = (Quaternion.LookRotation(treal_matrix.GetColumn(2), treal_matrix.GetColumn(1)));
            _rotation *= Quaternion.Euler(-90.0f, 0.0f, 0.0f);

            _position.x = treal_matrix.GetColumn(3).x;
            _position.y = treal_matrix.GetColumn(3).y;
            _position.z = treal_matrix.GetColumn(3).z;

            transform.position = _position;
            transform.rotation = _rotation;
            transform.localScale = _scale * 0.1f;
        }


        public void SetTransformByARKit(Matrix4x4 imageTrakerMatrix)
        {
            Debug.Log("UNITY: CTargetImage: SetTransformByARKit() ");

            //Note frome ARKit: since our plane mesh is actually 10mx10m in the world, we scale it here by 0.1f
            Matrix4x4 treal_matrix = _SLAMPose * imageTrakerMatrix;

            //Filtering Camera Pose 
            treal_matrix = filterManager.cameraPoseFilter(treal_matrix);

            _position.x = treal_matrix.GetColumn(3).x;
			_position.y = treal_matrix.GetColumn(3).y;
			_position.z = -treal_matrix.GetColumn(3).z;

			Quaternion q = Quaternion.LookRotation(treal_matrix.GetColumn(2), treal_matrix.GetColumn(1));
			q.x = -q.x;
			q.y = -q.y;
			// add this...
			q *= Quaternion.Euler(-90.0f, 0.0f, 0.0f);
			_rotation = q;


			float scaleFactorX = _width / _initScaleX;
			float scaleFactorZ = _height / _initScaleZ;

			_scale.x = _initScaleX * scaleFactorX; 							
			_scale.y = _initScaleY * scaleFactorZ;
			_scale.z = _initScaleZ * scaleFactorZ;

			transform.position = _position;
			transform.rotation = _rotation;            
			transform.localScale = _scale * 0.1f;      
		}

		public void SetModelVisible(bool isModelVisible)
		{
			TrackingStatus newStatus;
			if (isModelVisible)
				newStatus = TrackingStatus.Found;
			else
				newStatus = TrackingStatus.Lost;

			if (mLastStatus != newStatus)
			{
				if (newStatus == TrackingStatus.Found)
				{
                    if (mHandler != null)
                    {
					    mHandler.Found();
                    }

                    if (ARTro != null)
                    {
                        TrealBrowser.FireARFound(ARTro, this, contentsTROList);
                        Treal_Logger.SendLog(ActionCode.TRACK_START, ARTro.GetTRA().ProductId, TargeUUID);
                    }
                }
				else
				{
                    if (mHandler != null)
                    {
                        mHandler.Lost();
                    }

                    if (ARTro != null)
                    {
                        if (firstLostFlag)
                        {
                            TrealBrowser.FireARLost(ARTro, this, contentsTROList);
                            Treal_Logger.SendLog(ActionCode.TRACK_END, ARTro.GetTRA().ProductId, TargeUUID);
                        }

                        firstLostFlag = true;
                    }
                }
			}

			mLastStatus = newStatus;
		}

		public void SetImageSize(float width, float height)
		{
			_width = width;
			_height = height;

			SetScale();
		}

		private void SetScale()
		{
			float scaleFactorX = _width / _initScaleX;
			float scaleFactorZ = _height / _initScaleZ;

			if (_trackerType == ArTrackingEngine.TCAM )
            {
                _scale.x = _initScaleX * scaleFactorX * 10.0f;
                _scale.y = _initScaleY * scaleFactorZ * 10.0f;
                _scale.z = _initScaleZ * scaleFactorZ * 10.0f;
            }
            else
            {
                // Unity Plane's size unit is 10m. so converting size unit to 1m needs to multiply 0.1m.
                _scale.x = _width * 0.1f;
                _scale.y = _height * 0.1f;
                _scale.z = _height * 0.1f;
            }

        }

        public void SetSLAMCameraPose( Matrix4x4 matrix )
        {
            _SLAMPose = matrix;
        }
	}
}

