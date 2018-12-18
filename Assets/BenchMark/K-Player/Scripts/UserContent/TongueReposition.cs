using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueReposition : MonoBehaviour
{
    public GameObject Teeth;
    private SkinnedMeshRenderer skin;
    private Mesh bakedMesh;
    private int mouthVertex = 357;
    private int upperLipVertex = 265;


    public void Reposition (Vector3[] originVertices)
    {
        if (bakedMesh == null)
        {
            bakedMesh = new Mesh();
        }

        skin = transform.parent.Find("Source").GetComponent<SkinnedMeshRenderer>();

        skin.BakeMesh(bakedMesh);
        GenerateTongueMesh();
        GenerateToothMesh();
    }

    private void GenerateTongueMesh()
    {
        Vector3[] vList = bakedMesh.vertices;

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[4];

        int idx0 = 488;
        int idx1 = 227;
        int idx2 = 374;

        float x1 = vList[idx0].x;
        float y1 = vList[idx0].y;
        float x2 = vList[idx1].x;
        float y2 = vList[idx1].y;
        float x = vList[idx2].x;
        float y = vList[idx2].y;

        float a = (y1 - y2) / (x1 - x2);
        float p = y1 - a * x1;

        float distance = (float)(Math.Abs(a * x - y + p) / Math.Sqrt(a * a + 1));

        Vector3 pp = new Vector3(vList[idx0].x, vList[idx0].y, vList[idx0].z + 1);
        Vector3 side1 = vList[idx0] - vList[idx1];
        Vector3 side2 = vList[idx0] - pp;
        Vector3 perp = Vector3.Cross(side1, side2);
        Vector3 norPerp = Vector3.Normalize(perp);

        Vector3 newP1 = new Vector3(vList[idx0].x + (norPerp.x * distance), vList[idx0].y + (norPerp.y * distance), vList[idx0].z);
        Vector3 newP2 = new Vector3(vList[idx1].x + (norPerp.x * distance), vList[idx1].y + (norPerp.y * distance), vList[idx1].z);

        vertices[0] = newP1;  //vertices[0] = (vList[494]);
        vertices[1] = newP2;  //vertices[1] = (vList[233]);
        vertices[2] = (vList[idx0]);
        vertices[3] = (vList[idx1]);

        for (int i = 0; i < 4; ++i)
        {
            float d = vertices[i].x;
            vertices[i].x += (2.0f * (-d));
        }

        mesh.vertices = vertices;

        int[] tri = new int[6];

        tri[0] = 1;//tri[0] = 0;
        tri[1] = 3;//tri[1] = 2;
        tri[2] = 0;//tri[2] = 1;

        tri[3] = 3;//tri[3] = 2;
        tri[4] = 2;//tri[4] = 3;
        tri[5] = 0;//tri[5] = 1;

        mesh.triangles = tri;

        Vector3[] normals = new Vector3[4];

        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;
        normals[2] = -Vector3.forward;
        normals[3] = -Vector3.forward;

        mesh.normals = normals;

        Vector2[] uv = new Vector2[4];

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);

        mesh.uv = uv;
    }

    private void GenerateToothMesh()
    {
        Vector3[] vList = bakedMesh.vertices;

        Mesh mesh = new Mesh();
        Teeth.GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[4];

        int idx0 = 485;
        int idx1 = 248;
        int idx2 = 233;

        float x1 = vList[idx0].x;
        float y1 = vList[idx0].y;
        float x2 = vList[idx1].x;
        float y2 = vList[idx1].y;
        float x = vList[idx2].x;
        float y = vList[idx2].y;

        float a = (y1 - y2) / (x1 - x2);
        float p = y1 - a * x1;

        float distance = (float)(Math.Abs(a * x - y + p) / Math.Sqrt(a * a + 1));

        Vector3 pp = new Vector3(vList[idx0].x, vList[idx0].y, vList[idx0].z + 1);
        Vector3 side1 = vList[idx0] - vList[idx1];
        Vector3 side2 = vList[idx0] - pp;
        Vector3 perp = Vector3.Cross(side1, side2);
        Vector3 norPerp = Vector3.Normalize(perp);

        Vector3 newP1 = new Vector3(vList[idx0].x + (norPerp.x * distance), vList[idx0].y + (norPerp.y * distance), vList[idx0].z);
        Vector3 newP2 = new Vector3(vList[idx1].x + (norPerp.x * distance), vList[idx1].y + (norPerp.y * distance), vList[idx1].z);

        vertices[0] = newP1;  //vertices[0] = (vList[494]);
        vertices[1] = newP2;  //vertices[1] = (vList[233]);
        vertices[2] = (vList[idx0]);
        vertices[3] = (vList[idx1]);

        for (int i = 0; i < 4; ++i)
        {
            float d = vertices[i].x;
            vertices[i].x += (2.0f * (-d));
        }

        mesh.vertices = vertices;

        int[] tri = new int[6];

        tri[0] = 1;//tri[0] = 0;
        tri[1] = 3;//tri[1] = 2;
        tri[2] = 0;//tri[2] = 1;

        tri[3] = 3;//tri[3] = 2;
        tri[4] = 2;//tri[4] = 3;
        tri[5] = 0;//tri[5] = 1;

        mesh.triangles = tri;

        Vector3[] normals = new Vector3[4];

        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;
        normals[2] = -Vector3.forward;
        normals[3] = -Vector3.forward;

        mesh.normals = normals;

        Vector2[] uv = new Vector2[4];

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);

        mesh.uv = uv;
    }


    /*
    private void RepositionTeeth(Vector3[] originVertices)
    {
        if (Teeth == null)
        {
            return;
        }

        var ret_Rot = new Quaternion();
        var vertexIndex = mouthVertex;

        Vector3 origin_mouth_chin = originVertices[vertexIndex] - originVertices[5];
        Vector3 baked_mouth_chin = bakedMesh.vertices[vertexIndex] - bakedMesh.vertices[5];
        ret_Rot.SetFromToRotation(origin_mouth_chin, baked_mouth_chin);

        var euler = ret_Rot.eulerAngles;
        euler.y = 180;
        euler.z *= -1f;

        Teeth.transform.localEulerAngles = euler;

        var newPos = bakedMesh.vertices[vertexIndex];
        newPos.x *= -1f;
        newPos.z = 1f;
        Teeth.transform.position = newPos;
        //Teeth.transform.Translate(0, -2.5f, 0, Space.Self);
    }

    private void GenerateToothMesh()
    {
        Vector3[] vList = bakedMesh.vertices;

        Mesh mesh = new Mesh();
        Teeth.GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[4];

        vertices[0] = (vList[494]);
        vertices[1] = (vList[233]);
        vertices[2] = (vList[488]);
        vertices[3] = (vList[227]);

        for (int i = 0; i < 4; ++i)
        {
            float d = vertices[i].x;
            vertices[i].x += (2.0f * (-d));
        }

        mesh.vertices = vertices;

        int[] tri = new int[6];

        tri[0] = 1;//tri[0] = 0;
        tri[1] = 3;//tri[1] = 2;
        tri[2] = 0;//tri[2] = 1;

        tri[3] = 3;//tri[3] = 2;
        tri[4] = 2;//tri[4] = 3;
        tri[5] = 0;//tri[5] = 1;

        mesh.triangles = tri;

        Vector3[] normals = new Vector3[4];

        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;
        normals[2] = -Vector3.forward;
        normals[3] = -Vector3.forward;

        mesh.normals = normals;

        Vector2[] uv = new Vector2[4];

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);

        mesh.uv = uv;
    }
    */
}
