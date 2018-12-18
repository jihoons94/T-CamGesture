using System;
using UnityEngine;

namespace TCamera {

	/// <summary>
	/// NATIVE_GL_SHADER_POST_RENDER 렌더링 처리
	/// </summary>
	public class TCamRendererNativeGLShaderPostRender : TCamRenderer {

		public TCamRendererNativeGLShaderPostRender(TCam.RenderMethod renderMethod, Camera camera, Transform screen)
			: base(renderMethod, camera, screen)
		{
			if (mPreviewScreen) {
				mPreviewScreen.gameObject.SetActive (false);
			}
			mPreviewMaterial = null;
		}

		override public void StartPreview()
		{
			base.StartPreview ();

			if (mPreviewScreen) {
				mPreviewScreen.gameObject.SetActive (false);
			}
		}

		override public void StopPreview()
		{
			base.StopPreview ();
		}

		override public void HandlePreviewStart(bool frontFacing, int width, int height)
		{
			base.HandlePreviewStart (frontFacing, width, height);
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
		}

		override public void PostRender()
		{
			base.PostRender ();

			TCamPlugin.GLIssuePluginEvent (TCam.EventId.RENDER);
		}

		override public void RotateScreen()
		{
			base.RotateScreen ();

			TCamPlugin.SetScreenRotation (Screen.width, Screen.height, 180, mDisplayRotation);
		}

	}

}
