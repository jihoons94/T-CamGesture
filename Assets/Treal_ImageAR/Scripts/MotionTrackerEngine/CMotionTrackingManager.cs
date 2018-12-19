/** 
*
* Copyright 2016-2017 SK Telecom. All Rights Reserved.
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
using System.Diagnostics;
using System.IO;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using System;
using TCamera;
using UnityEngine.SceneManagement;


namespace Treal.BrowserCore
{
    public class CMotionTrackingManager : MonoBehaviour
    {
        public bool IsEditor;
        public static CMotionTrackingManager Instance = null;
        public MotionTracker _detector = null;

        private float halfPreviewWidth, halfPreviewHeight; // preview resolution
        private int previewWidth, previewHeight; // preview resolution
        private int screenWidth, screenHeight;
        private float widthRatio, heightRatio;

        private Vector2[] pts;
        private Vector2[] result;
        private int[] objLabel;
        private int[] objWidth;
        private int[] objHeight;
        float relative_ratio = 0;

        private int numOfObj = 0;
        public int numOfixed = 0;
        public int numOfTarget = 0;

        private int statIndx = 0;

        private bool isUpdated = false;
        private float thresh = 5;

        public List<Transform> fixed_Buttons;
        public List<Transform> moving_Target;
        public List<Transform> Random_movingObj;
        public List<Transform> Background;
        public List<Transform> Background2;
        public Camera mARCamera;
        private bool _working = false;

        private bool _Moving = true;

       // public float[] Amount;
        public Motion_Event Event;


        public bool Working
        {
            get { return _working; }
            set
            {
                _working = value;
            }
        }

        public void SetTrackRoIs(Vector2[] pts, int[] width, int[] height, int[] label, int num)
        {

            _detector.SetTrackRoIs(pts, width, height, label, num);
        }

        void FindandSet()
        {
            Random_movingObj.Clear();
            fixed_Buttons.Clear();
            moving_Target.Clear();

            GameObject MotionEvent = GameObject.FindWithTag("MotionEvent");
            Event = MotionEvent.GetComponent<Motion_Event>();

            GameObject objs = MotionEvent.transform.GetChild(0).gameObject;
            GameObject buttons = MotionEvent.transform.GetChild(1).gameObject;
            GameObject Target = MotionEvent.transform.GetChild(2).gameObject;
            GameObject Back = MotionEvent.transform.GetChild(3).gameObject;
            GameObject Back2 = MotionEvent.transform.GetChild(4).gameObject;

            for (int i = 0; i < Target.transform.childCount; i++)
            {
                moving_Target.Add(Target.transform.GetChild(i).transform);
            }

            for (int i = 0; i < buttons.transform.childCount; i++)
            {
                fixed_Buttons.Add(buttons.transform.GetChild(i).transform);
            }

            for (int i = 0; i < objs.transform.childCount; i++)
            {
                Random_movingObj.Add(objs.transform.GetChild(i).transform);
            }

            for (int i = 0; i < Back.transform.childCount; i++)
            {
                Background.Add(Back.transform.GetChild(i).transform);
            }

            for (int i = 0; i < Back2.transform.childCount; i++)
            {
                Background2.Add(Back2.transform.GetChild(i).transform);
            }

            numOfObj = moving_Target.Count + fixed_Buttons.Count + Random_movingObj.Count;
            numOfixed = fixed_Buttons.Count;
            numOfTarget = moving_Target.Count;
            //Amount = new float[numOfixed];

            //for(int i=0; i<Amount.Length; i++)
            //{
            //    Amount[i] = 0;
            //}
        }

        void Awake()
        {
            statIndx = 0;
            Debug.Log("CMotionTrackingManager: Awake()");

           
            FindandSet();
            _detector = MotionTracker.Instance;
            if(!IsEditor)
            _detector.CreateMotionTracker(1.0f, numOfObj);

            Destroy(CMotionTrackingManager.Instance);
            CMotionTrackingManager.Instance = this;
            pts = new Vector2[numOfObj];
            result = new Vector2[numOfObj];
            objLabel = new int[numOfObj];
            objHeight = new int[numOfObj];
            objWidth = new int[numOfObj];
            // This object remains until application exist.
            DontDestroyOnLoad(gameObject);
        }


        void setReleativePosition_Others(List<Transform> items, int prevWidth, int prevHeight, bool isFixed, float ratio)
        {
            for (int i = 0; i < items.Count; i++)
            {
                float px = items[i].localPosition.x * ratio;
                float py = items[i].localPosition.y * ratio;
                float scaleX = items[i].localScale.x * ratio;
                float scaleY = items[i].localScale.y * ratio;

                if (scaleX > prevWidth / 2.0 || scaleX < 0)
                    scaleX = 110 * ratio;

                if (scaleY > prevHeight / 2.0 || scaleY < 0)
                    scaleY = 110 * ratio;

                if (-(prevWidth - scaleX) / 2 > px || px > (prevWidth - scaleX) / 2)
                    px = UnityEngine.Random.Range(-(prevWidth - scaleX) / 2, (prevWidth - scaleX) / 2);

                if (-(prevHeight - scaleY) / 2 > py || py > (prevHeight - scaleY) / 2)
                    py = UnityEngine.Random.Range(-(prevHeight - scaleY) / 2, (prevHeight - scaleY) / 2);

                items[i].localPosition = new Vector3(px, py, 0);
                items[i].localScale = new Vector3(scaleX, scaleY, 0);
            }

        }

        void setReleativePosition(List<Transform> items, int prevWidth, int prevHeight, bool isFixed, float ratio)
        {
            int lastIndx = statIndx;
            for (int i = 0; i < items.Count; i++)
            {
                float px = items[i].localPosition.x * ratio;
                float py = items[i].localPosition.y * ratio;
                float scaleX = items[i].localScale.x * ratio;
                float scaleY = items[i].localScale.y * ratio;

                if (scaleX > prevWidth / 2.0 || scaleX < 0)
                    scaleX = 110 * ratio;

                if (scaleY > prevHeight / 2.0 || scaleY < 0)
                    scaleY = 110 * ratio;

                if (-(prevWidth - scaleX) / 2 > px || px > (prevWidth - scaleX) / 2)
                    px = UnityEngine.Random.Range(-(prevWidth - scaleX) / 2, (prevWidth - scaleX) / 2);

                if (-(prevHeight - scaleY) / 2 > py || py > (prevHeight - scaleY) / 2)
                    py = UnityEngine.Random.Range(-(prevHeight - scaleY) / 2, (prevHeight - scaleY) / 2);

                objHeight[i + lastIndx] = (int)(scaleX / 2 + 0.5f);
                objWidth[i + lastIndx] = (int)(scaleY / 2 + 0.5f);
                pts[i + lastIndx].x = (px + halfPreviewWidth);
                pts[i + lastIndx].y = (py + halfPreviewHeight);
                objLabel[i + lastIndx] = i + lastIndx;
                statIndx++;

                items[i].localPosition = new Vector3(px, py, 0);
                items[i].localScale = new Vector3(scaleX, scaleY, 0);


                isUpdated = true;
            }
        }

        public void StartMotionDetector()
        {
            _working = true;
            _detector.Start();
        }

        void Update()
        {

            if (_working)
            {
                if(_Moving)
                {
                    SetTrack_position(0, numOfTarget);
                }

                //float tt = Time.realtimeSinceStartup;
                if (isUpdated)
                {
                    _detector.UpdateTrackRoIs();
                }

                _detector.GetRoIMotion(ref result);
                //float t2 = Time.realtimeSinceStartup - tt;
                isUpdated = false;

                for (int n = 0; n < numOfObj; n++)
                {
                    if (result[n].sqrMagnitude > thresh) // 인식 되었을 경우에 이벤트
                    {
                        if (n >= numOfixed + numOfTarget) //랜덤 이동 대상
                        {
                            pts[n].x = UnityEngine.Random.Range(objWidth[n], previewWidth - objWidth[n]);
                            pts[n].y = UnityEngine.Random.Range(objHeight[n], previewHeight - objHeight[n]);
                            _detector.SetTrackRoI(pts[n].x, pts[n].y, objWidth[n], objHeight[n], objLabel[n]);

                            Random_movingObj[n - (numOfixed + numOfTarget)].localPosition = new Vector3(pts[n].x - halfPreviewWidth, pts[n].y - halfPreviewHeight, 0);
                            isUpdated = true;

                            Event.RandomEvent_On(n - (numOfixed + numOfTarget));
                        }
                        else if (numOfixed + numOfTarget > n && n >= numOfTarget) // 고정대상
                        {
                            
                            Event.FixedEvent_On(n - numOfTarget);
                            //fixed_Buttons[n - numOfTarget].gameObject.SetActive(false);
                        }
                        else // 움직이는 대상
                        {

                            
                            Event.MoveEvent_On(n);
                            //moving_Target[n].gameObject.SetActive(false);
                        }

                    }
                    else // 인식이 되지 않았을 경우의 이벤트
                    {
                        if (n < numOfTarget) // 움직이는 대상
                        {
                            Event.MoveEvent_Off(n);
                            //moving_Target[n].gameObject.SetActive(true);

                        }
                        else if (n < numOfixed + numOfTarget)// 고정 대상
                        {
                            
                            Event.FixedEvent_Off(n - numOfTarget);
                            //fixed_Buttons[n - numOfTarget].gameObject.SetActive(true);
                        }
                        else  //랜덤 대상
                        {
                            Event.RandomEvent_Off(n - (numOfixed + numOfTarget));
                        }


                    }
                }
            }
        }

        /// <summary>
        /// [jihoon]-움직이는 인식영역의 인식 위치를 동기화 시켜준다. 움직이는 인식 영역들은 연속적으로 배치되어야 한다.
        /// </summary>
        /// <param name="StartIndex">동기화 시킬 인식영역들의 시작 인덱스 값</param>
        /// <param name="MoveCount">동기화 시킬 인식영역들의 개수</param>
        public void SetTrack_position(int StartIndex, int MoveCount)
        {

            for (int i = StartIndex; i < MoveCount; i++)
            {
                pts[i].x = moving_Target[i].localPosition.x + halfPreviewWidth;
                pts[i].y = moving_Target[i].localPosition.y + halfPreviewHeight;
            }
            _detector.SetTrackRoIs(pts, objWidth, objHeight, objLabel, numOfTarget);
            isUpdated = true;
        }

       public void SetTrack_position_One(int _i, Vector3 _newPoint)
        {
            fixed_Buttons[_i].localPosition = _newPoint;
            pts[_i].x = fixed_Buttons[_i].localPosition.x + halfPreviewWidth;
            pts[_i].y = fixed_Buttons[_i].localPosition.y + halfPreviewHeight;
            _detector.SetTrackRoI(pts[_i].x, pts[_i].y, objWidth[_i+ numOfTarget], objHeight[_i + numOfTarget], objLabel[_i + numOfTarget]);
            isUpdated = true;
        }


        public void DetectorPause()
        {
            if (_detector != null && _working == true)
            {
                _working = false;
                _detector.Pause();
            }
            else
            {
            }
        }

        public void DetectorResume()
        {

            if (_detector != null)
            {
                _detector.Resume();
                _working = true;
            }
            else
            {
            }
        }

        private void DetectorStop()
        {
            if (_detector != null)
            {
                _working = false;
                _detector.Stop();
            }
            else
            {
            }
        }

        public bool SetCameraFrameNative(IntPtr webCamPixels, int size)
        {
            return _detector.SetCameraFrameNative(webCamPixels, size);
        }


        public int SetCameraParam(int width, int height, IMAGE_FORMAT format, IMAGE_FLIP flip)
        {
            previewWidth = width;
            previewHeight = height;
            halfPreviewHeight = height / 2.0f;
            halfPreviewWidth = width / 2.0f;
            mARCamera.projectionMatrix = Matrix4x4.Ortho(-previewWidth / 2, previewWidth / 2, -previewHeight / 2, previewHeight / 2, -10, 10);

            screenWidth = Screen.width;
            screenHeight = Screen.height;

            heightRatio = previewHeight / (float)screenHeight;
            widthRatio = previewWidth / (float)screenWidth;

            _detector.SetCameraParam(width, height, format, flip);
            relative_ratio = width / 1280.0f;
            Debug.Log("relative ratio : " + relative_ratio);
            setReleativePosition_Others(Background, width, height, true, relative_ratio);
            setReleativePosition_Others(Background2, width, height, true, relative_ratio);
            setReleativePosition(moving_Target, width, height, true, relative_ratio);
            setReleativePosition(fixed_Buttons, width, height, true, relative_ratio);
            setReleativePosition(Random_movingObj, width, height, false, relative_ratio);
            _detector.SetTrackRoIs(pts, objWidth, objHeight, objLabel, statIndx);

            return 1;

        }

    }

}
