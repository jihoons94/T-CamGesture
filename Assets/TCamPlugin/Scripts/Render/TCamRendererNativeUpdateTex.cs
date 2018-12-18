using System;
using UnityEngine;

namespace TCamera {

	/// <summary>
	/// NATIVE_UPDATE_TEXTURE 렌더링 처리
	/// </summary>
	public class TCamRendererNativeUpdateTex : TCamRenderer {

		public TCamRendererNativeUpdateTex(TCam.RenderMethod renderMethod, Camera camera, Transform screen)
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
		}

		override public void HandlePreviewStart(bool frontFacing, int width, int height)
		{
			base.HandlePreviewStart (frontFacing, width, height);

			if (mPreviewMaterial) {
				TCamTexture.SetSize (width, height);
				TCamTexture.SetMaterial (mPreviewMaterial, mTCam.GetPreviewFormat());
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

			TCamPlugin.GLIssuePluginEvent (TCam.EventId.RENDER);
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
