/** 
*
* Copyright 2016-2018 SK Telecom. All Rights Reserved.
*
* This file is part of T real Platform.
*
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
*/

using UnityEngine;

namespace CameraPoseFilter
{
    public enum FilterStyleConfig
    {
        None,
        Simple,
        Extended,
        SimpleKalman,
        MatrixKalman
    };


    [CreateAssetMenu(fileName = "PoseFilterConfig", menuName = "T real/PoseConfig", order = 1)]
    public class PoseFilterConfig : ScriptableObject
    {
        [Space(10), Header("Filter"), Space(5)]
        public FilterStyleConfig filterStyle = FilterStyleConfig.Extended;
    }
}
