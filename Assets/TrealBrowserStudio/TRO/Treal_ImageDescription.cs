using System;
using System.Collections.Generic;
using UnityEngine;

namespace Treal.Browser.AR
{
    [Serializable]
    public class Treal_ImageDescription
    {
        public string desc_id;
        public string desc_name;
        public string thumb;

        public string thumbnailPath;

        public List<Treal_ContentsInfo> tro_list;
    }

    [Serializable]
    public class Treal_ContentsInfo
    {
        public string contents_tro_id;
        public string contents_path;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }
}
