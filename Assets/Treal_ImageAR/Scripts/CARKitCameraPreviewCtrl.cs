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
using UnityEngine.Rendering;


#if USE_ARKIT
using UnityEngine.XR.iOS;
#endif

namespace Treal.BrowserCore
{
	public class CARKitCameraPreviewCtrl : CCameraPreviewCtrl
	{

		#if USE_ARKIT
		// ARKit Camera
		public Camera m_camera;
		private Material savedClearMaterial;

		// ARKit Camera Config
		[Header("AR Config Options")]
		public UnityARAlignment startAlignment = UnityARAlignment.UnityARAlignmentGravity;
		public UnityARPlaneDetection planeDetection = UnityARPlaneDetection.Horizontal;
		public bool getPointCloud = true;
		public bool enableLightEstimation = true;
		public bool enableAutoFocus = true;
		private bool sessionStarted = false;


		// ARKit Session
		private UnityARSessionNativeInterface m_session;
		public Material m_ClearMaterial;

		private CommandBuffer m_VideoCommandBuffer;
		private Texture2D _videoTextureY;
		private Texture2D _videoTextureCbCr;
		private Matrix4x4 _displayTransform;
		private bool bCommandBufferInitialized;

		// FrameData for VisionEngine
		private bool bTexturesInitialized;
		private int currentFrameIndex;
		private byte[] m_textureYBytes;
		private byte[] m_textureUVBytes;
		private byte[] m_textureYBytes2;
		private byte[] m_textureUVBytes2;
		private GCHandle m_pinnedYArray;
		private GCHandle m_pinnedUVArray;

		// Vision Engine
		CSpaceAREngine _engine = null;
		private bool bSpaceAREngineInitialized;
		public bool enableHybridTracker = false;

		// CARKitPreviewCtrl Instance
		public static CARKitCameraPreviewCtrl Instance = null;

		public override void Start()
		{
		Debug.Log("UNITY: CARKitCameraPreviewCtrl: Start ");
		CARKitCameraPreviewCtrl.Instance = this;

		// ARKit camera control
		m_session = UnityARSessionNativeInterface.GetARSessionNativeInterface();

		Application.targetFrameRate = 60;
		ARKitWorldTrackingSessionConfiguration config = new ARKitWorldTrackingSessionConfiguration();
		config.planeDetection = planeDetection;
		config.alignment = startAlignment;
		config.getPointCloudData = getPointCloud;
		config.enableLightEstimation = enableLightEstimation;
		config.enableAutoFocus = enableAutoFocus;

		foreach (UnityARVideoFormat vf in UnityARVideoFormat.SupportedVideoFormats())
		{
		if (vf.imageResolutionWidth == 1280f && vf.imageResolutionHeight == 720f)
		{
		config.videoFormat = vf.videoFormatPtr;
		}
		}

		if (config.IsSupported)
		{
		m_session.RunWithConfig(config);
		UnityARSessionNativeInterface.ARFrameUpdatedEvent += FirstFrameUpdate;
		}

		if (m_camera == null)
		{
		m_camera = Camera.main;
		}

		// End of ARKit camera control


		UnityARSessionNativeInterface.ARFrameUpdatedEvent += UpdateFrame;
		bCommandBufferInitialized = false;

		UnityARSessionNativeInterface.ARFrameUpdatedEvent += UpdateCameraForVision;
		bSpaceAREngineInitialized = false;
		currentFrameIndex = 0;
		bTexturesInitialized = false;
		}

		void UpdateCameraForVision(UnityARCamera camera)
		{
		if (!bTexturesInitialized)
		{
		InitializeTextures(camera);
		}
		UnityARSessionNativeInterface.ARFrameUpdatedEvent -= UpdateCameraForVision;

		}

		void InitializeTextures(UnityARCamera camera)
		{
		int numYBytes = camera.videoParams.yWidth * camera.videoParams.yHeight;
		int numUVBytes = camera.videoParams.yWidth * camera.videoParams.yHeight / 2; //quarter resolution, but two bytes per pixel

		m_textureYBytes = new byte[numYBytes];
		m_textureUVBytes = new byte[numUVBytes];
		m_textureYBytes2 = new byte[numYBytes];
		m_textureUVBytes2 = new byte[numUVBytes];
		m_pinnedYArray = GCHandle.Alloc(m_textureYBytes);
		m_pinnedUVArray = GCHandle.Alloc(m_textureUVBytes);
		bTexturesInitialized = true;
		}

		IntPtr PinByteArray(ref GCHandle handle, byte[] array)
		{
		handle.Free();
		handle = GCHandle.Alloc(array, GCHandleType.Pinned);
		return handle.AddrOfPinnedObject();
		}

		byte[] ByteArrayForFrame(int frame, byte[] array0, byte[] array1)
		{
		return frame == 1 ? array1 : array0;
		}

		byte[] YByteArrayForFrame(int frame)
		{
		return ByteArrayForFrame(frame, m_textureYBytes, m_textureYBytes2);
		}

		byte[] UVByteArrayForFrame(int frame)
		{
		return ByteArrayForFrame(frame, m_textureUVBytes, m_textureUVBytes2);
		}


		void FirstFrameUpdate(UnityARCamera cam)
		{
		sessionStarted = true;
		UnityARSessionNativeInterface.ARFrameUpdatedEvent -= FirstFrameUpdate;
		}

		void UpdateFrame(UnityARCamera cam)
		{
		_displayTransform = new Matrix4x4();
		_displayTransform.SetColumn(0, cam.displayTransform.column0);
		_displayTransform.SetColumn(1, cam.displayTransform.column1);
		_displayTransform.SetColumn(2, cam.displayTransform.column2);
		_displayTransform.SetColumn(3, cam.displayTransform.column3);
		}

		void InitializeCommandBuffer()
		{
		m_VideoCommandBuffer = new CommandBuffer();
		m_VideoCommandBuffer.Blit(null, BuiltinRenderTextureType.CurrentActive, m_ClearMaterial);
		GetComponent<Camera>().AddCommandBuffer(CameraEvent.BeforeForwardOpaque, m_VideoCommandBuffer);
		bCommandBufferInitialized = true;

		}

		void OnDestroy()
		{
		GetComponent<Camera>().RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, m_VideoCommandBuffer);
		UnityARSessionNativeInterface.ARFrameUpdatedEvent -= UpdateFrame;

		m_session.SetCapturePixelData(false, IntPtr.Zero, IntPtr.Zero);

		m_pinnedYArray.Free();
		m_pinnedUVArray.Free();


		bCommandBufferInitialized = false;
		bSpaceAREngineInitialized = false;
		}

		// Update is called once per frame

		void Update()
		{
		if (m_camera != null && sessionStarted)
		{
		// JUST WORKS!
		Matrix4x4 matrix = m_session.GetCameraPose();
		m_camera.transform.localPosition = UnityARMatrixOps.GetPosition(matrix);
		m_camera.transform.localRotation = UnityARMatrixOps.GetRotation(matrix);

		m_camera.projectionMatrix = m_session.GetCameraProjection();
		}

		if (enableHybridTracker)
		{
		if (m_session != null)
		CTrackingManager.Instance.SetSLAMCameraPose(m_session.GetCameraPose());
		else
		CTrackingManager.Instance.SetSLAMCameraPose(Matrix4x4.identity);
		}
		}

		void InitializeSpaceAREngine(int width, int height)
		{
		if (_engine == null)
		_engine = new CSpaceAREngine(width, height, ProcessMode.IMAGETRACKER);
		else
		_engine.resume();

		bSpaceAREngineInitialized = true;

		}

		#if !UNITY_EDITOR && UNITY_IOS

		public void OnPreRender()
		{
		//ARTextureHandles handles = UnityARSessionNativeInterface.GetARSessionNativeInterface ().GetARVideoTextureHandles();
		ARTextureHandles handles = m_session.GetARVideoTextureHandles();
		if (handles.textureY == System.IntPtr.Zero || handles.textureCbCr == System.IntPtr.Zero)
		{
		return;
		}

		if (!bCommandBufferInitialized) {
		InitializeCommandBuffer ();
		}

		Resolution currentResolution = Screen.currentResolution;

		// Texture Y
		if (_videoTextureY == null) {
		_videoTextureY = Texture2D.CreateExternalTexture(currentResolution.width, currentResolution.height,
		TextureFormat.R8, false, false, (System.IntPtr)handles.textureY);
		_videoTextureY.filterMode = FilterMode.Bilinear;
		_videoTextureY.wrapMode = TextureWrapMode.Repeat;
		m_ClearMaterial.SetTexture("_textureY", _videoTextureY);
		}

		// Texture CbCr
		if (_videoTextureCbCr == null) {
		_videoTextureCbCr = Texture2D.CreateExternalTexture(currentResolution.width, currentResolution.height,
		TextureFormat.RG16, false, false, (System.IntPtr)handles.textureCbCr);
		_videoTextureCbCr.filterMode = FilterMode.Bilinear;
		_videoTextureCbCr.wrapMode = TextureWrapMode.Repeat;
		m_ClearMaterial.SetTexture("_textureCbCr", _videoTextureCbCr);
		}

		_videoTextureY.UpdateExternalTexture(handles.textureY);
		_videoTextureCbCr.UpdateExternalTexture(handles.textureCbCr);

		m_ClearMaterial.SetMatrix("_DisplayTransform", _displayTransform);

		///

		if (!bTexturesInitialized)
		return;

		currentFrameIndex = (currentFrameIndex + 1) % 2;

		m_session.SetCapturePixelData(true, PinByteArray(ref m_pinnedYArray, YByteArrayForFrame(currentFrameIndex)), PinByteArray(ref m_pinnedUVArray, UVByteArrayForFrame(currentFrameIndex)));


		if (!bSpaceAREngineInitialized)
		{
		InitializeSpaceAREngine(1280, 720);
		}
		else
		{
		_engine.process(unchecked((UIntPtr)(ulong)(long)(m_pinnedYArray.AddrOfPinnedObject())));
		}
		///
		}
		#else

		public void SetYTexure(Texture2D YTex)
		{
		_videoTextureY = YTex;
		}

		public void SetUVTexure(Texture2D UVTex)
		{
		_videoTextureCbCr = UVTex;
		}

		public void OnPreRender()
		{

		if (!bCommandBufferInitialized)
		{
		InitializeCommandBuffer();
		}

		m_ClearMaterial.SetTexture("_textureY", _videoTextureY);
		m_ClearMaterial.SetTexture("_textureCbCr", _videoTextureCbCr);

		m_ClearMaterial.SetMatrix("_DisplayTransform", _displayTransform);
		}
		#endif

		#else
		public override void Start()
		{
		}
		#endif
	}
}