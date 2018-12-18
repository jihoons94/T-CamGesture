using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerBakedMeshManager
{
    private static bool hasSaved = false;
    private static Mesh bakedMesh;
    private static Vector3[] originVertices;
    private static Vector3 originMeshWorldScale;


    public static void SaveOriginMesh(SkinnedMeshRenderer faceMesh)
    {
        if (hasSaved || faceMesh == null)
        {
            return;
        }

        if (bakedMesh == null)
        {
            bakedMesh = new Mesh();
        }

        originMeshWorldScale = faceMesh.transform.lossyScale;

        // bake mesh
        faceMesh.BakeMesh(bakedMesh);

        if (originVertices == null)
        {
            originVertices = bakedMesh.vertices;
        }

        hasSaved = true;
    }

    public static Vector3[] GetOriginVertice()
    {
        return originVertices;
    }

    public static Vector3 GetOriginMeshWorldScale()
    {
        return originMeshWorldScale;
    }

    public static void Release()
    {
        bakedMesh = null;
        originVertices = null;
        hasSaved = false;
    }
}
