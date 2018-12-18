using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StickerEnums
{
    public enum StickerAnchor
    {
        HEAD = 111,
        EYE_L = 339,
        EYE_R = 522,
        CHEEK_L = 48,
        CHEEK_R = 406,
        EAR_L = 105,
        EAR_R = 299,
        NOSE = 146,
        MOUTH = 357,
        CHIN = 5,
        EYEBROW_L = 293,
        EYEBROW_R = 116
    }

    public readonly static Dictionary<StickerAnchor, int> StickerAnchorTable = new Dictionary<StickerAnchor, int>
    {
        { StickerAnchor.HEAD, 111 },
        { StickerAnchor.EYE_L, 339 },
        { StickerAnchor.EYE_R, 522 },
        { StickerAnchor.CHEEK_L, 48 },
        { StickerAnchor.CHEEK_R, 406 },
        { StickerAnchor.EAR_L, 105 },
        { StickerAnchor.EAR_R, 299 },
        { StickerAnchor.NOSE, 146 },
        { StickerAnchor.MOUTH, 357 },
        { StickerAnchor.CHIN, 5 },
        { StickerAnchor.EYEBROW_L, 293 },
        { StickerAnchor.EYEBROW_R, 116 }
    };


    public enum StickerLayer
    {
        BACKGROUND,
        FACE,
        STICKER,
        EMOTION
    }

    public enum StickerCategory
    {
        GENERAL,
        SAMPLE1,
        SAMPLE2,
        SAMPLE3,
        SAMPLE4,
        SAMPLE5,
        CUSTOM
    }

    public readonly static Dictionary<StickerCategory, string> StickerCategoryTable = new Dictionary<StickerCategory, string>
    {
        { StickerCategory.GENERAL, "CT_16010101600068100" },
        { StickerCategory.SAMPLE1, "CT_16010101600068101" },
        { StickerCategory.SAMPLE2, "CT_16010101600068102" },
        { StickerCategory.SAMPLE3, "CT_16010101600068103" },
        { StickerCategory.SAMPLE4, "CT_16010101600068104" },
        { StickerCategory.SAMPLE5, "CT_16010101600068105" },
        { StickerCategory.CUSTOM, "CT_16010101600068200" }
    };
}
