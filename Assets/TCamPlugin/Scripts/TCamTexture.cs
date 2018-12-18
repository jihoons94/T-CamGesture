using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;


namespace TCamera {

	/// <summary>
	/// 프리뷰 텍스처 관리
	/// </summary>
	public class TCamTexture
	{
		public const string TAG = "TCamTexture";

		static int texWidth = 0, texHeight = 0;

		static Texture2D tex = null;
		static IntPtr texPtr = IntPtr.Zero;
		static Texture2D yTex = null;
		static IntPtr yTexPtr = IntPtr.Zero;
		static Texture2D uvTex = null;
		static IntPtr uvTexPtr = IntPtr.Zero;
		static Texture2D uTex = null;
		static IntPtr uTexPtr = IntPtr.Zero;
		static Texture2D vTex = null;
		static IntPtr vTexPtr = IntPtr.Zero;


		public static void SetSize(int width, int height)
		{
			texWidth = width;
			texHeight = height;
		}

		public static void SetTexture()
		{
			if (Application.platform == RuntimePlatform.Android) {
				TCamPlugin.SetTexture (
					texPtr == IntPtr.Zero ? TCam.INVALID : texPtr.ToInt32 (),
					yTexPtr == IntPtr.Zero ? TCam.INVALID : yTexPtr.ToInt32 (),
					uvTexPtr == IntPtr.Zero ? TCam.INVALID : uvTexPtr.ToInt32 (),
					uTexPtr == IntPtr.Zero ? TCam.INVALID : uTexPtr.ToInt32 (),
					vTexPtr == IntPtr.Zero ? TCam.INVALID : vTexPtr.ToInt32 ()
				);
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				TCamPlugin.SetTexture (texPtr, yTexPtr, uvTexPtr, uTexPtr, vTexPtr);
			}
		}

		public static void ResetTexture(Material material)
		{
			if (material) {
				Shader shader = material.shader;
				if (shader.name.Equals ("TCam/Texture")) {
					material.SetTexture ("_YTex", null);
					material.SetTexture ("_UTex", null);
					material.SetTexture ("_VTex", null);
				} else {
					material.mainTexture = null;
				}
			}

			if (tex) {
				MonoBehaviour.DestroyImmediate (tex);
				tex = null;
				texPtr = IntPtr.Zero;
			}

			if (yTex) {
				MonoBehaviour.DestroyImmediate (yTex);
				yTex = null;
				yTexPtr = IntPtr.Zero;
			}

			if (uvTex) {
				MonoBehaviour.DestroyImmediate (uvTex);
				uvTex = null;
				uvTexPtr = IntPtr.Zero;
			}

			if (uTex) {
				MonoBehaviour.DestroyImmediate (uTex);
				uTex = null;
				uTexPtr = IntPtr.Zero;
			}

			if (vTex) {
				MonoBehaviour.DestroyImmediate (vTex);
				vTex = null;
				vTexPtr = IntPtr.Zero;
			}

			TCamPlugin.ResetTexture ();
		}

		public static void SetMaterial(Material material, TCamera.TCamParameters.PreviewFormat format)
		{
			yTex = GetYTexture ();
			yTexPtr = yTex.GetNativeTexturePtr ();
			uTex = GetUTexture ();
			uTexPtr = uTex.GetNativeTexturePtr ();
			vTex = GetVTexture ();
			vTexPtr = vTex.GetNativeTexturePtr ();

			material.shader = Shader.Find ("TCam/Texture");
			material.SetTexture ("_YTex", yTex);
			material.SetTexture ("_UTex", uTex);
			material.SetTexture ("_VTex", vTex);
			material.SetInt ("_Format", (int) format);
		}

		public static void SetMaterialExternal(Material material, int texId)
		{
			tex = GetExternalTexture (texId);

			material.shader = Shader.Find ("Unlit/Texture");
			material.mainTexture = tex;
		}

		public static void LoadData(byte[] y, byte[] u, byte[] v)
		{
			yTex.LoadRawTextureData (y);
			yTex.Apply ();

			uTex.LoadRawTextureData (u);
			uTex.Apply ();

			vTex.LoadRawTextureData (v);
			vTex.Apply ();
		}

		static Texture2D GetTexture()
		{
			if (tex == null) {
				if (Application.platform == RuntimePlatform.Android) {
					tex = new Texture2D (texWidth, texHeight, TextureFormat.RGB24, false);
				} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
					tex = new Texture2D (texWidth, texHeight, TextureFormat.BGRA32, false);
				}
				tex.filterMode = FilterMode.Bilinear;
				tex.wrapMode = TextureWrapMode.Clamp;

				Color color = Color.black;
				for (int y = 0; y < tex.height; y++) {
					for (int x = 0; x < tex.width; x++) {
						tex.SetPixel(x, y, color);
					}
				}

				tex.Apply ();
			}

			return tex;
		}

		static Texture2D GetYTexture()
		{
			if (yTex == null) {
				yTex = new Texture2D (texWidth, texHeight, TextureFormat.Alpha8, false);
				yTex.filterMode = FilterMode.Bilinear;
				yTex.wrapMode = TextureWrapMode.Clamp;

				Color color = new Vector4 (0, 0, 0, 0);
				for (int y = 0; y < texHeight; y++) {
					for (int x = 0; x < texWidth; x++) {
						yTex.SetPixel(x, y, color);
					}
				}

				yTex.Apply ();
			}

			return yTex;
		}

		static Texture2D GetUVTexture()
		{
			if (uvTex == null) {
				// GL_RGBA4 -> RGBA4444 NOK
				uvTex = new Texture2D (texWidth / 2, texHeight / 2, TextureFormat.RGBA4444, false);
				uvTex.filterMode = FilterMode.Bilinear;
				uvTex.wrapMode = TextureWrapMode.Clamp;
				uvTex.Apply ();
			}

			return uvTex;
		}

		static Texture2D GetUTexture()
		{
			if (uTex == null) {
				uTex = new Texture2D (texWidth / 2, texHeight / 2, TextureFormat.Alpha8, false);
				uTex.filterMode = FilterMode.Bilinear;
				uTex.wrapMode = TextureWrapMode.Clamp;

				Color color = new Vector4 (0, 0, 0, 0.5f);
				for (int y = 0; y < texHeight / 2; y++) {
					for (int x = 0; x < texWidth / 2; x++) {
						uTex.SetPixel(x, y, color);
					}
				}

				uTex.Apply ();
			}

			return uTex;
		}

		static Texture2D GetVTexture()
		{
			if (vTex == null) {
				vTex = new Texture2D (texWidth / 2, texHeight / 2, TextureFormat.Alpha8, false);
				vTex.filterMode = FilterMode.Bilinear;
				vTex.wrapMode = TextureWrapMode.Clamp;

				Color color = new Vector4 (0, 0, 0, 0.5f);
				for (int y = 0; y < texHeight / 2; y++) {
					for (int x = 0; x < texWidth / 2; x++) {
						vTex.SetPixel(x, y, color);
					}
				}

				vTex.Apply ();
			}

			return vTex;
		}

		static Texture2D GetExternalTexture(int texId)
		{
			if (tex == null) {
				texPtr = new IntPtr (texId);
				tex = Texture2D.CreateExternalTexture (texWidth, texHeight, TextureFormat.RGBA32, false, true, texPtr);
				tex.filterMode = FilterMode.Bilinear;
				tex.wrapMode = TextureWrapMode.Clamp;
				tex.UpdateExternalTexture (texPtr);
			}

			return tex;
		}

	}

}
