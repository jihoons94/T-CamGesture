using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;
using AOT;


namespace Treal.BrowserCore
{
    public class MotionTracker
    {

        // Use this for initialization
        private static MotionTracker instance;
        public static IntPtr m_detector;
        public int maxObj = 100;
        private MotionTracker() { }

        private int frameWidth = 0;
		private int frameHeight = 0;
		public bool isMotionDetectorWorking = false;

        
        public static MotionTracker Instance
        {
            get
            {
                if (instance == null)
                {
                    //RegisterDebugCallback(OnDebugCallback);
                    instance = new MotionTracker();
                    Debug.Log("MotionTracker: Create motion tracker");

                }
                return instance;
            }
        }

            
        public bool CreateMotionTracker(float ratio, int num)
        {
            if (num < 100)
                maxObj = num;
            else
                maxObj = 0;

            m_detector = it_skt_motion_CreateMotionTracker(ratio, maxObj);
            return true;
        }

        ~MotionTracker()
        {
            Debug.Log ("MotionTracker: ~MotionDetector() ");
            it_skt_motion_Pause(m_detector);
            it_skt_motion_Stop(m_detector);
            it_skt_motion_ReleaseMotionTracker(m_detector);
        }

        public bool Start()
        {
			Debug.Log ("MotionTracker: Start()");
            return it_skt_motion_Start(m_detector);

        }

        public bool Stop()
        {
			Debug.Log ("MotionTracker: Stop()");
            return it_skt_motion_Stop(m_detector);
        }

        public bool Pause()
        {
			Debug.Log ("MotionTracker: Pause()");
            return it_skt_motion_Pause(m_detector);
        }

        public bool Resume()
        {
			Debug.Log ("MotionTracker: Resume()");
            return it_skt_motion_Resume(m_detector);
        }

        public bool SetCameraParam(int width, int height, IMAGE_FORMAT format, IMAGE_FLIP flip)
        {
			isMotionDetectorWorking = true;
			frameWidth = width;
			frameHeight = height;
            return it_skt_motion_SetCameraParam(m_detector, width, height, (int)format, (int)flip);
        }

		public bool SetCameraFrame(Color32[] webCamPixels)
        {
            GCHandle pixelsPinnedArray = GCHandle.Alloc(webCamPixels, GCHandleType.Pinned);
            bool result = it_skt_motion_SetCameraFrame(m_detector, pixelsPinnedArray.AddrOfPinnedObject(), webCamPixels.Length * 4);
            pixelsPinnedArray.Free();
            return result;
        }

        public bool SetCameraFrameNative(UIntPtr webCamPixels, int size)
        {
            if (IntPtr.Size == 8)
            {
                IntPtr iwebCamPixels = unchecked((IntPtr)(long)(ulong)webCamPixels);
                return it_skt_motion_SetCameraFrame(m_detector, iwebCamPixels, size);
            }
            else
            {
                IntPtr iwebCamPixels = unchecked((IntPtr)(int)(uint)webCamPixels);
                return it_skt_motion_SetCameraFrame(m_detector, iwebCamPixels, size);
            }
        }

        public bool SetCameraFrameNative(IntPtr webCamPixels, int size)
        {
            return it_skt_motion_SetCameraFrame(m_detector, webCamPixels, size);
        }
        
		public bool SetTrackRoI(float px, float py, int width, int height, int label)
		{
            if (label < 0 || label > maxObj)
                return false;

            return it_skt_motion_SetTrackRoI(m_detector, px, py, width, height, label);
        }
        public bool SetTrackRoIs(Vector2[] pts, int[] width, int[] height, int[] label, int num)
        {
            if (num < 0 || num > maxObj)
                return false;

            return it_skt_motion_SetTrackRoIs(m_detector, pts, width, height, label, num);
        }
        public bool UpdateTrackRoIs()
        {
            return it_skt_motion_UpdateTrackRoIs(m_detector);
        }
        public bool GetRoIMotion(ref Vector2[] mv)
        {
			return it_skt_motion_GetRoIMotion(m_detector, mv);
        }

        public bool ResetTrackRoIs()
        {
            return it_skt_motion_ReSetTrackRoIs(m_detector);
        }

        //[DllImport("MotionTracker", CallingConvention = CallingConvention.Cdecl)]
        //static extern void RegisterDebugCallback(debugCallback cb);
        //delegate void debugCallback(IntPtr request, int color, int size);
        //enum Color { red, green, blue, black, white, yellow, orange };
        //[MonoPInvokeCallback(typeof(debugCallback))]
        //static void OnDebugCallback(IntPtr request, int color, int size)
        //{
        //    //Ptr to string
        //    string debug_string = Marshal.PtrToStringAnsi(request, size);

        //    //Add Specified Color
        //    debug_string =
        //        String.Format("{0}{1}{2}{3}{4}",
        //        "<color=",
        //        ((Color)color).ToString(),
        //        ">",
        //        debug_string,
        //        "</color>"
        //        );

        //    UnityEngine.Debug.Log(debug_string);
        //}



#if UNITY_IOS && !UNITY_EDITOR_OSX
    [DllImport("__Internal")]
    private static extern IntPtr it_skt_motion_CreateMotionTracker(float ratio, int num);
    [DllImport("__Internal")]
    private static extern void it_skt_motion_ReleaseMotionTracker(IntPtr detector);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_Start(IntPtr detector);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_Stop(IntPtr detector);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_Pause(IntPtr detector);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_Resume(IntPtr detector);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_SetCameraParam(IntPtr detector, int width, int height, int format, int flip);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_SetCameraFrame(IntPtr detector, IntPtr pData, int length);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_SetTrackRoI(IntPtr detector, float px, float py, int width, int height, int label);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_SetTrackRoIs(IntPtr detector, Vector2[] pts, int[] width, int[] height, int[] label, int num);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_UpdateTrackRoIs(IntPtr detector);
    [DllImport("__Internal")]
	private static extern bool it_skt_motion_GetRoIMotion(IntPtr detector, Vector2[] mv);
    [DllImport("__Internal")]
    private static extern IntPtr it_skt_motion_CreateMotionTracker(float ratio, int num);
    [DllImport("__Internal")]
    private static extern void it_skt_motion_ReleaseMotionTracker(IntPtr detector);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_Start(IntPtr detector);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_Stop(IntPtr detector);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_Pause(IntPtr detector);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_Resume(IntPtr detector);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_SetCameraParam(IntPtr detector, int width, int height, int format, int flip);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_SetCameraFrame(IntPtr detector, IntPtr pData, int length);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_SetTrackRoI(IntPtr detector, float px, float py, int width, int height, int label);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_SetTrackRoIs(IntPtr detector, Vector2[] pts, int[] width, int[] height, int[] label, int num);
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_ReSetTrackRoIs(IntPtr tracker);            
    [DllImport("__Internal")]
    private static extern bool it_skt_motion_UpdateTrackRoIs(IntPtr detector);
    [DllImport("__Internal")]
	private static extern bool it_skt_motion_GetRoIMotion(IntPtr detector, Vector2[] mv);
#else
        [DllImport("MotionTracker")]
        private static extern IntPtr it_skt_motion_CreateMotionTracker(float ratio, int num);
        [DllImport("MotionTracker")]
        private static extern void it_skt_motion_ReleaseMotionTracker(IntPtr detector);
        [DllImport("MotionTracker")]
        private static extern bool it_skt_motion_Start(IntPtr detector);
        [DllImport("MotionTracker")]
        private static extern bool it_skt_motion_Stop(IntPtr detector);
        [DllImport("MotionTracker")]
        private static extern bool it_skt_motion_Pause(IntPtr detector);
        [DllImport("MotionTracker")]
        private static extern bool it_skt_motion_Resume(IntPtr detector);
        [DllImport("MotionTracker")]
        private static extern bool it_skt_motion_SetCameraParam(IntPtr detector, int width, int height, int format, int flip);
        [DllImport("MotionTracker")]
        private static extern bool it_skt_motion_SetCameraFrame(IntPtr detector, IntPtr pData, int length);
        [DllImport("MotionTracker")]
        private static extern bool it_skt_motion_SetTrackRoI(IntPtr detector, float px, float py, int width, int height, int label);
        [DllImport("MotionTracker")]
        private static extern bool it_skt_motion_SetTrackRoIs(IntPtr detector, Vector2[] pts, int[] width, int[] height, int[] label, int num);
        [DllImport("MotionTracker")]
        private static extern bool it_skt_motion_ReSetTrackRoIs(IntPtr tracker);
        [DllImport("MotionTracker")]
        private static extern bool it_skt_motion_UpdateTrackRoIs(IntPtr detector);
        [DllImport("MotionTracker")]
        private static extern bool it_skt_motion_GetRoIMotion(IntPtr detector, Vector2[] mv);
#endif
    }

}

