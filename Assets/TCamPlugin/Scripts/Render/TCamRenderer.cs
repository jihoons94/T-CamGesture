using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TCamera {

	/// <summary>
	/// 프리뷰 렌더링 인터페이스
	/// </summary>
	public class TCamRenderer {
		protected TCam mTCam;
		protected TCam.RenderMethod mRenderMethod;

		protected Camera mPreviewCamera;
        protected Camera mARCamera;
        protected Transform mPreviewScreen;
		protected Material mPreviewMaterial;
		protected Quaternion mBaseRotation;

		protected bool mFrontFacing;
		protected int mPreviewWidth, mPreviewHeight;
		protected int mSensorOrientation = TCam.INVALID;
		protected int mDisplayRotation = TCam.INVALID;
		protected int mRotation = TCam.INVALID;

		protected bool mMiniPreview;
		protected Transform mMiniPreviewScreen;
		protected Material mMiniPreviewMaterial;
		protected Texture2D mMiniPreviewTexture = null;
		protected byte[] mMiniPreviewData = null;

		public static TCamRenderer Get(TCam.RenderMethod renderMethod, Camera camera, Transform screen)
		{
            if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return new TCamRendererPC(renderMethod, camera, screen);
            }
            else
            {
                switch (renderMethod)
                {
                    case TCam.RenderMethod.UNITY:
                        return new TCamRendererUnity(renderMethod, camera, screen);
                    case TCam.RenderMethod.NATIVE_UPDATE_TEXTURE:
                        return new TCamRendererNativeUpdateTex(renderMethod, camera, screen);
                    case TCam.RenderMethod.NATIVE_UPDATE_SURFACE_TEXTURE:
                        return new TCamRendererNativeUpdateSurfaceTex(renderMethod, camera, screen);
                    case TCam.RenderMethod.NATIVE_GL_SHADER:
                        return new TCamRendererNativeGLShader(renderMethod, camera, screen);
                    case TCam.RenderMethod.NATIVE_GL_SHADER_POST_RENDER:
                        return new TCamRendererNativeGLShaderPostRender(renderMethod, camera, screen);
                }

            }
            return null;
        }

		public TCamRenderer(TCam.RenderMethod renderMethod, Camera camera, Transform screen)
		{
			mTCam = TCam.Instance;
			mRenderMethod = renderMethod;

            //var arBg = new UnityEngine.XR.ARBackgroundRenderer();
            //arBg.camera = camera;

            mPreviewCamera = camera;
			mPreviewScreen = screen;

			if (mPreviewCamera && mPreviewScreen) {
				mPreviewCamera.gameObject.SetActive (true);
				mPreviewScreen.gameObject.SetActive (true);
				mPreviewMaterial = mPreviewScreen.GetComponent<Renderer> ().material;
				mBaseRotation = mPreviewScreen.rotation;
			}
		}

		virtual public void StartPreview()
		{
			if (mPreviewCamera && mPreviewScreen) {
				mPreviewCamera.gameObject.SetActive (true);
				mPreviewScreen.gameObject.SetActive (true);
			}
			if (mMiniPreview) {
				mMiniPreviewScreen.gameObject.SetActive (true);
			}
		}

		virtual public void StopPreview()
		{
			Debug.Log ("Renderder Stop");
			mSensorOrientation = TCam.INVALID;
			mDisplayRotation = TCam.INVALID;
			mRotation = TCam.INVALID;

			if (mPreviewMaterial) {
				TCamTexture.ResetTexture (mPreviewMaterial);
			}

			if (mMiniPreview) {
				StopMiniPreview ();
				mMiniPreviewScreen.gameObject.SetActive (false);
			}

			if (mPreviewCamera && mPreviewScreen) {
				mPreviewCamera.gameObject.SetActive (false);
				mPreviewScreen.gameObject.SetActive (false);
			}
		}

		virtual public void HandlePreviewStart(bool frontFacing, int width, int height)
		{
			mFrontFacing = frontFacing;
			mPreviewWidth = width;
			mPreviewHeight = height;

			mSensorOrientation = mTCam.GetSensorOrientation ();

			if (mPreviewMaterial) {

                if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                }
                else
                {
                    ScaleScreen(width, height);
                }
			}

			if (mMiniPreview) {
				StartMiniPreview (width, height);
			}
		}

		virtual public IntPtr HandlePreviewUpdate(int frameId, IntPtr frameData)
		{
			if (mMiniPreview) {
				Marshal.Copy (frameData, mMiniPreviewData, 0, mPreviewWidth * mPreviewHeight * 4);
			}

			return IntPtr.Zero;
		}

		virtual public void Update()
		{
			int rotation = mTCam.GetPreviewRotation();
			if (mRotation != rotation) {
				mRotation = rotation;
				mDisplayRotation = mTCam.GetDisplayRotation ();

				RotateScreen ();
			}

			if (mPreviewMaterial) {
				FitScreen ();
			}

			if (mMiniPreview) {
				FitMiniPreview ();
			}
		}

		virtual public void PostRender()
		{
		}

		virtual public void RotateScreen()
		{
		}

		void ScaleScreen(int width, int height)
		{
			bool hflip, vflip;
			mTCam.GetPreviewFlip (out hflip, out vflip);
			if (hflip) {
				width = -width;
			}
			if (vflip) {
				height = -height;
			}
			mPreviewScreen.localScale = new Vector3 (width, height, 1);
		}

		void FitScreen()
		{
			TCamera.TCamParameters.Scale scale = mTCam.GetScale ();
			if (scale == TCamera.TCamParameters.Scale.NONE) {
				return;
			}

			int previewWidth, previewHeight;
			int screenWidth = Screen.width;
			int screenHeight = Screen.height;
			float widthScale, heightScale;
			int baseSize;
			float fitSize;
			float fullSize;

			mTCam.GetPreviewResolution (out previewWidth, out previewHeight);

			bool portrait = ((mSensorOrientation == 0 || mSensorOrientation == 180) && (mDisplayRotation == 90 || mDisplayRotation == 270))
				|| ((mSensorOrientation == 90 || mSensorOrientation == 270) && (mDisplayRotation == 0 || mDisplayRotation == 180));
			// [ fix previous orientation's screen width, height
			if (portrait) {
				if (screenWidth > screenHeight) {
					screenWidth = Screen.height;
					screenHeight = Screen.width;
				}
			} else {
				if (screenWidth < screenHeight) {
					screenWidth = Screen.height;
					screenHeight = Screen.width;
				}
			}
			// ]

			// Galaxy S7(1440x2560)에서 640x480 preview
			if (portrait) {
				baseSize = previewWidth / 2;								// 640 / 2 = 320
				widthScale = (float)screenWidth / (float)previewHeight;		// 1440 / 480 = 3
				heightScale = (float)screenHeight / (float)previewWidth;	// 2560 / 640 = 4
			} else {
				baseSize = previewHeight / 2;								// 480 / 2 = 240
				widthScale = (float)screenWidth / (float)previewWidth;		// 2560 / 640 = 4
				heightScale = (float)screenHeight / (float)previewHeight;	// 1440 / 480 = 3
			}

			if (widthScale < heightScale) {
				// 위, 아래 여백이 생기는 경우
				fitSize = baseSize * (heightScale / widthScale);
				fullSize = baseSize;
			} else {
				// 좌, 우 여백이 생기는 경우
				fitSize = baseSize;
				fullSize = baseSize * (heightScale / widthScale);
			}

            ScaleScreen(previewWidth, previewHeight);
            //mPreviewCamera.orthographicSize = scale == TCamera.TCamParameters.Scale.FIT ? fitSize : fullSize;
            mPreviewCamera.projectionMatrix = Matrix4x4.Ortho(-previewWidth / 2, previewWidth / 2, -previewHeight / 2, previewHeight / 2, 0, 1000);
        }

        public void SetMiniPreview(Transform transform)
		{
			if (mRenderMethod == TCam.RenderMethod.NATIVE_UPDATE_SURFACE_TEXTURE) {
				mMiniPreviewScreen = transform;
				if (transform) {
					mMiniPreviewMaterial = mMiniPreviewScreen.GetComponent<Renderer> ().material;
					mMiniPreview = true;
				} else {
					StopMiniPreview ();
				}
			}
		}

		void StartMiniPreview(int width, int height)
		{
			mMiniPreviewTexture = new Texture2D (width, height, TextureFormat.RGBA32, false);

			mMiniPreviewTexture.filterMode = FilterMode.Bilinear;
			mMiniPreviewTexture.wrapMode = TextureWrapMode.Clamp;

			mMiniPreviewTexture.Apply ();

			mMiniPreviewData = new byte[mPreviewWidth * mPreviewHeight * 4];

			mMiniPreviewMaterial.mainTexture = mMiniPreviewTexture;

			ScaleMiniPreview ();
		}

		void StopMiniPreview()
		{
			if (mMiniPreviewTexture) {
				MonoBehaviour.DestroyImmediate (mMiniPreviewTexture);
			}
			mMiniPreviewMaterial = null;
			mMiniPreviewData = null;
			mMiniPreview = false;
		}

		void ScaleMiniPreview()
		{
			int width = mPreviewWidth / 4;
			int height = width * mPreviewHeight / mPreviewWidth;

			mMiniPreviewScreen.localScale = new Vector3 (width, height, 1);
			//Debug.Log ("TCamRenderer ScaleMiniPreviewScreen width=" + width + " height=" + height);
		}

		void FitMiniPreview()
		{
			mMiniPreviewTexture.LoadRawTextureData (mMiniPreviewData);
			mMiniPreviewTexture.Apply ();

			int screenWidth = Screen.width;
			int screenHeight = Screen.height;

			bool portrait = ((mSensorOrientation == 0 || mSensorOrientation == 180) && (mDisplayRotation == 90 || mDisplayRotation == 270))
				|| ((mSensorOrientation == 90 || mSensorOrientation == 270) && (mDisplayRotation == 0 || mDisplayRotation == 180));
			// [ fix previous orientation's screen width, height
			if (portrait) {
				// portrait
				if (screenWidth > screenHeight) {
					screenWidth = Screen.height;
					screenHeight = Screen.width;
				}
			} else {
				// landscape
				if (screenWidth < screenHeight) {
					screenWidth = Screen.height;
					screenHeight = Screen.width;
				}
			}
			// ]

			int cameraHeight = (int) mPreviewCamera.orthographicSize;
			int cameraWidth = (int) cameraHeight * screenWidth / screenHeight;

			int width = (int) mMiniPreviewScreen.localScale.x;
			int height = (int) mMiniPreviewScreen.localScale.y;

			int x = cameraWidth - width / 2;
			int	y = cameraHeight - height / 2;

			mMiniPreviewScreen.localPosition = new Vector3 (x, y, -1);
			//Debug.Log ("TCamRenderer FitMiniPreviewScreen cw=" + cameraWidth + " ch=" + cameraHeight + " w=" + width + " h=" + height + " x=" + x + " y=" + y);
		}

	}

}
