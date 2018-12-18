using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TCamera {

	/// <summary>
	/// UNITY 렌더링 처리
	/// </summary>
	public class TCamRendererUnity : TCamRenderer {
		const int FRAME_DATA_POOL_SIZE = 5;
		FrameData[] mFrameDataPool;

		public TCamRendererUnity(TCam.RenderMethod renderMethod, Camera camera, Transform screen)
			: base(renderMethod, camera, screen)
		{
		}

		override public void StartPreview()
		{
			base.StartPreview ();

			mFrameDataPool = new FrameData[FRAME_DATA_POOL_SIZE];
			for (int i = 0; i < FRAME_DATA_POOL_SIZE; i++) {
				mFrameDataPool [i] = new FrameData ();
			}
		}

		override public void StopPreview()
		{
			base.StopPreview ();

			for (int i = 0; i < FRAME_DATA_POOL_SIZE; i++) {
				FrameData fd = mFrameDataPool [i];
				fd.Clear ();
			}
			mFrameDataPool = null;
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

			IntPtr data = IntPtr.Zero;
			TCamera.TCamParameters.PreviewFormat format = mTCam.GetPreviewFormat ();

			for (int i = 0; i < FRAME_DATA_POOL_SIZE; i++) {
				FrameData fd = mFrameDataPool [i];
				if (fd.id == TCam.INVALID) {
					int previewWidth = 0, previewHeight = 0;
					mTCam.GetPreviewResolution (out previewWidth, out previewHeight);
					fd.GetData (previewWidth, previewHeight, frameId, format);

					mTCam.SetCurrentFrame (frameId);

					if (format == TCamera.TCamParameters.PreviewFormat.GRAYSCALE) {
						data = Marshal.UnsafeAddrOfPinnedArrayElement (fd.y, 0);
					} else if (format == TCamera.TCamParameters.PreviewFormat.NV21 || format == TCamera.TCamParameters.PreviewFormat.NV12) {
						data = Marshal.UnsafeAddrOfPinnedArrayElement (fd.yuv, 0);
					}

//					GCHandle fdPinnedArray = GCHandle.Alloc (fd.y, GCHandleType.Pinned);
//					tcam.OnPreviewUpdate (frameId, fd.y, fdPinnedArray.AddrOfPinnedObject(), tcam.mPreviewWidth, tcam.mPreviewHeight);
//					fdPinnedArray.Free ();
					break;
				}
			}

			return data;
		}

		override public void Update()
		{
			base.Update ();

			int frameId = mTCam.GetCurrentFrame ();
			if (frameId != TCam.INVALID) {
				byte[] y = null;
				byte[] u = null;
				byte[] v = null;
				for (int i = 0; i < FRAME_DATA_POOL_SIZE; i++) {
					FrameData fd = mFrameDataPool [i];
					if (fd.id < frameId) {
						fd.id = TCam.INVALID;
					} else if (fd.id == frameId) {
						y = fd.y;
						u = fd.u;
						v = fd.v;
					}
				}

				if (y != null && u != null && v != null) {
					TCamTexture.LoadData (y, u, v);
				}
			}
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


		class FrameData
		{
			public int id = TCam.INVALID;
			public byte[] yuv = null;
			public byte[] y = null;
			public byte[] uv = null;
			public byte[] u = null;
			public byte[] v = null;

			public void GetData(int width, int height, int frameId, TCamera.TCamParameters.PreviewFormat format)
			{
				id = frameId;

				if (format == TCamera.TCamParameters.PreviewFormat.NV21 || format == TCamera.TCamParameters.PreviewFormat.NV12) {
					if (yuv == null) {
						yuv = new byte[width * height + width * height / 2];
					}
				}

				if (y == null) {
					y = new byte[width * height];
				}

				if (u == null) {
					u = new byte[width * height / 4];
				}

				if (v == null) {
					v = new byte[width * height / 4];
				}

				TCamPlugin.GetPreviewData (yuv, y, uv, u, v);
			}

			public void Clear()
			{
				id = TCam.INVALID;
				yuv = null;
				y = null;
				uv = null;
				u = null;
				v = null;
			}
		}

	}

}
