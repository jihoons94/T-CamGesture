using UnityEngine;

[ExecuteInEditMode]
public class StickerSetting : MonoBehaviour
{

    public SkinnedMeshRenderer FaceMesh;
    public StickerEnums.StickerAnchor Anchor;
    public StickerEnums.StickerLayer Layer;
    public int LayerOrder;
    public Vector2 PositionOffset;
    public float Rotation;
    public Vector2 Scale;
    public bool UseVertex;
    public int Vertex;
    


    public void UpdateTransform(Mesh bakedMesh, Vector3[] originVertices)
    {
        transform.localPosition = GetAnchorPos(bakedMesh, false);

        // calculate scale offset
        //float sx = FaceMesh.transform.lossyScale.x * Scale.x;
        //float sy = FaceMesh.transform.lossyScale.y * Scale.y;
        //transform.localScale = new Vector3(sx, sy, 1);


        var faceWorldScale = FaceMesh.transform.lossyScale;
        if (faceWorldScale.x == 0 || faceWorldScale.y == 0)
        {
            transform.localScale = faceWorldScale;
            return;
        }

        originVertices = StickerBakedMeshManager.GetOriginVertice();
        var originWorldScale = StickerBakedMeshManager.GetOriginMeshWorldScale();

        // calculate scale offset
        Vector3 origin_top_bottom = originVertices[111] - originVertices[5];
        Vector3 baked_top_bottom = bakedMesh.vertices[111] - bakedMesh.vertices[5];
        Vector3 origin_left_right = originVertices[318] - originVertices[75];
        Vector3 baked_left_right = bakedMesh.vertices[318] - bakedMesh.vertices[75];

        //Debug.Log("baked_top_bottom.magnitude: " + baked_top_bottom.magnitude);
        //Debug.Log("origin_top_bottom.magnitude: " + origin_top_bottom.magnitude / originWorldScale.y);
        //Debug.Log("baked_left_right.magnitude: " + baked_left_right.magnitude / faceWorldScale.y);
        //Debug.Log("origin_left_right.magnitude: " + origin_left_right.magnitude / originWorldScale.x);

        var bakedTB = baked_top_bottom.magnitude / faceWorldScale.y;
        var bakedLR = baked_left_right.magnitude / faceWorldScale.x;
        var originTB = origin_top_bottom.magnitude / originWorldScale.y;
        var originLR = origin_left_right.magnitude / originWorldScale.x;

        Vector3 newScale = new Vector3(bakedTB / originTB, bakedLR / originLR);

        //Vector3 newScale = new Vector3(baked_top_bottom.magnitude / origin_top_bottom.magnitude,
        //    baked_left_right.magnitude / origin_left_right.magnitude);

        newScale.x *= Scale.x * faceWorldScale.x;
        newScale.y *= Scale.y * faceWorldScale.y;
        newScale.z = 1;

        if (newScale.x == Mathf.Infinity || newScale.y == Mathf.Infinity)
        {
            transform.localScale = Vector3.zero;
            return;
        }

        transform.localScale = newScale;

        // calculate rotation offset
        var ret_Rot = new Quaternion();
        var vertexIndex = StickerEnums.StickerAnchorTable[Anchor];

        if (vertexIndex == 5)
        {
            vertexIndex = 357;
        }

        Vector3 origin_angle_compare = originVertices[vertexIndex] - originVertices[5]; // 5 = chin
        Vector3 baked_angle_compare = bakedMesh.vertices[vertexIndex] - bakedMesh.vertices[5];
        ret_Rot.SetFromToRotation(origin_angle_compare, baked_angle_compare);

        Vector3 euler = transform.localEulerAngles;
        euler.z = -FaceMesh.transform.eulerAngles.z + Rotation;
        euler.z -= ret_Rot.eulerAngles.z;
        transform.localEulerAngles = euler;

        // calculate position offset
        var posOffset = PositionOffset;
        posOffset.x *= -1f;
        posOffset.x *= faceWorldScale.x;
        posOffset.y *= faceWorldScale.y;

        transform.Translate(posOffset.x, posOffset.y, 0, transform);

        UpdateLayer();
    }

    public void CalculateOffset()
    {
        if (transform.hasChanged)
        {
            // get anchor pos
            var pos = GetAnchorPos(null, false);

            // get offset
            var offset = pos - transform.position;
            offset.y *= -1f;
            offset.z = 0;

            PositionOffset = offset;

            // rotation
            var rot = transform.eulerAngles;
            Rotation = rot.z;

            // scale
            var scale = transform.localScale;
            Scale = scale;

            transform.hasChanged = false;
        }
    }

    public void UpdateLayer()
    {
        var pos = transform.localPosition;

        if (Layer == StickerEnums.StickerLayer.BACKGROUND)
        {
            pos.z = 2;
        }
        else if (Layer == StickerEnums.StickerLayer.STICKER)
        {
            pos.z = -1;
        }
        else if (Layer == StickerEnums.StickerLayer.EMOTION)
        {
            pos.z = -3;
        }

        pos.z -= LayerOrder * 0.01f;

        transform.localPosition = pos;
    }

    public Vector3 GetAnchorPos(Mesh bakedMesh = null, bool calculateOffset = true)
    {
        Mesh mesh = null;

        if (bakedMesh == null)
        {
            if (FaceMesh == null)
            {
                FaceMesh = GetFace();

                if (FaceMesh == null)
                {
                    return Vector3.one;
                }
            }

            mesh = new Mesh();
            FaceMesh.BakeMesh(mesh);
        }
        else
        {
            mesh = bakedMesh;
        }
        

        var vertex = UseVertex ? Vertex : StickerEnums.StickerAnchorTable[Anchor];
        Vector3 posOffset = PositionOffset;
        

        var worldScale = FaceMesh.transform.lossyScale;
        

        if (worldScale.x == 0 || worldScale.y == 0)
        {
            return Vector3.one;
        }

        posOffset.x *= worldScale.x;
        posOffset.y *= worldScale.y;


        var calculatedVertex = calculateOffset ? mesh.vertices[vertex] + posOffset : mesh.vertices[vertex];

        calculatedVertex.x /= worldScale.x;
        calculatedVertex.y /= worldScale.y;

        var pos = FaceMesh.localToWorldMatrix.MultiplyPoint(calculatedVertex);

        return pos;
    }

    public SkinnedMeshRenderer GetFace()
    {
        var faceObj = GameObject.Find("FaceRoot");

        if (faceObj == null)
        {
            faceObj = GameObject.Find("FaceRoot(Clone)");
        }

        if (faceObj != null)
        {
            return faceObj.GetComponentInChildren<SkinnedMeshRenderer>();
        }

        return null;
    }
}
