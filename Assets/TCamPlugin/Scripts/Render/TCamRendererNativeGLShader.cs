using System;
using UnityEngine;

namespace TCamera {

	/// <summary>
	/// NATIVE_GL_SHADER 렌더링 처리
	/// </summary>
	public class TCamRendererNativeGLShader : TCamRenderer {

		public TCamRendererNativeGLShader(TCam.RenderMethod renderMethod, Camera camera, Transform screen)
			: base(renderMethod, camera, screen)
		{
			if (mPreviewCamera) {
				mPreviewCamera.gameObject.SetActive (false);
			}
			if (mPreviewScreen) {
				mPreviewScreen.gameObject.SetActive (false);
			}
			mPreviewMaterial = null;
		}

		override public void StartPreview()
		{
			base.StartPreview ();
			if (mPreviewCamera) {
				mPreviewCamera.gameObject.SetActive (false);
			}
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
			//TCamPlugin.GLIssuePluginEvent (TCam.EventId.RENDER);

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

			TCamPlugin.SetScreenRotation (Screen.width, Screen.height, mSensorOrientation, mDisplayRotation);
		}

	}

}
