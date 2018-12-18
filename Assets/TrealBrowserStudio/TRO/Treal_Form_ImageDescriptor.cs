using UnityEngine;
using System;
using System.Collections.Generic;

namespace Treal.Browser.AR
{
    [Serializable]
    public class Treal_Form_ImageDescriptor
    {
        public string descriptorName;
        public List<Treal_Form_TargetImage> targetImageList = new List<Treal_Form_TargetImage>();
    }
}