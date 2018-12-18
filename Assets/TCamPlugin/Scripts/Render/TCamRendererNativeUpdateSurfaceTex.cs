using System;
using UnityEngine;

namespace TCamera {

	/// <summary>
	/// NATIVE_UPDATE_SURFACE_TEXTURE 렌더링 처리
    /// Android Only
	/// </summary>
	public class TCamRendererNativeUpdateSurfaceTex : TCamRenderer {
		int mPreviewTexture = TCam.INVALID;

		public TCamRendererNativeUpdateSurfaceTex(TCam.RenderMethod renderMethod, Camera camera, Transform screen)
			: base(renderMethod, camera, screen)
		{
		}

		override public void StartPreview()
		{
			base.StartPreview ();
		}

		override public void StopPreview()
		{
			base.StopPreview ();

			mPreviewTexture = TCam.INVALID;
		}

		override public void HandlePreviewStart(bool frontFacing, int width, int height)
		{
			base.HandlePreviewStart (frontFacing, width, height);

			if (mPreviewMaterial) {
				TCamTexture.SetSize (width, height);
				TCamTexture.SetTexture ();
			}
		}

		override public IntPtr HandlePreviewUpdate(int frameId, IntPtr frameData)
		{
			base.HandlePreviewUpdate (frameId, frameData);

			mTCam.SetCurrentFrame (frameId);
			return frameData;
		}

		override public void Update()
		{
			base.Update ();

			if (mPreviewTexture == TCam.INVALID) {
				mPreviewTexture = TCamPlugin.GetPreviewTexture ();
				//Debug.Log ("TCamRendererNativeUpdateSurfaceTexture _HandleFrameStart texId=" + texId);
				if (mPreviewTexture != 0) {
					TCamTexture.SetMaterialExternal (mPreviewMaterial, mPreviewTexture);
				}
			}

			//TCamPlugin.GLIssuePluginEvent (TCam.EventId.RENDER);
		}

		override public void PostRender()
		{
			base.PostRender ();
		}

		override public void RotateScreen()
		{
			base.RotateScreen ();

			if (mPreviewScreen) {
				mPreviewScreen.rotation = mBaseRotation * Quaternion.AngleAxis (mRotation, Vector3.back);
			}
		}

	}

}
