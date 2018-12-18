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
using System.Runtime.InteropServices;
using UnityEngine;

namespace Treal.BrowserCore
{

	public enum IMAGE_FORMAT { UNKNOWN = 0, NV21 = 1, RGB888 = 2, GRAYSCALE = 3, RGBA = 4 };
	public enum IMAGE_FLIP { NO_FLIP = 0, HORIZONTAL_FLIP = 1, VERTICAL_FLIP = 2, BOTH_FLIP = 3 }
	public enum IMAGE_ORIENTATION { LANDSCAPE = 0, PORTRAIT = 1 }

	public class ImageTracker
	{
		private static ImageTracker instance;
        private static IImageTracker nativePlugin;

		private IntPtr m_imageTracker;

		private float m_imageWidth = 0;
		private float m_imageHeight = 0;
		private string m_uuid;
		private bool m_isTracking = false;

		public bool isImageTrackerWorking = false;

		private byte[] m_frameData = null;

		private ImageTracker() { }

		public static ImageTracker Instance
		{
			get
			{
				if (instance == null)
				{
                    Debug.Log ("UNITY: ImageTracker: Get Instance ");
					instance = new ImageTracker();
				}
				return instance;
			}
		}

		~ImageTracker()
		{
			ReleaseImageTracker();
		}

		public void CreateImageTracker()
		{
            Debug.Log ("UNITY: ImageTracker: CreateImageTracker() ");

            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerIOSPlugin();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    nativePlugin = new ImageTrackerAndroidPlugin();
                }
                else
                {
                    nativePlugin = new ImageTrackerAndroidPlugin();
                }
            }

            m_imageTracker = nativePlugin.itCreateImageTracker_();
		}

        public void StartImageTracker(ArTrackingEngine tracker)
        {
            Debug.Log ("UNITY: ImageTracker: StartImageTracker() ");
            if(tracker == ArTrackingEngine.ARKIT)
            {
                m_imageWidth = 1280;
                m_imageHeight = 720f;

                SetCameraParam((int)m_imageWidth, (int)m_imageHeight, IMAGE_FORMAT.GRAYSCALE, IMAGE_FLIP.NO_FLIP, IMAGE_ORIENTATION.LANDSCAPE);
            }
            else if (tracker == ArTrackingEngine.ARCORE)
            {
                m_imageWidth = 640f;
                m_imageHeight = 480f;
                SetCameraParam((int)m_imageWidth, (int)m_imageHeight, IMAGE_FORMAT.GRAYSCALE, IMAGE_FLIP.NO_FLIP, IMAGE_ORIENTATION.LANDSCAPE); 
            }
            else
            {
            }

            ImageTracker.Instance.Start();
            CTrackingManager.Instance.Working = true;  // Tracking Start
        }

		public void ReleaseImageTracker()
		{
            Debug.Log ("UNITY: ImageTracker: ReleaseImageTracker() ");
            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerIOSPlugin();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    nativePlugin = new ImageTrackerAndroidPlugin();
                }
            }
            nativePlugin.itReleaseImageTracker_(m_imageTracker);
		}

		public int Start()
		{
            Debug.Log ("UNITY: ImageTracker: Start() ");
            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerIOSPlugin();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    nativePlugin = new ImageTrackerAndroidPlugin();
                }
            }
            return nativePlugin.itStart_(m_imageTracker);
		}

		public int Stop()
		{
            Debug.Log ("UNITY: ImageTracker: Stop() ");
            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerIOSPlugin();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    nativePlugin = new ImageTrackerAndroidPlugin();
                }
            }
			return nativePlugin.itStop_(m_imageTracker);
		}

		public int Pause()
		{
            Debug.Log ("UNITY: ImageTracker: Pause() ");
            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerIOSPlugin();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    nativePlugin = new ImageTrackerAndroidPlugin();
                }
            }
            return nativePlugin.itPause_(m_imageTracker);
		}

		public int Resume()
		{
            Debug.Log ("UNITY: ImageTracker: Resume() ");
            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerIOSPlugin();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    nativePlugin = new ImageTrackerAndroidPlugin();
                }
            }
            return nativePlugin.itResume_(m_imageTracker);
		}

		public int ActivateDescriptor(IntPtr descSet)
		{
            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerIOSPlugin();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    nativePlugin = new ImageTrackerAndroidPlugin();
                }
            }
            return nativePlugin.itActivateDescriptor_(m_imageTracker, descSet);
		}

		public int DeactivateDescriptor(IntPtr descSet)
		{
            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerIOSPlugin();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    nativePlugin = new ImageTrackerAndroidPlugin();
                }
            }
            return nativePlugin.itDeactivateDescriptor_(m_imageTracker, descSet);
		}

		public int SetCameraParam(int width, int height, IMAGE_FORMAT format, IMAGE_FLIP flip, IMAGE_ORIENTATION orientation)
		{
            Debug.Log ("UNITY: ImageTracker: SetCameraParam() ");
			isImageTrackerWorking = true;
			m_frameData = new byte[width * height * 4];
            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerIOSPlugin();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    nativePlugin = new ImageTrackerAndroidPlugin();
                }
            }

            return nativePlugin.itSetCameraParam_(m_imageTracker, width, height, (int)format, (int)flip, (int)orientation);
		}

		public int SetCameraFrame(Color32[] webCamPixels)
		{
            Debug.Log ("UNITY: ImageTracker: SetCameraFrame() ");
			GCHandle pixelsPinnedArray = GCHandle.Alloc(webCamPixels, GCHandleType.Pinned);
            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerIOSPlugin();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    nativePlugin = new ImageTrackerAndroidPlugin();
                }
            }

            int result = nativePlugin.itSetCameraFrame_(m_imageTracker, pixelsPinnedArray.AddrOfPinnedObject(), webCamPixels.Length * 4);
			pixelsPinnedArray.Free();
			return result;
		}

		public int SetCameraFrameNative(UIntPtr webCamPixels, int size)
		{
            Debug.Log ("UNITY: ImageTracker: SetCameraFrameNative() <UInt>");
			int result;

            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerIOSPlugin();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    nativePlugin = new ImageTrackerAndroidPlugin();
                }
            }

			if (IntPtr.Size == 8)
			{
				IntPtr iwebCamPixels = unchecked((IntPtr)(long)(ulong)webCamPixels);
                result = nativePlugin.itSetCameraFrame_(m_imageTracker, iwebCamPixels, size);
			}
			else
			{
				IntPtr iwebCamPixels = unchecked((IntPtr)(int)(uint)webCamPixels);
                result = nativePlugin.itSetCameraFrame_(m_imageTracker, iwebCamPixels, size);
			}
			return result;
		}

		public int SetCameraFrameNative(IntPtr webCamPixels, int size)
		{
            Debug.Log ("UNITY: ImageTracker: SetCameraFrameNative() <Int>");
			int result;
            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerIOSPlugin();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    nativePlugin = new ImageTrackerAndroidPlugin();
                }
            }

            result = nativePlugin.itSetCameraFrame_(m_imageTracker, webCamPixels, size);

			return result;
		}


		ImageTrackable[] imageTrackables = new ImageTrackable[64];

		public float[] GetCameraPose(Texture2D tex)
		{
            //Debug.Log ("UNITY: ImageTracker: GetCameraPose() ");
            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerIOSPlugin();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    nativePlugin = new ImageTrackerAndroidPlugin();
                }
            }

            IntPtr imageTrackingStatePtr = nativePlugin.itGetImageTrackingState_(m_imageTracker);
			ImageTrackingState imageTrackingState = (ImageTrackingState)Marshal.PtrToStructure(imageTrackingStatePtr, typeof(ImageTrackingState));
			int numTrackable = imageTrackingState.numTrackable;

			if (numTrackable > 0)
			{
				long longPtr = imageTrackingState.trackables.ToInt64();

				IntPtr imageTrackablePtr = (IntPtr)longPtr;
				for (int i = 0; i < numTrackable; i++)
				{
					imageTrackables[i] = (ImageTrackable)Marshal.PtrToStructure(imageTrackablePtr, imageTrackables[i].GetType());
				}
				m_uuid = imageTrackables[0].id;
				m_imageWidth = imageTrackables[0].width;
				m_imageHeight = imageTrackables[0].height;
				float[] pose = imageTrackables[0].pose;

				m_isTracking = true;
				return pose;
			}
			else
			{
				m_isTracking = false;
				return null;
			}
		}

		public float[] GetCameraPose()
		{
            IntPtr imageTrackingStatePtr = nativePlugin.itGetImageTrackingState_(m_imageTracker);
			ImageTrackingState imageTrackingState = (ImageTrackingState)Marshal.PtrToStructure(imageTrackingStatePtr, typeof(ImageTrackingState));

			int numTrackable = imageTrackingState.numTrackable;

            Debug.Log("UNITY: ImageTracker: GetCameraPose() numTrackable = " + numTrackable );
			if (numTrackable > 0)
			{
				long longPtr = imageTrackingState.trackables.ToInt64();

				IntPtr imageTrackablePtr = (IntPtr)longPtr;
				for (int i = 0; i < numTrackable; i++)
				{
					imageTrackables[i] = (ImageTrackable)Marshal.PtrToStructure(imageTrackablePtr, imageTrackables[i].GetType());
				}
				m_uuid = imageTrackables[0].id;
				m_imageWidth = imageTrackables[0].width;
				m_imageHeight = imageTrackables[0].height;
				float[] pose = imageTrackables[0].pose;

				m_isTracking = true;
				return pose;
			}
			else
			{
				m_isTracking = false;
				return null;
			}
		}

		public float GetImageWidth()
		{
			return m_imageWidth;
		}

		public float GetImageHeight()
		{
			return m_imageHeight;
		}

		public bool IsTracking()
		{
			return m_isTracking;
		}

		public string GetUuid()
		{
			return m_uuid;
		}

		public byte[] GetFrameData()
		{
			return m_frameData;
		}

		public IntPtr GetImageTracker()
		{
			return m_imageTracker;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ImageTrackable
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 37)]
			public string id;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public float[] pose;
			public float width;
			public float height;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ImageTrackingState
		{
			public int numTrackable;
			public IntPtr trackables;
			public IntPtr frameData;
			public int frameSize;
		}

        public interface IImageTracker
        {
            IntPtr itCreateImageTracker_();
            void itReleaseImageTracker_(IntPtr tracker);
            int itStart_(IntPtr tracker);
            int itStop_(IntPtr tracker);
            int itPause_(IntPtr tracker);
            int itResume_(IntPtr tracker);
            int itActivateDescriptor_(IntPtr tracker, IntPtr descSet);
            int itDeactivateDescriptor_(IntPtr tracker, IntPtr descSet);
            int itSetCameraParam_(IntPtr tracker, int width, int height, int format, int flip, int orientation);
            int itSetCameraFrame_(IntPtr tracker, IntPtr pData, int length);
            int itFlipFrame_(IntPtr tracker, IntPtr pData, int length, int flip);
            IntPtr itGetImageTrackingState_(IntPtr tracker);
        }

        public class ImageTrackerAndroidPlugin : IImageTracker
        {
            [DllImport("ImageTracker")]
            private static extern IntPtr itCreateImageTracker();
            [DllImport("ImageTracker")]
            private static extern void itReleaseImageTracker(IntPtr tracker);
            [DllImport("ImageTracker")]
            private static extern int itStart(IntPtr tracker);
            [DllImport("ImageTracker")]
            private static extern int itStop(IntPtr tracker);
            [DllImport("ImageTracker")]
            private static extern int itPause(IntPtr tracker);
            [DllImport("ImageTracker")]
            private static extern int itResume(IntPtr tracker);
            [DllImport("ImageTracker")]
            private static extern int itActivateDescriptor(IntPtr tracker, IntPtr descSet);
            [DllImport("ImageTracker")]
            private static extern int itDeactivateDescriptor(IntPtr tracker, IntPtr descSet);
            [DllImport("ImageTracker")]
            private static extern int itSetCameraParam(IntPtr tracker, int width, int height, int format, int flip, int orientation);
            [DllImport("ImageTracker")]
            private static extern int itSetCameraFrame(IntPtr tracker, IntPtr pData, int length);
            [DllImport("ImageTracker")]
            private static extern int itSetCameraFrame(IntPtr tracker, UIntPtr pData, int length);
            [DllImport("ImageTracker")]
            private static extern int itFlipFrame(IntPtr tracker, IntPtr pData, int length, int flip);
            [DllImport("ImageTracker")]
            private static extern IntPtr itGetImageTrackingState(IntPtr tracker);


            public IntPtr itCreateImageTracker_()
            {
                return itCreateImageTracker();
            }
            public void itReleaseImageTracker_(IntPtr tracker)
            {
                itReleaseImageTracker(tracker);
            }
            public int itStart_(IntPtr tracker)
            {
                return itStart(tracker);
            }
            public int itStop_(IntPtr tracker)
            {
                return itStop(tracker);
            }
            public int itPause_(IntPtr tracker)
            {
                return itPause(tracker);
            }
            public int itResume_(IntPtr tracker)
            {
                return itResume(tracker);
            }
            public int itActivateDescriptor_(IntPtr tracker, IntPtr descSet)
            {
                return itActivateDescriptor( tracker,  descSet);
            }
            public int itDeactivateDescriptor_(IntPtr tracker, IntPtr descSet)
            {
                return itDeactivateDescriptor(tracker, descSet);
            }
            public int itSetCameraParam_(IntPtr tracker, int width, int height, int format, int flip, int orientation)
            {
                return itSetCameraParam(tracker, width, height, format, flip, orientation);
            }
            public int itSetCameraFrame_(IntPtr tracker, IntPtr pData, int length)
            {
                return itSetCameraFrame(tracker, pData, length);
            }
            public int itFlipFrame_(IntPtr tracker, IntPtr pData, int length, int flip)
            {
                return itFlipFrame(tracker, pData, length, flip);
            }
            public IntPtr itGetImageTrackingState_(IntPtr tracker)
            {
                return itGetImageTrackingState(tracker);
            }
        }

        public class ImageTrackerIOSPlugin : IImageTracker
        {
            [DllImport("__Internal")]
            private static extern IntPtr itCreateImageTracker();
            [DllImport("__Internal")]
            private static extern void itReleaseImageTracker(IntPtr tracker);
            [DllImport("__Internal")]
            private static extern int itStart(IntPtr tracker);
            [DllImport("__Internal")]
            private static extern int itStop(IntPtr tracker);
            [DllImport("__Internal")]
            private static extern int itPause(IntPtr tracker);
            [DllImport("__Internal")]
            private static extern int itResume(IntPtr tracker);
            [DllImport("__Internal")]
            private static extern int itActivateDescriptor(IntPtr tracker, IntPtr descSet);
            [DllImport("__Internal")]
            private static extern int itDeactivateDescriptor(IntPtr tracker, IntPtr descSet);
            [DllImport("__Internal")]
            private static extern int itSetCameraParam(IntPtr tracker, int width, int height, int format, int flip, int orientation);
            [DllImport("__Internal")]
            private static extern int itSetCameraFrame(IntPtr tracker, IntPtr pData, int length);
            [DllImport("__Internal")]
            private static extern int itFlipFrame(IntPtr tracker, IntPtr pData, int length, int flip);
            [DllImport("__Internal")]
            private static extern IntPtr itGetImageTrackingState(IntPtr tracker);


            public IntPtr itCreateImageTracker_()
            {
                return itCreateImageTracker();
            }
            public void itReleaseImageTracker_(IntPtr tracker)
            {
                itReleaseImageTracker(tracker);
            }
            public int itStart_(IntPtr tracker)
            {
                return itStart(tracker);
            }
            public int itStop_(IntPtr tracker)
            {
                return itStop(tracker);
            }
            public int itPause_(IntPtr tracker)
            {
                return itPause(tracker);
            }
            public int itResume_(IntPtr tracker)
            {
                return itResume(tracker);
            }
            public int itActivateDescriptor_(IntPtr tracker, IntPtr descSet)
            {
                return itActivateDescriptor(tracker, descSet);
            }
            public int itDeactivateDescriptor_(IntPtr tracker, IntPtr descSet)
            {
                return itDeactivateDescriptor(tracker, descSet);
            }
            public int itSetCameraParam_(IntPtr tracker, int width, int height, int format, int flip, int orientation)
            {
                return itSetCameraParam(tracker, width, height, format, flip, orientation);
            }
            public int itSetCameraFrame_(IntPtr tracker, IntPtr pData, int length)
            {
                return itSetCameraFrame(tracker, pData, length);
            }
            public int itFlipFrame_(IntPtr tracker, IntPtr pData, int length, int flip)
            {
                return itFlipFrame(tracker, pData, length, flip);
            }
            public IntPtr itGetImageTrackingState_(IntPtr tracker)
            {
                return itGetImageTrackingState(tracker);
            }
        }

	}

}

