/** 
*
* Copyright 2016-2017 SK Telecom. All Rights Reserved.
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
	public class Singleton
	{
		private static Singleton instance;

		private Singleton() { }

		public static Singleton Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Singleton();
				}
				return instance;
			}
		}
	}

}


