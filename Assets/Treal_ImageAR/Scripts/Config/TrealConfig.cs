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

namespace Treal.BrowserCore
{
	public enum ArTrackingEngine
	{
        TCAM,
		ARKIT,
		ARCORE
	};

	[CreateAssetMenu(fileName ="TrealConfig", menuName ="T real/Config", order =1)]
	public class TrealConfig : ScriptableObject
	{
		[Space(1), Header("Tracking Engine"), Space(1)]
        public ArTrackingEngine arTracking = ArTrackingEngine.TCAM;

		[Space(1), Header("Debug"), Space(1)]
		public bool debugEnable = false;
	}

}
