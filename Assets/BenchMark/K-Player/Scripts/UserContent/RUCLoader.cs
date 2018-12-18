using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RUCLoader
{
    private static Mesh originalMesh;
    private static Texture2D face;
    public static StickerSettingOverview Load(string userFaceDataPath, string finalFacePath, string stickerPath, SkinnedMeshRenderer faceMesh, TongueReposition tongueReposition)
    {

        // parse user face data (vertex, uv)
        var userFaceDataJson = File.ReadAllText(userFaceDataPath);
        var userFaceData = JsonUtility.FromJson<UserFaceData>(userFaceDataJson);

        // load and set face texture
        if(face == null)
            face = new Texture2D(512, 512);

        if (!face.LoadImage(File.ReadAllBytes(finalFacePath))) {
            Debug.Log("Load Face Image Failed..");

            return null;
        }

        faceMesh.material.SetTexture("_MainTex", face);

        // load sticker assetbundle
        var stickerAB = AssetBundle.LoadFromFile(stickerPath);
        var stickerObject = GameObject.Instantiate(stickerAB.LoadAllAssets()[0] as GameObject);
        var stickerSetting = stickerObject.GetComponent<StickerSettingOverview>();

        if (originalMesh == null)
        {
            originalMesh = new Mesh();
        }

        // save oringinal mesh
        faceMesh.BakeMesh(originalMesh);

        // set vertex, uv
        faceMesh.sharedMesh.vertices = userFaceData.Vertices;
        faceMesh.sharedMesh.uv = userFaceData.UVs;

        if (tongueReposition != null)
        {
            tongueReposition.Reposition(originalMesh.vertices);
        }

        // set sticker
        stickerSetting.SetFaceMesh(faceMesh, originalMesh.vertices);


        stickerAB.Unload(false);
        stickerAB = null;

        return stickerSetting;
    }


    /// <summary>
    /// return to original mesh
    /// </summary>
    /// <param name="faceMesh"></param>
    public static void Unload(SkinnedMeshRenderer faceMesh)
    {
        if (face != null)
        {
            faceMesh.material.SetTexture("_MainTex", null);
            Object.DestroyImmediate(face, true);
        }

        if (originalMesh != null)
        {
            faceMesh.sharedMesh.vertices = originalMesh.vertices;
            faceMesh.sharedMesh.uv = originalMesh.uv;

            originalMesh = null;
        }

        //GameObject.FindObjectOfType<StickerSettingOverview>().DeleteAllStickersInScene();
    }

}