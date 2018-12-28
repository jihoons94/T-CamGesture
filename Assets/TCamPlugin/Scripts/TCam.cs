using UnityEngine;
using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine.UI;

namespace TCamera {

	/// <summary>
	/// TCam API
	/// </summary>
	public class TCam : MonoBehaviour
	{
        static public int State = 0;
/// <summary>
/// ///////////////////////////////////////////////////////
/// </summary>
		public const string TAG = "TCam";

		public const bool DEBUG = true;
		public const int INVALID = -1;

        public string[] mInvalidCamera2List;

		// jiyoun_choi@sk.com 181014: added for exposure control
//		private static Boolean enableExposureControl = false;
		public Toggle mExposureToggle;
		private static Boolean isExposureControlSetted = false;
		// _END jiyoun_choi@sk.com 181014: added for exposure control

		static TCam mInstance;
		/// <summary>
		/// TCam 인스턴스
		/// </summary>
		public static TCam Instance
		{
			get {
				if (!mInstance) {
                    Debug.Log ("UNITY: TCam: get - Instance");
					mInstance = GameObject.FindObjectOfType (typeof(TCam)) as TCam;
				}

				return mInstance;
			}
		}
		static TCamDevice[] mDevices;  

		/// <summary>
		/// 사용가능한 카메라 디바이스 정보
		/// </summary>
		public static TCamDevice[] Devices
		{
			get {
                Debug.Log ("UNITY: TCam: get - Devices");
				if (mDevices == null) {
					string devicesstr = TCamPlugin.GetDevices ();
                    Debug.Log(devicesstr);
                    if (String.IsNullOrEmpty (devicesstr)) {
						return null;
					}

					String[] deviceArray = devicesstr.Split (';');
					mDevices = new TCamDevice[deviceArray.Length];
					for (int i = 0; i < deviceArray.Length; i++) {
                        
						String[] deviceinfo = deviceArray [i].Split (':');
                            TCamDevice device = new TCamDevice();
                            device.m_Name = deviceinfo[0];
                            device.m_IsFrontFacing = deviceinfo[1].Equals("1") ? true : false;
                            mDevices[i] = device;

					}
				}

                return mDevices;
			}
		}

		TCamera.TCamParameters.Scale scale = TCamera.TCamParameters.Scale.FULL;
		TCamera.TCamParameters.PreviewFormat mPreviewFormat = TCamera.TCamParameters.PreviewFormat.NV21;

		/// <summary>
		/// 프리뷰용 Unity Camera Object
		/// </summary>
		public Camera mPreviewCamera;
        public Camera mARCamera;

        /// <summary>
        /// 프리뷰용 Unity Quad Object
        /// </summary>
        public Transform mPreviewScreen;

		/// <summary>
		/// 디버그용 Unity Quad Object
		/// </summary>
		public Transform mMiniPreviewScreen;

		TCamDevice mPreviewDevice;
		int mRequestPreviewWidth = 1280, mRequestPreviewHeight = 720, mRequestPreviewFps = 30;
		int mPreviewWidth, mPreviewHeight, mPreviewFps;
		bool mPreview = false;

        int mCurrentFrameId = INVALID;

		/*
		 * Android Phone
		 *     Galaxy S7
		 *         rear : 90, front : 270
		 * Android Tablet
		 *     Galaxy Note 10.1
		 * 	       rear : 0, front : 0
		 * iOS
		 *     rear : 0, front : 180 (unofficial)
		 */
		int mSensorOrientation = INVALID;

		Camera mMainCamera;

		bool isApplicationPaused = false; 

		bool mPlayerPC;

		void Start()
		{
            Debug.Log ("UNITY: TCam: Start()");


        }

		void Update()
		{
            //Debug.Log ("UNITY: TCam: Update()");
			_UpdateFrame ();
		}

        void _UpdateFrame()
        {
            //Debug.Log ("UNITY: TCam: _UpdateFrame()");
            if (!mPreview)
            {
                return;
            }

            mRenderer.Update();

            if (mPlayerPC)
            {
                _HandlePreviewUpdate(0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
        }

        /// <summary>
        /// 프리뷰를 시작한다. Play() 호출전에 카메라 디바이스, 해상도 등이 먼저 설정되어 있어야 한다.
        /// </summary>
        public void Play()
        {
            Debug.Log ("UNITY: TCam: Play()");
            _StartPreview(mRequestPreviewWidth, mRequestPreviewHeight, mRequestPreviewFps);
        }

        /// <summary>
        /// 프리뷰를 시작한다.
        /// </summary>
        /// <param name="front">front facing 카메라 디바이스를 사용할 지 여부</param>
        /// <param name="width">프리뷰 width</param>
        /// <param name="height">프리뷰 height</param>
        /// <param name="fps">프리뷰 fps</param>
        public void Play(bool front, int width, int height, int fps)
        {
            Debug.Log ("UNITY: TCam: Play2()");
            SetDevice(front);
            _StartPreview(width, height, fps);
        }

        /// <summary>
        /// 프리뷰를 시작한다.
        /// </summary>
        /// <param name="deviceId">카메라 디바이스 아이디, TCamDevice.name 값으로 설정한다.</param>
        /// <param name="width">프리뷰 width</param>
        /// <param name="height">프리뷰 height</param>
        /// <param name="fps">프리뷰 fps</param>
        public void Play(string deviceId, int width, int height, int fps)
        {
            Debug.Log ("UNITY: TCam: Play3()");
            SetDevice(deviceId);
            _StartPreview(width, height, fps);
        }

        /// <summary>
        /// 프리뷰를 종료한다.
        /// </summary>
        public void Stop()
        {
            Debug.Log ("UNITY: TCam: Stop()");
            _StopPreview();
        }

        /// <summary>
        /// 프리뷰를 캡처한다.
        /// 캡처가 완료되면 OnCaptureUpdate delegate가 호출된다.
        /// </summary>
        public void Capture()
        {
            Debug.Log ("UNITY: TCam: Capture()");
            TCamPlugin.Capture();
        }

        /// <summary>
        /// 프리뷰가 동작중인지 여부를 얻는다.
        /// </summary>
        /// <returns>프리뷰 동작 여부</returns>
        public bool IsPlaying()
        {
            return mPreview;
        }


        /// <summary>
        /// NATIVE_GL_SHADER_POST_RENDER 렌더링을 사용할 때, Unity Camera Object의 OnPostRender 에서 호출된다.
        /// </summary>
        public void PostRender()
		{
            //Debug.Log ("UNITY: TCam: PostRender()");
			if(mRenderer !=null)
				mRenderer.PostRender ();
		}

		public void OnApplicationPause(bool pauseStatus)
		{
            Debug.Log ("UNITY: TCam: OnApplicationPause() state = " + pauseStatus);

			isApplicationPaused = pauseStatus;
			if (isApplicationPaused == true) {
				TCamPlugin.OnPause ();
			} else {
				TCamPlugin.OnResume ();
			}
		}

		void OnApplicationQuit()
		{
            Debug.Log ("UNITY: TCam: OnApplicationQuit()");
			TCamPlugin.OnDestroy ();
		}

		/// <summary>
		/// TCam을 초기화한다.
		/// </summary>
		/// <param name="nativeCameraHardware">Android Camera Hardware 설정, iOS에서는 무시된다.</param>
		/// <param name="renderMethod">렌더링 방식 설정</param>
		/// <param name="nativeCopyFrameData">Native Plugin에서 프리뷰 프레임 데이터를 관리하는 렌더링 방식을 사용하는 경우 프레임 데이터를 복사해서 관리할 지 여부 설정</param>
        /// 
		public void Init
		(
			NativeCameraHardware nativeCameraHardware = NativeCameraHardware.AUTO,
			RenderMethod renderMethod = RenderMethod.NATIVE_GL_SHADER,
			bool nativeCopyFrameData = true
		)
		{
            Debug.Log ("UNITY: TCam: Init()");

			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				if (renderMethod == TCam.RenderMethod.NATIVE_UPDATE_SURFACE_TEXTURE) {
					renderMethod = TCam.RenderMethod.NATIVE_GL_SHADER;
				}
			}
			mRenderMethod = renderMethod;
            TCamPlugin.SetInvalidCamera2Device(mInvalidCamera2List);

			mNativeCameraHardware = (NativeCameraHardware) TCamPlugin.Init (name, (int)nativeCameraHardware, (int)mRenderMethod, nativeCopyFrameData);

			if (mPreviewScreen && mPreviewScreen.rotation.x != 0 || mPreviewScreen.rotation.y != 0 || mPreviewScreen.rotation.z != 0) {
				mPreviewScreen.rotation = Quaternion.Euler (0, 0, 0);
			}

			mRenderer = TCamRenderer.Get (mRenderMethod, mPreviewCamera, mPreviewScreen);

            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor )
            {
                mPlayerPC = true;
            }


            if (mPlayerPC) {
				mPreviewFormat = TCamera.TCamParameters.PreviewFormat.RGBA;
			} else if (Application.platform == RuntimePlatform.Android) {
				if (mRenderMethod == RenderMethod.NATIVE_UPDATE_SURFACE_TEXTURE) {
					mPreviewFormat = TCamera.TCamParameters.PreviewFormat.RGBA;
				} else {
					mPreviewFormat = TCamera.TCamParameters.PreviewFormat.NV21;
				}
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				mPreviewFormat = TCamera.TCamParameters.PreviewFormat.NV12;
			}
		}

		/// <summary>
		/// TCam에서 사용한 리소스를 정리한다.
		/// </summary>
		public void Dispose()
		{
            Debug.Log ("UNITY: TCam: Dispose()");

			TCamPlugin.Dispose ();
			GameObject.Destroy (mInstance);
			mInstance = null;
		}

        public void EnableMiniPreview(bool enable)
        {
            Debug.Log ("UNITY: TCam: Dispose()");
            mRenderer.SetMiniPreview(enable ? mMiniPreviewScreen : null);
        }

		/// <summary>
		/// 카메라 디바이스를 설정한다.
		/// </summary>
		/// <param name="front">front facing 카메라 디바이스 여부 설정</param>
		public void SetDevice(bool front)
		{
            Debug.Log ("UNITY: TCam: SetDevice(front)");

			_SetDevice (front);

			if (mPreviewDevice == null) {
				return;
			}

			TCamPlugin.SetDevice (mPreviewDevice.name);
		}

		/// <summary>
		/// 카메라 디바이스를 설정한다.
		/// </summary>
		/// <param name="deviceId">카메라 디바이스 아이디, TCamDevice.name 값으로 설정한다.</param>
		public void SetDevice(string deviceId)
		{
            Debug.Log ("UNITY: TCam: SetDevice(deviceId)");

			_SetDevice (deviceId);

			if (mPreviewDevice == null) {
				return;
			}

			TCamPlugin.SetDevice (mPreviewDevice.name);
		}

        void _SetDevice(string deviceId)
        {
            Debug.Log ("UNITY: TCam: _SetDevice() deviceID = " + deviceId);

            if (Devices != null && Devices.Length > 0)
            {
                if (Devices.Length == 1)
                {
                    mPreviewDevice = Devices[0];
                }
                else
                {
                    for (int i = 0; i < Devices.Length; i++)
                    {
                        if (Devices[i].name == deviceId)
                        {
                            mPreviewDevice = Devices[i];
                            break;
                        }
                    }
                }
            }
        }

        void _SetDevice(bool front)
        {
            Debug.Log ("UNITY: TCam: _SetDevice() front = " + front);

            if (Devices != null && Devices.Length > 0)
            {
                if (Devices.Length == 1)
                {
                    Devices[0].m_IsFrontFacing = front;
                    mPreviewDevice = Devices[0];
                }
                else
                {
                    for (int i = 0; i < Devices.Length; i++)
                    {
                        if (Devices[i].isFrontFacing == front)
                        {
                            mPreviewDevice = Devices[i];
                            break;
                        }
                    }
                }
            }
        }

        void _StartPreview(int width, int height, int fps)
        {
            Debug.Log ("UNITY: TCam: _StartPreview()");

            //if (mPreviewDevice == null)
            //{
            //    return;
            //}

            //mPreviewStartHandler = new _PreviewStartCallback(_HandlePreviewStart);
            //mPreviewUpdateHandler = new _PreviewUpdateCallback(_HandlePreviewUpdate);
            //mCaptureUpdateHandler = new _CaptureUpdateCallback(_HandleCaptureUpdate);
            //mRequestRenderHandler = new _RequestRenderCallback(_HandleRequestRender);
            //TCamPlugin.SetCallback(
            //    Marshal.GetFunctionPointerForDelegate(mPreviewStartHandler),
            //    Marshal.GetFunctionPointerForDelegate(mPreviewUpdateHandler),
            //    Marshal.GetFunctionPointerForDelegate(mCaptureUpdateHandler),
            //    Marshal.GetFunctionPointerForDelegate(mRequestRenderHandler));

            //TCamPlugin.StartPreview(width, height, fps);

            //mRenderer.StartPreview();

            //if (mPlayerPC)
            //{
            //    int w, h;
            //    TCamPlugin.GetPreviewResolution(out w, out h);
            //    _HandlePreviewStart(w, h, mRequestPreviewFps);
            //}

            //mPreview = true;
        }

        void _StopPreview()
        {
            Debug.Log ("UNITY: TCam: _StopPreview()");

            mPreview = false;
            mPreviewDevice = null;
            mRequestPreviewWidth = mRequestPreviewHeight = mRequestPreviewFps = 0;
            mPreviewWidth = mPreviewHeight = mPreviewFps = 0;
            mSensorOrientation = INVALID;

            mPreviewStartHandler = null;
            mPreviewUpdateHandler = null;
            mCaptureUpdateHandler = null;
            mRequestRenderHandler = null;

            SetCurrentFrame(INVALID);

            mRenderer.StopPreview();

            TCamPlugin.StopPreview();
        }

		#region _donotneed_edit

		#region native rendering option
		/// <summary>
		/// 프리뷰에 사용할 Native Camera Hardware 설정
		/// TCam 초기화시 Init() 에서 인자로 설정한다.
		/// 
		/// Android Only, iOS에서는 무시된다.
		/// </summary>
		public enum NativeCameraHardware
		{
			///<summary>
			///Android 버전에 따라 Legacy 또는 Camera2를 사용한다.
			///</summary>
			AUTO = 1,

			///<summary>
			///android.hardware.camera 패키지를 사용한다.
			///</summary>
			LEGACY,

			///<summary>
			///android.hardware.camera2 패키지를 사용한다. (Android 5.0 이상)
			///</summary>
			CAMERA2
		}

		[HideInInspector]
		public NativeCameraHardware mNativeCameraHardware;

		/// <summary>
		/// 프리뷰 렌더링 방식 설정
		/// TCam 초기화시 Init() 에서 인자로 설정한다.
		/// </summary>
		public enum RenderMethod
		{
			/// <summary>
			/// Native Plugin으로부터 프리뷰 프레임 데이터를 전달받아 Y, U, V 데이터를 복사해서 프레임 데이터를 관리한다.
			/// Unity 에서 생성한 프리뷰 텍스처에 Y, U, V 데이터를 Load해서 프리뷰를 표시한다.
			/// </summary>
			UNITY = 1,

			/// <summary>
			/// Native Plugin에서 프리뷰 프레임 데이터를 관리한다.
			/// Unity에서 생성된 프리뷰 텍스처에 GL로 데이터를 Load해서 프리뷰를 표시한다.
			/// </summary>
			NATIVE_UPDATE_TEXTURE,

			/// <summary>
			/// 프리뷰 프레임 데이터를 사용하지 않고 Surface Texture를 사용하여 프리뷰 텍스처를 업데이트한다.
			/// Native Plugin에서 프리뷰 텍스처를 생성하고 Unity에서는 External 텍스처를 생성하여 프리뷰를 표시한다.
			/// 
			/// 다른 RenderMethod에서는 YUV 포맷으로 되어 있는 프리뷰 프레임 데이터를 콜백으로 Unity에 전달 후 프리뷰 화면을 표시하는 반면,
			/// NATIVE_UPDATE_SURFACE_TEXTURE 에서는 업데이트된 프리뷰 텍스처로부터 RGBA 포맷으로 픽셀값을 읽어들인 후 콜백으로 Unity에 전달한다.
			/// 
			/// Android Only, iOS에서는 NATIVE_GL_SHADER로 동작한다.
			/// </summary>
			NATIVE_UPDATE_SURFACE_TEXTURE,

			/// <summary>
			/// Native Plugin에서 프리뷰 프레임 데이터를 관리한다.
			/// Native Plugin에서 생성한 텍스처와 모델, Shader를 사용하여 프레임 버퍼를 직접 업데이트하여 프리뷰를 표시한다.
			/// </summary>
			NATIVE_GL_SHADER,

			/// <summary>
			/// NATIVE_GL_SHADER와 렌더링 방식은 동일하나 렌더링 이벤트를 호출하는 시점에 차이가 있다.
			/// NATIVE_GL_SHADER 에서는 Update시에 렌더링 이벤트를 호출하는 반면,
			/// NATIVE_GL_SHADER_POST_RENDER 에서는 Unity Camera 컴포넌트의 OnPostRender시에 렌더링 이벤트를 호출한다.
			/// </summary>
			NATIVE_GL_SHADER_POST_RENDER,
		}

		[HideInInspector]
		public RenderMethod mRenderMethod;

		TCamRenderer mRenderer;

		/// <summary>
		/// GL.IssuePluginEvent 이벤트 아이디
		/// </summary>
		public enum EventId {
			/// <summary>
			/// GL 초기화 이벤트
			/// </summary>
			INIT = 8226 /*tcam*/,

			/// <summary>
			/// GL 종료 이벤트
			/// </summary>
			DISPOSE,

			/// <summary>
			/// Preview 시작 이벤트
			/// </summary>
			START,

			/// <summary>
			/// Previwe 종료 이벤트
			/// </summary>
			STOP,

			/// <summary>
			/// Preview 렌더링 이벤트
			/// </summary>
			RENDER,
		}
		#endregion

		#region call back and event handler
		/// <summary>
		/// 프리뷰 시작을 알려주는 콜백
		/// </summary>
		/// <param name="width">프리뷰 width</param>
		/// <param name="height">프리뷰 height</param>
		/// <param name="fps">프리뷰 fps</param>
		public delegate void PreviewStartCallback (int width, int height, int fps);
		public event PreviewStartCallback OnPreviewStart;

		/// <summary>
		/// 프리뷰 업데이트를 알려주는 콜백
		/// </summary>
		/// <param name="frameId">현재 프레임 아이디 (프리뷰 시작부터 1씩 증가)</param>
		/// <param name="previewFormat">프리뷰 포맷</param>
		/// <param name="frameData">프레임 데이터 포인터</param>
		/// <param name="width">프리뷰 width</param>
		/// <param name="height">프리뷰 height</param>
		public delegate void PreviewUpdateCallback (int frameId, TCamera.TCamParameters.PreviewFormat previewFormat, IntPtr frameData, int width, int height);
		public event PreviewUpdateCallback OnPreviewUpdate;

		/// <summary>
		/// 캡처 업데이트를 알려주는 콜백
		/// </summary>
		/// <param name="data">캡처 데이터 포인터</param>
		/// <param name="size">캡처 데이터 크기</param>
		/// <param name="width">캡처 이미지 width</param>
		/// <param name="height">캡처 이미지 height</param>
		/// <param name="rotation">캡처 이미지 rotation</param>
		/// <param name="hflip">캡처 이미지 horizontal flip 여부</param>
		/// <param name="vflip">캡처 이미지 vertical flip 여부</param>
		public delegate void CaptureUpdateCallback (IntPtr data, int size, int width, int height, int rotation, bool hflip, bool vflip);
		public event CaptureUpdateCallback OnCaptureUpdate;


		delegate void _PreviewStartCallback(int width, int height, int fps);
		_PreviewStartCallback mPreviewStartHandler = null;

		delegate void _PreviewUpdateCallback(int frameId, IntPtr y, IntPtr uv, IntPtr u, IntPtr v);
		_PreviewUpdateCallback mPreviewUpdateHandler = null;

		delegate void _CaptureUpdateCallback(IntPtr data, int size, int width, int height, int rotation, bool hflip, bool vflip);
		_CaptureUpdateCallback mCaptureUpdateHandler = null;

		delegate void _RequestRenderCallback(int eventId);
		_RequestRenderCallback mRequestRenderHandler = null;

		[MonoPInvokeCallback(typeof(_PreviewStartCallback))]
		static void _HandlePreviewStart(int width, int height, int fps)
		{
            Debug.Log ("UNITY: TCam: _HandlePreviewStart()");

            //#if UNITY_EDITOR || UNITY_STANDALONE
            //width = 1280; height = 720;
            //#endif

            //if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            //{
            //    width = 1280; height = 720;
            //}

            TCam tcam = TCam.Instance;

			tcam.GetSensorOrientation();

			tcam.mPreviewWidth = width;
			tcam.mPreviewHeight = height;
			tcam.mPreviewFps = fps;

			tcam.mRenderer.HandlePreviewStart(tcam.mPreviewDevice.isFrontFacing, width, height);

			if (tcam.OnPreviewStart != null)
			{
				tcam.OnPreviewStart(width, height, fps);
			}
		}

		public void ExposureToggleValueChanged(Toggle change)
		{

			if (mExposureToggle.isOn) {
				TCamScript.Instance.setExposureValue ( true );
			} else {
				TCamScript.Instance.setExposureValue ( false );
			}
		}


		[MonoPInvokeCallback(typeof(_PreviewUpdateCallback))]
		static void _HandlePreviewUpdate(int frameId, IntPtr frameY, IntPtr frameUV, IntPtr frameU, IntPtr frameV)
		{
            //Debug.Log ("UNITY: TCam: _HandlePreviewUpdate()");

			TCam tcam = TCam.Instance;

			if (!tcam.mPreview)
			{
				return;
			}

			// jiyoun_choi@sk.com 181014: added for exposure control
//			if (enableExposureControl == true) {
//				if (isExposureControlSetted == false) {
//					isExposureControlSetted = true;
//
//					TCamScript.Instance.setExposureValue ();
//				}
//			}
			// _END jiyoun_choi@sk.com 181014: added for exposure control

			IntPtr data = tcam.mRenderer.HandlePreviewUpdate(frameId, frameY);
			if (tcam.OnPreviewUpdate != null)
			{
				tcam.OnPreviewUpdate(frameId, tcam.mPreviewFormat, data, tcam.mPreviewWidth, tcam.mPreviewHeight);
			}
		}

		[MonoPInvokeCallback(typeof(_CaptureUpdateCallback))]
		static void _HandleCaptureUpdate(IntPtr data, int size, int width, int height, int rotation, bool hflip, bool vflip)
		{			
            Debug.Log ("UNITY: TCam: _HandleCaptureUpdate()");

			TCam tcam = TCam.Instance;

			if (!tcam.mPreview)
			{
				return;
			}

			if (tcam.OnCaptureUpdate != null)
			{
				tcam.OnCaptureUpdate(data, size, width, height, rotation, hflip, vflip);
			}
		}

		[MonoPInvokeCallback(typeof(_RequestRenderCallback))]
		static void _HandleRequestRender(int eventId)
		{
            Debug.Log ("UNITY: TCam: _HandleRequestRender()");

			TCamDispatcher.Instance().Enqueue(() =>
				{
					TCamPlugin.GLIssuePluginEvent((TCam.EventId)eventId);
				});
		}
		#endregion

		#region TCam Getter and Setter

        /// <summary>
        /// 현재 사용중인 Android Camera Hardware를 얻는다.
        /// NativeCameraHardware.AUTO로 초기화한 경우 Camera2를 사용하는지 Legacy를 사용하는지를 알기 위해 사용한다.
        /// </summary>
        /// <returns>Android Camera Hardware</returns>
        public NativeCameraHardware GetNativeCameraHardware()
		{
            Debug.Log ("UNITY: TCam: GetNativeCameraHardware()");
			return mNativeCameraHardware;
		}

		/// <summary>
		/// 프리뷰 해상도를 설정한다.
		/// </summary>
		/// <param name="width">프리뷰 width</param>
		/// <param name="height">프리뷰 height</param>
		public void SetPreviewResolution(int width, int height)
		{
			mRequestPreviewWidth = width;
			mRequestPreviewHeight = height;

			if (mPreviewDevice == null) {
				return;
			}

			TCamPlugin.SetPreviewResolution (width, height);
		}

		/// <summary>
		/// 실제로 설정된 프리뷰 해상도를 얻는다.
		/// </summary>
		/// <param name="width">프리뷰 width</param>
		/// <param name="height">프리뷰 height</param>
		public void GetPreviewResolution(out int width, out int height)
		{
			width = height = 0;

			if (mPreviewDevice == null) {
				return;
			}

			TCamPlugin.GetPreviewResolution (out width, out height);
		}

		/// <summary>
		/// 프리뷰 FPS를 설정한다.
		/// </summary>
		/// <param name="fps">프리뷰 fps</param>
		public void SetPreviewFPS(int fps)
		{
			mRequestPreviewFps = fps;

			if (mPreviewDevice == null) {
				return;
			}

			TCamPlugin.SetPreviewFPS (fps);
		}

		/// <summary>
		/// 프리뷰 FPS를 얻는다.
		/// </summary>
		/// <returns>실제로 설정된 프리뷰 FPS</returns>
		public int GetPreviewFPS()
		{
			if (mPreviewDevice == null) {
				return 0;
			}

			return TCamPlugin.GetPreviewFPS ();
		}

		/// <summary>
		/// 캡처 이미지 해상도를 설정한다.
		/// </summary>
		/// <param name="width">캡처 이미지 width</param>
		/// <param name="height">캡처 이미지 height</param>
		public void SetCaptureResolution(int width, int height)
		{
			if (mPreviewDevice == null) {
				return;
			}

			TCamPlugin.SetCaptureResolution (width, height);
		}

		/// <summary>
		/// 실제로 설정된 캡처 이미지 해상도를 얻는다.
		/// </summary>
		/// <param name="width">캡처 이미지 width</param>
		/// <param name="height">캡처 이미지 height</param>
		public void GetCaptureResolution(out int width, out int height)
		{
			width = height = 0;

			if (mPreviewDevice == null) {
				return;
			}

			TCamPlugin.GetCaptureResolution (out width, out height);
		}

		/// <summary>
		/// exposure 모드를 설정한다.
		/// </summary>
		/// <returns>exposure 모드 설정 성공 여부</returns>
		/// <param name="mode">exposure 모드</param>
		public bool SetExposureMode (TCamera.TCamParameters.ExposureMode mode)
		{
			if (mPreviewDevice == null) {
				return false;
			}

			return TCamPlugin.SetExposureMode ((int) mode);
		}

		/// <summary>
		/// 최소 exposure 값을 얻는다.
		/// </summary>
		/// <returns>최소 exposure 값</returns>
		public float GetMinExposure()
		{
			if (mPreviewDevice == null) {
				return 0;
			}

			return TCamPlugin.GetMinExposure ();
		}

		/// <summary>
		/// 최대 exposure 값을 얻는다.
		/// </summary>
		/// <returns>최대 exposure 값</returns>
		public float GetMaxExposure ()
		{
			if (mPreviewDevice == null) {
				return 0;
			}

			return TCamPlugin.GetMaxExposure ();
		}

		/// <summary>
		/// exposure 값을 설정한다.
		/// </summary>
		/// <returns>실제 설정된 exposure 값</returns>
		/// <param name="exposure">exposure 값</param>
		public float SetExposure (float exposure)
		{
			if (mPreviewDevice == null) {
				return 0;
			}

			return TCamPlugin.SetExposure (exposure);
		}

		/// <summary>
		/// flash 지원 여부를 얻는다.
		/// </summary>
		/// <returns>flash 지원 여부</returns>
		public bool IsSupportFlash()
		{
			if (mPreviewDevice == null) {
				return false;
			}

			return TCamPlugin.IsSupportFlash ();
		}

		/// <summary>
		/// flash 모드를 설정한다.
		/// </summary>
		/// <returns>flash 모드 설정 성공 여부</returns>
		/// <param name="mode">flash 모드</param>
		public bool SetFlashMode (TCamera.TCamParameters.FlashMode mode)
		{
			if (mPreviewDevice == null) {
				return false;
			}

			return TCamPlugin.SetFlashMode ((int) mode);
		}

		/// <summary>
		/// focus 모드를 설정한다.
		/// </summary>
		/// <returns>focus 모드 설정 성공 여부</returns>
		/// <param name="mode">focus 모드</param>
		public bool SetFocusMode (TCamera.TCamParameters.FocusMode mode)
		{
			if (mPreviewDevice == null) {
				return false;
			}

			return TCamPlugin.SetFocusMode ((int) mode);
		}

		/// <summary>
		/// 카메라 디바이스, 해상도, fps를 설정한다.
		/// </summary>
		/// <param name="front">front facing 카메라 디바이스를 사용할 지 여부</param>
		/// <param name="width">프리뷰 width</param>
		/// <param name="height">프리뷰 height</param>
		/// <param name="fps">프리뷰 fps</param>
		public void SetPreview (bool front, int width, int height, int fps)
		{
			SetDevice (front);
			SetPreviewResolution (width, height);
			SetPreviewFPS (fps);
		}

		/// <summary>
		/// 카메라 디바이스, 해상도, fps를 설정한다.
		/// </summary>
		/// <param name="deviceId">카메라 디바이스 아이디, TCamDevice.name 값으로 설정한다.</param>
		/// <param name="width">프리뷰 width</param>
		/// <param name="height">프리뷰 height</param>
		/// <param name="fps">프리뷰 fps</param>
		public void SetPreview(string deviceId, int width, int height, int fps)
		{
			SetDevice (deviceId);
			SetPreviewResolution (width, height);
			SetPreviewFPS (fps);
		}

		/// <summary>
		/// 화면에 업데이트할 frame id를 설정한다.
		/// 화면 업데이트 후 설정된 frame id보다 낮은 frame id 값을 가지고 있는 프레임 데이터를 삭제한다.
		/// </summary>
		/// <param name="frameId">frame id</param>
		public void SetCurrentFrame(int frameId)
		{
			mCurrentFrameId = frameId;
			TCamPlugin.SetCurrentFrame (frameId);
		}

		/// <summary>
		/// 현재 frame id를 얻는다.
		/// </summary>
		/// <returns>현재 frame id</returns>
		public int GetCurrentFrame()
		{
			return mCurrentFrameId;
		}

		/// <summary>
		/// 프리뷰 화면 scale을 설정한다.
		/// </summary>
		/// <param name="scale">scale 값</param>
		public void SetScale(TCamera.TCamParameters.Scale scale)
		{
			this.scale = scale;
			TCamPlugin.SetScale ((int) scale);
		}

		/// <summary>
		/// 설정된 프리뷰 화면 scale 값을 얻는다.
		/// </summary>
		/// <returns>scale 값</returns>
		public TCamera.TCamParameters.Scale GetScale()
		{
			return scale;
		}

		/// <summary>
		/// Native 카메라 센서 orientation을 얻는다.
		/// </summary>
		/// <returns>카메라 센서 orientation</returns>
		public int GetSensorOrientation()
		{
          

            if (mSensorOrientation == INVALID) {
				mSensorOrientation = TCamPlugin.GetSensorOrientation ();
			}
			return mSensorOrientation;
		}

		/// <summary>
		/// Display Rotation값을 얻는다.
		/// </summary>
		/// <returns>Display Rotation 값</returns>
		public int GetDisplayRotation()
		{
			return TCamPlugin.GetDisplayRotation ();
		}

		/// <summary>
		/// 프리뷰 rotation 값을 얻는다. rotation 값은 Quad 모델을 기준으로 했을 때 -z 방향이다.
		/// GetPreviewFlip에서 얻은 flip값과 조합하여 프리뷰를 표시한다.
		/// </summary>
		/// <returns>프리뷰 rotation 값</returns>
		public int GetPreviewRotation()
		{
			int rotation = 0;
			int displayRotation = GetDisplayRotation ();
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				rotation = displayRotation - mSensorOrientation;
			} else {
				rotation = mSensorOrientation - displayRotation;
			}
			return rotation;
		}

		/// <summary>
		/// 프리뷰 화면 flip 여부를 얻는다. flip값은 Quad 모델을 기준으로 했을 때 적용되는 값이다.
		/// GetPreviewRotation에서 얻은 rotation값과 조합하여 프리뷰를 표시한다.
		/// </summary>
		/// <param name="hflip">horizontal flip 여부</param>
		/// <param name="vflip">vertical flip 여부</param>
		public void GetPreviewFlip(out bool hflip, out bool vflip)
		{
			hflip = false;
			vflip = false;

			if (mPreviewDevice != null && mSensorOrientation != INVALID) {
				bool frontFacing = mPreviewDevice.isFrontFacing;
				if (Application.platform == RuntimePlatform.IPhonePlayer) {
					vflip = true;
					if (frontFacing) {
						hflip = true;
					}
				} else if (Application.platform == RuntimePlatform.Android) {
					// phone
					if (mSensorOrientation == 90 || mSensorOrientation == 270) {
						if (!frontFacing) {
							vflip = true;
						}
					}
					// tablet
					if (mSensorOrientation == 0) {
						vflip = true;
						if (frontFacing) {
							hflip = true;
						}
					}
				}
			}
		}

		/// <summary>
		/// 프리뷰 포맷을 설정한다.
		/// </summary>
		/// <param name="format">프리뷰 포맷</param>
		public void SetPreviewFormat(TCamera.TCamParameters.PreviewFormat format)
		{
			mPreviewFormat = format;
		}

		/// <summary>
		/// 설정된 프리뷰 포맷을 얻는다.
		/// </summary>
		/// <returns>프리뷰 포맷</returns>
		public TCamera.TCamParameters.PreviewFormat GetPreviewFormat()
		{
			return mPreviewFormat;
		}

		#endregion

		#endregion
	
	}

	/// <summary>
	/// 카메라 디바이스
	/// </summary>
	public class TCamDevice
	{

		internal string m_Name;
		internal bool m_IsFrontFacing;

		/// <summary>
		/// 카메라 디바이스의 이름
		/// </summary>
		/// <value>The name.</value>
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}	

		/// <summary>
		/// front facing 여부
		/// </summary>
		/// <value><c>true</c> if is front facing; otherwise, <c>false</c>.</value>
		public bool isFrontFacing
		{
			get
			{
				return this.m_IsFrontFacing;
			}
		}

	}

}
