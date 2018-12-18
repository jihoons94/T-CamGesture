using UnityEngine;
using System;
using System.Collections.Generic;

namespace Treal.Browser.AR
{
    [Serializable]
    public class Treal_Form_TargetImage
    {
        public string targetName;
        public Texture2D targetTexture;
        public string thumbnailPath;
        public bool randomSelection;
        public List<Treal_ContentsForm> contentsList = new List<Treal_ContentsForm>();
    }
}