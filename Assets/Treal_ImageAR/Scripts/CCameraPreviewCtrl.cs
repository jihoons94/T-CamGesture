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
    public abstract class CCameraPreviewCtrl: MonoBehaviour
    {
        protected CCameraPreviewCtrl instance = null;
        protected bool bSendPreview = false;

        public virtual void Awake()
        {
            if (instance)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            ImageTracker.Instance.CreateImageTracker();
        }

        // Use this for initialization
        public abstract void Start();

    }
}
