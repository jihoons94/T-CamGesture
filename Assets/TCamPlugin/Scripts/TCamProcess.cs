using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TCamera {

	/// <summary>
	/// Unity Camera 컴포넌트에서 프리뷰 렌더링시 사용
	/// </summary>
	public class TCamProcess : MonoBehaviour {

		void OnPostRender() {
//			Debug.Log ("OnPostRender Start");

//			if (!mat)
//			{
//				// Unity has a built-in shader that is useful for drawing
//				// simple colored things. In this case, we just want to use
//				// a blend mode that inverts destination colors.
//				var shader = Shader.Find("Hidden/Internal-Colored");
//				mat = new Material(shader);
//				mat.hideFlags = HideFlags.HideAndDontSave;
//				// Set blend mode to invert destination colors.
//				mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
//				mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
//				// Turn off backface culling, depth writes, depth test.
//				mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
//				mat.SetInt("_ZWrite", 0);
//				mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
//			}
//	
//			GL.PushMatrix();
//			GL.LoadOrtho();
//	
//			// activate the first shader pass (in this case we know it is the only pass)
//			mat.SetPass(0);
//			// draw a quad over whole screen
//			GL.Begin(GL.QUADS);
//			GL.Vertex3(0, 0, 0);
//			GL.Vertex3(1, 0, 0);
//			GL.Vertex3(1, 1, 0);
//			GL.Vertex3(0, 1, 0);
//			GL.End();
//	
//			GL.PopMatrix();



//			GL.PushMatrix();
//			GL.LoadProjectionMatrix(GetComponent<camera>().projectionMatrix);
//			GL.Begin(GL.LINES);
//			GL.Color(color);
//			GL.Vertex(from);
//			GL.Vertex(to);
//			GL.End();
//			GL.PopMatrix();



			TCam.Instance.PostRender();
	
//			Debug.Log ("OnPostRender End");
		}

	}

}