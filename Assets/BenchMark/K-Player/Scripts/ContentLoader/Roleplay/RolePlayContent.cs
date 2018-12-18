using System.IO;
using System.Linq;
using Treal;
using UnityEngine;

public class RolePlayContent : MonoBehaviour, IContent
{
    public const string ROLEPLAY_PLAYDATA = "RolePlayPlayData.tro";

    private const string RUC_FACE_DATA = "userfacedata.json";
    private const string RUC_FACE_TEXTURE = "finalface.png";

    [SerializeField]
    public RolePlayPlayer mRolePlayPlayer;

    [SerializeField]
    public SkinnedMeshRenderer mFaceSkin;

    [SerializeField]
    public GameObject mTongue;
    [SerializeField]
    private MeshRenderer mTeethRenderer;

    [SerializeField]
    public Animator mIdleAnimator;

    [SerializeField]
    public int[] ShowTeethAnimationIdxs;

    private MeshRenderer mTongueRenderer;
    private TongueReposition mTongueReposition;


    [SerializeField]
    public Transform[] mEmotions;

    private int mLastEmotionIdx = -1;
    private StickerSettingOverview mEmotionSticker;
    private StickerSettingOverview mSticker;

    private void Awake()
    {
        mTongueRenderer = mTongue.GetComponent<MeshRenderer>();
        mTongueReposition = mTongue.GetComponent<TongueReposition>();
    }

    private void Update()
    {
        ApplyEmotionSticker();
        //ChangeTeethVisible();
    }

    #region IContent
    public KidsError LoadContent(KContent content)
    {
        loadTimeline(content);
        loadSticker(content);

        if (mSticker == null)
        {
            return KidsError.LoadGetInfoError;
        }


        //start idle animation;
        mIdleAnimator.SetTrigger("Start");

        return KidsError.None;
    }

    public void Pause()
    {
        mRolePlayPlayer.Pause();
    }

    public void Play()
    {
        mRolePlayPlayer.StartPlay();
    }

    public void SetTime(float time)
    {
        mRolePlayPlayer.SetTime(time);
    }

    public void Stop()
    {
        mRolePlayPlayer.Stop();
    }

    public void UnloadContent()
    {
        if (mEmotionSticker != null)
        {
            DestroyImmediate(mEmotionSticker.gameObject);
        }

        if (mSticker != null)
        {
            foreach (Transform child in mSticker.transform)
            {
                GameObject.DestroyImmediate(child.gameObject, true);
            }

            DestroyImmediate(mSticker.gameObject);
            mSticker = null;
        }

        RUCLoader.Unload(mFaceSkin);

        //RUCLoader.Unload(mFaceSkin);
        mRolePlayPlayer.ClearPlayData();
        DestroyImmediate(gameObject, true);
    }

    public void Hide()
    {
        mFaceSkin.enabled = false;
        mTongueRenderer.enabled = false;
        mTeethRenderer.enabled = false;

        if (mSticker != null)
            mSticker.gameObject.SetActive(false);

        if (mEmotionSticker != null)
            mEmotionSticker.gameObject.SetActive(false);
    }

    public void Show()
    {
        mFaceSkin.enabled = true;
        mTeethRenderer.enabled = true;
        mTongueRenderer.enabled = true;

        if (mSticker != null)
            mSticker.gameObject.SetActive(true);

        if (mEmotionSticker != null)
            mEmotionSticker.gameObject.SetActive(true);

        ApplyEmotionSticker();
        //ChangeTeethVisible();
    }
    #endregion

    #region private
    private void loadTimeline(KContent content)
    {
        var data = RolePlayLoader.LoadRolePlayPlayData(
                    Path.Combine(content.path, ROLEPLAY_PLAYDATA));
        mRolePlayPlayer.LoadPlayData(data);
    }

    private void loadSticker(KContent content)
    {
        if (string.IsNullOrEmpty(content.sticker))
            return;

        var faceDataPath = Path.Combine(content.path, RUC_FACE_DATA);
        var faceTexturePath = Path.Combine(content.path, RUC_FACE_TEXTURE);
        var stickerPath = Path.Combine(content.path, content.sticker);
        try
        {
            mSticker = RUCLoader.Load(faceDataPath, faceTexturePath, stickerPath, mFaceSkin, mTongueReposition);
        }
        catch (System.Exception)
        {
            mSticker = null;
        }
        
    }


    #endregion

    #region EmotionSticker
    private void ApplyEmotionSticker()
    {
        int idx;
        float weight;
        GetMostEmotion(out idx, out weight);
        ChangeEmotionSticker(idx, weight);
    }

    private void ChangeEmotionSticker(int idx, float weight)
    {
        if (mLastEmotionIdx != idx)
        {
            if (mEmotionSticker != null)
            {
                DestroyImmediate(mEmotionSticker.gameObject);
            }

            if (idx > 0 && mEmotions[idx] != null)
            {
                mEmotionSticker = Instantiate(mEmotions[idx]).GetComponent<StickerSettingOverview>();
                mEmotionSticker.SetFaceMesh(mFaceSkin);
            }

            mLastEmotionIdx = idx;
        }
    }

    private void ChangeTeethVisible()
    {
        foreach (var idx in ShowTeethAnimationIdxs)
        {
            if (mFaceSkin.GetBlendShapeWeight(idx) > 0f)
            {
                mTeethRenderer.enabled = true;

                return;
            }
        }
        mTeethRenderer.enabled = false;
    }

    private void GetMostEmotion(out int idx, out float weight)
    {
        idx = -1;
        weight = 80f;
        for (int i = 0; i < mEmotions.Length; i++)
        {
            if (mFaceSkin.sharedMesh.GetBlendShapeName(i).ToLower().Contains("idle") ||
                mFaceSkin.sharedMesh.GetBlendShapeName(i).ToLower().Contains("talk")
                )
            {
                continue;
            }
            var nv = mFaceSkin.GetBlendShapeWeight(i);
            if (nv > weight)
            {
                idx = i;
                weight = nv;
            }
        }
    }
    #endregion

}
