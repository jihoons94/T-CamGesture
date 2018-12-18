using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewCamCopy : MonoBehaviour
{
    public Camera PreviewCam;
    public RenderTexture Rt;

	// Use this for initialization
	IEnumerator Start ()
    {
        yield return new WaitForSeconds(3);
        GetComponent<Camera>().CopyFrom(PreviewCam);
        GetComponent<Camera>().targetTexture = Rt;

    }
}
