using UnityEngine;


[ExecuteInEditMode]
public class StickerSettingOverview : MonoBehaviour
{
    public StickerSetting[] StickerSettings;
    public bool IsDirty;

    // upload options
    public bool ShowUploadOption;
    public StickerEnums.StickerCategory Category;
    public Texture2D Thumbnail;


    private SkinnedMeshRenderer faceMesh;
    private Mesh bakedMesh;
    private Vector3[] originVertices;



    public void SetFaceMesh(SkinnedMeshRenderer faceMesh = null, Vector3[] originVertices = null)
    {
        if (StickerSettings != null)
        {
            if (faceMesh == null)
            {
                faceMesh = GetFace();
            }

            if (bakedMesh == null)
            {
                bakedMesh = new Mesh();
            }

            this.faceMesh = faceMesh;

            if (originVertices == null)
            {
                GetOriginalVetices();
            }
            else
            {
                this.originVertices = originVertices;
            }


            foreach (var setting in StickerSettings)
            {
                setting.FaceMesh = faceMesh;
            }

            StickerBakedMeshManager.SaveOriginMesh(faceMesh);

            UpdateTransform();
        }

    }

    public void UpdateTransform()
    {
        if (StickerSettings != null)
        {
            if (faceMesh != null)
            {
                faceMesh.BakeMesh(bakedMesh);

                foreach (var setting in StickerSettings)
                {
                    setting.UpdateTransform(bakedMesh, originVertices);
                }
            }
        }
    }

    public void UpdateTransformMobile()
    {
        UpdateTransform();
    }

    private void AdjustStickerScale(Mesh bakedMesh)
    {
        Vector3 origin_top_bottom = originVertices[111] - originVertices[5];
        Vector3 baked_top_bottom = bakedMesh.vertices[111] - bakedMesh.vertices[5];

        Vector3 origin_left_right = originVertices[318] - originVertices[75];
        Vector3 baked_left_right = bakedMesh.vertices[318] - bakedMesh.vertices[75];

        Vector3 newScale = new Vector3(baked_top_bottom.magnitude / origin_top_bottom.magnitude, baked_left_right.magnitude / origin_left_right.magnitude);
        var faceWorldScale = faceMesh.transform.lossyScale;

        if (faceWorldScale.x == 0 || faceWorldScale.y == 0)
        {
            transform.localScale = Vector3.zero;
            return;
        }

        newScale.x /= faceWorldScale.x;
        newScale.y /= faceWorldScale.y;
        newScale.z = 1;

        transform.localScale = newScale;
    }

    public void DeleteAllStickersInScene()
    {
        var stickers = FindObjectsOfType<StickerSettingOverview>();

        foreach (var item in stickers)
        {
            if (!Application.isPlaying)
            {
                DestroyImmediate(item.gameObject);
            }
            else
            {
                Destroy(item.gameObject);
            }
        }
    }

    private void GetOriginalVetices()
    {
        // bake mesh
        faceMesh.BakeMesh(bakedMesh);

        if (originVertices == null)
        {
            originVertices = bakedMesh.vertices;
        }
    }

    private void LateUpdate()
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        //transform.localScale = Vector3.one;

        UpdateTransform();
    }

    public SkinnedMeshRenderer GetFace()
    {
        var faceObj = GameObject.Find("FaceRoot");

        if (faceObj != null)
        {
            return faceObj.GetComponentInChildren<SkinnedMeshRenderer>();
        }

        return null;
    }
}
