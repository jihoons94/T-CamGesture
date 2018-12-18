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
using System.Runtime.InteropServices;
using System.Text;


namespace Treal.BrowserCore
{
    public class FaceDetector
    {

        // Use this for initialization
        private static FaceDetector instance;
        private static IFaceDetector nativePlugin;


        private static IntPtr m_detector;
        public string m_classifierFilePath;

        private FaceDetector() {
            
        }
        public bool isFaceDetectorWorking = false;


        public static FaceDetector Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("UNITY: FaceDetector: Instance");
                    instance = new FaceDetector();

                    if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        nativePlugin = new FaceDetectorIOSPlugin();
                    }
                    else if (Application.platform == RuntimePlatform.Android)
                    {
                        nativePlugin = new FaceDetectorAndroidPlugin();
                    }


                    m_detector = nativePlugin.it_skt_face_CreateFaceDetector_();

                    if (m_detector == null)
                    {
                        Debug.Log("m_detector null");
                    }
                }
                return instance;
            }
        }

        public int Load(string path)
        {
            Debug.Log("UNITY: FaceDetector: Load()");
            m_classifierFilePath = path;
            byte[] defaultBytes = Encoding.Default.GetBytes(m_classifierFilePath + "\0");
            byte[] convertedBytes = Encoding.Convert(Encoding.Default, Encoding.ASCII, defaultBytes);
            GCHandle pinnedArray = GCHandle.Alloc(convertedBytes, GCHandleType.Pinned);
            IntPtr pathPtr = pinnedArray.AddrOfPinnedObject();
            int flag = nativePlugin.it_skt_face_Load_(m_detector, pathPtr);
            pinnedArray.Free();
            return flag;
        }

        public int SavePreview(string path, float minThr, float maxThr)
        {
            Debug.Log("UNITY: FaceDetector: SavePreview()");
            m_classifierFilePath = path;
            byte[] defaultBytes = Encoding.Default.GetBytes(m_classifierFilePath + "\0");
            byte[] convertedBytes = Encoding.Convert(Encoding.Default, Encoding.ASCII, defaultBytes);
            GCHandle pinnedArray = GCHandle.Alloc(convertedBytes, GCHandleType.Pinned);
            IntPtr pathPtr = pinnedArray.AddrOfPinnedObject();
            int flag = nativePlugin.it_skt_face_SavePreview_(m_detector, pathPtr, minThr, maxThr);
            pinnedArray.Free();
            return flag;
        }

        ~FaceDetector()
        {
            Debug.Log("UNITY: FaceDetector: ~Load()");
            nativePlugin.it_skt_face_Pause_(m_detector);
            nativePlugin.it_skt_face_Stop_(m_detector);
            //itReleaseFaceDetector(m_detector);
        }

        public int Start()
        {
            Debug.Log("UNITY: FaceDetector: Start()");
            return nativePlugin.it_skt_face_Start_(m_detector);

        }

        public int Stop()
        {
            Debug.Log("UNITY: FaceDetector: Stop()");
            return nativePlugin.it_skt_face_Stop_(m_detector);
        }

        public int Pause()
        {
            Debug.Log("UNITY: FaceDetector: Pause()");
            return nativePlugin.it_skt_face_Pause_(m_detector);
        }

        public int Resume()
        {
            Debug.Log("UNITY: FaceDetector: Resume()");
            return nativePlugin.it_skt_face_Resume_(m_detector);
        }

        public int SetCameraParam(int width, int height, IMAGE_FORMAT format, IMAGE_FLIP flip, IMAGE_ORIENTATION orientation)
        {
            Debug.Log("UNITY: FaceDetector: SetCameraParam()");
            isFaceDetectorWorking = true;
            return nativePlugin.it_skt_face_SetCameraParam_(m_detector, width, height, (int)format, (int)flip, (int)orientation);
        }

        public int SetCameraFrame(Color32[] webCamPixels)
        {
            GCHandle pixelsPinnedArray = GCHandle.Alloc(webCamPixels, GCHandleType.Pinned);
            int result = nativePlugin.it_skt_face_SetCameraFrame_(m_detector, pixelsPinnedArray.AddrOfPinnedObject(), webCamPixels.Length * 4);
            pixelsPinnedArray.Free();
            return result;
        }

        public int SetCameraFrameNative(UIntPtr webCamPixels, int size)
        {
            int result;
            if (IntPtr.Size == 8)
            {
                IntPtr iwebCamPixels = unchecked((IntPtr)(long)(ulong)webCamPixels);
                result = nativePlugin.it_skt_face_SetCameraFrame_(m_detector, iwebCamPixels, size);
            }
            else
            {
                IntPtr iwebCamPixels = unchecked((IntPtr)(int)(uint)webCamPixels);
                result = nativePlugin.it_skt_face_SetCameraFrame_(m_detector, iwebCamPixels, size);
            }
            return result;
        }

        public int SetCameraFrameNative(IntPtr webCamPixels, int size)
        {
            int result;
            result = nativePlugin.it_skt_face_SetCameraFrame_(m_detector, webCamPixels, size);
            return result;
        }

        public int SetBoxParam(int x, int y, int width, int height)
        {
            Debug.Log("UNITY: FaceDetector: SetBoxParam()");

            return nativePlugin.it_skt_face_SetBoxParam_(m_detector, x, y, width, height, 0.15f);
        }

        public int SetDetectParam(int px, int py, int nframes)
        {
            Debug.Log("UNITY: FaceDetector: SetDetectParam()");

            return nativePlugin.it_skt_face_SetDetectParam_(m_detector, px, py, nframes);
        }

        public bool IsDetected(float minthr, float maxthr)
        {
            return nativePlugin.it_skt_face_IsDetected_(m_detector, minthr, maxthr) != 0;
        }

        public interface IFaceDetector
        {
            IntPtr it_skt_face_CreateFaceDetector_();
            void it_skt_face_ReleaseFaceDetector_(IntPtr detector);
            int it_skt_face_Start_(IntPtr detector);
            int it_skt_face_Stop_(IntPtr detector);
            int it_skt_face_Pause_(IntPtr detector);
            int it_skt_face_Resume_(IntPtr detector);
            int it_skt_face_Load_(IntPtr detector, IntPtr path);
            int it_skt_face_SetCameraParam_(IntPtr detector, int width, int height, int format, int flip, int orientation);
            int it_skt_face_SetCameraFrame_(IntPtr detector, IntPtr pData, int length);
            int it_skt_face_SetBoxParam_(IntPtr detector, int x, int y, int boxWidth, int boxHeight, float minRatio);
            int it_skt_face_SetDetectParam_(IntPtr detector, int px, int py, int nframes);
            int it_skt_face_IsDetected_(IntPtr detector, float minthr, float maxthr);
            int it_skt_face_SavePreview_(IntPtr detector, IntPtr path, float minthr, float maxthr);
        }

        public class FaceDetectorAndroidPlugin : IFaceDetector
        {
            [DllImport("FaceDetector")]
            private static extern IntPtr it_skt_face_CreateFaceDetector();
            [DllImport("FaceDetector")]
            private static extern void it_skt_face_ReleaseFaceDetector(IntPtr detector);
            [DllImport("FaceDetector")]
            private static extern int it_skt_face_Start(IntPtr detector);
            [DllImport("FaceDetector")]
            private static extern int it_skt_face_Stop(IntPtr detector);
            [DllImport("FaceDetector")]
            private static extern int it_skt_face_Pause(IntPtr detector);
            [DllImport("FaceDetector")]
            private static extern int it_skt_face_Resume(IntPtr detector);
            [DllImport("FaceDetector")]
            private static extern int it_skt_face_Load(IntPtr detector, IntPtr path);
            [DllImport("FaceDetector")]
            private static extern int it_skt_face_SetCameraParam(IntPtr detector, int width, int height, int format, int flip, int orientation);
            [DllImport("FaceDetector")]
            private static extern int it_skt_face_SetCameraFrame(IntPtr detector, IntPtr pData, int length);
            [DllImport("FaceDetector")]
            private static extern int it_skt_face_SetBoxParam(IntPtr detector, int x, int y, int boxWidth, int boxHeight, float minRatio);
            [DllImport("FaceDetector")]
            private static extern int it_skt_face_SetDetectParam(IntPtr detector, int px, int py, int nframes);
            [DllImport("FaceDetector")]
            private static extern int it_skt_face_IsDetected(IntPtr detector, float minthr, float maxthr);
            [DllImport("FaceDetector")]
            private static extern int it_skt_face_SavePreview(IntPtr detector, IntPtr path, float minthr, float maxthr);


            public IntPtr it_skt_face_CreateFaceDetector_()
            {
                return it_skt_face_CreateFaceDetector();
            }
            public void it_skt_face_ReleaseFaceDetector_(IntPtr detector)
            {
                it_skt_face_ReleaseFaceDetector( detector);
            }
            public int it_skt_face_Start_(IntPtr detector)
            {
                return it_skt_face_Start(detector);
            }
            public int it_skt_face_Stop_(IntPtr detector)
            {
                return it_skt_face_Stop(detector);
            }
            public int it_skt_face_Pause_(IntPtr detector)
            {
                return it_skt_face_Pause(detector);
            }
            public int it_skt_face_Resume_(IntPtr detector)
            {
                return it_skt_face_Resume(detector);
            }
            public int it_skt_face_Load_(IntPtr detector, IntPtr path)
            {
                return it_skt_face_Load(detector, path);
            }
            public int it_skt_face_SetCameraParam_(IntPtr detector, int width, int height, int format, int flip, int orientation)
            {
                return it_skt_face_SetCameraParam(detector, width, height, format, flip, orientation);
            }
            public int it_skt_face_SetCameraFrame_(IntPtr detector, IntPtr pData, int length)
            {
                return it_skt_face_SetCameraFrame(detector, pData, length);
            }
            public int it_skt_face_SetBoxParam_(IntPtr detector, int x, int y, int boxWidth, int boxHeight, float minRatio)
            {
                return it_skt_face_SetBoxParam(detector, x, y, boxWidth, boxHeight, minRatio);
            }
            public int it_skt_face_SetDetectParam_(IntPtr detector, int px, int py, int nframes)
            {
                return it_skt_face_SetDetectParam(detector, px, py, nframes);
            }
            public int it_skt_face_IsDetected_(IntPtr detector, float minthr, float maxthr)
            {
                return it_skt_face_IsDetected(detector, minthr, maxthr);
            }
            public int it_skt_face_SavePreview_(IntPtr detector, IntPtr path, float minthr, float maxthr)
            {
                return it_skt_face_SavePreview(detector, path, minthr, maxthr);
            }
        }

        public class FaceDetectorIOSPlugin : IFaceDetector
        {
            [DllImport("__Internal")]
            private static extern IntPtr it_skt_face_CreateFaceDetector();
            [DllImport("__Internal")]
            private static extern void it_skt_face_ReleaseFaceDetector(IntPtr detector);
            [DllImport("__Internal")]
            private static extern int it_skt_face_Start(IntPtr detector);
            [DllImport("__Internal")]
            private static extern int it_skt_face_Stop(IntPtr detector);
            [DllImport("__Internal")]
            private static extern int it_skt_face_Pause(IntPtr detector);
            [DllImport("__Internal")]
            private static extern int it_skt_face_Resume(IntPtr detector);
            [DllImport("__Internal")]
            private static extern int it_skt_face_Load(IntPtr detector, IntPtr path);
            [DllImport("__Internal")]
            private static extern int it_skt_face_SetCameraParam(IntPtr detector, int width, int height, int format, int flip, int orientation);
            [DllImport("__Internal")]
            private static extern int it_skt_face_SetCameraFrame(IntPtr detector, IntPtr pData, int length);
            [DllImport("__Internal")]
            private static extern int it_skt_face_SetBoxParam(IntPtr detector, int x, int y, int boxWidth, int boxHeight, float minRatio);
            [DllImport("__Internal")]
            private static extern int it_skt_face_SetDetectParam(IntPtr detector, int px, int py, int nframes);
            [DllImport("__Internal")]
            private static extern int it_skt_face_IsDetected(IntPtr detector, float minthr, float maxthr);
            [DllImport("__Internal")]
            private static extern int it_skt_face_SavePreview(IntPtr detector, IntPtr path, float minthr, float maxthr);


            public IntPtr it_skt_face_CreateFaceDetector_()
            {
                return it_skt_face_CreateFaceDetector();
            }
            public void it_skt_face_ReleaseFaceDetector_(IntPtr detector)
            {
                it_skt_face_ReleaseFaceDetector(detector);
            }
            public int it_skt_face_Start_(IntPtr detector)
            {
                return it_skt_face_Start(detector);
            }
            public int it_skt_face_Stop_(IntPtr detector)
            {
                return it_skt_face_Stop(detector);
            }
            public int it_skt_face_Pause_(IntPtr detector)
            {
                return it_skt_face_Pause(detector);
            }
            public int it_skt_face_Resume_(IntPtr detector)
            {
                return it_skt_face_Resume(detector);
            }
            public int it_skt_face_Load_(IntPtr detector, IntPtr path)
            {
                return it_skt_face_Load(detector, path);
            }
            public int it_skt_face_SetCameraParam_(IntPtr detector, int width, int height, int format, int flip, int orientation)
            {
                return it_skt_face_SetCameraParam(detector, width, height, format, flip, orientation);
            }
            public int it_skt_face_SetCameraFrame_(IntPtr detector, IntPtr pData, int length)
            {
                return it_skt_face_SetCameraFrame(detector, pData, length);
            }
            public int it_skt_face_SetBoxParam_(IntPtr detector, int x, int y, int boxWidth, int boxHeight, float minRatio)
            {
                return it_skt_face_SetBoxParam(detector, x, y, boxWidth, boxHeight, minRatio);
            }
            public int it_skt_face_SetDetectParam_(IntPtr detector, int px, int py, int nframes)
            {
                return it_skt_face_SetDetectParam(detector, px, py, nframes);
            }
            public int it_skt_face_IsDetected_(IntPtr detector, float minthr, float maxthr)
            {
                return it_skt_face_IsDetected(detector, minthr, maxthr);
            }
            public int it_skt_face_SavePreview_(IntPtr detector, IntPtr path, float minthr, float maxthr)
            {
                return it_skt_face_SavePreview(detector, path, minthr, maxthr);
            }
        }
    }
}
