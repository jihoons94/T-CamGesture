using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TCamera {

	/// <summary>
	/// WebCam 렌더링 처리
    /// PC Only
	/// </summary>
	public class TCamRendererPC : TCamRenderer {

		WebCamTexture mWebCamTexture;
		Color32[] mWebCamData;

		public TCamRendererPC(TCam.RenderMethod renderMethod, Camera camera, Transform screen)
			: base(renderMethod, camera, screen)
		{
		}

		override public void StartPreview()
		{
			base.StartPreview ();

			mWebCamTexture = (WebCamTexture) TCamPlugin.GetTexture ();
			if (mPreviewMaterial) {
				mPreviewMaterial.mainTexture = mWebCamTexture;
			}
		}

		override public void StopPreview()
		{
			base.StopPreview ();

			mWebCamTexture = null;
			if (mPreviewMaterial) {
				mPreviewMaterial.mainTexture = null;
			}
		}

		override public void HandlePreviewStart(bool frontFacing, int width, int height)
		{
			base.HandlePreviewStart (frontFacing, width, height);
		}

		override public IntPtr HandlePreviewUpdate(int frameId, IntPtr frameData)
		{
			base.HandlePreviewUpdate (frameId, frameData);

			if (mWebCamTexture != null) {

                if (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    if (mWebCamTexture.width * mWebCamTexture.height != 1280 * 720)
                    {
                        return IntPtr.Zero;
                    }
                }

                if (mWebCamData == null) {
					mWebCamData = new Color32[mWebCamTexture.width * mWebCamTexture.height];
				}

				mWebCamTexture.GetPixels32(mWebCamData);
				Debug.Log ("TCamRendererPC: HandlePreviewUpdate() mWebCamTexture size = " + mWebCamTexture.width * mWebCamTexture.height );

				return Marshal.UnsafeAddrOfPinnedArrayElement (mWebCamData, 0);
			}

			return IntPtr.Zero;
		}

		override public void Update()
		{
			base.Update ();
		}

		override public void PostRender()
		{
			base.PostRender ();
		}

		override public void RotateScreen()
		{
			base.RotateScreen ();
		}

	}

}
