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
using System.Runtime.InteropServices;

namespace TCamera {

	/// <summary>
	/// Native Plugin 연동 인터페이스
	/// </summary>
	public class TCamPlugin
	{
        /*
        #region native interface
#if UNITY_IPHONE
		[DllImport ("__Internal")]
#else
        [DllImport ("TCamPlugin")]
		#endif
		private static extern IntPtr GetRenderEventFunc();

        #if UNITY_IPHONE
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_Init(String name, int renderMethod, bool nativeCopyFrameData);
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_Dispose();
		[DllImport ("__Internal")]
		private static extern IntPtr TCamPlugin_GetDevices();
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_SetDevice(String deviceId);
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_SetPreviewResolution(int width, int height);
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_GetPreviewResolution(out int width, out int height);
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_SetPreviewFPS(int fps);
		[DllImport ("__Internal")]
		private static extern int TCamPlugin_GetPreviewFPS();
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_SetCaptureResolution(int width, int height);
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_GetCaptureResolution(out int width, out int height);
		[DllImport ("__Internal")]
		private static extern bool TCamPlugin_SetExposureMode(int mode);
		[DllImport ("__Internal")]
		private static extern float TCamPlugin_GetMinExposure();
		[DllImport ("__Internal")]
		private static extern float TCamPlugin_GetMaxExposure();
		[DllImport ("__Internal")]
		private static extern float TCamPlugin_SetExposure(float exposure);
		[DllImport ("__Internal")]
		private static extern bool TCamPlugin_SetFlashMode(int mode);
		[DllImport ("__Internal")]
		private static extern bool TCamPlugin_SetFocusMode(int mode);
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_StartPreview(int width, int height, int fps);
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_StopPreview();
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_Capture();
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_SetTexture(IntPtr texPtr, IntPtr yTexPtr, IntPtr uvTexPtr, IntPtr uTexPtr, IntPtr vTexPtr);
		[DllImport ("__Internal")]
		private static extern int TCamPlugin_GetSensorOrientation();
		[DllImport ("__Internal")]
		private static extern int TCamPlugin_GetDisplayRotation();

		[DllImport ("__Internal")]
		private static extern void TCamPlugin_SetScreenRotation(int width, int height, int sensorOrientation, int displayRotation);
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_GetPreviewData(byte[] yuv, byte[] y, byte[] uv, byte[] u, byte[] v);
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_SetCurrentFrame(int frameId);
		[DllImport ("__Internal")]
		private static extern void TCamPlugin_SetScale(int scale);
		[DllImport ("__Internal")]
		private static extern int TCamPlugin_GetPreviewTexture();

		[DllImport ("__Internal", CallingConvention = CallingConvention.StdCall)]
		private static extern void TCamPlugin_SetCallback(IntPtr previewStartCallback, IntPtr previewUpdateCallback, IntPtr captureUpdateCallback, IntPtr requestRenderCallback);
#else
        #region unused by java object
        [DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_Init(String name, int renderMethod, bool nativeCopyFrameData);
		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_Dispose();
		[DllImport ("TCamPlugin")]
		private static extern IntPtr TCamPlugin_GetDevices();
		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_SetDevice(String deviceId);
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_SetInvalidCamera2Device( String list );
		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_SetPreviewResolution(int width, int height);
		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_GetPreviewResolution(out int width, out int height);
		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_SetPreviewFPS(int fps);
		[DllImport ("TCamPlugin")]
		private static extern int TCamPlugin_GetPreviewFPS();
		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_SetCaptureResolution(int width, int height);
		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_GetCaptureResolution(out int width, out int height);
		[DllImport ("TCamPlugin")]
		private static extern bool TCamPlugin_SetExposureMode(int mode);
		[DllImport ("TCamPlugin")]
		private static extern float TCamPlugin_GetMinExposure();
		[DllImport ("TCamPlugin")]
		private static extern float TCamPlugin_GetMaxExposure();
		[DllImport ("TCamPlugin")]
		private static extern float TCamPlugin_SetExposure(float exposure);
		[DllImport ("TCamPlugin")]
		private static extern bool TCamPlugin_SetFlashMode(int mode);
		[DllImport ("TCamPlugin")]
		private static extern bool TCamPlugin_SetFocusMode(int mode);
		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_StartPreview(int width, int height, int fps);
		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_StopPreview();
		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_Capture();
		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_SetTexture(IntPtr texPtr, IntPtr yTexPtr, IntPtr uvTexPtr, IntPtr uTexPtr, IntPtr vTexPtr);
		[DllImport ("TCamPlugin")]
		private static extern int TCamPlugin_GetSensorOrientation();
		[DllImport ("TCamPlugin")]
		private static extern int TCamPlugin_GetDisplayRotation();
		#endregion

		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_SetScreenRotation(int width, int height, int sensorOrientation, int displayRotation);
		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_GetPreviewData(byte[] yuv, byte[] y, byte[] uv, byte[] u, byte[] v);
		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_SetCurrentFrame(int frameId);
		[DllImport ("TCamPlugin")]
		private static extern void TCamPlugin_SetScale(int scale);
		[DllImport ("TCamPlugin")]
		private static extern int TCamPlugin_GetPreviewTexture();

		[DllImport ("TCamPlugin", CallingConvention = CallingConvention.StdCall)]
		private static extern void TCamPlugin_SetCallback(IntPtr previewStartCallback, IntPtr previewUpdateCallback, IntPtr captureUpdateCallback, IntPtr requestRenderCallback);
		#endif

		#region test
		// Test
		#if UNITY_IPHONE
		[DllImport ("__Internal")]
		private static extern int TCamTest_GetRenderMethod();
		#else
		[DllImport ("TCamPlugin")]
		private static extern int TCamTest_GetRenderMethod();
		#endif
		#endregion
		#endregion
        */

        private static ITCamNativePlugin nativePlugin;

		public const string TAG = "TCamPlugin";
		public const bool DEBUG = true;


		static AndroidJavaObject mTCamPluginJO = null;

		//#if UNITY_EDITOR || UNITY_STANDALONE
		static string mRequestDevice;
		static int mRequestWidth, mRequestHeight, mRequestFps;
		static WebCamTexture mWebCamTexture;
		//#endif

		/**
		* @brief TCam Event를 Android(JAVA)단으로 전달한다.
		*
		* @param eventId: TCam.EventId 깂으로 다음과 같은 파마리터를 가진다.
		* INIT: GL 초기화 이벤트
		* DISPOSE: GL 종료 이벤트
		* START: Preview 시작 이벤트
		* STOP: Preview 종료 이벤트
		* RENDER: Preview Rendering 이벤트
		*/
		public static void GLIssuePluginEvent(TCam.EventId eventId)
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
            }
            else
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (nativePlugin == null)
                    {
                        nativePlugin = new TCamAndroidPlugin();
                    }

                    GL.IssuePluginEvent(nativePlugin.GetRenderEventFunc_(), (int)eventId);
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (nativePlugin == null)
                    {
                        nativePlugin = new TCamIOSPlugin();
                    }

                    GL.IssuePluginEvent(nativePlugin.GetRenderEventFunc_(), (int)eventId);
                }
                
            }
			//#if UNITY_EDITOR || UNITY_STANDALONE
			//#else
			//GL.IssuePluginEvent (GetRenderEventFunc (), (int) eventId);
			//#endif
		}

		/**
		* @brief TCam Android(JAVA) Object를 획득한다.
		*
		*/
		public static void GetTCamJavaObject()
		{
			if (mTCamPluginJO == null) {
				mTCamPluginJO = new AndroidJavaObject ("com.skt.tcam.TCamPlugin");

			}
		}

		/**
		* @brief TCam을 Pause 한다.
		*
		*/
		public static void OnPause()
		{
			if (mTCamPluginJO != null) {
				mTCamPluginJO.Call ("OnPause");
			}
		}

		/**
		* @brief TCam을 Resume 한다.
		*
		*/
		public static void OnResume()
		{
			if (mTCamPluginJO != null) {
				mTCamPluginJO.Call ("OnResume");
			}
		}

		/**
		* @brief 생성 된 TCam을 Destroy한다.
		*
		*/
		public static void OnDestroy()
		{
			if (mTCamPluginJO != null) {
				mTCamPluginJO.Call ("OnDestroy");
			}
		}

		/**
		* @brief TCam 초기화
		*
		* @param gameObjectName: Unity GameObject
		* @param nativeCamera: Native Camera
		* @param renderMethod: Rendering 방법 - NATIVE_GL_UPDATE_SHADER 권장
		* @param nativeCopyFrameData: Native에서 Frame data를 복사할지 설정. VisionEngine 사용을 위해선 true
		*/
		public static int Init(string gameObjectName, int nativeCamera, int renderMethod, bool nativeCopyFrameData)
		{
			int ret = 0;
			if (Application.platform == RuntimePlatform.Android) {
				//AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				//AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
				GetTCamJavaObject ();
				if (mTCamPluginJO != null) {
					ret = mTCamPluginJO.Call<int> ("Init", gameObjectName, (int)nativeCamera, (int)renderMethod, nativeCopyFrameData);
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                nativePlugin.TCamPlugin_Init_(gameObjectName, (int)renderMethod, false);
			}

			GLIssuePluginEvent (TCam.EventId.INIT);

			return ret;
		}

		/**
		* @brief TCam 종료
		*
		*/
		public static void Dispose()
		{
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					mTCamPluginJO.Call ("Dispose");
					mTCamPluginJO = null;
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                nativePlugin.TCamPlugin_Dispose_();
			}

			GLIssuePluginEvent (TCam.EventId.DISPOSE);
		}

		/**
		* @brief Device에 연결된 카메라들의 List를 획득한다.
		* @return Device에 연결된 카메라들의 List 목록
		*/
		public static string GetDevices()
		{
			string ret = null;
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                WebCamDevice[] devices = WebCamTexture.devices;
                for (int i = 0; i < devices.Length; i++)
                {
                    if (i != 0)
                    {
                        ret += ";";
                    }
                    ret += i.ToString() + ":" + (devices[i].isFrontFacing ? "1" : "0");
                }
            }
            else
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    GetTCamJavaObject();
                    if (mTCamPluginJO != null)
                    {
                        ret = mTCamPluginJO.Call<string>("GetDevices");
                    }
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (nativePlugin == null)
                    {
                        nativePlugin = new TCamIOSPlugin();
                    }

                    ret = Marshal.PtrToStringAnsi(nativePlugin.TCamPlugin_GetDevices_());
                }
            }

            /*
			#if UNITY_EDITOR || UNITY_STANDALONE
			WebCamDevice[] devices = WebCamTexture.devices;
			for (int i = 0; i < devices.Length; i++) {
				if (i != 0) {
					ret += ";";
				}
				ret += i.ToString() + ":" + (devices[i].isFrontFacing ? "1" : "0");
			}
			#else
			if (Application.platform == RuntimePlatform.Android) {
				GetTCamJavaObject ();
				if (mTCamPluginJO != null) {
					ret = mTCamPluginJO.Call<string> ("GetDevices");
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				ret = Marshal.PtrToStringAnsi(TCamPlugin_GetDevices ());
			}
			#endif
            */

			return ret;
		}

		/**
		* @brief Android에서 Camera2를 지원하지 않는 List를 등록한다.
		* @param mInvalidCamera2List Android에서 Camera2를 지원하지 않는 List들
		*/
        public static void SetInvalidCamera2Device(String[] mInvalidCamera2List)
        {
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
            }
            else
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    GetTCamJavaObject();
                    if (mTCamPluginJO != null)
                    {
                        foreach (String list in mInvalidCamera2List)
                        {
                            mTCamPluginJO.Call("SetInvalidCamera2Device", list);
                        }
                    }
                }
            }
            
            /*
            #if UNITY_EDITOR || UNITY_STANDALONE
            #else
            if (Application.platform == RuntimePlatform.Android) {
                GetTCamJavaObject ();
                if (mTCamPluginJO != null) {
                    foreach(String list in mInvalidCamera2List ){
                       mTCamPluginJO.Call ("SetInvalidCamera2Device", list);
                    }
                }
            }
            #endif
            */
        }

		/**
		* @brief Device에 연결된 카메라들의 List중 사용 할 Camera를 설정한다.
		* @param deviceName 사용 할 Camera 이름
		*/
		public static void SetDevice(string deviceName)
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                mRequestDevice = deviceName;
            }
            else
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (mTCamPluginJO != null)
                    {
                        mTCamPluginJO.Call("SetDevice", deviceName);
                    }
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (nativePlugin == null)
                    {
                        nativePlugin = new TCamIOSPlugin();
                    }

                    nativePlugin.TCamPlugin_SetDevice_(deviceName);
                }
            }

            /*
			#if UNITY_EDITOR || UNITY_STANDALONE
			mRequestDevice = deviceName;
			#else
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					mTCamPluginJO.Call ("SetDevice", deviceName);
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				TCamPlugin_SetDevice (deviceName);
			}
			#endif
            */
		}

		/**
		* @brief Camera Preview 해상도를 설정한다.
		* @param width: Camera Preview Width
		* @param height: Camera Preview Height
		*/
		public static void SetPreviewResolution(int width, int height)
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                mRequestWidth = width;
                mRequestHeight = height;
            }
            else
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (mTCamPluginJO != null)
                    {
                        mTCamPluginJO.Call("SetPreviewResolution", width, height);
                    }
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (nativePlugin == null)
                    {
                        nativePlugin = new TCamIOSPlugin();
                    }

                    nativePlugin.TCamPlugin_SetPreviewResolution_(width, height);
                }
            }

            /*
			#if UNITY_EDITOR || UNITY_STANDALONE
			mRequestWidth = width;
			mRequestHeight = height;
			#else
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					mTCamPluginJO.Call ("SetPreviewResolution", width, height);
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				TCamPlugin_SetPreviewResolution (width, height);
			}
			#endif
            */
		}

		/**
		* @brief 설정 된 Camera Preview 해상도를 획득한다. 단말마다 지원되는 해상도가 다르니 SetPreviewResolution후 GetPreviewResolution으로 설정 된 Preview 해상도 확인을 권장함.
		* @param out width: Camera Preview Width
		* @param out height: Camera Preview Height
		*/
		public static void GetPreviewResolution(out int width, out int height)
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                width = mWebCamTexture ? mWebCamTexture.width : mRequestWidth;
                height = mWebCamTexture ? mWebCamTexture.height : mRequestHeight;
            }
            else
            {
                width = height = 0;
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (mTCamPluginJO != null)
                    {
                        AndroidJavaObject jo = mTCamPluginJO.Call<AndroidJavaObject>("GetPreviewResolution");
                        if (jo.GetRawObject().ToInt32() != 0)
                        {
                            int[] resolution = AndroidJNIHelper.ConvertFromJNIArray<int[]>(jo.GetRawObject());
                            width = resolution[0];
                            height = resolution[1];
                        }
                    }
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (nativePlugin == null)
                    {
                        nativePlugin = new TCamIOSPlugin();
                    }

                    nativePlugin.TCamPlugin_GetPreviewResolution_(out width, out height);
                }
            }

            /*
			#if UNITY_EDITOR || UNITY_STANDALONE
			width = mWebCamTexture ? mWebCamTexture.width : mRequestWidth;
			height = mWebCamTexture ? mWebCamTexture.height : mRequestHeight;
			#else
			width = height = 0;
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					AndroidJavaObject jo = mTCamPluginJO.Call<AndroidJavaObject> ("GetPreviewResolution");
					if (jo.GetRawObject().ToInt32() != 0) {
						int[] resolution = AndroidJNIHelper.ConvertFromJNIArray<int[]>(jo.GetRawObject());
						width = resolution[0];
						height = resolution[1];
					}
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				TCamPlugin_GetPreviewResolution (out width, out height);
			}
			#endif
            */
		}

		/**
		* @brief Camera Preview FPS를 설정한다.
		* @param fps: Camera Preview fps
		*/
		public static void SetPreviewFPS(int fps)
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                mRequestFps = fps;
            }
            else
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (mTCamPluginJO != null)
                    {
                        mTCamPluginJO.Call("SetPreviewFPS", fps);
                    }
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (nativePlugin == null)
                    {
                        nativePlugin = new TCamIOSPlugin();
                    }

                    nativePlugin.TCamPlugin_SetPreviewFPS_(fps);
                }
            }

            /*
			#if UNITY_EDITOR || UNITY_STANDALONE
			mRequestFps = fps;
			#else
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					mTCamPluginJO.Call ("SetPreviewFPS", fps);
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				TCamPlugin_SetPreviewFPS (fps);
			}
			#endif
            */
		}

		/**
		* @brief 설정 된 Camera Preview FPS를 획득한다. 단말마다 지원되는 FPS가 다르니 SetPreviewFPS GetPreviewFPS 설정 된 FPS 확인을 권장함.
		* @param out fps: Camera Preview fps
		*/
		public static int GetPreviewFPS()
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return mRequestFps;
            }
            else
            {
                int ret = 0;
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (mTCamPluginJO != null)
                    {
                        ret = mTCamPluginJO.Call<int>("GetPreviewFPS");
                    }
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (nativePlugin == null)
                    {
                        nativePlugin = new TCamIOSPlugin();
                    }

                    ret = nativePlugin.TCamPlugin_GetPreviewFPS_();
                }
                return ret;
            }

            /*
			#if UNITY_EDITOR || UNITY_STANDALONE
			return mRequestFps;
			#else
			int ret = 0;
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					ret = mTCamPluginJO.Call<int> ("GetPreviewFPS");
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				ret = TCamPlugin_GetPreviewFPS ();
			}
			return ret;
			#endif
            */
		}

		/**
		* @brief 설정 된 Camera Capture 해상도를 설정한다. 
		* @param width: Camera Capture Width
		* @param height: Camera Capture Height
		*/
		public static void SetCaptureResolution(int width, int height)
		{
			/////////////////////////////////
			Debug.Log ("TCamPlugin: SetCaptureResolution()");
			/////////////////////////////////
			
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					mTCamPluginJO.Call ("SetCaptureResolution", width, height);

					/////////////////////////////////
					Debug.Log ("TCamPlugin: mTCamPluginJO.Call - SetCaptureResolution " + width + " " + height);
					/////////////////////////////////

				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                nativePlugin.TCamPlugin_SetCaptureResolution_(width, height);
			}
		}

		/**
		* @brief 설정 된 Camera Capture 해상도를 획득한다. 단말마다 지원되는 해상도가 다르니 SetCaptureResolution GetCaptureResolution 설정 된 Preview 해상도 확인을 권장함.
		* @param out width: Camera Capture Width
		* @param out height: Camera Capture Height
		*/
		public static void GetCaptureResolution(out int width, out int height)
		{
			width = height = 0;
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					AndroidJavaObject jo = mTCamPluginJO.Call<AndroidJavaObject> ("GetCaptureResolution");
					if (jo.GetRawObject().ToInt32() != 0) {
						int[] resolution = AndroidJNIHelper.ConvertFromJNIArray<int[]>(jo.GetRawObject());
						width = resolution[0];
						height = resolution[1];
					}
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                nativePlugin.TCamPlugin_GetCaptureResolution_(out width, out height);
				return;
			}
		}

		/**
		* @brief 설정 된 Camera Exposure Mode를 설정한다.
		* @param mode TCamParameters.ExposureMode를 참조하여 설정한다.
		*/
		public static bool SetExposureMode (int mode)
		{
			bool ret = false;
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					ret = mTCamPluginJO.Call<bool> ("SetExposureMode", (int) mode);
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                ret = nativePlugin.TCamPlugin_SetExposureMode_(mode);
			}
			return ret;
		}

		/**
		* @brief Preview내 최소 Exposure 값을 획득한다.
		* @return Preview내 최소 Exposure 값
		*/
		public static float GetMinExposure()
		{
			float ret = 0;
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					ret = mTCamPluginJO.Call<float> ("GetMinExposure");
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                ret = nativePlugin.TCamPlugin_GetMinExposure_();
			}
			return ret;
		}

		/**
		* @brief Preview내 최대 Exposure 값을 획득한다.
		* @return Preview내 최대 Exposure 값
		*/
		public static float GetMaxExposure ()
		{
			float ret = 0;
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					ret = mTCamPluginJO.Call<float> ("GetMaxExposure");
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                ret = nativePlugin.TCamPlugin_GetMaxExposure_();
			}
			return ret;
		}

		/**
		* @brief Exposure값을 세팅한다.
		* @param exposure 값
		*/
		public static float SetExposure (float exposure)
		{
			float ret = 0;
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					ret = mTCamPluginJO.Call<float> ("SetExposure", exposure);
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                ret = nativePlugin.TCamPlugin_SetExposure_(exposure);
			}
			return ret;
		}

		/**
		* @brief Camera에서 Flash를 사용하는지 확인한다,
		* @return Flash 사용 여부 확인
		*/
		public static bool IsSupportFlash()
		{
			bool ret = false;
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					ret = mTCamPluginJO.Call<bool> ("IsSupportFlash");
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				// TODO
				ret = true;
			}
			return ret;
		}

		/**
		* @brief 설정 된 Camera Flash Mode를 설정한다.
		* @param mode TCamParameters.FlashMode를 참조하여 설정한다.
		*/
		public static bool SetFlashMode (int mode)
		{
			bool ret = false;
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					ret = mTCamPluginJO.Call<bool> ("SetFlashMode", (int) mode);
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                ret = nativePlugin.TCamPlugin_SetFlashMode_(mode);
			}
			return ret;
		}

		/**
		* @brief 설정 된 Camera Focus Mode를 설정한다.
		* @param mode TCamParameters.FocusMode를 참조하여 설정한다.
		*/
		public static bool SetFocusMode (int mode)
		{
			bool ret = false;
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					ret = mTCamPluginJO.Call<bool> ("SetFocusMode", (int) mode);
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                ret = nativePlugin.TCamPlugin_SetFocusMode_(mode);
			}
			return ret;
		}

		/**
		* @brief Camera Preview를 시작한다.
		* @param width: Camera Preview Width
		* @param height: Camera Preview Height
		* @param fps: Camera Preview fps
		*/		
		public static void StartPreview(int width, int height, int fps)
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                int device = int.Parse(mRequestDevice);
                if (!mWebCamTexture)
                {
                    mWebCamTexture = new WebCamTexture(WebCamTexture.devices[device].name, mRequestWidth, mRequestHeight, mRequestFps);
                }
                if (!mWebCamTexture.isPlaying)
                {
                    mWebCamTexture.Play();
                }
            }
            else
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (mTCamPluginJO != null)
                    {
                        mTCamPluginJO.Call("StartPreview", width, height, fps);
                    }
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (nativePlugin == null)
                    {
                        nativePlugin = new TCamIOSPlugin();
                    }

                    nativePlugin.TCamPlugin_StartPreview_(width, height, fps);
                }

                GLIssuePluginEvent(TCam.EventId.START);
            }

            /*
			#if UNITY_EDITOR || UNITY_STANDALONE
			int device = int.Parse(mRequestDevice);
			if (!mWebCamTexture) {
				mWebCamTexture = new WebCamTexture (WebCamTexture.devices[device].name, mRequestWidth, mRequestHeight, mRequestFps);
			}
			if (!mWebCamTexture.isPlaying) {
				mWebCamTexture.Play ();
			}
			#else
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					mTCamPluginJO.Call ("StartPreview", width, height, fps);
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				TCamPlugin_StartPreview (width, height, fps);
			}

			GLIssuePluginEvent (TCam.EventId.START);
			#endif
            */
		}

		/**
		* @brief Camera Preview를 종료한다.
		*/	
		public static void StopPreview()
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                mWebCamTexture.Stop();
                mWebCamTexture = null;
            }
            else
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (mTCamPluginJO != null)
                    {
                        mTCamPluginJO.Call("StopPreview");
                    }
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (nativePlugin == null)
                    {
                        nativePlugin = new TCamIOSPlugin();
                    }

                    nativePlugin.TCamPlugin_StopPreview_();
                }

                if (Application.platform == RuntimePlatform.Android)
                {
                    mTCamPluginJO.Call("SetTexture", TCam.INVALID, TCam.INVALID, TCam.INVALID, TCam.INVALID, TCam.INVALID);
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (nativePlugin == null)
                    {
                        nativePlugin = new TCamIOSPlugin();
                    }

                    nativePlugin.TCamPlugin_SetTexture_(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                }

                GLIssuePluginEvent(TCam.EventId.STOP);
            }

            /*
			#if UNITY_EDITOR || UNITY_STANDALONE
			mWebCamTexture.Stop ();
			mWebCamTexture = null;
			#else
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					mTCamPluginJO.Call ("StopPreview");
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				TCamPlugin_StopPreview ();
			}

			if (Application.platform == RuntimePlatform.Android) {
				mTCamPluginJO.Call ("SetTexture", TCam.INVALID, TCam.INVALID, TCam.INVALID, TCam.INVALID, TCam.INVALID);
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				TCamPlugin_SetTexture (IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			}

			GLIssuePluginEvent (TCam.EventId.STOP);
			#endif
            */
		}

		/**
		* @brief Camera Capture를 수행한다.
		*/
		public static void Capture()
		{
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					mTCamPluginJO.Call ("Capture");
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                nativePlugin.TCamPlugin_Capture_();
			}
		}

		/**
		* @brief Webcam Texture를 획득한다. PC모드에서만 사용한다.
		*/
		public static Texture GetTexture()
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return mWebCamTexture;
            }
            else
            {
                return null;
            }

            /*
			#if UNITY_EDITOR || UNITY_STANDALONE
			return mWebCamTexture;
			#else
			return null;
			#endif
            */
		}

		/**
		* @brief Preview가 그려질 Texture를 세팅한다.
		* @param texId: texture ID
		* @param yTexId: Y Planar texture ID
		* @param uvtexId: uv Planar texture ID
		* @param utexId: u Planar texture ID
		* @param vtexId: v Planar texture ID
		*/
		public static void SetTexture(int texId, int yTexId, int uvTexId, int uTexId, int vTexId)
		{
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					mTCamPluginJO.Call ("SetTexture", texId, yTexId, uvTexId, uTexId, vTexId);
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                nativePlugin.TCamPlugin_SetTexture_(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			}
		}

        /**
        * @brief Preview가 그려질 Texture를 세팅한다.
        * @param texId: texture가 mapping 될 memory 주소(IntPtr)
        * @param yTexId: Y Planar texture가 mapping 될 memory 주소(IntPtr)
        * @param uvtexId: uv Planar texture가 mapping 될 memory 주소(IntPtr)
        * @param utexId: u Planar texture가 mapping 될 memory 주소(IntPtr)
        * @param vtexId: v Planar texture가 mapping 될 memory 주소(IntPtr)
        */
		public static void SetTexture(IntPtr tex, IntPtr yTex, IntPtr uvTex, IntPtr uTex, IntPtr vTex)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                nativePlugin.TCamPlugin_SetTexture_(tex, yTex, uvTex, uTex, vTex);
			}
		}

        /**
        * @brief Texture를 초기화 한다.
        */
		public static void ResetTexture()
		{
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					mTCamPluginJO.Call ("SetTexture", TCam.INVALID, TCam.INVALID, TCam.INVALID, TCam.INVALID, TCam.INVALID);
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                nativePlugin.TCamPlugin_SetTexture_(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			}
		}

        /**
        * @brief Sensor의 회전정보를 가져온다.
        * @return Sesnsor의 회전 값.
        */
		public static int GetSensorOrientation()
		{
			int ret = TCam.INVALID;
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					ret = mTCamPluginJO.Call<int> ("GetSensorOrientation");
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                ret = nativePlugin.TCamPlugin_GetSensorOrientation_();
			}
			return ret;
		}

        /**
        * @brief 화면의 회전정보를 가져온다.
        * @return 화면의 회전 값.
        */
		public static int GetDisplayRotation()
		{
			int ret = 0;
			if (Application.platform == RuntimePlatform.Android) {
				if (mTCamPluginJO != null) {
					ret = mTCamPluginJO.Call<int> ("GetDisplayRotation");
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                ret = nativePlugin.TCamPlugin_GetDisplayRotation_();
			}
			return ret;
		}

        /**
        * @brief 화면을 회전시킨.
        * @param screenWidth: 화면 길이
        * @param screenHeight: 화면 높이
        * @param sensorOrientation: Rotation시킬 값
        * @param displayRotation: Display rotation 상태
        */
		public static void SetScreenRotation(int screenWidth, int screenHeight, int sensorOrientation, int displayRotation)
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
            }
            else
            {
                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                nativePlugin.TCamPlugin_SetScreenRotation_(screenWidth, screenHeight, sensorOrientation, displayRotation);
            }

            /*
			#if UNITY_EDITOR || UNITY_STANDALONE
			#else
			TCamPlugin_SetScreenRotation (screenWidth, screenHeight, sensorOrientation, displayRotation);
			#endif
            */
		}

        /**
        * @brief 화면을 회전시킨.
        * @param yuv: YUV data가 저장 될 array
        * @param y: Y data가 저장 될 array
        * @param uv: UV data가 저장 될 array
        * @param u: U data가 저장 될 array
        * @param v: V data가 저장 될 array
        */
		public static void GetPreviewData(byte[] yuv, byte[] y, byte[] uv, byte[] u, byte[] v)
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
            }
            else
            {
                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                nativePlugin.TCamPlugin_GetPreviewData_(yuv, y, uv, u, v);
            }

            /*
			#if UNITY_EDITOR || UNITY_STANDALONE
			#else
			TCamPlugin_GetPreviewData (yuv, y, uv, u, v);
			#endif
            */
		}

        /**
        * @brief 현재 frameId를 세팅한다.
        * @param frameId: 현재 frameId
        */
		public static void SetCurrentFrame(int frameId)
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
            }
            else
            {
                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                nativePlugin.TCamPlugin_SetCurrentFrame_(frameId);
            }

            /*
			#if UNITY_EDITOR || UNITY_STANDALONE
			#else
			TCamPlugin_SetCurrentFrame (frameId);
			#endif
            */
		}

        /**
        * @brief 화면 Scale를 세팅한다.
        * @param scale: 설정 할 Scale값
        */
		public static void SetScale(int scale)
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
            }
            else
            {
                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                nativePlugin.TCamPlugin_SetScale_(scale);
            }

            /*
			#if UNITY_EDITOR || UNITY_STANDALONE
			#else
			TCamPlugin_SetScale (scale);
			#endif
            */
		}

        /**
        * @brief PreviewTexture ID를 가져온다 
        * @return PreviewTexture ID
        */
		public static int GetPreviewTexture()
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return 0;
            }
            else
            {
                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                return nativePlugin.TCamPlugin_GetPreviewTexture_();
            }

            /*
			#if UNITY_EDITOR || UNITY_STANDALONE
			return 0;
			#else
			return TCamPlugin_GetPreviewTexture ();
			#endif
            */
		}

        /**
        * @brief Callback을 등록한다.
        * @param previewStartCallback: Preview가 시작 될 시 호출 됨
        * @param previewUpdateCallback: Preview가 Update 될 시 호출 됨
        * @param captureUpdateCallback: Capture를 완료 되었을 시 호출 됨
        * @param requestRenderCallback: Rendering될 데이터가 준비되었을 시 호출 됨
        */
		public static void SetCallback(IntPtr previewStartCallback, IntPtr previewUpdateCallback, IntPtr captureUpdateCallback, IntPtr requestRenderCallback)
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
            }
            else
            {
                if (nativePlugin == null)
                {
                    nativePlugin = new TCamIOSPlugin();
                }

                nativePlugin.TCamPlugin_SetCallback_(previewStartCallback, previewUpdateCallback, captureUpdateCallback, requestRenderCallback);
            }

            /*
            #if UNITY_EDITOR || UNITY_STANDALONE
            #else
			TCamPlugin_SetCallback (previewStartCallback, previewUpdateCallback, captureUpdateCallback, requestRenderCallback);
            #endif
            */
        }
	}


    /**
     * brief Native plugin seperation interface 
     */
    public interface ITCamNativePlugin
    {
        IntPtr GetRenderEventFunc_();
        void TCamPlugin_Init_(String name, int renderMethod, bool nativeCopyFrameData);
        void TCamPlugin_Dispose_();
        IntPtr TCamPlugin_GetDevices_();
        void TCamPlugin_SetDevice_(String deviceId);
        void TCamPlugin_SetPreviewResolution_(int width, int height);
        void TCamPlugin_GetPreviewResolution_(out int width, out int height);
        void TCamPlugin_SetPreviewFPS_(int fps);
        int TCamPlugin_GetPreviewFPS_();
        void TCamPlugin_SetCaptureResolution_(int width, int height);
        void TCamPlugin_GetCaptureResolution_(out int width, out int height);
        bool TCamPlugin_SetExposureMode_(int mode);
        float TCamPlugin_GetMinExposure_();
        float TCamPlugin_GetMaxExposure_();
        float TCamPlugin_SetExposure_(float exposure);
        bool TCamPlugin_SetFlashMode_(int mode);
        bool TCamPlugin_SetFocusMode_(int mode);
        void TCamPlugin_StartPreview_(int width, int height, int fps);
        void TCamPlugin_StopPreview_();
        void TCamPlugin_Capture_();
        void TCamPlugin_SetTexture_(IntPtr texPtr, IntPtr yTexPtr, IntPtr uvTexPtr, IntPtr uTexPtr, IntPtr vTexPtr);
        int TCamPlugin_GetSensorOrientation_();
        int TCamPlugin_GetDisplayRotation_();
        void TCamPlugin_SetScreenRotation_(int width, int height, int sensorOrientation, int displayRotation);
        void TCamPlugin_GetPreviewData_(byte[] yuv, byte[] y, byte[] uv, byte[] u, byte[] v);
        void TCamPlugin_SetCurrentFrame_(int frameId);
        void TCamPlugin_SetScale_(int scale);
        int TCamPlugin_GetPreviewTexture_();
        void TCamPlugin_SetCallback_(IntPtr previewStartCallback, IntPtr previewUpdateCallback, IntPtr captureUpdateCallback, IntPtr requestRenderCallback);
        int TCamTest_GetRenderMethod_();
    }

    public class TCamAndroidPlugin : ITCamNativePlugin
    {
        [DllImport("TCamPlugin")]
        private static extern IntPtr GetRenderEventFunc();
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_Init(String name, int renderMethod, bool nativeCopyFrameData);
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_Dispose();
        [DllImport("TCamPlugin")]
        private static extern IntPtr TCamPlugin_GetDevices();
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_SetDevice(String deviceId);
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_SetInvalidCamera2Device(String list);
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_SetPreviewResolution(int width, int height);
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_GetPreviewResolution(out int width, out int height);
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_SetPreviewFPS(int fps);
        [DllImport("TCamPlugin")]
        private static extern int TCamPlugin_GetPreviewFPS();
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_SetCaptureResolution(int width, int height);
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_GetCaptureResolution(out int width, out int height);
        [DllImport("TCamPlugin")]
        private static extern bool TCamPlugin_SetExposureMode(int mode);
        [DllImport("TCamPlugin")]
        private static extern float TCamPlugin_GetMinExposure();
        [DllImport("TCamPlugin")]
        private static extern float TCamPlugin_GetMaxExposure();
        [DllImport("TCamPlugin")]
        private static extern float TCamPlugin_SetExposure(float exposure);
        [DllImport("TCamPlugin")]
        private static extern bool TCamPlugin_SetFlashMode(int mode);
        [DllImport("TCamPlugin")]
        private static extern bool TCamPlugin_SetFocusMode(int mode);
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_StartPreview(int width, int height, int fps);
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_StopPreview();
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_Capture();
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_SetTexture(IntPtr texPtr, IntPtr yTexPtr, IntPtr uvTexPtr, IntPtr uTexPtr, IntPtr vTexPtr);
        [DllImport("TCamPlugin")]
        private static extern int TCamPlugin_GetSensorOrientation();
        [DllImport("TCamPlugin")]
        private static extern int TCamPlugin_GetDisplayRotation();
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_SetScreenRotation(int width, int height, int sensorOrientation, int displayRotation);
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_GetPreviewData(byte[] yuv, byte[] y, byte[] uv, byte[] u, byte[] v);
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_SetCurrentFrame(int frameId);
        [DllImport("TCamPlugin")]
        private static extern void TCamPlugin_SetScale(int scale);
        [DllImport("TCamPlugin")]
        private static extern int TCamPlugin_GetPreviewTexture();
        [DllImport("TCamPlugin", CallingConvention = CallingConvention.StdCall)]
        private static extern void TCamPlugin_SetCallback(IntPtr previewStartCallback, IntPtr previewUpdateCallback, IntPtr captureUpdateCallback, IntPtr requestRenderCallback);
        [DllImport("TCamPlugin")]
        private static extern int TCamTest_GetRenderMethod();


        public IntPtr GetRenderEventFunc_()
        {
            return GetRenderEventFunc();
        }

        public void TCamPlugin_Init_(string name, int renderMethod, bool nativeCopyFrameData)
        {
            TCamPlugin_Init(name, renderMethod, nativeCopyFrameData);
        }

        public void TCamPlugin_Dispose_()
        {
            TCamPlugin_Dispose();
        }

        public IntPtr TCamPlugin_GetDevices_()
        {
            return TCamPlugin_GetDevices();
        }

        public void TCamPlugin_Capture_()
        {
            TCamPlugin_Capture();
        }

        public void TCamPlugin_GetCaptureResolution_(out int width, out int height)
        {
            TCamPlugin_GetCaptureResolution(out width, out height);
        }

        public int TCamPlugin_GetDisplayRotation_()
        {
            return TCamPlugin_GetDisplayRotation();
        }

        public float TCamPlugin_GetMaxExposure_()
        {
            return TCamPlugin_GetMaxExposure();
        }

        public float TCamPlugin_GetMinExposure_()
        {
            return TCamPlugin_GetMinExposure();
        }

        public void TCamPlugin_GetPreviewData_(byte[] yuv, byte[] y, byte[] uv, byte[] u, byte[] v)
        {
            TCamPlugin_GetPreviewData(yuv, y, uv, u, v);
        }

        public int TCamPlugin_GetPreviewFPS_()
        {
            return TCamPlugin_GetPreviewFPS();
        }

        public void TCamPlugin_GetPreviewResolution_(out int width, out int height)
        {
            TCamPlugin_GetPreviewResolution(out width, out height);
        }

        public int TCamPlugin_GetPreviewTexture_()
        {
            return TCamPlugin_GetPreviewTexture();
        }

        public int TCamPlugin_GetSensorOrientation_()
        {
            return TCamPlugin_GetSensorOrientation();
        }

        public void TCamPlugin_SetCallback_(IntPtr previewStartCallback, IntPtr previewUpdateCallback, IntPtr captureUpdateCallback, IntPtr requestRenderCallback)
        {
            TCamPlugin_SetCallback(previewStartCallback, previewUpdateCallback, captureUpdateCallback, requestRenderCallback);
        }

        public void TCamPlugin_SetCaptureResolution_(int width, int height)
        {
            TCamPlugin_SetCaptureResolution_(width, height);
        }

        public void TCamPlugin_SetCurrentFrame_(int frameId)
        {
            TCamPlugin_SetCurrentFrame(frameId);
        }

        public void TCamPlugin_SetDevice_(string deviceId)
        {
            TCamPlugin_SetDevice(deviceId);
        }

        public float TCamPlugin_SetExposure_(float exposure)
        {
            return TCamPlugin_SetExposure(exposure);
        }

        public bool TCamPlugin_SetExposureMode_(int mode)
        {
            return TCamPlugin_SetExposureMode(mode);
        }

        public bool TCamPlugin_SetFlashMode_(int mode)
        {
            return TCamPlugin_SetFlashMode(mode);
        }

        public bool TCamPlugin_SetFocusMode_(int mode)
        {
            return TCamPlugin_SetFocusMode(mode);
        }

        public void TCamPlugin_SetPreviewFPS_(int fps)
        {
            TCamPlugin_SetPreviewFPS(fps);
        }

        public void TCamPlugin_SetPreviewResolution_(int width, int height)
        {
            TCamPlugin_SetPreviewResolution(width, height);
        }

        public void TCamPlugin_SetScale_(int scale)
        {
            TCamPlugin_SetScale(scale);
        }

        public void TCamPlugin_SetScreenRotation_(int width, int height, int sensorOrientation, int displayRotation)
        {
            TCamPlugin_SetScreenRotation(width, height, sensorOrientation, displayRotation);
        }

        public void TCamPlugin_SetTexture_(IntPtr texPtr, IntPtr yTexPtr, IntPtr uvTexPtr, IntPtr uTexPtr, IntPtr vTexPtr)
        {
            TCamPlugin_SetTexture(texPtr, yTexPtr, uvTexPtr, uTexPtr, vTexPtr);
        }

        public void TCamPlugin_StartPreview_(int width, int height, int fps)
        {
            TCamPlugin_StartPreview(width, height, fps);
        }

        public void TCamPlugin_StopPreview_()
        {
            TCamPlugin_StopPreview();
        }

        public int TCamTest_GetRenderMethod_()
        {
            return TCamTest_GetRenderMethod();
        }
    }

    public class TCamIOSPlugin : ITCamNativePlugin
    {
        [DllImport("__Internal")]
        private static extern IntPtr GetRenderEventFunc();
        [DllImport("__Internal")]
        private static extern void TCamPlugin_Init(String name, int renderMethod, bool nativeCopyFrameData);
        [DllImport("__Internal")]
        private static extern void TCamPlugin_Dispose();
        [DllImport("__Internal")]
        private static extern IntPtr TCamPlugin_GetDevices();
        [DllImport("__Internal")]
        private static extern void TCamPlugin_SetDevice(String deviceId);
        [DllImport("__Internal")]
        private static extern void TCamPlugin_SetInvalidCamera2Device(String list);
        [DllImport("__Internal")]
        private static extern void TCamPlugin_SetPreviewResolution(int width, int height);
        [DllImport("__Internal")]
        private static extern void TCamPlugin_GetPreviewResolution(out int width, out int height);
        [DllImport("__Internal")]
        private static extern void TCamPlugin_SetPreviewFPS(int fps);
        [DllImport("__Internal")]
        private static extern int TCamPlugin_GetPreviewFPS();
        [DllImport("__Internal")]
        private static extern void TCamPlugin_SetCaptureResolution(int width, int height);
        [DllImport("__Internal")]
        private static extern void TCamPlugin_GetCaptureResolution(out int width, out int height);
        [DllImport("__Internal")]
        private static extern bool TCamPlugin_SetExposureMode(int mode);
        [DllImport("__Internal")]
        private static extern float TCamPlugin_GetMinExposure();
        [DllImport("__Internal")]
        private static extern float TCamPlugin_GetMaxExposure();
        [DllImport("__Internal")]
        private static extern float TCamPlugin_SetExposure(float exposure);
        [DllImport("__Internal")]
        private static extern bool TCamPlugin_SetFlashMode(int mode);
        [DllImport("__Internal")]
        private static extern bool TCamPlugin_SetFocusMode(int mode);
        [DllImport("__Internal")]
        private static extern void TCamPlugin_StartPreview(int width, int height, int fps);
        [DllImport("__Internal")]
        private static extern void TCamPlugin_StopPreview();
        [DllImport("__Internal")]
        private static extern void TCamPlugin_Capture();
        [DllImport("__Internal")]
        private static extern void TCamPlugin_SetTexture(IntPtr texPtr, IntPtr yTexPtr, IntPtr uvTexPtr, IntPtr uTexPtr, IntPtr vTexPtr);
        [DllImport("__Internal")]
        private static extern int TCamPlugin_GetSensorOrientation();
        [DllImport("__Internal")]
        private static extern int TCamPlugin_GetDisplayRotation();
        [DllImport("__Internal")]
        private static extern void TCamPlugin_SetScreenRotation(int width, int height, int sensorOrientation, int displayRotation);
        [DllImport("__Internal")]
        private static extern void TCamPlugin_GetPreviewData(byte[] yuv, byte[] y, byte[] uv, byte[] u, byte[] v);
        [DllImport("__Internal")]
        private static extern void TCamPlugin_SetCurrentFrame(int frameId);
        [DllImport("__Internal")]
        private static extern void TCamPlugin_SetScale(int scale);
        [DllImport("__Internal")]
        private static extern int TCamPlugin_GetPreviewTexture();
        [DllImport("__Internal", CallingConvention = CallingConvention.StdCall)]
        private static extern void TCamPlugin_SetCallback(IntPtr previewStartCallback, IntPtr previewUpdateCallback, IntPtr captureUpdateCallback, IntPtr requestRenderCallback);
        [DllImport("__Internal")]
        private static extern int TCamTest_GetRenderMethod();


        public IntPtr GetRenderEventFunc_()
        {
            return GetRenderEventFunc();
        }

        public void TCamPlugin_Init_(string name, int renderMethod, bool nativeCopyFrameData)
        {
            TCamPlugin_Init(name, renderMethod, nativeCopyFrameData);
        }

        public void TCamPlugin_Dispose_()
        {
            TCamPlugin_Dispose();
        }

        public IntPtr TCamPlugin_GetDevices_()
        {
            return TCamPlugin_GetDevices();
        }

        public void TCamPlugin_Capture_()
        {
            TCamPlugin_Capture();
        }

        public void TCamPlugin_GetCaptureResolution_(out int width, out int height)
        {
            TCamPlugin_GetCaptureResolution(out width, out height);
        }

        public int TCamPlugin_GetDisplayRotation_()
        {
            return TCamPlugin_GetDisplayRotation();
        }

        public float TCamPlugin_GetMaxExposure_()
        {
            return TCamPlugin_GetMaxExposure();
        }

        public float TCamPlugin_GetMinExposure_()
        {
            return TCamPlugin_GetMinExposure();
        }

        public void TCamPlugin_GetPreviewData_(byte[] yuv, byte[] y, byte[] uv, byte[] u, byte[] v)
        {
            TCamPlugin_GetPreviewData(yuv, y, uv, u, v);
        }

        public int TCamPlugin_GetPreviewFPS_()
        {
            return TCamPlugin_GetPreviewFPS();
        }

        public void TCamPlugin_GetPreviewResolution_(out int width, out int height)
        {
            TCamPlugin_GetPreviewResolution(out width, out height);
        }

        public int TCamPlugin_GetPreviewTexture_()
        {
            return TCamPlugin_GetPreviewTexture();
        }

        public int TCamPlugin_GetSensorOrientation_()
        {
            return TCamPlugin_GetSensorOrientation();
        }

        public void TCamPlugin_SetCallback_(IntPtr previewStartCallback, IntPtr previewUpdateCallback, IntPtr captureUpdateCallback, IntPtr requestRenderCallback)
        {
            TCamPlugin_SetCallback(previewStartCallback, previewUpdateCallback, captureUpdateCallback, requestRenderCallback);
        }

        public void TCamPlugin_SetCaptureResolution_(int width, int height)
        {
            TCamPlugin_SetCaptureResolution_(width, height);
        }

        public void TCamPlugin_SetCurrentFrame_(int frameId)
        {
            TCamPlugin_SetCurrentFrame(frameId);
        }

        public void TCamPlugin_SetDevice_(string deviceId)
        {
            TCamPlugin_SetDevice(deviceId);
        }

        public float TCamPlugin_SetExposure_(float exposure)
        {
            return TCamPlugin_SetExposure(exposure);
        }

        public bool TCamPlugin_SetExposureMode_(int mode)
        {
            return TCamPlugin_SetExposureMode(mode);
        }

        public bool TCamPlugin_SetFlashMode_(int mode)
        {
            return TCamPlugin_SetFlashMode(mode);
        }

        public bool TCamPlugin_SetFocusMode_(int mode)
        {
            return TCamPlugin_SetFocusMode(mode);
        }

        public void TCamPlugin_SetPreviewFPS_(int fps)
        {
            TCamPlugin_SetPreviewFPS(fps);
        }

        public void TCamPlugin_SetPreviewResolution_(int width, int height)
        {
            TCamPlugin_SetPreviewResolution(width, height);
        }

        public void TCamPlugin_SetScale_(int scale)
        {
            TCamPlugin_SetScale(scale);
        }

        public void TCamPlugin_SetScreenRotation_(int width, int height, int sensorOrientation, int displayRotation)
        {
            TCamPlugin_SetScreenRotation(width, height, sensorOrientation, displayRotation);
        }

        public void TCamPlugin_SetTexture_(IntPtr texPtr, IntPtr yTexPtr, IntPtr uvTexPtr, IntPtr uTexPtr, IntPtr vTexPtr)
        {
            TCamPlugin_SetTexture(texPtr, yTexPtr, uvTexPtr, uTexPtr, vTexPtr);
        }

        public void TCamPlugin_StartPreview_(int width, int height, int fps)
        {
            TCamPlugin_StartPreview(width, height, fps);
        }

        public void TCamPlugin_StopPreview_()
        {
            TCamPlugin_StopPreview();
        }

        public int TCamTest_GetRenderMethod_()
        {
            return TCamTest_GetRenderMethod();
        }
    }
}
